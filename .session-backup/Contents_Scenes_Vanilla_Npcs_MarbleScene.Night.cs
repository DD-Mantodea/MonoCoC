// 原版 classes/scenes/npcs/MarbleScene.hx 行 3005-3907（块 D：营地大段/强奸线/坏结局/产卵/夜眠）。
// 主文件 MarbleScene.cs（58 行）已由 task-19 落地战斗收尾段；本文件是 partial 分段之一。
// 注释里的行号一律指 MarbleScene.hx。

using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public partial class MarbleScene
    {
        // ── Marble 自己的 PregnancyStore：event / eventTriggered（原版 MarbleScene.hx:48-49）──
        // 原版用 flags MARBLE_PREGNANCY_TYPE / MARBLE_PREGNANCY_INCUBATION 承载。
        // 这里在块 D 内自带只读 helper（名字带 MarblePreg 前缀，避免和 Core.cs 的 MarblePregnancy* 撞名），
        // 语义照 PregnancyStore.hx:191-221。事件阈值 [648, 528, 432, 288, 144] 只注册给 PREGNANCY_PLAYER
        // （MarbleScene.hx:49 addPregnancyEventSet）。
        private const int PregnancyNoticeMask = 0x7FFF0000; // PregnancyStore.hx:102
        private const int PregnancyPlayerType = 25;         // PregnancyStore.hx:33 PREGNANCY_PLAYER
        private const int PregnancyOvielixirEggsType = 5;   // PregnancyStore PREGNANCY_OVIELIXIR_EGGS

        private static int MarblePregRawType => CoC.Instance.Flags.Get(VanillaFlags.MarblePregnancyType);

        private static int MarblePregType => MarblePregRawType & 0x0000FFFF; // PregnancyStore.hx:101

        private static int MarblePregIncubation => CoC.Instance.Flags.Get(VanillaFlags.MarblePregnancyIncubation);

        // 对应原版 PregnancyStore.event getter（PregnancyStore.hx:191-201）
        private static int MarblePregnancyEvent()
        {
            var type = MarblePregType;

            if (type == 0)
                return 0;

            if (type == PregnancyPlayerType)
            {
                int[] events = [648, 528, 432, 288, 144];

                for (var i = 0; i < events.Length; i++)
                    if (MarblePregIncubation > events[i])
                        return i + 1;

                return events.Length + 1;
            }

            return 1;
        }

        // 对应原版 PregnancyStore.eventTriggered（PregnancyStore.hx:203-221）：notice 位存在 type flag 高位，
        // 同一个阶段只触发一次。
        private static int MarblePregnancyEventTriggered()
        {
            var stage = MarblePregnancyEvent();
            var lastNoticed = MarblePregRawType & PregnancyNoticeMask;
            var currentNoticed = stage * 65536;

            if (currentNoticed == lastNoticed)
                return 0;

            CoC.Instance.Flags.Set(VanillaFlags.MarblePregnancyType, MarblePregType + currentNoticed);

            return stage;
        }

        // 原版 Creature.manWoman（Creature.hx:2886-2917）：本块 MarbleScene.hx:3605 用一次。
        // C# 的 Creature 还没有这个方法，按原版分支在 partial 内落地（caps=false）。
        private static string MarbleManWoman()
        {
            if (Player.CockTotal() > 0)
                return Player.HasVagina() ? "futa" : "man";

            return Player.HasVagina() ? "woman" : "eunuch";
        }
        // Haxe MarbleCampSexNew()   outputText 132 
        public void MarbleCampSexNew()
        {
            // 原版 MarbleScene.hx:3005-3006 直接 marbleSprite(true)，没有 clearOutput。
            MarbleSprite(true);
            if (Player.HasStatusEffect(VanillaStatusEffects.Infested))
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.CallMarbleForRelease".Localized());
                return ;
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 0)
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.AskMarbleReleaseSourly".Localized());
                return ;
            }
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            if (!Player.HasTailInsteadOfLegs())
            {
                if (Player.HasPerk(VanillaPerks.MarblesMilk) && (int)Player.Gender > 0 && Random.Shared.Next((int)(2)) == 0)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.AdmitHorninessToMarble".Localized());
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.CuteSweetieAcceptance".Localized());
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LeadToComfySeat".Localized());
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.BareLowerArmorLie".Localized());
                    if (Player.Tallness < 66)
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.RightBreast".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.LeftBreast".Localized());
                    }
                    if (Player.Tallness > 90)
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.BendOverBreastAccess".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.MarbleHandDownAbdomen".Localized(
                        MarbleNips()
                    ));
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.DrinkOfMeCommand".Localized());
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleTeatMilkFlows".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ChuckleReadyEagerCocks".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ChuckleReadyPlaceholder".Localized(
                            Player.VaginaDescript(0)
                        ));
                        if (Player.Wetness() < 2)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ChideLackOfEagerness".Localized());
                        }
                        else
                        {
                            if (Player.Wetness() < 4)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.NeedSomethingDownHere".Localized());
                            }
                            else
                            {
                                if (Player.Wetness() < 5)
                                {
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.QuestionWetnessStimulus".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.NeedTowelOffRemark".Localized());
                                }
                            }
                        }
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.SmileThenBeginStroking".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GentleStrokesIntensify".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.DoesItHurtAssurance".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MoanIntoBreastGiggle".Localized());
                        if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) == 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighsRubbingSeemWet".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighsRubHardPoking".Localized());
                        }
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ReachCockBaseRoll".Localized());
                        }
                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LowerToClitRubbing".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TugStrokeForCum".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.StrokingBecomesUnrelenting".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.RapidStrokesEjaculation".Localized());
                        if (Player.CumQ() < 500)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CoverHandKeepJerking".Localized());
                        }
                        else
                        {
                            if (Player.CumQ() < 1000)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.LargeLoadRopesFly".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.EnormousLoadCoversGround".Localized());
                            }
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PullAwayFromBreast".Localized());
                        if (Player.CumQ() >= 1000)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.WipeNippleOnOther".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckOtherBreastPlea".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.HandOnClitRub".Localized(
                            Player.VaginaDescript(0)
                        ));
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EncourageFurtherSuckling".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MoanIntoNippleGiggle".Localized());
                        if (Player.GetClitLength() < 3)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CarefulIntenseRubbing".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.GrabClitPerverseGrin".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ReturnToFingerPumping".Localized());
                        if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) == 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighRubbingWetAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighRubHardAgain".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EdgeAlreadyFingerRub".Localized(
                            Player.VaginaDescript(0)
                        ));
                        if (Player.Wetness() >= 4)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.MessyNeverStopsOrgasm".Localized());
                        }
                        if (Player.Wetness() > 4)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FemcumShootsOnArm".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.FunStatesPullAway".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.RiseSitBesideNursing".Localized());
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.MilkFlowDiminishes".Localized());
                    Player.SlimeFeed();
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) + 20);
                }
                else
                {
                    if (Random.Shared.Next((int)(2)) == 0 && (int)Player.Gender > 0)
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.WalkUpToLover".Localized());
                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TentGrowingInArmor".Localized());
                        }
                        else
                        {
                            if (Player.HasVagina())
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.MoistnessUnderArmor".Localized());
                            }
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TenderEyesStare".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.UndressEachOther".Localized());
                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThrobsWord".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitQuiverArousal".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SentencePeriodParagraphBreak".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KinkyLookCoos".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.WarmthReplacesHands".Localized());
                        if (Player.HasVagina() && !Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueProbeAppropriate".Localized(
                                Player.VaginaDescript(0)
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueWrapRodPrecum".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.AmazingJobThrashing".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MarbleKeepsGoing".Localized());
                        if (Player.HasVagina() && !Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.RelentlessCuntProbing".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleUrethraJuices".Localized());
                        }
                        if (Player.CockTotal() > 1)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.AlternateSuckAndJerk".Localized());
                        }
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.BallsChurnArousal".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EcstasyMoanGrabHead".Localized());
                        if (Player.Gender == Gender.Female)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueFuckVerb".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.DeepthroatVerb".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.HipsThrustOnTongue".Localized());
                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LipsEncloseCockTight".Localized());
                            if (Player.CockTotal() > 1)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.StrokeAnotherCock".Localized());
                            }
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.OrganicCockRing".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FirmTongueLikeDong".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.LusciousMinistrationsTooMuch".Localized());
                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CockTwitchesInThroat".Localized());
                            if (Player.HasPerk(VanillaPerks.MarblesMilk))
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UsuallyWord".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UsedToWord".Localized());
                            }
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleHerTits".Localized());
                            if (Player.CumQ() >= 500)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SeedSpoutDistendBelly".Localized());
                            }
                            if (Player.CumQ() > 1500)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReadyToGiveBirth".Localized());
                            }
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LipsClingCumTube".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.QuiverBeforeFemSpunk".Localized(
                                Player.VaginaDescript(0)
                            ));
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KeepSuckingAfterOrgasm".Localized());
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GetsUpTiredKiss".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.AntsyMarbleAsk".Localized());
                        if (Player.Corruption + Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) < 66)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FindSecludedCampSpot".Localized());
                        }
                        else
                        {
                            if (Player.Corruption + Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) < 132)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.StripDownMidCamp".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.LookForThirdNobody".Localized());
                            }
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SmilePushBreastsTogether".Localized());
                        if (Player.BiggestBreastSize() > 3)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.PressBreastsOnPlaceholder".Localized(
                                Appearance.BiggestBreastSizeDescript(Player),
                                MarbleNips()
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.PressBreastsOnChest".Localized());
                        }
                        if (Player.Cocks.Count > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SlipPlaceholderBetweenBreasts".Localized(
                                Player.CockDescript(0)
                            ));
                            if (Player.Cocks.Count > 1)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UnsatisfiedEngulfRest".Localized());
                            }
                        }
                        if (Player.Vaginas.Count > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SlideDownBodyCaress".Localized(
                                Player.VaginaDescript(0),
                                MarbleNips(),
                                Player.VaginaDescript(0)
                            ));
                            if (Player.GetClitLength() < 2)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.BreastsDownBothLegs".Localized(
                                    Player.VaginaDescript(0)
                                ));
                            }
                            else
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitBetweenBreastPump".Localized());
                            }
                        }
                        if (Player.Gender == Gender.Genderless)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FoundWayMakeCum".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.OverwhelmingOrgasmRacks".Localized());
                        if (Player.Cocks.Count > 0)
                        {
                            if (Player.CumQ() < 25)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SmallCumAmountBreasts".Localized());
                            }
                            else
                            {
                                if (Player.CumQ() < 250)
                                {
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.FairCumAmountFace".Localized());
                                }
                                else
                                {
                                    if (Player.CumQ() < 500)
                                    {
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KeepCummingUntilDrenched".Localized());
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EndlessOrgasmBothSoaked".Localized());
                                    }
                                }
                            }
                        }
                        if (Player.Vaginas.Count > 0)
                        {
                            if (Player.Vaginas[0].Wetness < 2)
                            {
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SmallGirlyFluidLeak".Localized());
                            }
                            else
                            {
                                if (Player.Vaginas[0].Wetness <= 3)
                                {
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LiberalGirlyFluid".Localized());
                                }
                                else
                                {
                                    if (Player.Vaginas[0].Wetness < 5)
                                    {
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.DecentlyDrenchedFluid".Localized());
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MinuteToRecoverBlush".Localized());
                                    }
                                }
                            }
                        }
                        if (Player.Gender == Gender.Genderless)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.DifferentYetOrgasm".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SatisfiedStandRiverWash".Localized());
                    }
                }
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.ApproachAskSexualRelease".Localized());
                OutputText("Scenes.Marblescene.MarbleCampSexNew.LongJourneyEnjoyTrip".Localized());
                OutputText("Scenes.Marblescene.MarbleCampSexNew.TailMassageOffer".Localized());
                if (Player.Libido >= 50)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.HookClothingHemQuietly".Localized());
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.AndPlaceholder".Localized(
                        MarbleCock()
                    ));
                }
                OutputText("Scenes.Marblescene.MarbleCampSexNew.ShamelessMockOffense".Localized(
                    Player.Mf("pervert", "hussy")
                ));
                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReturnToTailCrawl".Localized());
                if ((int)Player.Gender > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.ReachSlitPanting".Localized());
                    if (Player.HasCock())
                    {
                        OutputText(Player.MultiCockDescriptLight());
                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.AndWord".Localized());
                        }
                    }
                    if (Player.HasVagina())
                    {
                        OutputText(Player.VaginaDescript());
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.TreasureAlreadyHere".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GrabsYourShaft".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PenetratesPussy".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LongTongueLickPhrase".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TipWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitWord".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.CryPleasureTailWrap".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SemenLoadIntoMouth".Localized());
                    }
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PussySquirtsValley".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.WhereGenitalsWouldBe".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReleaseGripHelpStand".Localized());
            }
            Player.Orgasm("Generic");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -3f));
        }

        // Haxe MarbleNips()   outputText 0 
        public string MarbleNips()
        {

            if (!Player.HasStatusEffect(VanillaStatusEffects.MarbleSpecials))
            {
                Player.CreateStatusEffect(VanillaStatusEffects.MarbleSpecials, 0, 1, 0, 0);
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 1) == 0)
            {
                Player.ChangeStatusValue(VanillaStatusEffects.MarbleSpecials, 2, 1);
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 1) == 1)
            {
                return "nipples";
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 1) == 4)
            {
                return "quad-nipples";
            }
            return "nipples(MARBLE NIP ERROR)";
        }

        // Haxe MarbleCock()   outputText 0 
        public string MarbleCock()
        {

            var descript = "";
            int rando = 0;
            if (Random.Shared.Next(3) == 0)
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 3)
                {
                    descript = "squat ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 3 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 5)
                {
                    descript = "short ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 5 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 7)
                {
                    descript = "average ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 7 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 9)
                {
                    descript = "long ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 10 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 13)
                {
                    descript = "huge ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 13 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 18)
                {
                    descript = "massive ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 18 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) < 30)
                {
                    descript = "enormous ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) >= 30)
                {
                    descript = "towering ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) <= .75f)
                {
                    descript += "narrow ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) > 1 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) <= 1.4f)
                {
                    descript += "ample ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) > 1.4f && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) <= 2)
                {
                    descript += "broad ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) > 2 && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) <= 3.5f)
                {
                    descript += "fat ";
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) > 3.5f)
                {
                    descript += "distended ";
                }
            }
            rando = Random.Shared.Next(10);
            if (rando >= 0 && rando <= 4)
            {
                descript += "cock";
            }
            if (rando == 5 || rando == 6)
            {
                descript += "prick";
            }
            if (rando == 7)
            {
                descript += "pecker";
            }
            if (rando > 7)
            {
                descript += "shaft";
            }
            return descript;
        }

        // Haxe RapeDAHMARBLEZ()   outputText 19 
        // 原版 MarbleScene.hx:3442 rapeDAHMARBLEZ（块 D；Core.cs:950 private partial 声明，本块实现）
        private partial void RapeDAHMARBLEZ()
        {
            ClearOutput();

            var raped = false;
            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.DecideToRoughRape".Localized());
            if (Player.HasPerk(VanillaPerks.Evade))
            {
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.DuckUnderSlap".Localized());
                raped = true;
            }
            else
            {
                if (Player.Strength >= 80)
                {
                    OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.PushHerOntoBed".Localized());
                    raped = true;
                }
            }
            if (!raped)
            {
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.SheForcesYouOut".Localized());
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ResolveNotUnderestimate".Localized());
            }
            else
            {
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.FreeCocksFromArmor".Localized());
                    if (Player.Cocks[0].Thickness < 3)
                    {
                        if (Player.Cocks[0].Length <= 8)
                        {
                            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ForceInAsFar".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ForceInToHilt".Localized());
                        }
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.GruntWhileThrusting".Localized());
                        if (Player.Corruption >= 33)
                        {
                            MarbleRapeCorruptFantasy();
                        }
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntThenCreampie".Localized());
                        Player.Orgasm("Dick");
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.StruggleWithGirth".Localized(
                            Player.CockDescript(0)
                        ));
                        if (Player.Corruption >= 33)
                        {
                            MarbleRapeCorruptFantasy();
                        }
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntThenPullOut".Localized());
                        Player.Orgasm("Dick");
                    }
                }
                else
                {
                    if (Player.Vaginas.Count > 0)
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.LookAroundRoom".Localized());
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ShareDildoWithMarble".Localized(
                            Player.VaginaDescript(0)
                        ));
                        if (Player.Corruption >= 33)
                        {
                            MarbleRapeCorruptFantasy();
                        }
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntAfterDildoOrgasm".Localized());
                        Player.Orgasm("Vaginal");
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.LackingGenitalsInstead".Localized());
                        if (Player.IsCorruptEnough(33))
                        {
                            MarbleRapeCorruptFantasy();
                        }
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ShoveFistInside".Localized());
                        Player.ApplyStats(new DynStat(VanillaStats.Lust, -20f));
                    }
                }
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.RevelBeforeRollover".Localized());
                Player.HealthChange(-9001);
                Player.Health = 1;
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.WakeWithHeadache".Localized());
            }
            ReturnToCamp(4);
            Player.CreateStatusEffect(VanillaStatusEffects.MarbleRapeAttempted, 0, 0, 0, 0);
        }

        // Haxe MarbleRapeCorruptFantasy()   outputText 3 
        private partial void MarbleRapeCorruptFantasy()
        {

            OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.HelplessBeforeOnslaught".Localized());
            if (Player.Corruption >= 66)
            {
                OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.ClearCorruptFantasy".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.SmileOneDayThought".Localized());
            }
        }

        // Haxe MarbleAfterRapeBattle()   outputText 1 
        public void MarbleAfterRapeBattle()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeBattle.SpotMarbleAtBarn".Localized());
            AddButton(0, "Yes", MarbleAfterRapeYes);
            AddButton(1, "No", MarbleAfterRapeNo);
        }

        // Haxe MarbleAfterRapeYes()   outputText 1 
        public void MarbleAfterRapeYes()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeYes.ApproachBarnSpotted".Localized());
            AddButton(0, "Yes", MarbleAfterRapeStartFight);
            AddButton(1, "No", MarbleAfterRapeNo);
        }

        // Haxe MarbleAfterRapeStartFight()   outputText 1 
        public void MarbleAfterRapeStartFight()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeStartFight.EnterCombatStance".Localized());
            // 原版 MarbleScene.hx:3562-3565 marbleAfterRapeStartFight：startCombat(new Marble(), true)
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "marble")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe MarbleAfterRapeNo()   outputText 1 
        public void MarbleAfterRapeNo()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeNo.LeaveNotWorthIt".Localized());
            ReturnToCamp(1);
        }

        // Haxe MarbleBadEndFollowup()   outputText 20 
        private partial void MarbleBadEndFollowup()
        {
            ClearOutput();

            var morph = Player.Race;
            var approxHeight = "";
            if (Player.Tallness < 54)
            {
                approxHeight = "very short";
            }
            else
            {
                if (Player.Tallness < 66)
                {
                    approxHeight = "short";
                }
                else
                {
                    if (Player.Tallness < 72)
                    {
                        approxHeight = "average";
                    }
                    else
                    {
                        if (Player.Tallness < 84)
                        {
                            approxHeight = "tall";
                        }
                        else
                        {
                            approxHeight = "very tall";
                        }
                    }
                }
            }
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.LearnToGetAlong".Localized());
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.YoungChampionApproaches".Localized());
            if (morph == "human")
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.EyesLightUp".Localized());
                // 原版 MarbleScene.hx:3602-3604：`if (approxHeight != "average height") outputText(approxHeight + " ");`
                // 原版赋值是 "average"、判定字符串是 "average height"，条件恒真——按原样保留，不修这个 bug。
                if (approxHeight != "average height")
                {
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.PlaceholderToken".Localized(approxHeight));
                }
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.HumanByBarnDoor".Localized(
                    Player.Mf("male", "female"),
                    MarbleManWoman()
                ));
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.GladToHaveFriend".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.SurprisedToSeeYou".Localized(
                    Player.Mf("male", "female"),
                    morph,
                    morph,
                    morph,
                    morph
                ));
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.HumanityLeftBehind".Localized());
            }
            if (approxHeight == "very short" || approxHeight == "short")
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.ShorterThanLastTime".Localized());
            }
            else
            {
                if (approxHeight == "tall" || approxHeight == "very tall")
                {
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.BiggerThanLastTime".Localized());
                }
            }
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.JoinAtMyCamp".Localized());
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.FemaleVoiceCallsOut".Localized());
            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.TallCowGirlPregnant".Localized());
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.OlderChampionIntroduces".Localized());
                if (Player.Gender == Gender.Hermaphrodite && Player.Mf("m", "f") == "f" && Player.BiggestCockArea() < 15)
                {
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.StammersYourChild".Localized(
                        Player.VaginaDescript(0)
                    ));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.CongratulateNewFamily".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.TallCowGirlAppears".Localized());
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.IntroduceNewChampion".Localized());
            }
            if (KnowAddiction == 1)
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.MarbleAsksLikeMilk".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.ParagraphBreak".Localized());
            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.AbandonQuestToMilk".Localized());
            GameOver();
        }

        // Haxe MarblePoopsBaybees()   outputText 34 
        private partial void MarblePoopsBaybees()
        {

            // 原版 MarbleScene.hx:3653 `pregnancy.type == PREGNANCY_PLAYER`：这里的 pregnancy 是 Marble 自己的
            // PregnancyStore（MarbleScene.hx:44/48，flags MARBLE_PREGNANCY_*），不是玩家的。
            if (MarblePregType == PregnancyPlayerType)
            {
                OutputText("Scenes.Marblescene.MarblePoopsBaybees.MarbleAnnouncesBirth".Localized());
                OutputText("Scenes.Marblescene.MarblePoopsBaybees.HeadWithBovineEars".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurified) > 0 && Random.Shared.Next((int)(2)) == 0)
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.SonFullyDelivered".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.HandBoyToMarble".Localized());
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.AskWhySheHesitated".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.ChildStopsCrying".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.SucklingJustAsWonderful".Localized());
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.OffersOtherBreast".Localized());
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.ChildIntoNursery".Localized());
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleBoys, CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) + 1);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.DaughterFullyDelivered".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.TraitNotPassedOn".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlLooksLikeMom".Localized());
                    if (Player.HasPerk(VanillaPerks.MarblesMilk))
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.WontGetAddicted".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlSucklingWonderful".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.OtherBreastForGirl".Localized());
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlIntoNursery".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 0)
                    {
                        var temp = Random.Shared.Next((int)(10));
                        var __sw0_3 = temp;
                        if (__sw0_3 == 0)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.AuraName".Localized());
                        }
                        if (__sw0_3 == 1)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.MiranName".Localized());
                        }
                        if (__sw0_3 == 2)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.LinName".Localized());
                        }
                        if (__sw0_3 == 3)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.MaryName".Localized());
                        }
                        if (__sw0_3 == 4)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.BessName".Localized());
                        }
                        if (__sw0_3 == 5)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.TinaName".Localized());
                        }
                        if (__sw0_3 == 6)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.RillName".Localized());
                        }
                        if (__sw0_3 == 7)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.WendyName".Localized());
                        }
                        if (__sw0_3 == 8)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.RainyName".Localized());
                        }
                        if (__sw0_3 == 9)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.NickyName".Localized());
                        }
                        if (true /* default */)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.GlitchyName".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.GoodNameForHer".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.NameHerTomorrow".Localized());
                    }
                }
                CoC.Instance.Flags.Set(VanillaFlags.MarbleKids, CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) + 1);
                // 原版 MarbleScene.hx:3736 doNext(playerMenu)
                DoNext(PlayerMenu);
            }
            else
            {
                // 原版 MarbleScene.hx:3739 `pregnancy.type == PREGNANCY_OVIELIXIR_EGGS`（Marble 自己的妊娠）
                if (MarblePregType == PregnancyOvielixirEggsType)
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.HearMoaningNearRocks".Localized());
                    if (Random.Shared.Next((int)(2)) == 0)
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.SmallEggPile".Localized());
                    }
                    else
                    {
                        if (Random.Shared.Next((int)(2)) == 0)
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.LargeEggPile".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.HugeEggPile".Localized());
                        }
                    }
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.MarbleStretchesLegs".Localized());
                    Identifier itype = default;
                    if (Random.Shared.Next((int)(3)) == 1)
                    {
                        itype = VanillaConsumables.LargeEggs[Random.Shared.Next((int)(VanillaConsumables.LargeEggs.Length))];
                    }
                    else
                    {
                        itype = VanillaConsumables.SmallEggs[Random.Shared.Next((int)(VanillaConsumables.SmallEggs.Length))];
                    }
                    // 未移植：原版 MarbleScene.hx:3763-3767 用 game.timeQ（CoC.hx:407）分「camp.doSleep / playerMenu」两支；
                    // C# 没有 timeQ（EmberScene.cs:7117 同缺口），这里统一走原版 else 的 playerMenu 分支，不猜时间。
                    InventoryScene.TakeItem(itype, PlayerMenu);
                }
            }
        }

        // Haxe MarbleNightSleepFlavor()   outputText 27 
        public bool MarbleNightSleepFlavor()
        {
            // 原版 MarbleScene.hx:3771-3772 是营地睡眠流程里追加文本，没有 clearOutput。
            MarbleSprite();
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleNurseryConstruction) == 0 && Player.Pregnancy.Type == VanillaPregnancy.Marble && (Player.Pregnancy.Incubation <= 128))
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BuildingNurseryNotice".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleNurseryConstruction, CoC.Instance.Flags.Get(VanillaFlags.MarbleNurseryConstruction) + 1);
            }
            var __sw0_0 = MarblePregnancyEventTriggered();
            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleGainedWeight".Localized());
            }
            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BellySlightlyBigger".Localized());
            }
            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BellyObviouslyPregnant".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleNurseryConstruction) == 0 && MarblePregType != PregnancyOvielixirEggsType)
                {
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleNurseryConstruction, CoC.Instance.Flags.Get(VanillaFlags.MarbleNurseryConstruction) + 1);
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BusyWithNursery".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.CloseBoldParagraph".Localized());
            }
            if (__sw0_0 == 5)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.PregnancyAdvances".Localized());
            }
            if (__sw0_0 == 6)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.NearGivingBirth".Localized());
            }
            if (Player.HasStatusEffect(VanillaStatusEffects.Infested))
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleGrossedOutBecauseWormInfestation) != 1)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleRejectsWorms".Localized());
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleGrossedOutBecauseWormInfestation, 1);
                }
                return false;
            }
            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WhisperedBedRequest".Localized());
                return false;
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 50 || Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleAlreadyThereShort".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 0)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.TurnsAwayAnnoyed".Localized());
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 20)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.LiesContentedlyNearby".Localized());
                        }
                        else
                        {
                            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 30)
                            {
                                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SoftSighInBedroll".Localized());
                            }
                            else
                            {
                                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 40)
                                {
                                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.FidgetsInBedroll".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SeemsWorkedUp".Localized());
                                }
                            }
                        }
                    }
                    if (Player.Lust < 33 && CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 20)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.DriftOffToSleep".Localized());
                    }
                    else
                    {
                        if (Player.Lust < 33)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WondersWhatSheWants".Localized());
                            if (Random.Shared.Next((int)(2)) == 0)
                            {
                                MarbleCuddlin();
                            }
                            else
                            {
                                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.NotArousedEnough".Localized());
                            }
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.AskMarbleForSex".Localized());
                            ClearButtons();
                            AddButton(0, "Ask", AtNightAskMarbleForSomeSexMaybe);
                            AddButton(1, "Don't Ask", PlayerMenu);
                            return true;
                        }
                    }
                    // 原版 MarbleScene.hx:3856 outputText(" ");（<50 lust 分支收尾的空格文本）
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SpaceOnly".Localized());
                }
                else
                {
                    if (Player.Lust < 33)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.HornyMarbleAsksHelp".Localized());
                        if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.DesperateForSucking".Localized(
                                MarbleCock()
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WantsWomanhoodLicked".Localized());
                        }
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.OrallyPleasurePrompt".Localized());
                        ClearButtons();
                        AddButton(0, "Yes", MarbleNomNoms);
                        AddButton(1, "No", PlayerMenu);
                        if (Player.HasTailInsteadOfLegs() && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) > 0)
                        {
                            AddButton(2, "Tailjob", GiveMarbleTailjobRelease);
                        }
                        else
                        {
                            // 原版 MarbleScene.hx:3878 addButtonDisabled(2, "Tailjob", "This scene requires you to have a naga tail. Marble should have a cock.")
                            AddButtonDisabled(2, "Tailjob", "This scene requires you to have a naga tail. Marble should have a cock.");
                        }
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) < 5)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.HornyMarbleWantsSex".Localized());
                            ClearButtons();
                            AddButton(0, "Yes", () => MarbleNightSexIntro());
                            AddButton(1, "No", PlayerMenu);
                            AddButton(2, "Oral", MarbleNomNoms);
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.EagerLookFromMarble".Localized());
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.EscortToOurBed".Localized());
                            ClearButtons();
                            // 原版 MarbleScene.hx:3896-3897 两条按钮各有 hint
                            AddButton(0, "Lovemaking", MarbleNightLovemaking).Hint("Have at it.");
                            AddButton(1, "Sleep", MarbleNightRefusal).Hint("Not tonight.");
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        // Haxe PcPregWithMarblesKids()   outputText 0 
    }
}

