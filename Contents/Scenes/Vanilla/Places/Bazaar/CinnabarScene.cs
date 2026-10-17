using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<CinnabarScene>("coc:cinnabar")]
    public class CinnabarScene : Scene
    {

        public override void Entry()
        {
            CinnabarAppearance();
        }

        public Action? CinnabarAppearance(bool output = true)
        {

            if (View.Time.Hours < 15 || View.Time.Hours > 20)
                return null;

            if (output)
                if (CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberEncounters) == 0)
                    OutputText("Scenes.Cinnabar.CinnabarAppearance.GenerouslyProportioned".Localized((noFur ? "" : " over her ruddy fur"), (noFur ? "a pair of rat-like ears on her head" : "an unmistakable, rat-like muzzle")));
                else
                    OutputText("Scenes.Cinnabar.CinnabarAppearance.OnTheProwl".Localized());

            return CinnabarGreeting;
        }

        public void CinnabarGreeting()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberEncounters) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CinnabarNumberEncounters, CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberEncounters) + 1);

                if (!Player.HasCock())
                {
                    OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToRat".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.BuyOneHour".Localized(), CinnabarNonHugeDickings);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                }
                else
                    if (Player.BiggestCockArea() < 100)
                    {
                        OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToRatAgain".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Buy1hour".Localized(), CinnabarNonHugeDickings);

                        AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                    }
                    else
                    {
                        OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToRatThird".Localized(
                             Player.Mf("stud", "baby")
                        ));

                        if (Player.CockTotal() > 1 && (Player.Cocks.Count <= 1 ? 0 : Player.CockArea(Player.CockIndicesBySize()[1])) >= 75)
                            OutputText("Scenes.Cinnabar.CinnabarGreeting.ThoseThingsWord".Localized());
                        else
                            OutputText("Scenes.Cinnabar.CinnabarGreeting.ThatThingWord".Localized());

                        OutputText("Scenes.Cinnabar.CinnabarGreeting.NameIsCinnabar".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.FuckHer".Localized(), CinnabarGetsFUKKKKED);

                        if (Player.CockTotal() > 2 && (Player.Cocks.Count <= 1 ? 0 : Player.CockArea(Player.CockIndicesBySize()[1])) >= 75)
                            AddButton(1, "Common.Button.MultiFuck".Localized(), CinnabarMultiCockPortalFuckFest);
                        else
                            AddButton(1, "Common.Button.MultiFuck".Localized(), null);

                        AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                    }
            }
            else
                if (!Player.HasCock())
                {
                    OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToCinnabar".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Buy1hour".Localized(), CinnabarNonHugeDickings);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                }
                else
                    if (Player.BiggestCockArea() < 100)
                    {
                        OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToCinnabarAgain".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Buy1hour".Localized(), CinnabarNonHugeDickings);

                        AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                    }
                    else
                    {
                        OutputText("Scenes.Cinnabar.CinnabarGreeting.WalksToRatFourth".Localized(
                             Player.Mf("stud", "baby")
                        ));

                        if (Player.CockTotal() > 2 && (Player.Cocks.Count <= 1 ? 0 : Player.CockArea(Player.CockIndicesBySize()[1])) >= 75)
                            OutputText("Scenes.Cinnabar.CinnabarGreeting.ThoseThingsWordAgain".Localized());
                        else
                            OutputText("Scenes.Cinnabar.CinnabarGreeting.ThatThingWordAgain".Localized());

                        OutputText("Scenes.Cinnabar.CinnabarGreeting.RatherYouPlug".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.FuckHer".Localized(), CinnabarGetsFUKKKKED);

                        if (Player.CockTotal() > 2 && (Player.Cocks.Count <= 1 ? 0 : Player.CockArea(Player.CockIndicesBySize()[1])) >= 75)
                            AddButton(1, "Common.Button.MultiFuck".Localized(), CinnabarMultiCockPortalFuckFest);
                        else
                            AddButton(1, "Common.Button.MultiFuck".Localized(), null);

                        AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
                    }
        }

        public void CinnabarNonHugeDickings()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarHouseVisited, CoC.Instance.Flags.Get(GameFlagIds.CinnabarHouseVisited) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarNumberTimesFucked, CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberTimesFucked) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 0);

            if (Player.Gems < 50)
            {
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.CinnabarTeases".Localized());

                Go<BazaarScene>();

                return ;
            }

            Player.Gems -= 50;

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.OnceAssent".Localized((noFur ? "hands" : "paws"), (noFur ? "" : " with her soft, well-groomed fur")));

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.BeforeAnswer".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.DragsYou".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.GivesWhore".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.GrabsBlack".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.CoosHappily".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.TooShocked".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            if (!Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.AndYourAsshole".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.YourVaginaAndAsshole".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.JustInchesAway".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.PantsWiggles".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.RollsBetween".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SlipsTwoDigits".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.WantsPenetrated".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SatisfiedPower".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ReleaseSqueeze".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ReleasePressure".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SettleToBusiness".Localized());

            if (Player.HasVagina())
                Player.CuntChange(8, true, true, false);
            else
                Player.ButtChange(8, true, true, false);

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.UnbiddenHarden".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FemcumSoaks".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.RocksBack".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.CaressesPassage".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.WorksBody".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.PantingBitch".Localized(
                 Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ShrugsPullsUp".Localized());

            if (Player.VaginalCapacity() < 30)
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersWord".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersNoFist".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.TellsHowMuch".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SheEvenWorks".Localized());

                if (Player.AnalCapacity() < 30)
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.HerFingersWord".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.HerFistWord".Localized());

                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.IntoAsshole".Localized());
            }

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.GaspAndMoan".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.GrowingAroused".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SheSqueaks".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.PressBrutalizing".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.TheRatsWord".Localized());

            if (Player.HasVagina())
            {
                if (Player.VaginalCapacity() < 30 && Player.AnalCapacity() < 30)
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersWordAgain".Localized());
                else
                    if (Player.VaginalCapacity() >= 30 && Player.AnalCapacity() >= 30)
                        OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FistsWord".Localized());
                    else
                        OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersAndFist".Localized());
            }
            else
                if (Player.AnalCapacity() < 30)
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersWordThird".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ClenchedFingers".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.PlungeFurther".Localized());

            if (Player.HasCock() && !Player.HasVagina())
            {
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SoftPadsGrip".Localized());

                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.NotContent".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.PuddlesFloor".Localized());

                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SentencePeriod".Localized());
                }
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.LicksAndConvulses".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FingersWordFourth".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.HandWord".Localized());

                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.HungryEmbrace".Localized());
            }

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.SoftPadsRub".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.OrgasmIntense".Localized());

            if (!(Player.Wetness() < 3))
                if (Player.Wetness() < 5)
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ArmSoaked".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ArmAndChest".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.ParagraphBreak".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.RollsOver".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.FloorMopped".Localized());

            OutputText("Scenes.Cinnabar.CinnabarNonHugeDickings.BeforeNext".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CinnabarGetsFUKKKKED()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarHouseVisited, CoC.Instance.Flags.Get(GameFlagIds.CinnabarHouseVisited) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarNumberTimesFucked, CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberTimesFucked) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 0);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.AsSoonAs".Localized(
                 Player.ArmorName,

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.GrownWobbly".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BendMeOver".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CinnabarFuckedCorruptPreviously) > 0)
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BeGentler".Localized(
                     Player.Mf("stud", "baby")
                ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.CriesButStares".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.EagerRodentSpins".Localized());

            if (Player.Strength >= 60 || Player.Tallness >= 72)
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.ALittleWord".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.OnYourHeels".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SwayWoozy".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.FingertipsBuried".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SquirmsKicks".Localized(
                 Player.Mf("real man", "horse-herm")
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SmilesSpreads".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.FollowsGaze".Localized(
                 Player.Mf("stud", "bitch"),

                 Player.CockHead(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BeenAWhile".Localized());

            if (Player.CockArea(x) < 200)
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.CentaurMareWince".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.DragonessCry".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.LookAtMirrors".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SureEnough".Localized(
                 Player.Mf("stud", "slut")
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BallHand".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.EnjoyingRaunchiness".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.LittleWorried".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.LittleAmazed".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BatterThrough".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.TunnelSqueezes".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.NeededEncouragement".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SackAnd".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BeforePullingTight".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.HitsPlush".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SheCries".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.ContinueThe".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.SlapHerAss".Localized());

            OutputText("Scenes.Cinnabar.CinnabarGetsFUKKKKED.BarelyRegisters".Localized());

            if (Player.CockArea(x) >= 200)
            {
                if (Player.Corruption >= 75)
                    DoNext(() => FuckCinnabarHugerAndCorruptedEr());
                else
                    DoNext(() => CinnabarHuger());
            }
            else
                DoNext(() => CinnabarHuge());
        }

        public void FuckCinnabarHugerAndCorruptedEr()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 1);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.AlmostThere".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CinnabarFuckedCorruptPreviously) > 0)
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.DoingItAgain".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ParagraphBreak".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.OnePushSent".Localized(
                 NumberUtils.Num2Text((int)(Player.Cocks[x].Length / 12)),

                 Player.CockDescript(x),

                OptionStore.Instance.Get("furry", 0) <= 0 ? "rusty-haired" : "rust-red"
            ));

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SheProtests".Localized(
                 Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ShouldveThought".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.NoCumSleeve".Localized((noFur ? "" : " furry")));

            if (!Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.CrotchWord".Localized());
            else
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SheathWord".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SquirtingJuices".Localized(
                 Player.CockHead(x),

                 Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length >= 60)
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.OnlyNowRealize".Localized());
            else
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.OnlyNowRealizeAgain".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.HeadAbsurdlyTiny".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ScreamsRedRat".Localized((noFur ? "-haired" : "")));

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ParagraphBreakThird".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.TwistsFist".Localized());

            if (Player.Intelligence >= 50)
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.DimlyRegister".Localized());
            else
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.TooMuchFor".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.HalfSecondLater".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.PullsDirtyOut".Localized((noFur ? "hand" : "paw"), Player.Mf("studs", "bitches")));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.BodyChurn".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.StretchedLips".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.LipsGlaze".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.LookPregnant".Localized());
                        else
                            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.LookLikeHugely".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.LivingOnahole".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.CollapseUnder".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SomeTimeLater".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SlapsYouHard".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SitUpCompletely".Localized(
                 Player.Mf("stud", "fucker")
            ));

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.TossesBack".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SavePregnantBelly".Localized());
            else
                OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.SaveForHer".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.HugePussyLips".Localized());

            OutputText("Scenes.Cinnabar.FuckCinnabarHugerAndCorruptedEr.OnceLetsGo".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, -2f));

            Player.HealthChange(-25);

            DoNext(() => ReturnToCamp(1));
        }

        public void CinnabarHuger()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 0);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cinnabar.CinnabarHuger.AlmostThere".Localized((noFur ? "skin" : "fur")));

            OutputText("Scenes.Cinnabar.CinnabarHuger.RestSinksIn".Localized(Player.CockHead(x), (noFur ? "living" : "furry")));

            OutputText("Scenes.Cinnabar.CinnabarHuger.TrueToWords".Localized((noFur ? "skin" : "fur"), Player.CockDescript(x), Player.CockDescript(x)));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Cinnabar.CinnabarHuger.BaseWord".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.ProdsAsshole".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length < 24)
                OutputText("Scenes.Cinnabar.CinnabarHuger.InchesWord".Localized(
                     NumberUtils.Num2Text(Player.Cocks[x].Length)
                ));
            else
                OutputText("Scenes.Cinnabar.CinnabarHuger.FeetWord".Localized(
                     NumberUtils.Num2Text((float)(Player.Cocks[x].Length / 12))
                ));

            OutputText("Scenes.Cinnabar.CinnabarHuger.AllOverAgain".Localized());

            if (Player.Cocks[x].Length >= 60)
                OutputText("Scenes.Cinnabar.CinnabarHuger.NoWayThis".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.ParagraphBreak".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.SquirmingWrithing".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls == 0)
                OutputText("Scenes.Cinnabar.CinnabarHuger.PluralS".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.GooClaiming".Localized(
                 Player.Mf("MASTER", "MISTRESS")
            ));

            OutputText("Scenes.Cinnabar.CinnabarHuger.PerverseWords".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Cinnabar.CinnabarHuger.RatGirlGained".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Cinnabar.CinnabarHuger.SpoogePaunch".Localized());
                else
                    if (Player.CumQ() < 1500)
                        OutputText("Scenes.Cinnabar.CinnabarHuger.SpoogePackedBelly".Localized());
                    else
                        OutputText("Scenes.Cinnabar.CinnabarHuger.SpoogePregnancy".Localized());
            }

            OutputText("Scenes.Cinnabar.CinnabarHuger.GoesLimp".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.PopsFromHer".Localized(
                 Player.CockHead(x)
            ));

            if (Player.Cocks[x].Length >= 60)
                OutputText("Scenes.Cinnabar.CinnabarHuger.ShouldBeStickingUp".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarHuger.SheMustBe".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.AbusedCervix".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.SmilesUp".Localized(
                 Player.Mf("studs", "breeders")
            ));

            OutputText("Scenes.Cinnabar.CinnabarHuger.SmileBack".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Cinnabar.CinnabarHuger.SloshingBelly".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuger.SentencePeriod".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -4f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CinnabarHuge()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 0);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Cinnabar.CinnabarHuge.AlmostThereThough".Localized(Player.CockDescript(x), (noFur ? "face" : "muzzle"), Player.CockHead(x)));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Cinnabar.CinnabarHuge.SheathWord".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarHuge.BaseWord".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.PeriodBreak".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.KissBroken".Localized(
                 Player.Mf("stud", "bitch")
            ));

            OutputText("Scenes.Cinnabar.CinnabarHuge.SoFuckingPerverse".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Cinnabar.CinnabarHuge.SheathWordAgain".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarHuge.ShaftWord".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.ProdsAsshole".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.JuicesLikeRain".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Cinnabar.CinnabarHuge.YourBallsWord".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.YourLegsAnd".Localized());

            if (!Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Cinnabar.CinnabarHuge.BaseOfCock".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarHuge.TwitchingCockSheath".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.RubsHandOver".Localized(Player.CockHead(x), (noFur ? "skin" : "fur"), (noFur ? "skin" : "fur")));

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Cinnabar.CinnabarHuge.AndSoMuch".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.ClaimMeImYours".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.DontHoldBack".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Cinnabar.CinnabarHuge.ProstateSqueezedDry".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarHuge.SoreAndEmpty".Localized());

            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Cinnabar.CinnabarHuge.RatGirlGained".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Cinnabar.CinnabarHuge.JigglingSpoogePaunch".Localized());
                else
                    if (Player.CumQ() < 1500)
                        OutputText("Scenes.Cinnabar.CinnabarHuge.SpoogePackedBelly".Localized());
                    else
                        OutputText("Scenes.Cinnabar.CinnabarHuge.SpoogePregnancy".Localized());
            }

            OutputText("Scenes.Cinnabar.CinnabarHuge.SlumpsPussyStained".Localized((noFur ? "smooth skin turning into a cum-soaked" : "silky fur turning into a cum-matted")));

            OutputText("Scenes.Cinnabar.CinnabarHuge.BlackBrutalizedLips".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Cinnabar.CinnabarHuge.LooksOver".Localized(
                 Player.Mf("studs", "breeders")
            ));

            if (Player.CumQ() > 500)
                OutputText("Scenes.Cinnabar.CinnabarHuge.SloshingBelly".Localized());

            OutputText("Scenes.Cinnabar.CinnabarHuge.SentencePeriod".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CinnabarMultiCockPortalFuckFest()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarHouseVisited, CoC.Instance.Flags.Get(GameFlagIds.CinnabarHouseVisited) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarNumberTimesFucked, CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberTimesFucked) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.CinnabarFuckedCorruptPreviously, 0);

            var x = Player.BiggestCockIndex();

            var y = Player.CockIndicesBySize()[1];

            var z = Player.CockIndicesBySize()[2];

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.HandsFlyThrough".Localized());

            if (Player.Lust >= 70)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.GroansWhenFeels".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SighsBlissfully".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.WobblyFromDelicate".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.UnderYourArm".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.UpAgainstYou".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SupportingYou".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.MirthfulEyesWatch".Localized());

            if (Player.Lust >= 70)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.YouHaveWord".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ShesGivenYou".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ShesTeasing".Localized((noFur ? "" : " of her fur")));

            if (CoC.Instance.Flags.Get(GameFlagIds.CinnabarHouseVisited) == 0)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.LeansGuiding".Localized());
            else
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BlushesHard".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ParagraphBreak".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.PullsToDoor".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.LongTailHovers".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.StrugglingOut".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.LoudSnorts".Localized((noFur ? "skin" : "fur")));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SlapHerFace".Localized(
                 Player.CockDescript(x),

                 Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BouncesTits".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BehemothsTwitching".Localized(
                 Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.PuffyDrippyLips".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ComeFuckMe".Localized(
                 Player.Mf("master", "mistress"),

                 Player.Mf("stud", "baby")
            ));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.RockForwards".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(y)
            ));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.MoansDelighted".Localized(Player.CockDescript(z), (noFur ? " warm" : " furry")));

            if (Player.CockTotal() == 4)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ExtraDickDives".Localized());
            else
                if (Player.CockTotal() > 4)
                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ExtraDicksDive".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SensationDivine".Localized());

            if (Player.CockTotal() >= 4)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.TitFuckWhile".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SlipperyWarmth".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() >= 4)
            {
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SoftCoated".Localized((noFur ? "skin becomes" : ", downy fur on the rat's chest is starting to mat against her skin,")));

                if (Player.CockTotal() == 4)
                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ExtraDickWord".Localized());
                else
                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.RemainingDicksWord".Localized());

                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SentencePeriod".Localized());
            }

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.InHerMouth".Localized(Player.CockDescript(z), (noFur ? "wet lips bury themselves" : "a wet muzzle buries itself")));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.FeelCumming".Localized(
                 Player.MultiCockDescriptLight(),

                 NumberUtils.Num2Text(Player.CockTotal()),

                 Player.CockDescript(z)
            ));

            if (Player.CockTotal() >= 4)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.TitsShiningWhite".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.PeriodBreak".Localized());

            if (Player.CumQ() >= 500 && Player.CumQ() < 1000)
            {
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BellyGurglesGut".Localized());

                if (Player.CockTotal() >= 4)
                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.RatsTitsCompletely".Localized());

                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.FinishFilling".Localized());
            }
            else
                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BellyGurglesSkin".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.BestOfAllBed".Localized());

                    OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.ParagraphBreakThird".Localized());
                }

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.SlutRatGargles".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.IncessantLicking".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.HugeCumFilledBelly".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.LongPinkTongue".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.GruntsBeforeReturning".Localized());

            OutputText("Scenes.Cinnabar.CinnabarMultiCockPortalFuckFest.WhileDressing".Localized(
                 Player.Mf("stud", "baby")
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -6f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}