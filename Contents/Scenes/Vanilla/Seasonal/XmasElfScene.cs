using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<XmasElfScene>("coc:xmas_elf")]
    public class XmasElfScene : Scene
    {

        public override void Entry()
        {
            XmasBitchEncounter();
        }

        public void XmasBitchEncounter()
        {
            ClearOutput();

            View.Time.PassMinutes((int)Math.Round((double)((Random.Shared.Next(3)) * 60)));

            var first = CoC.Instance.Flags.Get(GameFlagIds.PcEncounteredChristmasElfBefore) == 0;

            OutputText("Scenes.Xmaself.XmasBitchEncounter.SleepDisturbed".Localized());

            OutputText("Scenes.Xmaself.XmasBitchEncounter.StandsInnocent".Localized());

            if (first)
                OutputText("Scenes.Xmaself.XmasBitchEncounter.IsAnElf".Localized());
            else
                OutputText("Scenes.Xmaself.XmasBitchEncounter.SameElf".Localized());

            OutputText("Scenes.Xmaself.XmasBitchEncounter.BroughtYou".Localized());

            if (!first)
            {
                OutputText("Scenes.Xmaself.XmasBitchEncounter.ConfusedReappearance".Localized());

                OutputText("Scenes.Xmaself.XmasBitchEncounter.GigglesSilly".Localized());
            }
            else
            {
                OutputText("Scenes.Xmaself.XmasBitchEncounter.ConfusedAppearance".Localized());

                OutputText("Scenes.Xmaself.XmasBitchEncounter.GigglesSillyAgain".Localized());
            }

            OutputText("Scenes.Xmaself.XmasBitchEncounter.PresentIsYours".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Openpresent".Localized(), OpenXmasPresent);

            AddButton(1, "Common.Button.UnwrapElf".Localized(), null);

            AddButton(4, "Scenes.Lumberjack.Button.Decline".Localized(), DeclineXmasPresent);

            if (!Player.IsPureEnough(90) || SceneInstance<JojoScene>.Get().IsJojoCorrupted() || Player.HasStatusEffect(StatusEffectIds.Exgartuan) || SceneInstance<AmilyScene>.Get().AmilyCorrupt() || CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) > 0 || CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 || CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) > 0)
            {
                OutputText("Scenes.Xmaself.XmasBitchEncounter.NodsBouncing".Localized());

                if (!first)
                    OutputText("Scenes.Xmaself.XmasBitchEncounter.HereWeGoAgain".Localized());

                AddButton(1, "Common.Button.UnwrapElf".Localized(), null);

                return ;
            }

            if (Player.IsGenderless())
            {
                OutputText("Scenes.Xmaself.XmasBitchEncounter.NodsBouncingAgain".Localized());

                AddButton(1, "Common.Button.UnwrapElf".Localized(), null);

                return ;
            }

            OutputText("Scenes.Xmaself.XmasBitchEncounter.NodsBouncingThird".Localized());

            AddButton(1, "Common.Button.UnwrapElf".Localized(), UnwrapElfyPresent);
        }

        public void DeclineXmasPresent()
        {
            ClearOutput();

            OutputText("Scenes.Xmaself.DeclineXmasPresent.ShakesHeadNo".Localized());

            OutputText("Scenes.Xmaself.DeclineXmasPresent.BeforeReact".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);

            AwardAchievement(AchievementIds.HolidayChristmasI);

            Go<CampScene>();
        }

        public void OpenXmasPresent()
        {
            ClearOutput();

            OutputText("Scenes.Xmaself.OpenXmasPresent.RipsRibbons".Localized());

            if (!Player.IsPureEnough(90) || SceneInstance<JojoScene>.Get().IsJojoCorrupted() || Player.HasStatusEffect(StatusEffectIds.Exgartuan) || SceneInstance<AmilyScene>.Get().AmilyCorrupt() || CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) > 0 || CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 || CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) > 0)
            {
                OutputText("Scenes.Xmaself.OpenXmasPresent.ShockContents".Localized());

                OutputText("Scenes.Xmaself.OpenXmasPresent.BeforeUtter".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Xmaself.OpenXmasPresent.EmptyPresent".Localized());

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Coal), 1);

                CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
            }
            else
                if (Player.Corruption <= 33)
                {
                    OutputText("Scenes.Xmaself.OpenXmasPresent.SurpriseContents".Localized());

                    if (Player.Intelligence > Player.Strength)
                    {
                        Player.Inventory.Add(ItemRegistry.Create(ArmorIds.SS_ROBE), 1);

                        PlayerMenu();
                    }
                    else
                    {
                        Player.Inventory.Add(ItemRegistry.Create(ArmorIds.SilkArmor), 1);

                        PlayerMenu();
                    }

                    CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
                }
                else
                    if (Player.Corruption < 60)
                    {
                        OutputText("Scenes.Xmaself.OpenXmasPresent.SurpriseContentsAgain".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ItemIds.GroPlus), 1);

                        PlayerMenu();

                        CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
                    }
                    else
                    {
                        OutputText("Scenes.Xmaself.OpenXmasPresent.SurpriseContentsThird".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight), 1);

                        CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
                    }

            AwardAchievement(AchievementIds.HolidayChristmasI);
        }

        public void UnwrapElfyPresent()
        {
            ClearOutput();

            OutputText("Scenes.Xmaself.UnwrapElfyPresent.TossesPresent".Localized());

            if (Player.Tallness > 60)
                OutputText("Scenes.Xmaself.UnwrapElfyPresent.PressesAgainst".Localized());
            else
                OutputText("Scenes.Xmaself.UnwrapElfyPresent.CuddlesUp".Localized());

            OutputText("Scenes.Xmaself.UnwrapElfyPresent.GettingMore".Localized());

            OutputText("Scenes.Xmaself.UnwrapElfyPresent.YouPrefix".Localized());

            if (Player.Speed < 25)
                OutputText("Scenes.Xmaself.UnwrapElfyPresent.ClumsilyWord".Localized());
            else
                OutputText("Scenes.Xmaself.UnwrapElfyPresent.EasilyWord".Localized());

            OutputText("Scenes.Xmaself.UnwrapElfyPresent.ReachAround".Localized());

            ClearButtons();

            if (Player.IsHerm())
            {
                AddButton(0, "Common.Button.FuckElf".Localized(), DickXmasElfGo);

                AddButton(1, "Common.Button.DildoFun".Localized(), VagFuckXmasElf);
            }
            else
                if (Player.HasCock())
                    DickXmasElfGo();
                else
                    VagFuckXmasElf();
        }

        public void DickXmasElfGo()
        {
            ClearOutput();

            if (Player.CockArea(0) < 80)
            {
                OutputText("Scenes.Xmaself.DickXmasElfGo.PushesDown".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.MountsWithoutPretense".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.GruntPistoning".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.AbsolutelyRight".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.SpunkFillsWomb".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.SplatterEverywhere".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.ParagraphBreak".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.OrgasmEnds".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.AsideFromCock".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.EffortHoldingOut".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.TooHot".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.DumpingWaves".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.OrgasmLasts".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.SnapsOutOfIt".Localized(
                    Player.CockDescript(0)
                ));

                OutputText("Scenes.Xmaself.DickXmasElfGo.FucksRelentlessly".Localized());
            }
            else
            {
                OutputText("Scenes.Xmaself.DickXmasElfGo.PushesDownAgain".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.GrabsAhold".Localized(
                    Player.CockHead()
                ));

                OutputText("Scenes.Xmaself.DickXmasElfGo.WrapsArms".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.MakingJiggle".Localized());
                else
                    OutputText("Scenes.Xmaself.DickXmasElfGo.TightButt".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.HumsHappily".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.LooksOverShoulder".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.BeforePuzzle".Localized(
                    Player.CockHead()
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.BallsWord".Localized());
                else
                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Xmaself.DickXmasElfGo.SheathWord".Localized());
                    else
                        OutputText("Scenes.Xmaself.DickXmasElfGo.BodyWord".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.BlowsAndBlows".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.PullsBack".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.RBallsSuffix".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.GentlePressure".Localized(
                    Player.CockHead()
                ));

                OutputText("Scenes.Xmaself.DickXmasElfGo.PresentAsks".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.FlipsAround".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Xmaself.DickXmasElfGo.RecoilShooting".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.HugsSqueezes".Localized());

                OutputText("Scenes.Xmaself.DickXmasElfGo.ThickRopes".Localized());
            }

            XmasFuckFollowup();

            Player.Orgasm("Dick", true, 20);
        }

        public void VagFuckXmasElf()
        {
            ClearOutput();

            OutputText("Scenes.Xmaself.VagFuckXmasElf.PushesDown".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.WinksSpreads".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.HasHerHands".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Xmaself.VagFuckXmasElf.RubsTip".Localized(
                Player.VaginaDescript(0)
            ));

            Player.CuntChange(15, true, true, false);

            OutputText("Scenes.Xmaself.VagFuckXmasElf.ParagraphBreak".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.MerryChristmas".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Xmaself.VagFuckXmasElf.DeterminedGetOff".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.YouCum".Localized());

            if (Player.Vaginas[0].Wetness == 5)
                OutputText("Scenes.Xmaself.VagFuckXmasElf.GushingFluids".Localized());
            else
                if (Player.Vaginas[0].Wetness >= 3)
                    OutputText("Scenes.Xmaself.VagFuckXmasElf.DrippingFluids".Localized());
                else
                    OutputText("Scenes.Xmaself.VagFuckXmasElf.SquishingWetly".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.DrilledByHer".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Xmaself.VagFuckXmasElf.WontLetYou".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.ThrowsHeadBack".Localized());

            OutputText("Scenes.Xmaself.VagFuckXmasElf.BlacksOut".Localized());

            XmasFuckFollowupFems();

            Player.Orgasm("Vaginal", true, 10);
        }

        public void XmasFuckFollowup()
        {
            ClearOutput();

            HideUpDown();

            OutputText("Scenes.Xmaself.XmasFuckFollowup.AwakensSore".Localized());

            OutputText("Scenes.Xmaself.XmasFuckFollowup.DidYouEnjoy".Localized());

            if (Player.HasPerk(PerkIds.ElvenBounty))
                OutputText("Scenes.Xmaself.XmasFuckFollowup.AgainBanner".Localized());

            OutputText("Scenes.Xmaself.XmasFuckFollowup.QuestionMark".Localized());

            AwardAchievement(AchievementIds.HolidayChristmasI);

            AddButton(0, "Common.Button.Yes".Localized(), () => XmasPerkM(true));

            AddButton(1, "Common.Button.No".Localized(), XmasSmart);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            if (Player.Sensitivity + 20 > 80)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));
            else
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 15f));

            CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
        }

        public void XmasFuckFollowupFems()
        {
            ClearOutput();

            HideUpDown();

            View.Time.PassMinutes((int)Math.Round((double)((6 - View.Time.Hours) * 60)));

            OutputText("Scenes.Xmaself.XmasFuckFollowupFems.AwakensSore".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Xmaself.XmasFuckFollowupFems.MightHaveDone".Localized());

            if (Player.HasPerk(PerkIds.ElvenBounty))
                OutputText("Scenes.Xmaself.XmasFuckFollowupFems.AgainBanner".Localized());

            OutputText("Scenes.Xmaself.XmasFuckFollowupFems.QuestionMark".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => XmasPerkM(false));

            AddButton(1, "Common.Button.No".Localized(), XmasSmart);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            if (Player.Sensitivity + 20 > 80)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));
            else
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 15f));

            CoC.Instance.Flags.Set(GameFlagIds.PcEncounteredChristmasElfBefore, DateTime.Now.Year);
        }

        public void XmasPerkM(bool dicked)
        {
            ClearOutput();

            if (!Player.HasPerk(PerkIds.ElvenBounty))
            {
                if (dicked)
                    Player.CreatePerk(PerkIds.ElvenBounty, 250, 0, 0, 0);
                else
                    Player.CreatePerk(PerkIds.ElvenBounty, 0, 15, 0, 0);

                OutputText("Scenes.Xmaself.XmasPerkM.NewPerkBounty".Localized());
            }
            else
            {
                OutputText("Scenes.Xmaself.XmasPerkM.PerkEnhancedBounty".Localized());

                if (dicked)
                {
                    Player.AddPerkValue(PerkIds.ElvenBounty, 1, 250);

                    OutputText("Scenes.Xmaself.XmasPerkM.BonusCumProduction".Localized());
                }
                else
                {
                    Player.AddPerkValue(PerkIds.ElvenBounty, 2, 15);

                    OutputText("Scenes.Xmaself.XmasPerkM.BonusFertility".Localized());
                }
            }

            View.Time.LastPassedHours = 0;

            Go<CampScene>();
        }

        public void XmasSmart()
        {

            HideUpDown();

            OutputText("Scenes.Xmaself.XmasSmart.NodsFeelingSmart".Localized());

            View.Time.LastPassedHours = 0;

            Go<CampScene>();

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 15f));
        }

    }
}