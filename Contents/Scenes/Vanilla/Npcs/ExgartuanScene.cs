using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<ExgartuanScene>("coc:exgartuan")]
    public class ExgartuanScene : Scene
    {

        public override void Entry()
        {
            FountainEncounter();
        }

        public void FountainEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RoamingShiftingSands".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PlacardFountainEndowment".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DrinkFromFountain".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DrinkFountainEndowment);

            AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
        }

        public void DrinkFountainEndowment()
        {
            ClearOutput();

            int temp = 0;

            var changed = false;

            Player.SlimeFeed();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CupHandsDrink".Localized());

            if (Random.Shared.Next(5) == 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SenseOfWisdom".Localized());

                Player.Experience += 200;

                changed = true;
            }

            if (!Player.HasStatusEffect(StatusEffectIds.Exgartuan) && !changed && Random.Shared.Next(2) == 0)
            {
                float choices = 0;

                if ((Player.CockTotal() > 0) && (Player.CockArea(0) >= 100))
                        choices += 1;

                if (Player.BiggestBreastSize() >= 12)
                    choices += 1;

                if (choices > 0)
                {
                    if (choices > 1)
                        if (Random.Shared.Next(2) == 0)
                            ExgartuanInfestDick();
                        else
                            ExgartuanInfestTits();

                    if (choices == 1)
                        if (Player.BiggestBreastSize() >= 12)
                            ExgartuanInfestTits();
                        else
                            ExgartuanInfestDick();

                    changed = true;
                }
            }

            if (Random.Shared.Next(3) == 0 && Player.BiggestBreastSize() > 1)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BalloonsGrowFourSizes".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                Player.NippleLength += .3f;

                temp = Player.BreastRows.Count;

                while (temp > 0)
                {
                    temp--;

                    Player.BreastRows[temp].BreastRating += 4;
                }

                changed = true;
            }

            if (Random.Shared.Next(3) == 0 && Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CocksTighterInArmor".Localized());

                temp = Player.Cocks.Count;

                while (temp > 0)
                {
                    temp--;

                    Player.IncreaseCock(temp, 3);

                    Player.Cocks[temp].ThickenCock(0.3f);
                }

                changed = true;
            }

            if (Random.Shared.Next(4) == 0 && Player.HasVagina())
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ClitPlumpsUp".Localized());

                Player.Vaginas[0].ClitLength += 2;

                if (Player.GetClitLength() < 6)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BigAsACock".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BiggerThanMostCocks".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NowExclamation".Localized());

                changed = true;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void ExgartuanInfestDick()
        {

            if (Player.Cocks[0].Id != BodyPartTypes.Demon)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockPuffsNodules".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.VoiceSplitsAirSatiate".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HellWasThat".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LookDownDemonicCock".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ItSuggestsHost".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CertainlyUnexpected".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, 10f));

                Player.Cocks[0].Id = BodyPartTypes.Demon;
            }
            else
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockPuffsEvenLarger".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.VoiceSplitsAirTouch".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HellWasThatAgain".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DemonicCockAgain".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ItSuggestsHostAgain".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CertainlyUnexpectedAgain".Localized());
            }

            Player.IncreaseCock(0, 1);

            Player.Cocks[0].ThickenCock(0.5f);

            Player.CreateStatusEffect(StatusEffectIds.Exgartuan, 1, 0, 0, 0);
        }

        public void ExgartuanInfestTits()
        {

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoundsGrowMuchLarger".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            var temp = Player.BreastRows.Count;

            while (temp > 0)
            {
                temp--;

                Player.BreastRows[temp].BreastRating += 7;
            }

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.VoiceSplitsAir".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LookAboutConfusion".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OhForFucksSake".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GroundHitsYouFall".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SpurtsMilk".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CertainlyUnexpectedThird".Localized());

            Player.CreateStatusEffect(StatusEffectIds.Exgartuan, 2, 0, 0, 0);
        }

        public void ExgartuanMasturbation()
        {
            ClearOutput();

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
            {
                if (Player.Corruption < 15)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SheepishlyFindRocks".Localized());

                if (Player.Corruption >= 15 && Player.Corruption < 30)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MakeSureAlone".Localized());

                if (Player.Corruption >= 30 && Player.Corruption < 60)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HappilyRemoveArmor".Localized());

                if (Player.Corruption >= 60 && Player.Corruption < 80)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StripExaggerated".Localized());

                if (Player.Corruption >= 80)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StripFondlingBits".Localized());

                if (Player.Corruption < 33)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TooTiredToResist".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SighSqueezeGlobs".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 0)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TentacleCurlsCockRing".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DemonVoiceTeases".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FeebleProtest".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AlreadyGivingIn".Localized());

                    if (Player.Balls > 0)
                        OutputText(Player.BallsDescriptLight());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LoinsWord".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AchingToSquirt".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoUseFighting".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NowSatedDeflates".Localized());
                }
                else
                    if (Player.Corruption < 66)
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.UnableHandleArtificialLust".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SqueezeFewDollops".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DemonSpeaksTeasing".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WordsRingTrue".Localized());

                        if (Player.BiggestBreastSize() >= 5)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShoveBetweenBreasts".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WorkShaftRelentlessly".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PantMoanForced".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LookAtYourselfTaunt".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OrgasmSpreadsDemon".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EldersCouldSeeYou".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TemporarilySatedDeflates".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HappilyGivingIn".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NeverContentIdly".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HesSoRight".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockTakenWithIdea".Localized(
                            Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SoooGoodAtThis".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OhGodsHesRight".Localized());

                        if (Player.BiggestBreastSize() >= 5)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsSmashTitfuck".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsRepositionCock".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LeanDownAutoFellatio".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ExplodeFillMouth".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TemporarilySatedDeflatesAgain".Localized());
                    }

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, .25f), new DynStat(GameStats.Corruption, 1f));
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2)
                {
                    if (Player.Corruption < 50)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShrugOffTopPreparing".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShrugOffTopEagerly".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsRiseUnbidden".Localized());

                    if (Player.HasPlainSkin())
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FleshWord".Localized());
                    else
                        if (Player.HasFur())
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FurryCoveredFlesh".Localized());
                        else
                            if (Player.HasScales())
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SoftScalyFlesh".Localized());
                            else
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GooeySurface".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PausingSqueezeCaress".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.VoiceTauntsNipplePlay".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FingersWrapNipples".Localized());

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JuiceWord".Localized());

                        if (Player.Cocks.Count > 0)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AndPreCum".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SoaksYourGroin".Localized());
                    }
                    else
                        if (Player.Cocks.Count > 0)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PreCumSoaksGroin".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WarmthRadiates".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoreTurnedOn".Localized());

                    if (Player.NippleLength < 2)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PinchPullTooMuch".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StrokeElongatedNipples".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShakeWrigglePleasure".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, .25f), new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, 1f));

                    if (Player.BiggestLactation() > 1)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CalmDownMilkDribbling".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BlushRedressSilent".Localized());

                    Player.Orgasm("Tits", false);
                }

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, (12 + Random.Shared.Next(7)));

            DoNext(() => ReturnToCamp(1));
        }

        public void ExgartuanBored()
        {

            float select = 0;

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.CockArea(0) >= 100)
            {
                select = Random.Shared.Next(9);

                if (select == 0)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MuffledVoicePipesUp".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InsideQuestion".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InYourBalls".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BetterRelieveMeSoon".Localized());
                }
                else
                    if (select == 1)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TinyVoiceMutters".Localized());
                    else
                        if (select == 2)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StirringInsideArmor".Localized());
                        else
                            if (select == 3)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ExgartuanOrders".Localized());
                            else
                                if (select == 4)
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockExpandContract".Localized());
                                else
                                    if (select == 5)
                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorBulgesStifled".Localized());
                                    else
                                        if (select == 6)
                                        {
                                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HarderToConcentrate".Localized());

                                            if (Player.Balls > 0)
                                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TeasingBallsack".Localized());
                                            else
                                                if (Player.HasVagina())
                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PokingLabia".Localized());
                                                else
                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StrokingPerineum".Localized());

                                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ComeOnChampion".Localized());
                                        }
                                        else
                                            if (select == 7)
                                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PrickShufflesSide".Localized());
                                            else
                                                if (select == 8)
                                                {
                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockRubsThighs".Localized());

                                                    if (Player.Balls > 0)
                                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BoilingInYourBalls".Localized());
                                                    else
                                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CookingDeepWithin".Localized());

                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SomewhereToDeliver".Localized());
                                                }
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2 && Player.BiggestBreastSize() >= 12)
                {
                    select = Random.Shared.Next(8);

                    switch (select)
                    {
                        case 0:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OminousJigglingBreasts".Localized());
                        }

                        break;

                        case 1:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FewHoursPeaceStir".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                        }

                        break;

                        case 2:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WakesWobblePleasantly".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.YouSigh".Localized());
                        }

                        break;

                        case 3:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestForceOff".Localized());
                        }

                        break;

                        case 4:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmsHugChest".Localized());
                        }

                        break;

                        case 5:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestFidgetMuffled".Localized());
                        }

                        break;

                        case 6:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SilentAloneTimeEnds".Localized());
                        }

                        break;

                        case 7:
                        {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MenacingJiggle".Localized());
                        }

                        break;
                    }
                }
        }

        public bool ExgartuanCombatUpdate()
        {

            if (CombatController.Instance!.Context.Enemy.ShortName != "tentacle beast" || CombatController.Instance!.Context.Enemy.ShortName != "worms" || CombatController.Instance!.Context.Enemy.ShortName != "demons")
                return false;

            float select = 0;

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
            {
                if (CombatController.Instance!.Context.Enemy.ShortName != "coc:goblin" && Random.Shared.Next(3) == 0)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HarmonicVoiceGibberish".Localized());

                    CombatController.Instance!.Context.Enemy.AddStatusValue(StatusEffectIds.BonusVCapacity, 1, Player.CockArea(0) * .1f);

                    CombatController.Instance!.Context.Enemy.Lust += 10;

                    return true;
                }
                else
                    if (CombatController.Instance!.Context.Enemy.ShortName != "fetish cultist")
                    {
                        select = Random.Shared.Next(3);

                        switch (select)
                        {
                            case 0:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CultistEyesFixate".Localized());
                            }

                            break;

                            case 1:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CultistTouchingHerself".Localized());
                            }

                            break;

                            case 2:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CultistFixatedLump".Localized());
                            }

                            break;
                        }

                        CombatController.Instance!.Context.Enemy.Lust += 10;

                        return true;
                    }
                    else
                        if (CombatController.Instance!.Context.Enemy.HasVagina())
                        {
                            select = Random.Shared.Next(10);

                            switch (select)
                            {
                                case 0:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThoughtsWrappingAround".Localized(
                                    CombatController.Instance!.Context.Enemy.Mf("him", "her")
                                ));

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlanceDownSensitiveBulge".Localized());
                                }

                                break;

                                case 1:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DesireCourses".Localized(
                                    CombatController.Instance!.Context.Enemy.Mf("him", "her")
                                ));
                                }

                                break;

                                case 2:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WarmthSpreadsVisions".Localized());

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThatsTerribleDemonFault".Localized());
                                }

                                break;

                                case 3:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WantToEndFight".Localized());

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SnapBackCurse".Localized());
                                }

                                break;

                                case 4:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FightGoneOnLong".Localized());
                                }

                                break;

                                case 5:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MomentHardConcentrate".Localized());
                                }

                                break;

                                case 6:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PleasureRadiatesBeing".Localized());
                                }

                                break;

                                case 7:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MaybeLayDownArms".Localized());
                                }

                                break;

                                case 8:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TightAssholesGapingVaginas".Localized());
                                }

                                break;

                                case 9:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.IncreasinglyDifficult".Localized());
                                }

                                break;
                            }

                            Player.ApplyStats(new DynStat(GameStats.Lust, 4 + Random.Shared.Next(5)));

                            return true;
                        }
                        else
                            if (CombatController.Instance!.Context.Enemy.Cocks.Count > 0)
                            {
                                select = Random.Shared.Next(8);

                                switch (select)
                                {
                                    case 0:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoControlHandsUndo".Localized());
                                    }

                                    break;

                                    case 1:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HeedlessBothHands".Localized());
                                    }

                                    break;

                                    case 2:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockSquirmsOut".Localized());
                                    }

                                    break;

                                    case 3:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WaistExpandsErect".Localized(
                                        NumberUtils.Num2Text(Player.Cocks[0].Length)
                                    ));
                                    }

                                    break;

                                    case 4:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DisobeyingHandsUndo".Localized());
                                    }

                                    break;

                                    case 5:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImpossibleTent".Localized());
                                    }

                                    break;

                                    case 6:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorOpensItself".Localized());
                                    }

                                    break;

                                    case 7:
                                    {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorSplaysOpen".Localized());
                                    }

                                    break;
                                }

                                CombatController.Instance!.Context.Enemy.Lust -= 5 + Random.Shared.Next(10);

                                return true;
                            }
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2)
                    if (CombatController.Instance!.Context.Enemy.Cocks.Count > 0)
                    {
                        select = Random.Shared.Next(8);

                        switch (select)
                        {
                            case 0:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FlirtyFemaleVoice".Localized());
                            }

                            break;

                            case 1:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BreastsWobbleOwnAccord".Localized());
                            }

                            break;

                            case 2:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GirlishVoiceCalls".Localized());
                            }

                            break;

                            case 3:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestBouncePlayfully".Localized());
                            }

                            break;

                            case 4:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SultryVoiceTeases".Localized());
                            }

                            break;

                            case 5:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AlluringVoicePerks".Localized());
                            }

                            break;

                            case 6:
                            {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoistnessFormingChest".Localized());
                            }

                            break;
                        }

                        CombatController.Instance!.Context.Enemy.Lust += CombatController.Instance!.Context.Enemy.Libido / 10 + 5;

                        return true;
                    }
                    else
                        if (Random.Shared.Next(3) == 0)
                        {
                            select = Random.Shared.Next(3);

                            switch (select)
                            {
                                case 0:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FantasyBrutallySqueezing".Localized());
                                }

                                break;

                                case 1:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BlushColorsCheeks".Localized());
                                }

                                break;

                                case 2:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoanEscapesLips".Localized());
                                }

                                break;

                                case 3:
                                {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BattleStanceInterrupted".Localized());
                                }

                                break;
                            }

                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(2 + Player.Sensitivity / 10)));

                            if (Random.Shared.Next(3) == 0)
                                CombatController.Instance!.Context.Enemy.Lust += 5 + CombatController.Instance!.Context.Enemy.Libido / 10;

                            return true;
                        }

            return false;
        }

        public void ExgartuanArmorShift()
        {

            var changed = false;

            if (Player.Armor.Id == ItemIds.BeeArmor)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SilkenLoinclothChitin".Localized());

                Player.ModArmorName = "crotch-hugging sexy black chitin armor-plating";

                changed = true;
            }
            else
                if (Player.Armor.Id == ItemIds.GelArmor)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GreenGelPlate".Localized());

                    Player.ModArmorName = "crotch-hugging glistening gel-armor plates";

                    changed = true;
                }
                else
                    if (Player.Armor.Id == ArmorIds.LeatherArmor)
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LeatherArmorShifts".Localized());

                        Player.ModArmorName = "crotch-hugging leather armor segments";

                        changed = true;
                    }
                    else
                        if (Player.Armor.Id == ArmorIds.SkimpyArmor)
                        {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChainmailBikiniIndecent".Localized());

                            Player.ModArmorName = "crotch-hugging practically indecent steel armor";

                            changed = true;
                        }
                        else
                            if (Player.Armor.Id == ArmorIds.R_BDYST)
                            {
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThinTransparentBodysuit".Localized());

                                Player.ModArmorName = "crotch-hugging red, high-society bodysuit";

                                changed = true;
                            }
                            else
                                if (Player.Armor.Id == ArmorIds.SilkArmor)
                                {
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FineSilkUndoes".Localized());

                                    Player.ModArmorName = "crotch-hugging spider-silk armor";

                                    changed = true;
                                }
                                else
                                    if (Player.Armor.Id == ArmorIds.S_SWMWR)
                                    {
                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MinisculeSwimwear".Localized());

                                        Player.ModArmorName = "crotch-hugging slutty swimwear";

                                        changed = true;
                                    }
                                    else
                                        if (Player.Armor.Id == ArmorIds.Chainmail)
                                        {
                                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TakenBySurpriseChainmail".Localized());

                                            Player.ModArmorName = "crotch-hugging full-body chainmail";

                                            changed = true;
                                        }
                                        else
                                            if (Player.Armor.Id == ArmorIds.ChainBikini)
                                            {
                                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChainmailBikiniRearranges".Localized());

                                                Player.ModArmorName = "crotch-hugging revealing chainmail bikini";

                                                changed = true;
                                            }
                                            else
                                                if (Player.Armor.Id == ArmorIds.PlateArmor)
                                                {
                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ClenchFistsPlatemail".Localized());

                                                    Player.ModArmorName = "crotch-hugging full platemail";

                                                    changed = true;
                                                }
                                                else
                                                    if (Player.Armor.Id == ArmorIds.ScaleArmor)
                                                    {
                                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SteelScalesFlap".Localized());

                                                        Player.ModArmorName = "crotch-hugging scale-mail armor";

                                                        changed = true;
                                                    }
                                                    else
                                                        if (Player.Armor.Id == ArmorIds.LeatherRobes)
                                                        {
                                                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LeatherArmorShiftsAgain".Localized());

                                                            Player.ModArmorName = "crotch-hugging black leather armor surrounded by voluminous robes";

                                                            changed = true;
                                                        }
                                                        else
                                                            if (Player.Armor.Id == ArmorIds.FetishWear)
                                                            {
                                                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RubberOutfitCompresses".Localized());

                                                                Player.ModArmorName = "crotch-hugging rubber fetish clothes";

                                                                changed = true;
                                                            }
                                                            else
                                                                if (Player.Armor.Id == ArmorIds.KokiriClothes)
                                                                {
                                                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LayerBeneathTunic".Localized());

                                                                    Player.ModArmorName = "crotch-hugging green adventurer's clothes";

                                                                    changed = true;
                                                                }
                                                                else
                                                                    if (Player.Armor.Id == ArmorIds.Overall)
                                                                    {
                                                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DenimOveralls".Localized());

                                                                        Player.ModArmorName = "crotch-hugging white shirt and overalls";

                                                                        changed = true;
                                                                    }
                                                                    else
                                                                        if (Player.Armor.Id == ArmorIds.ComfortableClothes)
                                                                        {
                                                                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ClothingShiftsTight".Localized());

                                                                            Player.ModArmorName = "crotch-hugging clothes";

                                                                            changed = true;
                                                                        }

            if (Player.Armor.Id == ArmorIds.DivineArmor || Player.Armor.Id == ArmorIds.TBARMOR)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SilkenLoinclothBark".Localized());

                Player.ModArmorName = "crotch-hugging " + Player.Armor.LongName;

                changed = true;
            }

            if (Player.Corruption < 33)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CringeBlushCrimson".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CringeAtOutfit".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PivotHipsShowOff".Localized());

            if ((changed) && (!Player.HasPerk(PerkIds.BulgeArmor)))
                    Player.CreatePerk(PerkIds.BulgeArmor, 0, 0, 0, 0);
        }

        public void ExgartuanWormCure()
        {

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.YourWord".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GroinWord".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrowWarmNoHot".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OnlyRoomForOne".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.Infested);
        }

        public void ExgartuanLactationAdjustment()
        {

            float boobs = 0;

            if (Player.BiggestLactation() > 1)
            {
                if (Random.Shared.Next(2) == 0 || Player.HasStatusEffect(StatusEffectIds.Feeder))
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NipplesWarmSensitive".Localized());

                    Player.BoostLactation(Player.BreastRows.Count);
                }
                else
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NipplesTightenUp".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    boobs = Player.BreastRows.Count;

                    while (boobs > 0)
                    {
                        boobs--;

                        Player.BreastRows[(int)(boobs)].Lactation = 0;
                    }
                }
            }
            else
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestFeelsCold".Localized());

                    Player.BoostLactation(Player.BreastRows.Count);
                }
                else
                    if (Random.Shared.Next(2) == 0)
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InnerSurfaceArouses".Localized());

                        Player.NippleLength += .5f;
                    }
                    else
                        if (Player.NippleLength > .5f)
                        {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TimePassesChestDifferent".Localized());

                            Player.NippleLength -= .5f;
                        }
        }

        public void ExgartuanBeeRape()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrinEmbraceIdea".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WalkingForwardSwagger".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SmoothCarapaceArms".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.KnowYourKnots".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoPleaseNeverFit".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShutUpTakeIt".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InsectilePussyTight".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BeeGirlAbusedLimp".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TightHolePushesBeyond".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PullOutSatisfied".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RedressWhistling".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 2f));

            ClearButtons();

            AddButton(0, "Common.Button.LeaveHer".Localized(), LeaveBeePostRape);

            AddButton(1, "Common.Button.FreeHer".Localized(), FreeBeePostRape);
        }

        public void FreeBeePostRape()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TakePityUntie".Localized());

            DoNext(() => ReturnToCamp(1));

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));
        }

        public void LeaveBeePostRape()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SmileCruellySmack".Localized());

            DoNext(() => ReturnToCamp(1));

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));
        }

        public void ExgartuanSleepSurprise()
        {

            if (Player.Corruption <= 20 && Player.HasPerk(PerkIds.BulgeArmor))
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LightBreezeSkims".Localized(
                    CampSleep.HomeDesc()
                ));

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlideHandsHair".Localized());

                if (Player.ArmorName != "crotch-hugging slutty swimwear" && Player.ArmorName != "crotch-hugging revealing chainmail bikini")
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemovePieceByPiece".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlareAlteredArmor".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ListlesslyStaring".Localized());

                DoNext(() => ExgartuanBulgeTortureII());

                return ;
            }
            else
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InterruptsPeacefulSleep".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DiaphragmSpasming".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.VoiceThrumsInHead".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioedExgartuan) == 0)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HowToPleaseDick".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioedExgartuan) < 2)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RememberHowHard".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioedExgartuan) < 10)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HowManyTimesDone".Localized());
                        else
                        {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WakeyWakeyRoutine".Localized());

                            if (Player.BiggestBreastSize() > 1)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WrapTitsAroundMe".Localized());
                            else
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WrapArmsAroundMe".Localized());

                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WereSoHorny".Localized());
                        }

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SayEndParagraph".Localized());

                if (Player.Corruption < 33)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrabPerversePrickPry".Localized());

                    if (!Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BaseWord".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SheathWord".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InsteadOfPullingFree".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BouncingBreastsAroundIt".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoansVibrateLodged".Localized(
                        Player.CockHead()
                    ));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioedExgartuan) < 5)
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrabOversizedOrgan".Localized(
                            Player.CockHead()
                        ));

                        if (Player.BiggestBreastSize() >= 2)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SqueezeTitsBiceps".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SqueezeCaressCock".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BothHandsStrokeFaster".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrabSwollenFlesh".Localized(
                            Player.CockHead()
                        ));

                        if (Player.BiggestBreastSize() >= 2)
                        {
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BicepsSqueezeMashing".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));

                            if (Player.HasFuckableNipples() && Player.BiggestLactation() > 1)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MilkAndLubricants".Localized());
                            else
                                if (Player.HasFuckableNipples())
                                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LubricantsWord".Localized());
                                else
                                    if (Player.BiggestLactation() > 1)
                                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MilkWord".Localized());

                            if (Player.HasFuckableNipples() || Player.HasFuckableNipples())
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FromNipplePuddle".Localized());
                        }

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakThird".Localized());
                    }

                if (Player.Gender == Gender.Hermaphrodite)
                    if (Player.Balls == 0)
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TheWord".Localized());

                        if (Player.Vaginas[0].Wetness < 2)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PuffyWord".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 4)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlisteningWord".Localized());
                            else
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DrippingWord".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OuterLipsEngorged".Localized());

                        if (Player.HasScales())
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ScalesWord".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SkinWord".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SurroundingYourSex".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.GetClitLength() > 3)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OrStrokeYourClit".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ToFullySatisfyAll".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakFourth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TheWordAgain".Localized());

                        if (Player.Vaginas[0].Wetness < 2)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PuffyWordAgain".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 4)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlisteningWordAgain".Localized());
                            else
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DrippingWordAgain".Localized());

                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OuterLipsEngorgedAgain".Localized(
                            Player.VaginaDescript(0)
                        ));
                    }

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SpitFoamsIntruder".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DesireShocksWrongness".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DesireSinksHooks".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DesireRushesBlood".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LiquidHotPressure".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.IfSomeoneWereWatching".Localized());

                if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JudgingByJojo".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DickFleshBulging".Localized());

                var temp = (int)(Player.CumQ());

                if (temp < 50)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SurprisinglyLight".Localized());
                else
                    if (temp < 150)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StickyWord".Localized());
                    else
                        if (temp < 300)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LargeThick".Localized());
                        else
                            if (temp < 800)
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LongThickBlast".Localized());
                            else
                                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImmenselyVoluminous".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DepositSquishesHang".Localized());

                if (temp < 500)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PumpedInto".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PumpedFull".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ObsceneLiquidSloshing".Localized(
                    Player.AssDescript()
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BallsPullTight".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SentencePeriodAgain".Localized());

                temp = (int)(Player.CumQ());

                if (temp > 500)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GradualCurveBelly".Localized());

                    if (temp > 1500)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DoesntStopGrowing".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SentencePeriodThird".Localized());

                    if (temp > 2500)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BackPressureBlastsFree".Localized());
                }

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakFifth".Localized());

                if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SplatterMouseCum".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EyesRollBackDeflates".Localized());

                if (Player.BiggestBreastSize() < 1)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.OverYourChest".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BetweenYourTits".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AcrossBellyRetreats".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                if (Player.LibidoPercent < 60)
                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f));
                else
                    if (Player.LibidoPercent < 80)
                        Player.ApplyStats(new DynStat(GameStats.Libido, .5f));
                    else
                        Player.ApplyStats(new DynStat(GameStats.Libido, .25f));

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                CoC.Instance.Flags.Set(GameFlagIds.TimesAutofellatioedExgartuan, CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioedExgartuan) + 1);

                Player.SlimeFeed();
            }

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, 25);

            DoNext(PlayerMenu);
        }

        public void ExgartuanBulgeTortureII()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FeelsLikeFewMinutes".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AgainReachFor".Localized());

            if (Player.ArmorName != "crotch-hugging slutty swimwear" && Player.ArmorName != "crotch-hugging revealing chainmail bikini")
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LastRemainingPiece".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.YourArmorWord".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StillFirmlyAttached".Localized());

            if (Player.ArmorName == "crotch-hugging full platemail" || Player.ArmorName == "crotch-hugging scale-mail armor")
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorIncrediblyStiff".Localized());

            if (Player.ArmorName == "crotch-hugging practically indecent steel armor" || Player.ArmorName == "crotch-hugging full-body chainmail" || Player.ArmorName == "crotch-hugging revealing chainmail bikini")
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorIncrediblyDense".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmorSixInchSteel".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MorePushingPulling".Localized());

            if (Player.Balls > 0 || Player.HasVagina())
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AlongWithAnythingElse".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PeriodBreak".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DamnableDemon".Localized());

            if (Player.BiggestBreastSize() >= 2 && Player.BreastRows.Count == 1)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.QuestBeginsOnChest".Localized());
            else
                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LinedDownChest".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LittleElseNipples".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FourFingersGapingTeat".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TappingTicklingTeasing".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TooPreoccupiedBelow".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            DoNext(() => ExgartuanBulgeTortureIII());
        }

        public void ExgartuanBulgeTortureIII()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FueledBySpite".Localized(
                Player.ArmorName
            ));

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DesireIncreasingClawing".Localized());

            if (Player.ArmorName == "crotch-hugging full platemail" || Player.ArmorName == "crotch-hugging scale-mail")
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BewildermentSeepedThrough".Localized());
            else
                if (Player.ArmorName == "crotch-hugging practically indecent steel armor" || Player.ArmorName == "crotch-hugging full-body chainmail" || Player.ArmorName == "crotch-hugging revealing chainmail bikini")
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FixationOnDrop".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StrangeFeelingBetrayal".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThumbForefingerSkeptical".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EyesWidenedTeethClenched".Localized(
                CampSleep.HomeDesc()
            ));

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FindFirstBitRelief".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FranticBreathingHeartbeat".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JustBeforeGiveUp".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockRemainsFlaccid".Localized(
                    Player.CockDescript(1)
                ));
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ExtraCocksRemainFlaccid".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThroughBlurryVision".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LifetimePassed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoWordsExchanged".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) < 6)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WillCannotCompare".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) >= 6 && CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) < 10)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NeverThoughtCelibacy".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.ExgartuanTightpantsMasturbateCount) < 12)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.KeepThisUpLonger".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThatTimeAgain".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakSixth".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ExgartuanPeriod".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DevilOfDickings".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestWorkHard".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BaseToTip".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BallsChurnMighty".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakSeventh".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BreathCeases".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ModestWord".Localized());
            else
                if (Player.CumQ() < 150)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SizablySticky".Localized());
                else
                    if (Player.CumQ() < 300)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MightyThick".Localized());
                    else
                        if (Player.CumQ() < 800)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImmenselyVoluminousAgain".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.Monumental".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LoadWorksUpDemon".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BlastThickDemonicJizz".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ColossalWaveDemonicJizz".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HitsSquareInFace".Localized());

            Player.Orgasm("Dick");

            Player.CreateStatusEffect(StatusEffectIds.Jizzpants, 1, 0, 0, 0);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ExgartuanBulgeTortureIV());
        }

        public void ExgartuanBulgeTortureIV()
        {
            ClearOutput();

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WakeNextMorningBed".Localized());

            if (Player.ArmorName != "crotch-hugging slutty swimwear" && Player.ArmorName != "crotch-hugging revealing chainmail bikini")
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.YourArmorWordAgain".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SinglePieceOfArmor".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RiseCumRuns".Localized());

            if (FollowerRegistry.All.Count > 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ConsiderAskingSomeone".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GlanceAroundCabin".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PayGlanceToExgartuan".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DamnDemons".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, 25);

            DoNext(PlayerMenu);
        }

        public void BoobGartuanSURPRISE()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SnickeringVoicesWakes".Localized(
                    Player.Mf("chump", "skank")
                ));

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DoorBargesImp".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShoutsSnidelyNemus".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NotLongEnoughNemus".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TooLongHonestly".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WorriedForgotten".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ReplyThrowingBack".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImpSnickersDrawCloser".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NemusPeersTable".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BothIdolsTomb".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FaceRemainsCalm".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WarningTurnsMoaning".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PleasureVanishesGaze".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FirstImpNods".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PleasedRubKnuckles".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AlwaysWonderedWhere".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ReturnToSender".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SchemeAsWell".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TeaseScoopSecondIdol".Localized());
            }
            else
            {
                BoobgartuanSurprise3();

                return ;
            }

            DoNext(() => BoobgartuanSurprise2());
        }

        public void BoobgartuanSurprise2()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoTimeDecidingDirection".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoOrdinaryImp".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HeyTeensyRememberMe".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HaventGottenPrettier".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TimedToGetFucked".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JestMongrelSnarls".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImpsRoundBend".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MonsterImpRaisesArm".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BrushAgainstScrotum".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CoastUnderBeast".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DevilishSmileCracks".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakEighth".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CrackOfLightning".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NemusBarkingOrders".Localized(
                    Player.Mf("asshole", "slut")
                ));

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImpsAtGateWrench".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JumpClimbShowyFlip".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WaveGoodbyeToNemus".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BlowKissToNemus".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FlipNemusMightyBird".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SilhouetteInStorm".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SuddenHeftInChest".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SwearFeltRumbling".Localized());
            }

            DoNext(() => BoobgartuanSurprise3());
        }

        public void BoobgartuanSurprise3()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MoonGreetsWideEyed".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LightTickleOnChin".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WokenUpOutdoors".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TenseMinutesRealizeWhere".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MorePreparedThisTime".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SitUpPiecePuzzle".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 4)
                {
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ExgartuanAgainNotFondled".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) >= 2)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PluralSLessons".Localized());

                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WouldHaveSunkIn".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 8)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AreYouDenseChampion".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestSmackGob".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LiftHeadShakeGround".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TormentingGoneOnLong".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LoveForeplayRidiculous".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakNinth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CantOverpowerMe".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HaventStartedCurriculum".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 8)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HowForgetCaress".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FindAnythingToGrab".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MakeOutSlightWarmth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.YieldsBuildingLust".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ReadyForYourFinal".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 8)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThinkYoullRemember".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EndsOpeningAct".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SolidHoldLoosens".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MountingNeedManhandle".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ThrashingEndedGaze".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakTenth".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsRiseControlSlips".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.IncreasinglyClearCannotSit".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GladToSitBack".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsWasteNoTime".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HipsGrindingMoist".Localized());

                if (Player.HasCock() && !Player.HasVagina())
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PreCumWord".Localized());

                if (Player.HasVagina() && !Player.HasCock())
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.JuiceWordAgain".Localized());

                if (Player.HasCock() && Player.HasVagina())
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AromaticCocktail".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TitMassageExcite".Localized());
            }

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DemonessContinuesWork".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GargantuanGlandsTremble".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TooFarGone".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WiggleFingersAnticipation".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BearDownGraspNothing".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BroughtOnArtificially".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PawUselesslySeal".Localized());

            if (Player.BiggestLactation() < 1 || Player.LactationQ() == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestLactateProfusely".Localized());
            else
                if (Player.BiggestLactation() < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BreastsMountingLactation".Localized());

            if (Player.BiggestLactation() < 4)
                Player.BoostLactation(3);

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NipplesInvertedRelease".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GripTooStrong".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) == 0)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.InsteadOfDreamingImps".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ReadyToGraduate".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) < 8)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NeverAgainForgetTits".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LookForwardBeatingYou".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakEleventh".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ChestRumbleQuake".Localized());

            if (Player.AverageNipplesPerBreast() > 1)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WhereOnceMultipleNipples".Localized());

                if (Player.NippleLength < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EachSwollenFourInches".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SentencePeriodFourth".Localized());
            }
            else
                if (Player.NippleLength < 4)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NipplesMuchLarger".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NipplesMuchThicker".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PayAlterationsLittleMind".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MixingInWithThe".Localized());

                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PreWord".Localized());
                else
                    if (Player.Gender == Gender.Female)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FemSpunkWord".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LiquidsWord".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AlreadyCoatingHips".Localized());
            }
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SentencePeriodFifth".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.TugTheseMelonsDry".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HandsWorkOnNips".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HeavySighPressureSubside".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.FingertipsSweepDenial".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f), new DynStat(GameStats.Corruption, 2f));

            Player.Orgasm("Tits", false);

            CoC.Instance.Flags.Set(GameFlagIds.BoobgartuanSurpriseCount, CoC.Instance.Flags.Get(GameFlagIds.BoobgartuanSurpriseCount) + 1);

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, 25);

            DoNext(PlayerMenu);
        }

        public void ExgartuanNagaStoleMyMasturbation()
        {
            ClearOutput();

            if (Player.Corruption < 15)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SheepishlyFindRocksAgain".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.IrritatingWord".Localized());

            if (Player.Corruption <= 25)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MakeSureAloneAgain".Localized());
            else
                if (Player.Corruption <= 50)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HappilyRemoveArmorAgain".Localized());
                else
                    if (Player.Corruption <= 75)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StripExaggeratedAgain".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.StripFondlingBitsAgain".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ParagraphBreakTwelfth".Localized());

            if (Player.Corruption < 33)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.GrownWearyGiveIn".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemainingToolHiding".Localized());
                else
                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemainingPolesHiding".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SentencePeriodSixth".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HowTheHellManage".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArmsFrozen".Localized());
            }

            if (Player.Corruption > 33 && Player.Corruption < 66)
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ShouldntLookForward".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.HaventFiguredPleasure".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoWaitPrettyClear".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemainingToolStillHiding".Localized());
                else
                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemainingPolesStillHiding".Localized());
                    else
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LeanDownEmbraceFrozen".Localized());
            }
            else
            {
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NoMoreFoolingAround".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SecondToolForced".Localized());
                else
                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.RemainingToolsForced".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CockMakesGoodPumping".Localized());

                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EverGladWoundUp".Localized());
            }

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SuddenTumbleBackside".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AppallinglySpitefulUrethra".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PossessedShaftWorksDown".Localized());

            if (Player.Intelligence > 9)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ArentStupidHazardGuess".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MayBeStupidClear".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.EndsForeplayCoiling".Localized());

            if (Player.Cocks[0].Length >= Player.Tallness * .5f)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.NotEnoughToEnvelop".Localized());
            else
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.AsImpressiveTailCapable".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.PressureTremendousGrip".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.DastardlyDickPhaseTwo".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SelfStimulationNeverGood".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CantMeasureUp".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CorruptCockSpeaksUp".Localized());
                else
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.BothOurExpertise".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.WitsEndRelease".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ModestWordAgain".Localized());
            else
                if (Player.CumQ() < 150)
                    OutputText("Scenes.Exgartuan.ExgartuanMasturbate.SizablyStickyAgain".Localized());
                else
                    if (Player.CumQ() < 300)
                        OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MightyThickAgain".Localized());
                    else
                        if (Player.CumQ() < 800)
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.ImmenselyVoluminousThird".Localized());
                        else
                            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.MonumentalAgain".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.LoadWorkingUpCock".Localized());

            OutputText("Scenes.Exgartuan.ExgartuanMasturbate.CoatedFaceReturns".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, (16 + Random.Shared.Next(7)));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .25f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}