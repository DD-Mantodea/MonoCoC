// 原版 classes/scenes/npcs/MarbleScene.hx 行 3909-5042（块 E：夜晚性爱线后半 / 物品外观线 / 挤奶酒吧线）。
// 主文件 MarbleScene.cs 已是 partial；注释里的行号一律指 MarbleScene.hx。
// 该段由 PortKit emit 输出按 Haxe 行范围抽取后手工修正（含本地化 key 映射与跨块契约）。

using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.Utils;
using MonoCoC.OptionSystem;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public partial class MarbleScene
    {
        // 原版 PregnancyStore.INCUBATION_MARBLE = 368（PregnancyStore.hx:72）；
        // C# 的 VanillaPregnancy 只有 coc:marble 这个 type id，没有对应的 incubation 常量。
        private const int MarbleIncubation = 368;

        // 原版 PregnancyStore.knockUpChance(floor=20, ceiling=60, father=player)（PregnancyStore.hx:169-176）。
        private static bool MarbleKnockUpChance(int floor = 20, int ceiling = 60)
        {
            if (MarblePregType != 0)
                return false;

            return RandomUtils.Chance(Math.Clamp(floor + Player.VirilityQ(), floor, ceiling));
        }

        // 原版 PregnancyStore.knockUp / knockUpForce（PregnancyStore.hx:149-167）：只写不覆盖已有孕况。
        private static void MarbleKnockUp(int type, int incubation)
        {
            if (MarblePregType != 0)
                return;

            var notice = MarblePregRawType & PregnancyNoticeMask;

            CoC.Instance.Flags.Set(VanillaFlags.MarblePregnancyType, notice + type);
            CoC.Instance.Flags.Set(VanillaFlags.MarblePregnancyIncubation, incubation);
        }

        public bool PcPregWithMarblesKids()
        {

            return Player.Pregnancy.Type == VanillaPregnancy.Marble && Player.Pregnancy.Incubation <= 280;
        }

        // Haxe MarblePregWithPCKids()   outputText 0 
        public bool MarblePregWithPCKids()
        {

            // 原版 marblePregWithPCKids()：Marble 自己的孕床用 flags 承载（MARBLE_PREGNANCY_TYPE / INCUBATION），
            // 不是 Player.Pregnancy。PREGNANCY_PLAYER = 25，见 MarbleScene.Night.cs 的 PregnancyPlayerType。
            return MarblePregType == PregnancyPlayerType && MarblePregIncubation <= 280;
        }

        // Haxe MarbleCuddlin()   outputText 6 
        public void MarbleCuddlin()
        {

            OutputText("Scenes.Marblescene.MarbleCuddlin.MarbleReachesOver".Localized());
            if (!PcPregWithMarblesKids() && !MarblePregWithPCKids())
            {
                if (Player.Tallness <= 78)
                {
                    OutputText("Scenes.Marblescene.MarbleCuddlin.HeadOntoHerChest".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleCuddlin.ArmsWrappedAroundYou".Localized());
                }
            }
            else
            {
                if (MarblePregWithPCKids() && !PcPregWithMarblesKids())
                {
                    OutputText("Scenes.Marblescene.MarbleCuddlin.EarToSwollenBelly".Localized());
                }
                else
                {
                    if (PcPregWithMarblesKids())
                    {
                        OutputText("Scenes.Marblescene.MarbleCuddlin.PetsYourBelly".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCuddlin.HandsOnEachBelly".Localized());
                    }
                }
            }
        }

        // Haxe AtNightAskMarbleForSomeSexMaybe()   outputText 4 
        public void AtNightAskMarbleForSomeSexMaybe()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.TurnToFaceMarble".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 0)
            {
                OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.AnnoyedBackOfHead".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 20)
                {
                    OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.FoolAroundRefused".Localized());
                    MarbleCuddlin();
                }
                else
                {
                    OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.FoolAroundAccepted".Localized());
                    MarbleNightSexIntro(false);
                    return ;
                }
            }
            Go<Camp>();
        }

        // Haxe MarbleNightLovemaking()   outputText 6 
        public void MarbleNightLovemaking()
        {

            OutputText("Scenes.Marblescene.MarbleNightLovemaking.HintAtIntimacy".Localized());
            if (MarblePregWithPCKids())
            {
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.PatsPregnantBelly".Localized());
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.CradleHerBelly".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.StillHasNeeds".Localized());
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.NoPentUpNeeds".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleNightLovemaking.UsherHerInside".Localized());
            // 原版 var fitting:String = ...（MarbleScene.hx:3972）；emit 把它写成了非法的 if 声明。
            var fitting = Player.HasCock() && Player.CockThatFits(MarbleCuntCapacity()) < 0 ? "fitting " : "";
            AddButton(0, "Massage", MarbleNightMassage, "Help the cow unwind and get off with rubbing and kissing.");
            // 原版 .disableIf(!player.hasCock() || fitting != "", "Requires a " + fitting + "cock.")（MarbleScene.hx:3974）
            if (!Player.HasCock() || fitting != "")
                AddButtonDisabled(1, "Marble Rides", "Requires a " + fitting + "cock.", "Marble Rides");
            else
                AddButton(1, "Marble Rides", MarbleNightRiding, "Let her dominantly take charge while you enjoy yourself.");
            AddButton(2, "Sex", () => MarbleNightSexIntro());
        }

        // Haxe MarbleNightRefusal()   outputText 2 
        public void MarbleNightRefusal()
        {

            OutputText("Scenes.Marblescene.MarbleNightRefusal.DeclineHerOffer".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRefusal.SettleBesideHer".Localized());
            Go<Camp>();
        }

        // Haxe MarbleNightSexIntro()   outputText 19 
        public void MarbleNightSexIntro(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
            {
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.ParagraphBreak".Localized());
            }
            MarbleSprite(true);
            int x = Player.CockThatFits(MarbleCuntCapacity());
            if (x < 0)
            {
                x = 0;
            }
            else
            {
                if (Player.Gender == Gender.Male && Player.CockArea(x) > MarbleCuntCapacity())
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.WishForIntimacy".Localized(
                        Player.CockDescript(x)
                    ));
                    Go<Camp>();
                }
            }
            OutputText("Scenes.Marblescene.MarbleNightSexIntro.WhisperInEar".Localized());
            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.RollOntoBack".Localized());
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.HeadBetweenBreasts".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));
                if (Player.CockTotal() > 0)
                {
                    OutputText(Player.MultiCockDescriptLight());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexIntro.AndYourWord".Localized());
                    }
                }
                if (Player.HasVagina())
                {
                    OutputText(Player.VaginaDescript());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.CaressAndWhisper".Localized());
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.UndressedTurnOver".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.HandInsideClothes".Localized());
                if (Player.CockTotal() > 0)
                {
                    OutputText(Player.MultiCockDescriptLight());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexIntro.AndYourWordAgain".Localized());
                    }
                }
                if (Player.HasVagina())
                {
                    OutputText(Player.VaginaDescript(0));
                }
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.SlipHandInside".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.RubHerThenLower".Localized(
                        MarbleCock()
                    ));
                }
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.FingersDownWomanhood".Localized());
            }
            if (Player.Gender == Gender.Hermaphrodite)
            {
                if (Player.CockArea(x) <= MarbleCuntCapacity())
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.AsksWhichPart".Localized());
                    ClearButtons();
                    AddButton(0, "Dick", MarbleNightSexDudes);
                    AddButton(1, "Vagina", MarbleNightSexChicks);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.TooBigForHer".Localized(
                        Player.CockDescript(x),
                        Player.VaginaDescript(0)
                    ));
                    MarbleNightSexChicks();
                }
            }
            else
            {
                if (Player.Gender == Gender.Male)
                {
                    MarbleNightSexDudes();
                }
                else
                {
                    MarbleNightSexChicks();
                }
            }
        }

        // Haxe MarbleNightMassage()   outputText 5 
        public void MarbleNightMassage()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightMassage.PushHerOntoBed".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassage.HelpHerRelax".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassage.PullTopOff".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassage.MassageNotKiss".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassage.HiddenUnderFat".Localized());
            MarbleNightMassageEnd();
        }

        // Haxe MarbleNightMassageEnd()   outputText 3 
        public void MarbleNightMassageEnd()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.SuddenDeepKiss".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.FingersToNethers".Localized());
            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.ReleaseFromGrip".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Libido, -1f), new DynStat(VanillaStats.Sensitivity, 2f));
            Player.Orgasm("Generic");
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
            Go<Camp>();
        }

        // Haxe MarbleNightRiding()   outputText 10 
        public void MarbleNightRiding()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightRiding.StripShowRequest".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.WontHaveToMove".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.SheWasntLying".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.GaspPainPleasure".Localized(
                MarbleCuntCapacity()
            ));
            OutputText("Scenes.Marblescene.MarbleNightRiding.GrindSlowsDown".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.PressOnNavel".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.ArchesAndBounces".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.GrabsYourShoulders".Localized(
                MarbleCuntCapacity()
            ));
            OutputText("Scenes.Marblescene.MarbleNightRiding.SlamsDownDeep".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRiding.PinchAndGiggle".Localized());
            Player.Orgasm("Dick");
            MarblePreggoChance();
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -1f));
            if (Player.Libido >= 50)
            {
                MarbleNightRidingCont();
            }
            else
            {
                MarbleNightEnd();
            }
        }

        // Haxe MarbleNightRidingCont()   outputText 11 
        public void MarbleNightRidingCont()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SlowHipMotion".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.CaughtOffGuard".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.LovesHowYouNurse".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.MuffledResponse".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SighsAndStrokes".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.LoudSquelching".Localized(
                MarbleCuntCapacity()
            ));
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.DivineMovements".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.TeatPulledOut".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SuckTeatDeep".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.PantsAndMoans".Localized());
            OutputText("Scenes.Marblescene.MarbleNightRidingCont.RockAndShudder".Localized());
            Player.Orgasm("Dick");
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            HighLibidoTryst = true;
            Go<Camp>();
        }

        // Haxe MarbleNightLustEnd()   outputText 2 
        public void MarbleNightLustEnd()
        {
            ClearOutput();

            HighLibidoTryst = false;
            OutputText("Scenes.Marblescene.MarbleNightLustEnd.SoreButContent".Localized());
            OutputText("Scenes.Marblescene.MarbleNightLustEnd.GoodMorningKiss".Localized());
            Go<Camp>();
        }

        // Haxe MarbleNightEnd()   outputText 13 
        public void MarbleNightEnd()
        {
            ClearOutput();

            if (!MarblePregWithPCKids() && !PcPregWithMarblesKids())
            {
                OutputText("Scenes.Marblescene.MarbleNightEnd.RestAndRecover".Localized());
                OutputText("Scenes.Marblescene.MarbleNightEnd.NodInSilence".Localized());
                OutputText("Scenes.Marblescene.MarbleNightEnd.PlayfulWink".Localized());
                OutputText("Scenes.Marblescene.MarbleNightEnd.CurlUpTogether".Localized());
                OutputText("Scenes.Marblescene.MarbleNightEnd.DriftIntoSleep".Localized(
                    MarbleCuntCapacity()
                ));
            }
            else
            {
                if (MarblePregWithPCKids())
                {
                    OutputText("Scenes.Marblescene.MarbleNightEnd.RubsSwollenBelly".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.CannotBeUpset".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.CheckTheCalf".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.PullUpPillows".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightEnd.ListensToBaby".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.AsksWhatSheHears".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.RelaxedHeartbeat".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightEnd.PullUpBlankets".Localized());
                }
            }
            Go<Camp>();
        }

        // Haxe MarbleNightSexDudes()   outputText 21 
        public void MarbleNightSexDudes()
        {
            ClearOutput();

            int x = Player.CockThatFits(MarbleCuntCapacity());
            if (x < 0)
            {
                x = 0;
            }
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            OutputText("Scenes.Marblescene.MarbleNightSexDudes.SlideTipInside".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[x].Length > 8)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.EndOfTheLine".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.FullyEnveloped".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleNightSexDudes.EyesLocked".Localized());
            if (Player.Corruption + Player.Libido < Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) + 10)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.SheTakesControl".Localized());
                if (Player.Tallness < 66)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.BackIntoBosom".Localized());
                }
                else
                {
                    if (Player.Tallness < 84)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexDudes.AgainstHerPillows".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                    else
                    {
                        if (Player.BiggestBreastSize() >= 4)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSexDudes.LandsOnHerFace".Localized(
                                Appearance.BiggestBreastSizeDescript(Player)
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSexDudes.FlattensHerBreasts".Localized());
                        }
                    }
                }
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.GrinsAtHerFrown".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.DeepFastThrusts".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.ToysWithNipples".Localized(
                        Appearance.BiggestBreastSizeDescript(Player),
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.TooMuchToBear".Localized(
                        Player.CockDescript(x)
                    ));
                }
            }
            OutputText("Scenes.Marblescene.MarbleNightSexDudes.SheClenchesTight".Localized());
            if (Player.CumQ() < 250)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.CriesOutInJoy".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.FilledToBursting".Localized());
            }
            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.OfCourseWord".Localized());
                if (Player.Cocks.Count > 2)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.RemainingCocks".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.OtherToolWord".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.UnleashesOwnLoad".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.TrappedBetweenBodies".Localized(
                    MarbleCock()
                ));
            }
            MarblePreggoChance();
            MarbleSexFinish();
        }

        // Haxe MarbleNightSexChicks()   outputText 33 
        public void MarbleNightSexChicks()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) == 0)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.LiftOntoDoubleDildo".Localized(
                    Player.VaginaDescript(0)
                ));
                Player.CuntChange(14, true);
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.AdjustToShaftFeeling".Localized());
                if (Player.Corruption + Player.Libido < Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) + 10)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinAndLiftHips".Localized(
                        Player.VaginaDescript(0)
                    ));
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.SlowDownOffer".Localized());
                    if (Player.BiggestBreastSize() >= 4)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.HandsOffPaceYourself".Localized(
                            Appearance.BiggestBreastSizeDescript(Player),
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.TeaseChestBegin".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BounceMoanOnDildo".Localized());
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GaspAtBreastPlay".Localized());
                    if (Player.BiggestBreastSize() >= 4)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.FlickChestInRhythm".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.GripHandsToChest".Localized(
                            MarbleNips()
                        ));
                    }
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.RapidPistonNearEdge".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.MutualOrgasmClench".Localized(
                    Player.VaginaDescript(0)
                ));
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.YourLoadOnChest".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.ShudderThenCollapse".Localized());
            }
            else
            {
                Player.SlimeFeed();
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.GuideTipAndLower".Localized(
                    Player.VaginaDescript(0),
                    MarbleCock(),
                    Player.VaginaDescript(0),
                    MarbleCock()
                ));
                if (Player.VaginalCapacity() > CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) * CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness))
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.HiltedShudderPraise".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BottomOutSadAsk".Localized(
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinFitExtraTease".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.AdjustToHerWithin".Localized());
                if (Player.Corruption + Player.Libido < Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) + 10)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinLiftOnMember".Localized(
                        Player.VaginaDescript(0),
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BreathCatchOffer".Localized());
                    if (Player.BiggestBreastSize() >= 4)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.LetYouSetPace".Localized(
                            Appearance.BiggestBreastSizeDescript(Player),
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.TeaseNippleBegin".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.MoanAndRideHer".Localized(
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GaspThenBreastPlay".Localized());
                    if (Player.BiggestBreastSize() >= 4)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.ToyAtMatchingPace".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.PullHandsToChest".Localized(
                            MarbleNips()
                        ));
                    }
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.PistonHerToEdge".Localized(
                        MarbleCock()
                    ));
                }
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.SeedFillingOrgasm".Localized(
                    Player.VaginaDescript(0),
                    MarbleCock()
                ));
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.SameTimePrefix".Localized());
                    if (Player.Cocks.Count > 1)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.EachOfPrefix".Localized());
                    }
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.LoadSpraysOntoChest".Localized());
                }
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.CollapseAfterShudder".Localized());
                Player.KnockUp(VanillaPregnancy.Marble, MarbleIncubation, 150);
                Player.CuntChange(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness) * CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength), true);
            }
            MarbleSexFinish();
        }

        // Haxe MarblePreggoChance()   outputText 0 
        public void MarblePreggoChance()
        {

            if (MarbleKnockUpChance() && (Player.HasPerk(VanillaPerks.MarblesMilk) || CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5))
            {
                // 原版 pregnancy.knockUp(PregnancyStore.PREGNANCY_PLAYER, 720)（MarbleScene.hx:4333）
                MarbleKnockUp(PregnancyPlayerType, 720);
            }
        }

        // Haxe MarbleSexFinish()   outputText 1 
        public void MarbleSexFinish()
        {

            OutputText("Scenes.Marblescene.MarbleSexFinish.RollAsideToSleep".Localized());
            Player.Orgasm("Generic");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -1f));
            Go(SceneRegistry.CreateOrDefault("coc:camp"));
        }

        // Haxe MarbleNomNoms()   outputText 11 
        public void MarbleNomNoms()
        {
            ClearOutput();

            MarbleSprite(true);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                Player.SlimeFeed();
                if (Player.Corruption < 30)
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.ReluctantAgree".Localized(
                        MarbleCock()
                    ));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.TastySuckAppeal".Localized(
                        MarbleCock()
                    ));
                }
                OutputText("Scenes.Marblescene.MarbleNomNoms.SkirtUpErectReveal".Localized(
                    MarbleCock()
                ));
                if (Player.HasStatusEffect(VanillaStatusEffects.SlimeCraving) || Player.Libido + Player.Corruption - Player.Intelligence > 35)
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.MuskyHeadFog".Localized(
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNomNoms.SighAboveHandPlaced".Localized(
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNomNoms.SaltyGaspSwallow".Localized());
                    Player.ApplyStats(new DynStat(VanillaStats.Intelligence, -0.2f), new DynStat(VanillaStats.Libido, -0.2f));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.SteadyYourThoughts".Localized(
                        MarbleCock()
                    ));
                    OutputText("Scenes.Marblescene.MarbleNomNoms.RealizeItWasMarble".Localized(
                        MarbleCock()
                    ));
                }
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNomNoms.SkirtUpWetView".Localized());
                OutputText("Scenes.Marblescene.MarbleNomNoms.HoneyTasteSpur".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleNomNoms.MarbleThanksInvite".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(Random.Shared.Next((int)(10)) + Player.Libido / 10)));
            Go<Camp>();
        }

        // Haxe MarbleDigsDraftsYo()   outputText 7 
        public void MarbleDigsDraftsYo()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedIncubiDraft));
            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.HandBottleSheStudies".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) > 60)
            {
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.SmileCorruptionOffer".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.UncertainDemonRefusal".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.DropPotionGrabCrotch".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) > 50)
            {
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.BigWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.NervousWord".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.SmileHermNotice".Localized());
            ReturnToCamp(1);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickType, 1);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickLength, 7);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickThickness, 2);
        }

        // Haxe MarblepinkEgg()   outputText 0 
        public void MarblepinkEgg()
        {

            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggPink));
            MarblePEggEffects();
        }

        // Haxe MarbleLPinkEgg()   outputText 0 
        public void MarbleLPinkEgg()
        {

            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggLargePink));
            MarblePEggEffects();
        }

        // Haxe MarblePEggEffects()   outputText 4 
        public void MarblePEggEffects()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarblePEggEffects.HandPinkEgg".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) > 50)
            {
                OutputText("Scenes.Marblescene.MarblePEggEffects.CockTiredQuestion".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarblePEggEffects.KeepCockQuestion".Localized());
            }
            OutputText("Scenes.Marblescene.MarblePEggEffects.WatchAbsorbRestore".Localized(
                MarbleCock()
            ));
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickType, 0);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickLength, 0);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleDickThickness, 0);
            ReturnToCamp(1);
        }

        // Haxe MarbleOvulatesLikeMadDawg()   outputText 3 
        public void MarbleOvulatesLikeMadDawg()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleNurseryConstruction) < 100 && Player.Pregnancy.Type == VanillaPregnancy.PlayerFather)
            {
                OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.RefuseElixirNursery".Localized());
                ReturnToCamp(1);
                return ;
            }
            else
            {
                if (Player.IsPregnant())
                {
                    OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.BellyRumbleAdvance".Localized());
                    var newIncubation = Player.Pregnancy.Incubation - (40 + Random.Shared.Next((int)(30)));
                    if (newIncubation <= 0)
                    {
                        newIncubation = 1;
                    }
                    Player.KnockUp(Player.Pregnancy.Type, newIncubation, 100, 1);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.BellyFullEggsSoon".Localized());
                    Player.KnockUp(VanillaPregnancy.OvielixirEggs, VanillaPregnancy.OvielixirEggsIncubation + 46, 100, 1);
                }
            }
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.OviElixir));
            ReturnToCamp(1);
        }

        // Haxe MarbleCuntCapacity()   outputText 0 
        public float MarbleCuntCapacity()
        {

            float size = 36;
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) >= 1)
            {
                size += 10;
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) >= 2)
            {
                size += 20;
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
            {
                size += 14;
            }
            return size;
        }

        // Haxe GiveMurbleProBova()   outputText 2 
        public void GiveMurbleProBova()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) == 0)
            {
                OutputText("Scenes.Marblescene.GiveMurbleProBova.HandProBovaFirst".Localized());
                AddButton(0, "Yes", GiveMarbleTheProBovas4Sho);
                AddButton(1, "No", GiveItem);
            }
            else
            {
                OutputText("Scenes.Marblescene.GiveMurbleProBova.SecondProBovaBottle".Localized());
                AddButton(0, "Yes", GiveMarbleTheProBovas4Sho);
                AddButton(1, "No", GiveItem);
            }
        }

        // Haxe GiveMarbleTheProBovas4Sho()   outputText 6 
        public void GiveMarbleTheProBovas4Sho()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.LaBova));
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) == 0)
            {
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.NodDownAndGasp".Localized());
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.TestNewNipples".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleBovaLevel, 1);
                MarbleAffection += 4;
                MarbleAddiction += 4;
            }
            else
            {
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.DeepBreathLoveVow".Localized());
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.ItchyFurGrowsTaller".Localized());
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.PainHandsOverFace".Localized());
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.ChangesStopGasp".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleBovaLevel, 2);
                MarbleSprite();
            }
            ReturnToCamp(1);
        }

        // Haxe MarbleNip()   outputText 0 
        public string MarbleNip()
        {

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) > 0)
            {
                return "quad-nipple";
            }
            else
            {
                return "nipple";
            }
        }

        // Haxe MarbleAppearance()   outputText 30 
        public void MarbleAppearance()
        {
            ClearOutput();

            ShowImage("monster-marble");
            OutputText("Scenes.Marblescene.MarbleAppearance.MarbleIsAPrefix".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) == 0)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.HeightToken".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) == 1)
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.HeightTallToken".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.HeightTallestToken".Localized());
                }
            }
            OutputText("Scenes.Marblescene.MarbleAppearance.TallCowGirlDescription".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) <= 1)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.NormalHumanFace".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BovineHumanFace".Localized());
            }
            OutputText("Scenes.Marblescene.MarbleAppearance.BrownHairCowEars".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 20)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.SmallHornPair".Localized());
            }
            else
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 50)
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.MediumHornPair".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.LargeHornPair".Localized());
                }
            }
            OutputText("Scenes.Marblescene.MarbleAppearance.WideThighsAndRear".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) <= 1)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.HumanThighFurPrefix".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.OddJointedHoovesClause".Localized());
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 15)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.PillowyBreastPair".Localized(
                    MarbleNip(),
                    MarbleBreastSize()
                ));
            }
            else
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 25)
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.LargeBreastPair".Localized(
                        MarbleNip(),
                        MarbleBreastSize()
                    ));
                }
                else
                {
                    if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 35)
                    {
                        OutputText("Scenes.Marblescene.MarbleAppearance.BasketballBreastPair".Localized(
                            MarbleNip(),
                            MarbleBreastSize()
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleAppearance.BasketballBreastPairLarger".Localized(
                            MarbleNip(),
                            MarbleBreastSize()
                        ));
                    }
                }
            }
            var __sw0_0 = Player.Pregnancy.Event;
            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.GainedSomeWeight".Localized());
            }
            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellySlightSwelling".Localized());
            }
            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyObviousPregnancy".Localized());
            }
            if (__sw0_0 == 5)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyVeryPregnant".Localized());
            }
            if (__sw0_0 == 6)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyExtremeQuiver".Localized());
            }
            if ((CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) < 5 && CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) >= 20) || (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5 && CoC.Instance.Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) >= 4))
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.FidgetNeedsMilking".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.GrownSinceArrival".Localized(
                    MarbleCock(),
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength)),
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness))
                ));
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 50)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.PussySmallClit".Localized());
            }
            else
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) <= 75)
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.CuntMediumClit".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleAppearance.FuckHoleClitParted".Localized());
                }
            }
            if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 0) == 0)
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.VirginBumHole".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleAppearance.BumHoleBetweenCheeks".Localized());
            }
            InteractWithMarbleAtCamp();
        }

        // Haxe GiveMarbleTailjobRelease()   outputText 13 
        public void GiveMarbleTailjobRelease()
        {
            ClearOutput();

            MarbleSprite(true);
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.NotInMoodAlternative".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.GrinTailUpLeg".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TailBetweenThighs".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.MoistureSufficient".Localized(
                NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength))
            ));
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.HerHandsJoinIn".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.CoilsOverErection".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.QuickClimaxAfterward".Localized());
            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.WarmTailStroking".Localized());
            if (Player.IsCorruptEnough(70))
            {
                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.ProwessMarkingProof".Localized());
                if (Player.Body.Get<TonguePart>().Id == VanillaBodyPartTypes.Snake || Player.HasLongTongue())
                {
                    OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TasteBothSecretions".Localized());
                }
                if (Player.HasLongTongue())
                {
                    OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.ShowForMarble".Localized());
                }
                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TeaseAgainQuestion".Localized());
                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.RepeatWouldBeNice".Localized());
                Player.SlimeFeed();
            }
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 20f));
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            Go<Camp>();
        }

        // Haxe MilkMarble()   outputText 15 
        public void MilkMarble()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleMilkedBefore) < 1)
            {
                OutputText("Scenes.Marblescene.MilkMarble.MilkBrightEyeGreeting".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.HarmlessStallVisit".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.HandHoldWarpFarm".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.LoverShowsStall".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.AmorousGuideFurnitureTour".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.CuriousAskProgress".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.ApologizeSuggestMilking".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.PerkUpPullCups".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) < 1)
                {
                    OutputText("Scenes.Marblescene.MilkMarble.TwoWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MilkMarble.EightWord".Localized());
                }
                OutputText("Scenes.Marblescene.MilkMarble.AttachSuctionCups".Localized(
                    MarbleNips()
                ));
                OutputText("Scenes.Marblescene.MilkMarble.AsksWhatsInMind".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleMilkedBefore, 1);
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarble.EscortToStallOffer".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.MarblePleasedAgrees".Localized());
                OutputText("Scenes.Marblescene.MilkMarble.HookedUpSheAsks".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5)
            {
                CoC.Instance.Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours, 0);
            }
            ClearButtons();
            AddButton(0, "Cunnilingus", MilkMarbleCunnilingling);
            if (Player.HasCockThatFits(MarbleCuntCapacity()) && (Player.IsBiped() || Player.HasTailInsteadOfLegs() || Player.IsGoo()))
            {
                AddButton(1, "Fuck Her", MilkMarbleFuckDatCowPussy);
            }
            if (Player.HasVagina() && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0 && (Player.IsBiped() || Player.IsGoo()))
            {
                AddButton(2, "Take Her", MilkMarbleTakeHerDick);
            }
            AddButton(3, "Her on Bar", MilkMarbleOnTheBar);
            AddButton(14, "Leave", () => MilkMarbleNoMilking());
        }

        // Haxe MilkMarbleCunnilingling()   outputText 30 
        public void MilkMarbleCunnilingling()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SuggestLickingWhileMilked".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HugeSmileAgreement".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.ChairSeatLegsSpread".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) < 1)
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SemiHumanWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.InhumanWord".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LegsClearView".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.GetShowOnRoad".Localized());
            if (Player.IsBiped())
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.KneesCrawlForward".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BodyLowInchForward".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AnimalStalkingApproach".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HoofLegArmRest".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BowStrandsGift".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.DeserveItReply".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.UnwrapBowPresent".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.TryItNowInvite".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.NoSecondAsking".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HipsJerkTugging".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LongDrawnOutCry".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.FirstFlickSqueeze".Localized(
                    MarbleCock()
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.FirstFlickChairGrip".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LaughSuggestPause".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BiteMeNowDemand".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.CannotRefuseRequest".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BovineGratefulCry".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SpurtOverLap".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.MilkersAmplifyPleasure".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AskWasItGood".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LovelyThanksNotice".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.RetieBowForLater".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AwkwardPeckFarewell".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
            var pLust = (int)(20 + ((Player.Libido + Player.Corruption) / 10));
            Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(pLust)));
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, 1f));
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleFuckDatCowPussy()   outputText 34 
        public void MilkMarbleFuckDatCowPussy()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WonderAloudIntercourse".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TemptingOfferInspect".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.FlourishArmorReveal".Localized(
                Player.CockDescript(x)
            ));
            if ((Player.HasPerk(VanillaPerks.BulgeArmor) || Player.ModArmorName == "backless female teacher's clothes" || Player.ModArmorName == "bridle bit and saddle set" || Player.ModArmorName == "headdress, necklaces, and many body-chains" || Player.ModArmorName == "bondage patient clothes" || Player.ModArmorName == "crotch-revealing clothes" || Player.ModArmorName == "cute servant's clothes" || Player.ModArmorName == "maid's clothes" || Player.ModArmorName == "servant's clothes") && Player.HasCock())
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.AppreciateShowCloser".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WillWorkInspect".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.StepForwardShowGoods".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SquatCarefulLookover".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SeriousApprovalVerdict".Localized());
            if (Player.Tallness < 68)
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.DeepHugLaughter".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.ReassureDuringHysterics".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SettleDownReturnHug".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.BreakAwayToBench".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.HuskyVoiceAskReady".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.GrinAlwaysReady".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WordsBarelyEngulf".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.HandsPressStomachRock".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MilkSpurtEachThrust".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.StopsMovingWorried".Localized());
            if (Player.HasPerk(VanillaPerks.MarblesMilk))
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SitUpMilkingTakeover".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MachineFailsHandDrain".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.FlowSlowsToTrickle".Localized());
                Player.Orgasm("Generic");
                Player.Orgasm("Generic");
                Player.ApplyStats(new DynStat(VanillaStats.Lust, 20f));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 15);
                Player.ApplyStats(new DynStat(VanillaStats.Libido, .3f));
                if (Player.Corruption < Player.StatusEffectValue(VanillaStatusEffects.Marble, 4))
                {
                    Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
                }
                Player.ApplyStats(new DynStat(VanillaStats.Toughness, .1f));
                Player.ApplyStats(new DynStat(VanillaStats.Strength, .1f));
            }
            else
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 4) > 30)
                {
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TaintedFluidCraving".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SpeakingThenQuiet".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.RubbingNotEnough".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MutualOrgasmCry".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SouthernVoiceInterrupt".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SurprisedStareTense".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MetalSlamWarning".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.LaughReattachCups".Localized());
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 5);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.AssureBucketContinue".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.RollSoftFleshMoans".Localized(
                        MarbleNips()
                    ));
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.GaspOwnHandMilking".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MinutesToOrgasmPeak".Localized());
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TripleClimaxHour".Localized());
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
                }
            }
            Player.Orgasm("Generic");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f));
            MarblePreggoChance();
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleTakeHerDick()   outputText 21 
        public void MilkMarbleTakeHerDick()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.AskPenetrateWhileMilked".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.ThoughtfulTailSwish".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FrownAtSillyLook".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SuggestRailingLean".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.RatherLookEyes".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SilenceStripQuickly".Localized());
            if (Player.Tallness < 56)
            {
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.GrabBoxClause".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.LeanRailingDisplay".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.LickLipsNervousTail".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.StripSkirtTakePosition".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.RetortNotSuggested".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SlowEntryInchStep".Localized());
            if (Player.VaginalCapacity() < CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength) * CoC.Instance.Flags.Get(VanillaFlags.MarbleDickThickness))
            {
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.WaitAdjustEachInch".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FinalInsertTailUp".Localized());
            Player.CuntChange(8, true, true, false);
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.StrangeFeelingThrust".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.PlayBacksideMassage".Localized());
            if (Player.Body.Butt <= 4 && !Player.IsGoo())
            {
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.TautWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.JigglingWord".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FleshLengthPushPull".Localized(
                NumberUtils.Num2Text((float)(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength)))
            ));
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.WildFloodingSeed".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.HourLaterCalmDown".Localized());
            Player.Orgasm("Vaginal");
            Player.Orgasm("Vaginal");
            Player.Orgasm("Vaginal");
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
            Player.ApplyStats(new DynStat(VanillaStats.Libido, .5f));
            Player.KnockUp(VanillaPregnancy.Marble, MarbleIncubation, 150);
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleOnTheBar()   outputText 13 
        public void MilkMarbleOnTheBar()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.SuggestBarPlay".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.ConsiderProposition".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.AssureWorthwhile".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.SeriousCounteroffer".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.WonderingTurnAround".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.GetMeReady".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.LiftSkirtExpose".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PleasedMoanTailLift".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.TeaseEntranceFinger".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PutFingerInside".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PushIndexFinger".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.OfferTailHelp".Localized());
            if (Player.CockThatFits(MarbleCuntCapacity()) > 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.ContemplateTailGrab".Localized());
            }
            ClearButtons();
            AddButton(0, "Follow Tail", MilkMarbleBarFollowTail);
            AddButton(1, "Cunnilingus", MilkMarbleBarCunnilingling);
            if (Player.HasCockThatFits(MarbleCuntCapacity()))
            {
                AddButton(2, "Pull Tail", MilkMarbleBarPullTail);
            }
            AddButton(3, "Leave", MilkMarbleLeaveAfterBar);
        }

        // Haxe MilkMarbleBarFollowTail()   outputText 23 
        public void MilkMarbleBarFollowTail()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());
            var y = (int)(Player.CockThatFits2(MarbleCuntCapacity()));
            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AskTailDestination".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.LowChuckleWait".Localized());
            if (x >= 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailSearchesCrotch".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TauntFoundIt".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ObviousCannotHide".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.FunLooking".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.SlowEnjoyableEntry".Localized());
                if (y >= 0)
                {
                    OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ComfortableCheekSqueeze".Localized(
                        Player.CockDescript(y)
                    ));
                }
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.GuideStringTightens".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ThreefoldSensation".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailPullsHandOut".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AskFoundRightSpot".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.WhatGaveItAway".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.JustHunch".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AdjustToyGrip".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.StuckNeedHand".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailReleasesPressure".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.BeyondPlaything".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.FreelyBuildPleasure".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.BarelyNoticeCall".Localized());
            if (x > 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.RootGripRelaxes".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.DeathGripRelease".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.SternLookTurnaround".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 10);
            if (x >= 0)
            {
                Player.Orgasm("Generic");
                MarblePreggoChance();
            }
            else
            {
                var pLust = (int)(10 + Player.Libido / 10);
                Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(pLust)));
            }
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleBarCunnilingling()   outputText 15 
        public void MilkMarbleBarCunnilingling()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.AskReleaseWrist".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.WetConsentGranted".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.KneelBehindScent".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.GripTailRoot".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.BemusedResponse".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.RingGripTailSlide".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.SurprisedMoan".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.CarefulDabHarvest".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.TailAroundHead".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.RidingOutOrgasm".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.NuzzleSoftBum".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.WillingAfterAll".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.AssureFullEffort".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.TailFlicksOut".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.PauseTeasing".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 15);
            var pLust = (int)(10 + Player.Libido / 10);
            Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(pLust)));
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, 1f));
            View.Time.Hours += 1;
            ReturnToCamp(1);
        }

        // Haxe MilkMarbleBarPullTail()   outputText 37 
        public void MilkMarbleBarPullTail()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GrabTailTakeCharge".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.PainOutburst".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.IgnoreProtestsSmack".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AngryHoofStomp".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TakeChargeWarning".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.StraddleFaceShake".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.MemberCheekRubbing".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.EnjoyingControl".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) == 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuffocatingHipDrop".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TooRoughFaceGentle".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GentleSettleBreathing".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LickDemand".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LickApproved".Localized());
                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                {
                    OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LongTongueFill".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TongueDeepWriggle".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ForceMouthCockShove".Localized(
                    NumberUtils.Num2Text((float)(CoC.Instance.Flags.Get(VanillaFlags.MarbleDickLength)))
                ));
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TooRoughMouthGentle".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GentleThrustAdjustment".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuckDemand".Localized());
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LipsWrappedApproval".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.HipsLiftedInstruction".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SootheHurtFur".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ApologyExperiment".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TongueSweatyFur".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.DidYouLikeIt".Localized());
            if (Silly)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.BrushTeethTail".Localized());
            }
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.NodsFeelingBetter".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AskHerPlan".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.RepositionOverWaist".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AlreadyDecided".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.HipShakeSpin".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuddenEntrancePush".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.QuiveringQuestion".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.UnprecedentedAggression".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.UproariousRiding".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ExhaustedGasps".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.BosomSnuggle".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 20f));
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 15);
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f));
            MarblePreggoChance();
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleLeaveAfterBar()   outputText 2 
        public void MilkMarbleLeaveAfterBar()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleLeaveAfterBar.SilentFingerOrgasm".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleLeaveAfterBar.MildDisappointment".Localized());
            var pLust = (int)(5 + Player.Libido / 20);
            Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(pLust)));
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) + 5);
            ReturnToCamp(2);
        }

        // Haxe MilkMarbleNoMilking()   outputText 2 
        public void MilkMarbleNoMilking()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleNoMilking.DeclinePolitely".Localized());
            OutputText("Scenes.Marblescene.MilkMarbleNoMilking.FarewellChairSeat".Localized());
            ReturnToCamp(2);
        }
    }
}
