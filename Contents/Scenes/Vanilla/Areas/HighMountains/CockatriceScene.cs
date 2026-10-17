using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
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
using MonoCoC.Contents.Enemies.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<CockatriceScene>("coc:cockatrice")]
    public class CockatriceScene : Scene
    {

        public override void Entry()
        {
            Greeting();
        }

        public void Greeting()
        {
            ClearOutput();

            Enemy cockatrice = null!;

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredCockatrices) == 0)
            {
                cockatrice = EnemyRegistry.Create(EnemyIds.Cockatrice);

                OutputText("Scenes.Cockatricescene.Greeting.FollowTrails".Localized());

                OutputText("Scenes.Cockatricescene.Greeting.CantPickDetails".Localized());

                OutputText("Scenes.Cockatricescene.Greeting.ApproachesCarefully".Localized());

                ShowImage("monster-cockatrice");

                OutputText("Scenes.Cockatricescene.Greeting.SixFeetTall".Localized((noFur ? " shoulders and upper arms" : ""), (noFur ? "human" : "beaked"), (noFur ? "smooth skin" : "smooth cream scales of its underbelly")));

                OutputText("Scenes.Cockatricescene.Greeting.FeatheredArms".Localized());

                OutputText("Scenes.Cockatricescene.Greeting.ShiftsStance".Localized());

                OutputText("Scenes.Cockatricescene.Greeting.WideHips".Localized((noFur ? "" : " feathered")));

                OutputText("Scenes.Cockatricescene.Greeting.NoticesYou".Localized());

            }
            else
            {
                if (Random.Shared.Next(100) < 40)
                    cockatrice = new CockatriceEnemy();
                else
                    cockatrice = EnemyRegistry.Create(EnemyIds.Cockatrice);

                ShowImage("monster-cockatrice");

                OutputText("Scenes.Cockatricescene.Greeting.ClimbsAgain".Localized());

                if (cockatrice.CanFly())
                    OutputText("Scenes.Cockatricescene.Greeting.WingsUnfurled".Localized());

                OutputText("Scenes.Cockatricescene.Greeting.HopsStones".Localized());
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryCockatrices);

            CombatScene.Start(cockatrice, () => Go<CampScene>());

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredCockatrices, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredCockatrices) + 1);
        }

        public void DefeatCockatrice()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                ShowImage("cockatrice-fallen-hp");
            else
                ShowImage("cockatrice-fallen-lust");

            OutputText("Scenes.Cockatricescene.DefeatCockatrice.FallsToKnees".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Cockatricescene.DefeatCockatrice.ThoroughlyBeaten".Localized());
            else
                OutputText("Scenes.Cockatricescene.DefeatCockatrice.StrokesLength".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RideHimVag".Localized(), null);

            AddButton(1, "Common.Button.RideHimAnal".Localized(), null);

            AddButton(2, "Common.Button.ButtFuck".Localized(), null);

            AddButton(3, "Common.Button.OralCock".Localized(), null);

            AddButton(4, "Common.Button.OralVag".Localized(), null);

            AddButton(5, "Common.Button.TaurSex".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Cockatricescene.DefeatCockatrice.WhatDoWithHim".Localized());

                if (!Player.IsTaur() && !Player.IsDrider())
                {
                    if (Player.HasVagina())
                        AddButton(0, "Common.Button.RideHimVag".Localized(), CockatriceRideHimVag);

                    AddButton(1, "Common.Button.RideHimAnal".Localized(), CockatriceRideHimAnal);

                    if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                        AddButton(2, "Common.Button.ButtFuck".Localized(), CockatriceButtfuck);

                    if (Player.HasCock())
                        AddButton(3, "Common.Button.OralCock".Localized(), CockatriceOralCock);

                    if (Player.HasVagina())
                        AddButton(4, "Common.Button.OralVag".Localized(), CockatriceOralVag);
                }
                else
                {
                    if (Player.IsTaur() && (Player.HasVagina() || Player.HasCock()))
                        AddButton(5, "Common.Button.TaurSex".Localized(), CockatriceTaurButtFuck);

                    if (Player.IsDrider() && (Player.HasVagina() || Player.HasCock()))
                        AddButton(5, "Common.Button.DriderSex".Localized(), CockatriceDriderButtFuck);
                }
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void CockatriceRideHimVag()
        {
            ClearOutput();

            RideCockatriceForeplay();

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.BeggingFace".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.HissesMelodic".Localized());

            ShowImage("cockatrice-ridehim-vag");

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.TaperedBumpy".Localized((noFur ? "skin" : "scales"), (noFur ? "mouth" : "beak")));

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.RisesUpLength".Localized());

            if (Player.Vaginas[0].Wetness >= 2)
                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SquelchingWord".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SlappingWord".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.NoisesCloser".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.OtherNipples".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.OtherNipple".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SucksPinching".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.YourWord".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.BallsChurn".Localized());

                OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.LetsLoose".Localized());

                if (Player.CumQ() <= 150)
                    OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.FewSquirts".Localized());
                else
                    if (Player.CumQ() <= 350)
                        OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SquirtAfterSquirt".Localized());
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SeveralRopes".Localized());
                        else
                            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.SeveralRopesAgain".Localized());
            }

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.GroansUnder".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.GetDressed.Two".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.CantHelpSmile".Localized((noFur ? "nose" : "beak")));

            OutputText("Scenes.Cockatricescene.CockatriceRideHimVag.DoThisAgain".Localized());

            Player.Orgasm("Vaginal");

            Player.KnockUp(PregnancyIds.Cockatrice, PregnancyIds.CockatriceIncubation, 100, 1);

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceRideHimAnal()
        {
            ClearOutput();

            RideCockatriceForeplay();

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.RoundedPupils".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.LitheMuscle".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.SmirksRaisesHips".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.HissesMelodic".Localized());

            ShowImage("cockatrice-ridehim-anal");

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.TipSlidesIn".Localized());

            if (Player.Anal.Looseness <= 1)
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.StretchesPassage".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.CantHelpMoan".Localized());

            if (Player.Anal.Looseness <= 1)
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.FeelFull".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.EnjoyHisSize".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.HandsOnAss".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.BeginsAscent".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.WhinesClaws".Localized());

            if (Player.HasCock() || Player.HasVagina())
            {
                if (Player.HasVagina())
                    OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.PussySoaking".Localized());

                if (Player.HasCock() && Player.HasVagina())
                    OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.AndWord".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.CockSpurting".Localized((noFur ? "" : " scaled")));

                    if (Player.CumQ() < 150)
                        OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.SmallLoad".Localized());
                    else
                        if (Player.CumQ() < 350)
                            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.MediumLoad".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.LargeLoad".Localized());
                            else
                                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.ColossalLoad".Localized());
                }
            }
            else
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.WringsDry".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.EnjoyAfterglow".Localized());

            if (Player.Armor != Armor.Nothing)
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.GetsDressed".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.GrabsGear".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.ShakesHimself".Localized((noFur ? "nose" : "beak")));

            OutputText("Scenes.Cockatricescene.CockatriceRideHimAnal.DoThisAgain".Localized());

            Player.ButtChange(80, true);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceButtfuck()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.MakesWayOver".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.OrdersKnees".Localized((noFur ? "" : "under his feathers"), (noFur ? "" : "and their soft feathers")));

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.NaughtyTail".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.FollowsHips".Localized((noFur ? "" : " fluffy")));

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.SucksFingers".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.SlipsFinger".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.MemberStrains".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.ThrustsInOut".Localized());

            ShowImage("cockatrice-win-buttfuck");

            if (Player.BiggestCockLength() <= 12)
            {
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.FeedsMore".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Cockatricescene.CockatriceButtfuck.BallsAgainst".Localized((Player.Balls == 1 ? "presses" : "press"), (noFur ? "" : " fluffy"), (noFur ? "" : ", caressed by his downy plumage in a delightful way")));
            }
            else
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.FeedsMoreAgain".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.MovementShudder".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.BeginsThrust".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.SpanksAgain".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.BallsChurning".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.HeatWelling".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.LastThrust".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.BallsTightening".Localized((Player.Balls == 1 ? "it gets" : "they get")));
            else
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.HeatInferno".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.KnotSwells".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.OtherCocksSpurt".Localized((Player.Cocks.Count > 2 ? "cocks only manage" : "cock only manages")));

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.TightRing".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.CumsExplosively".Localized((noFur ? "chin" : "beak")));

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Cockatricescene.CockatriceButtfuck.OtherCocksBehind".Localized((Player.Cocks.Count > 2 ? "cocks coat" : "cock coats"), (noFur ? "" : " his ruffled feathers and")));

            OutputText("Scenes.Cockatricescene.CockatriceButtfuck.SlipsOut".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceOralCock()
        {
            ClearOutput();

            RegisterMacro("longestCockLength", () => Player.LongestCockLength() > 0 ? "1" : "");

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.CasuallyApproach".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.KnowsWhatExpect".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.TimeForFun".Localized((noFur ? "" : " fluffy"), (noFur ? "mouth" : "beak")));

            ShowImage("cockatrice-ridehim-oral");

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.CoveredInSaliva".Localized((noFur ? "" : "His beak is surprisingly soft, more like a firm rubber, and wonderfully warm and wet inside. ")));

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.MoaningWhisper".Localized((noFur ? "lips" : "beak"), (noFur ? "drips down to turn his feathers from blue to white" : "makes his feathered features turn blue to white")));

            OutputText("Scenes.Cockatricescene.CockatriceOralCock.FallsToPanting".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceOralVag()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.CasuallyApproach".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.KnowsWhatExpect".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.TimeForFun".Localized((noFur ? "" : " fluffy"), (noFur ? "mouth" : "beak"), (noFur ? "mouth" : "beak"), (noFur ? "the tip of his nose" : "the curve of his beak, the firmness against your [clit] exquisite"), (noFur ? "soft lips" : "firm beak")));

            ShowImage("cockatrice-ridehim-cunnilingus");

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.SlipsHandsAround".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.MelodicHum".Localized((noFur ? "lips" : "beak")));

            OutputText("Scenes.Cockatricescene.CockatriceOralVag.GetsFullControl".Localized((noFur ? "nose" : "beak")));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceTaurButtFuck()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.DecideWhatToDo".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.GrabbingArm".Localized((noFur ? "" : "His fluffy plumage tickles your thighs, while his scaled belly glides across your flesh. ")));

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.HissesClawsDig".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.ShowMeHowMuch".Localized());

            ShowImage("cockatrice-ridehim-taur-buttfuck");

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.QuickShiftHips".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceTaurButtFuck.StrengthReturns".Localized());

            Player.Orgasm("Anal");

            Player.ButtChange(80, true);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceDriderButtFuck()
        {
            ClearOutput();

            var CHOICE_PUSSY = 1;

            var CHOICE_COCK = 2;

            int choice = 0;

            if (Player.HasCock() && Player.HasVagina())
                choice = Random.Shared.Next(2) == 0 ? CHOICE_PUSSY : CHOICE_COCK;
            else
                choice = Player.HasVagina() ? CHOICE_PUSSY : CHOICE_COCK;

            ShowImage("cockatrice-ridehim-drider");

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.DecideWhatToDo".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.TakesByHand".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.NiceAndHard".Localized((noFur ? "" : " feathered")));

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.GentlyRock".Localized());

            if (choice == CHOICE_PUSSY)
                OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.SlipsTwoFingers".Localized());
            else
                OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.ChucklesStrokes".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.SpendMoment".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceDriderButtFuck.MovesAwayBlush".Localized());

            Player.Orgasm("Anal");

            Player.ButtChange(80, true);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RideCockatriceForeplay()
        {
            ClearOutput();

            const int FOREPLAY_NEUTRAL = 0;

            const int FOREPLAY_BLOWJOB = 1;

            const int FOREPLAY_VAGINAL = 2;

            int chooser = FOREPLAY_NEUTRAL;

            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.MakesWayOver".Localized());

            Player.BiggestBreastSize();

            if (Player.Armor != Armor.Nothing)
                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.StrippingArmor".Localized());

            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.StandingOver".Localized());

            if (Player.BiggestBreastSize() >= BreastCups.H)
                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.AtLeastTries".Localized());
            else
                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.AskingPermission".Localized());

            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.TellHimStart".Localized());

            if (Player.HasVagina() && Player.HasCock())
                chooser = Random.Shared.Next(2) == 0 ? FOREPLAY_VAGINAL : FOREPLAY_BLOWJOB;
            else
                if (Player.HasVagina())
                    chooser = FOREPLAY_VAGINAL;
                else
                    if (Player.HasCock())
                        chooser = FOREPLAY_BLOWJOB;
                    else
                        chooser = FOREPLAY_NEUTRAL;

            switch (chooser)
            {
                case FOREPLAY_VAGINAL:
                {
                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SlownessUnnatural".Localized());
                }

                break;

                case FOREPLAY_BLOWJOB:
                {
                var cockLength = Player.BiggestCockLength();

                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SlownessUnnaturalAgain".Localized((noFur ? "mouth" : "beak"), (noFur ? "" : "You're surprised by how soft his beak is, the edge not hard and sharp as you had expected, but more like a layer of firm rubber. ")));

                if (cockLength <= 5)
                    OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.WetHeatMouth".Localized((noFur ? "" : "his beak brushing against your crotch ")));
                else
                    if (cockLength <= 12)
                        OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.WetHeatMouthAgain".Localized((noFur ? "" : "his beak brushing against your crotch ")));
                    else
                        if (cockLength <= 18)
                        {
                            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.WetHeatMouthThird".Localized());

                            if (Player.HasVagina() || Player.Balls > 0)
                            {
                                if (Player.HasVagina())
                                    OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SlitDrips".Localized());

                                if (Player.Balls > 0)
                                    OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SwingRoughlyAgain".Localized());

                                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.CockSheathing".Localized());
                            }
                        }
                        else
                        {
                            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.WetHeatMouthFourth".Localized());

                            if (Player.HasVagina() || Player.Balls > 0)
                            {
                                if (Player.HasVagina())
                                    OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SlitDripsAgain".Localized());

                                if (Player.Balls > 0)
                                    OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SwingRoughlyAgain".Localized());

                                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.IncompleteThrust".Localized());
                            }

                            OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.MemberSlick".Localized());
                        }
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.SlownessUnnaturalThird".Localized());

                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.NipplesMoisten".Localized());

                OutputText("Scenes.Cockatricescene.RideCockatriceForeplay.RoughLickCircular".Localized((noFur ? "skin" : "scales")));
            }
        }

        public void LoseToCockatrice()
        {
            ClearOutput();

            if ((Player.IsTaur() || Player.IsDrider()) && Player.HasVagina())
                CockatriceLossOral();
            else
                if (Player.IsTaur() || Player.IsDrider())
                    CockatriceLossOral();
                else
                    if (Player.HasVagina())
                        CockatriceLossVaginal();
                    else
                        CockatriceLossAnal();

            CombatScene.CleanupAfterCombat();
        }

        public void CockatriceLossAnal()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceLossAnal.FallsToGround".Localized());

            ShowImage("cockatrice-loss-butt");

            OutputText("Scenes.Cockatricescene.CockatriceLossAnal.BucksHips".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossAnal.BeginsWithdraw".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossAnal.BreathyMoans".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossAnal.WithdrawsSoftening".Localized((noFur ? "" : " and feathered rump")));

            Player.ButtChange(80, true);

            Player.Orgasm("Anal");
        }

        public void CockatriceLossVaginal()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceLossVaginal.FallsToGround".Localized());

            ShowImage("cockatrice-loss-vag");

            OutputText("Scenes.Cockatricescene.CockatriceLossVaginal.RubsTip".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossVaginal.PumpsRutting".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossVaginal.TelltalePulsing".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossVaginal.FillsPussy".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true);

            Player.KnockUp(PregnancyIds.Cockatrice, PregnancyIds.CockatriceIncubation, 100, 1);

            Player.Orgasm("Vaginal");
        }

        public void CockatriceBirth()
        {

            ShowImage("birth-cockatrice");

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Cockatricescene.CockatriceBirth.TerriblePressure".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Cockatricescene.CockatriceBirth.SuddenShift".Localized(
                 Player.VaginaDescript()
            ));

            Player.CuntChange(15, true, true, false);

            OutputText("Scenes.Cockatricescene.CockatriceBirth.TurnsYouOn".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            OutputText("Scenes.Cockatricescene.CockatriceBirth.AbleToStand".Localized(
                 NumberUtils.Num2Text((float)(int)Math.Floor(Player.TotalFertility() / 7))
            ));

            OutputText("Scenes.Cockatricescene.CockatriceBirth.LightBreaks".Localized(
                 NumberUtils.Num2Text((float)(int)Math.Floor(Player.TotalFertility() / 7))
            ));

            OutputText("Scenes.Cockatricescene.CockatriceBirth.QuicklyAdapt".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Cockatricescene.CockatriceBirth.ShakenDeeply".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Cockatricescene.CockatriceBirth.PicksYourselfUp".Localized());
                else
                    OutputText("Scenes.Cockatricescene.CockatriceBirth.SoftSmileLeave".Localized());

            if (Player.Body.Hips < 25)
            {
                Player.Body.Hips += 1;

                OutputText("Scenes.Cockatricescene.CockatriceBirth.ExtraThickness".Localized());
            }
        }

        public void CockatriceLossOral()
        {
            ClearOutput();

            OutputText("Scenes.Cockatricescene.CockatriceLossOral.FallsToGround".Localized());

            ShowImage("cockatrice-loss-oral");

            OutputText("Scenes.Cockatricescene.CockatriceLossOral.RoughlyFucksFace".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossOral.TwitchesMouth".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossOral.LetsGoFace".Localized());

            OutputText("Scenes.Cockatricescene.CockatriceLossOral.ReturnToCamp".Localized());
        }

    }
}