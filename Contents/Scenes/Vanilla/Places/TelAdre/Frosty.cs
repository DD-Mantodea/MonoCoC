using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<Frosty>("coc:frosty")]
    public class Frosty : Scene
    {

        public override void Entry()
        {
            ApproachFrosty();
        }

        public void ApproachFrosty()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetFrosty) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MetFrosty, 1);

                OutputText("Scenes.Frosty.ApproachFrosty.ApproachPink".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CantThinkLong".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.ShareSilence".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.ThirtyGemsYell".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CrossesEyes".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.NewBusiness".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.MakesRoom".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.ToBeSimple".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.SaidBluntly".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.ImANymph".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.BrushesHair".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.SurprisedCouldntTell".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.StandsShowOff".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.WavesHandDeal".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously);

                AddButton(1, "Common.Button.No".Localized(), NoContractForMeSloot);
            }
            else
            {
                OutputText("Scenes.Frosty.ApproachFrosty.FrostysStand".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CupcakeOneLine".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CupcakeFiveLine".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CupcakeTenLine".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.CupcakeTwentyLine".Localized());

                OutputText("Scenes.Frosty.ApproachFrosty.BulkPrice".Localized());

                FrostyMainMenu();
            }
        }

        public void FrostyMainMenu()
        {

            ClearButtons();

            if (Player.Gems >= 15)
                AddButton(0, "Common.Button.X1".Localized(), () => CupcakeBuy(1));

            if (Player.Gems >= 60)
                AddButton(1, "Common.Button.X5".Localized(), () => CupcakeBuy(5));

            if (Player.Gems >= 110)
                AddButton(2, "Common.Button.X10".Localized(), () => CupcakeBuy(10));

            if (Player.Gems >= 200)
                AddButton(3, "Common.Button.X20".Localized(), () => CupcakeBuy(20));

            if (CoC.Instance.Flags.Get(GameFlagIds.SignedFrostysContract) == 0)
                AddButton(8, "Common.Button.Contract".Localized(), GetAFuckingFuckContractFromFucks);
            else
                AddButton(8, "Common.Button.Specials".Localized(), FrostysLimitedSpecialMenu);

            AddButton(14, "Common.Button.Leave".Localized(), null );
        }

        public void FrostySpecialsMenu(bool free = false)
        {

            ClearButtons();

            if (!free && CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) < 5)
                OutputText("Scenes.Frosty.FrostySpecialsMenu.NotEnoughPoints".Localized());
            else
                if (!free && CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) < 15)
                    OutputText("Scenes.Frosty.FrostySpecialsMenu.NotEnoughPointsAgain".Localized());

            if (free || CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) >= 5)
                AddButton(0, "Common.Button.Hand".Localized(), FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp);

            if (free || CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) >= 15)
                AddButton(1, "Common.Button.Mouth".Localized(), UseFrostysMouth);

            if (free || CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) >= 1)
                AddButton(8, "Common.Button.EatHerOut".Localized(), CunnilingateFrosty);

            AddButton(14, "Common.Button.Back".Localized(), ApproachFrosty);
        }

        public void CupcakeBuy(int arg = 1)
        {
            ClearOutput();

            OutputText("Scenes.Frosty.CupcakeBuy.MakesPurchase".Localized());

            if (arg == 1)
                Player.Gems -= 15;

            if (arg == 5)
                Player.Gems -= 60;

            if (arg == 10)
                Player.Gems -= 110;

            if (arg == 20)
                Player.Gems -= 200;

            FrostyPoints(arg);

            Player.RefillHunger(5 * arg);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachFrosty);
        }

        public void YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SignedFrostysContract, 1);

            OutputText("Scenes.Frosty.YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously.RewardedSex".Localized());

            OutputText("Scenes.Frosty.YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously.BetaTester".Localized());

            OutputText("Scenes.Frosty.YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously.ForgotToMention".Localized());

            OutputText("Scenes.Frosty.YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously.DoYouAccept".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesIWantYourFreeSampleYouFuckingDiseasedCupcakeSlut);

            AddButton(1, "Common.Button.No".Localized(), NoIDontWantAFreebieDiseaseYouSlut);
        }

        public void YesIWantYourFreeSampleYouFuckingDiseasedCupcakeSlut()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.YesIWantYourFreeSampleYouFuckingDiseasedCupcakeSlut.FollowsBooth".Localized());

            FrostySpecialsMenu(true);
        }

        public void NoIDontWantAFreebieDiseaseYouSlut()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.NoIDontWantAFreebieDiseaseYouSlut.TooBusy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), null );
        }

        public void NoContractForMeSloot()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.NoContractForMeSloot.ContractSeemsWrong".Localized());

            OutputText("Scenes.Frosty.NoContractForMeSloot.StillBuyCupcakes".Localized());

            OutputText("Scenes.Frosty.NoContractForMeSloot.NodsSuggestion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), null );
        }

        public void GetAFuckingFuckContractFromFucks()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.GetAFuckingFuckContractFromFucks.ZoomsAround".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesFrostyLetsHaveSexForCupcakesWhoThoughtThisWasAGoodIdeaIMeanSeriously);

            AddButton(1, "Common.Button.No".Localized(), NoIDontWantToSignYourFuckingContractGoddamned);
        }

        public void NoIDontWantToSignYourFuckingContractGoddamned()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.NoIDontWantToSignYourFuckingContractGoddamned.DiscouragedOh".Localized());

            FrostyMainMenu();
        }

        public void FrostysLimitedSpecialMenu()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.FrostysLimitedSpecialMenu.SpinsMenu".Localized());

            OutputText("Scenes.Frosty.FrostysLimitedSpecialMenu.ServicePricing".Localized());

            OutputText("Scenes.Frosty.FrostysLimitedSpecialMenu.CurrentPoints".Localized(
                 FrostyPoints()
            ));

            FrostySpecialsMenu();
        }

        public int FrostyPoints(int arg = 0)
        {

            if (arg == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints);

            CoC.Instance.Flags.Set(GameFlagIds.FrostyPoints, CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) + arg);

            if (CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.FrostyPoints, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.FrostyPoints);
        }

        public void FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp()
        {
            ClearOutput();

            if (!Player.HasCock() && !Player.HasVagina())
            {
                GenderlessCatchAllForFrosty();

                return ;
            }
            else
            {
                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.GigglesOwnName".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.DidntNotice".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SyrupSong".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SyrupSticky".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.DontBeShy".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.WrapsFingers".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FullyErect".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.OooohYour".Localized());

                if (Player.BiggestCockLength() < 7)
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LittleWord".Localized());
                else
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.BigWord".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FriendWaiting".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.RegripsCock".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.MakesHarder".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.TalkingToCock".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.RubsOnFace".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.CumPools".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.ThankBusiness".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.WhosGonnaClean".Localized());

                if (Player.BiggestCockLength() < 7)
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LittleWordAgain".Localized());
                else
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.BigWordAgain".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FriendCovered".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LowTonedVoice".Localized());

                OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.GigglingBehind".Localized());
            }

            if (!(Player.HasCock() && !Player.HasVagina()))
                if (Player.HasVagina() && !Player.HasCock())
                {
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.GigglesOwnNameAgain".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.MovedArmrest".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.PeeksInside".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.GigglesAmused".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.ReachesUnderChair".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SyrupOozes".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.TinyCircles".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FartherFarther".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SpreadsLips".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FingersRapidly".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.HelpingJuices".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LicksJuices".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SlouchesChair".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LetsRelax".Localized());
                }
                else
                {
                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SnickersOwnName".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.ThrowsChair".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.MovedArmrestAgain".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.ChargingDouble".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.LiftsCock".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.StandsDrips".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.PoolsHead".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.YourBalls".Localized());
                    else
                        OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.YourPussyLips".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.CockThrobsPussy".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.OneLittleTouch".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.FunOverSoon".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.GeezAtLeast".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.EndedCurlyHair".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.TwoFingers".Localized());

                    if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
                        OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.TheBimbo".Localized());
                    else
                        OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.ABimbo".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.VisionBlurs".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.RubbingBoth".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SweetRelease".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.DirectsAttention".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.SitsExhausted".Localized());

                    OutputText("Scenes.Frosty.FrostysHandsAreColdHolyShitWhyDontYouWarmTHoseMitsUp.YellsServices".Localized());
                }

            FrostyPoints(-5);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void UseFrostysMouth()
        {
            ClearOutput();

            if (!Player.HasCock() && !Player.HasVagina())
            {
                GenderlessCatchAllForFrosty();

                return ;
            }
            else
            {
                OutputText("Scenes.Frosty.UseFrostysMouth.RollsEyes".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.BeforeMouthSex".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.GigglesAtHands".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.DeepSwig".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.SideTurn".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.PartyStarted".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.TongueRunsLaps".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.HoneyConnects".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.HandsBehindHead".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.BeAsRough".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.HowTrue".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.MassagesUnderside".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.LetsGoFrosty".Localized());

                OutputText("Scenes.Frosty.UseFrostysMouth.HearsNomming".Localized());
            }

            if (!(Player.HasCock() && !Player.HasVagina()))
                if (Player.HasVagina() && !Player.HasCock())
                {
                    OutputText("Scenes.Frosty.UseFrostysMouth.RollsEyesAgain".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.HearsRummaging".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.GigglesAtHandsAgain".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.DeepSwigAgain".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.PartyStartedAgain".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.LaunchesHead".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.LicksBackUp".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.SticksLipOut".Localized());
                }
                else
                {
                    OutputText("Scenes.Frosty.UseFrostysMouth.RollsEyesThird".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.BeforeMouthSexAgain".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.ForgetHands".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.DeepSwigThird".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.PartyStartedThird".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.LicksFromBottom".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.PopsCockOut".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.BothOrgans".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.CoversHead".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.LegsDisappear".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.ShakesThought".Localized());

                    OutputText("Scenes.Frosty.UseFrostysMouth.HoldsRope".Localized());
                }

            FrostyPoints(-15);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void GenderlessCatchAllForFrosty()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SeenGenderlessFrostyRejection) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.SeenGenderlessFrostyRejection, CoC.Instance.Flags.Get(GameFlagIds.SeenGenderlessFrostyRejection) + 1);

                OutputText("Scenes.Frosty.GenderlessCatchAllForFrosty.LooksAtService".Localized());

                OutputText("Scenes.Frosty.GenderlessCatchAllForFrosty.ReserveRight".Localized());

                OutputText("Scenes.Frosty.GenderlessCatchAllForFrosty.DirtyLook".Localized());
            }
            else
                OutputText("Scenes.Frosty.GenderlessCatchAllForFrosty.CheesyJingle".Localized());

            FrostySpecialsMenu();
        }

        public void CunnilingateFrosty()
        {
            ClearOutput();

            OutputText("Scenes.Frosty.CunnilingateFrosty.LooksAtService".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.HoldsFingerNozzle".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.LikeMyCream".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.UncrossesLegs".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.BringsMouth".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.LapsUpGlob".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.LicksLipsFull".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.ExcitementBuilt".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.TiredOfCream".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.PushesFaceHarder".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.WrapsLongLegs".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.PullsMouthOff".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.HandPassesMinotaur".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.ExplainsMinotaur".Localized());

            OutputText("Scenes.Frosty.CunnilingateFrosty.SignContract".Localized());

            FrostyPoints(-1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}