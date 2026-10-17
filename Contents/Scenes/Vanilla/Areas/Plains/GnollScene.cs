using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Plains
{
    [Register<GnollScene>("coc:gnoll")]
    public class GnollScene : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            GnollEncounter();
        }

        public void GnollEncounter()
        {
            ClearOutput();

            ShowImage("monster-gnoll");

            OutputText("Scenes.Gnollscene.GnollEncounter.DistantLaughter".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Gnoll), () => Go<CampScene>());
        }

        public void GetRapedByGnoll()
        {
            ClearOutput();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDream)))
            {
                YoDawgIHeardULiekGNollBallzzzdahdakjldh();

                return ;
            }

            OutputText("Scenes.Gnollscene.GetRapedByGnoll.GnollSound".Localized());

            if (Player.HasCock())
            {
                OutputText(Player.MultiCockDescriptLight());

                if (Player.HasVagina())
                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.AndFragment".Localized());
            }

            if (Player.HasVagina())
                OutputText(Player.VaginaDescript(0));

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Gnollscene.GetRapedByGnoll.SentencePeriod".Localized());
            else
                OutputText("Scenes.Gnollscene.GetRapedByGnoll.FlatGroin".Localized());

            if (!Player.HasCock())
            {
                ShowImage("gnoll-fuck-vag");

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.LooksDisappointed".Localized(
                         Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(9, true, true, false);

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.ParagraphBreak".Localized());
                }
                else
                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.StopsLaughing".Localized());
            }

            if (Player.CockTotal() > 0)
            {
                ShowImage("gnoll-fuck-with-her-clit");

                if (Player.ThickestCockThickness() > 2)
                {
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.FishesInto".Localized());
                    else
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.HandsDart".Localized());

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.SaysRelish".Localized(
                         Appearance.CockNoun(Player.Cocks[Player.ThickestCock()].Id),

                         Appearance.CockNoun(Player.Cocks[Player.ThickestCock()].Id)
                    ));

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.GaspsFeeling".Localized());
                }
                else
                {
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.SaysBarestTrace".Localized());
                    else
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.SaysGrabs".Localized());

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.RunsHandUp".Localized(
                         Appearance.CockNoun(Player.Cocks[Player.ThickestCock()].Id)
                    ));

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.ScreamsPain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.BallsWord".Localized());
                    else
                        OutputText("Scenes.Gnollscene.GetRapedByGnoll.GroinWord".Localized());

                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.OnlyCanLaugh".Localized());

                    if (Player.Cocks.Count > 0) Player.Cocks[Player.ThickestCock()].Thicken(0.25f);
                }
            }

            OutputText("Scenes.Gnollscene.GetRapedByGnoll.InNoTime".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Gnollscene.GetRapedByGnoll.BouncesUpDown".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Gnollscene.GetRapedByGnoll.HipsBuck".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.SackDescript());
                else
                    OutputText("Scenes.Gnollscene.GetRapedByGnoll.BodyWord".Localized());

                OutputText("Scenes.Gnollscene.GetRapedByGnoll.SeizesTighter".Localized());

                OutputText("Scenes.Gnollscene.GetRapedByGnoll.FinallyExplode".Localized());

                OutputText("Scenes.Gnollscene.GetRapedByGnoll.GigglesBouncing".Localized());
            }
            else
                OutputText("Scenes.Gnollscene.GetRapedByGnoll.SquirmsDirt".Localized());

            OutputText("Scenes.Gnollscene.GetRapedByGnoll.SlumpsAgainst".Localized());

            OutputText("Scenes.Gnollscene.GetRapedByGnoll.WhimperBubbles".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatHyena()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Gnollscene.DefeatHyena.CollapsesBackwards".Localized());
            else
                OutputText("Scenes.Gnollscene.DefeatHyena.HeavesGiant".Localized());

            OutputText("Scenes.Gnollscene.DefeatHyena.AtYourMercy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Dickdownclit".Localized(), null);

            AddButton(1, "Common.Button.Dickinass".Localized(), null);

            AddButton(2, "Common.Button.Takeherclit".Localized(), null);

            AddButton(3, "Common.Button.Suckherclit".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.Dickdownclit".Localized(), DickDownGnollClit);

                    AddButton(1, "Common.Button.Dickinass".Localized(), DickInGnollAss);
                }

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.Takeherclit".Localized(), TakeGnollClit);

                AddButton(3, "Common.Button.Suckherclit".Localized(), SuckGnollClit);
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void DickDownGnollClit()
        {
            ClearOutput();

            ShowImage("gnoll-fuck-with-her-clit");

            OutputText("Scenes.Gnollscene.DickDownGnollClit.NotAnOpportunity".Localized());

            OutputText("Scenes.Gnollscene.DickDownGnollClit.InchByInch".Localized());

            var x= Player.BiggestCockIndex();

            if (Player.Cocks[x].Length > 9)
                OutputText("Scenes.Gnollscene.DickDownGnollClit.FeelsCockhead".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Gnollscene.DickDownGnollClit.FeelsEndOf".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Gnollscene.DickDownGnollClit.BeginsRoll".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Gnollscene.DickDownGnollClit.DoesntTakeLong".Localized());

            OutputText("Scenes.Gnollscene.DickDownGnollClit.QuickAndHard".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Gnollscene.DickDownGnollClit.ConvulsesPumping".Localized(
                     Player.CockDescript(x)
                ));
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Gnollscene.DickDownGnollClit.ConvulsesCumInside".Localized(
                         Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Gnollscene.DickDownGnollClit.ConvulsesPouring".Localized(
                         Player.CockDescript(x)
                    ));

            OutputText("Scenes.Gnollscene.DickDownGnollClit.LustyGroan".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void DickInGnollAss()
        {
            ClearOutput();

            ShowImage("gnoll-win-fuck-her-butt");

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            var y= Player.CockThatFits2(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Gnollscene.DickInGnollAss.MustBeTaught".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Cocks.Count > 1 && Player.CockArea(x) < CombatController.Instance!.Context.Enemy.AnalCapacity() && y != -1)
                {
                    OutputText("Scenes.Gnollscene.DickInGnollAss.PoundsAway".Localized(
                         Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.Cocks.Count > 2 && Player.CockArea(x) + Player.CockArea(y) < CombatController.Instance!.Context.Enemy.AnalCapacity())
                        OutputText("Scenes.Gnollscene.DickInGnollAss.FewThrustsMore".Localized());

                    if (Player.CockTotal() > 3 && Player.CockArea(0) + Player.CockArea(1) + Player.CockArea(2) < CombatController.Instance!.Context.Enemy.AnalCapacity())
                        OutputText("Scenes.Gnollscene.DickInGnollAss.SomeTime".Localized(
                             Player.CockDescript(y)
                        ));

                    if (Player.CockTotal() > 4 && Player.CockArea(0) + Player.CockArea(1) + Player.CockArea(2) + Player.CockArea(3) < CombatController.Instance!.Context.Enemy.AnalCapacity())
                        OutputText("Scenes.Gnollscene.DickInGnollAss.AndThenNext".Localized());

                    if (Player.CockTotal() > 5 && Player.CockArea(0) + Player.CockArea(1) + Player.CockArea(2) + Player.CockArea(4) + Player.CockArea(5) < CombatController.Instance!.Context.Enemy.AnalCapacity())
                        OutputText("Scenes.Gnollscene.DickInGnollAss.AndTheNext".Localized());

                    if (Player.CockTotal() > 6 && Player.CockArea(0) + Player.CockArea(1) + Player.CockArea(2) + Player.CockArea(4) + Player.CockArea(5) + Player.CockArea(6) < CombatController.Instance!.Context.Enemy.AnalCapacity())
                        OutputText("Scenes.Gnollscene.DickInGnollAss.NextUntilAll".Localized());

                    OutputText("Scenes.Gnollscene.DickInGnollAss.ParagraphBreak".Localized());
                }

                OutputText("Scenes.Gnollscene.DickInGnollAss.GruntPounds".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.LetsSlump".Localized());
            }
            else
            {
                OutputText("Scenes.Gnollscene.DickInGnollAss.MindMadeUp".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.StrokesEagerly".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Gnollscene.DickInGnollAss.LetsSqueal".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Gnollscene.DickInGnollAss.RemainderEndowments".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.BeginsThrust".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.ResignsHerself".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.HandsSlideDown".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Gnollscene.DickInGnollAss.VoluminousPreCum".Localized());

                OutputText("Scenes.Gnollscene.DickInGnollAss.SentencePeriodBreak".Localized());

                if (Player.CumQ() <= 250)
                {
                    OutputText("Scenes.Gnollscene.DickInGnollAss.PressingFingertips".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Gnollscene.DickInGnollAss.OtherEndowment".Localized());

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Gnollscene.DickInGnollAss.SSoakWord".Localized());
                        else
                            OutputText("Scenes.Gnollscene.DickInGnollAss.SoaksWord".Localized());

                        OutputText("Scenes.Gnollscene.DickInGnollAss.BackAndRear".Localized());
                    }

                    OutputText("Scenes.Gnollscene.DickInGnollAss.PullOutAudible".Localized());
                }
                else
                    if (Player.CumQ() <= 500)
                    {
                        OutputText("Scenes.Gnollscene.DickInGnollAss.GrippingSides".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Gnollscene.DickInGnollAss.UnleashingTorrent".Localized((noFur ? "paints" : "mats down the short fur on")));

                        OutputText("Scenes.Gnollscene.DickInGnollAss.MuscularRing".Localized(
                             Player.CockDescript(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Gnollscene.DickInGnollAss.BearsDown".Localized(
                             Player.CockDescript(x)
                        ));

                        if (Player.CockTotal() > 1)
                            if (Player.CockTotal() > 2)
                                OutputText("Scenes.Gnollscene.DickInGnollAss.RemainingEndowments".Localized());
                            else
                                OutputText("Scenes.Gnollscene.DickInGnollAss.RemainingEndowment".Localized());

                        OutputText("Scenes.Gnollscene.DickInGnollAss.AudibleSchlick".Localized());
                    }

                OutputText("Scenes.Gnollscene.DickInGnollAss.LeavesGnoll".Localized());
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void SuckGnollClit()
        {
            ClearOutput();

            ShowImage("gnoll-win-suck-her-clit");

            OutputText("Scenes.Gnollscene.SuckGnollClit.SinceFirstSaw".Localized());

            OutputText("Scenes.Gnollscene.SuckGnollClit.YelpsSurprise".Localized());

            OutputText("Scenes.Gnollscene.SuckGnollClit.OnlyTooHappy".Localized());

            OutputText("Scenes.Gnollscene.SuckGnollClit.LegsTwitch".Localized());

            OutputText("Scenes.Gnollscene.SuckGnollClit.ClitDickBegins".Localized());

            OutputText("Scenes.Gnollscene.SuckGnollClit.LeavesHerThere".Localized());

            Player.Orgasm("Lips", false);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            CombatScene.CleanupAfterCombat();
        }

        public void TakeGnollClit()
        {
            ClearOutput();

            OutputText("Scenes.Gnollscene.TakeGnollClit.RollsOnto".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.CuntChange(9, true, true, false);

            OutputText("Scenes.Gnollscene.TakeGnollClit.ParagraphBreak".Localized());

            ShowImage("gnoll-fuck-vag");

            OutputText("Scenes.Gnollscene.TakeGnollClit.SlithersLoveCanal".Localized());

            OutputText("Scenes.Gnollscene.TakeGnollClit.ShoutReach".Localized());

            OutputText("Scenes.Gnollscene.TakeGnollClit.FakeDick".Localized());

            OutputText("Scenes.Gnollscene.TakeGnollClit.GetYourFeet".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void YoDawgIHeardULiekGNollBallzzzdahdakjldh()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubusDream));

            OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.RollingLaughter".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.AndPrefix".Localized());

            OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.RoughMotions".Localized());

            OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.ProtestsTheft".Localized());

            OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.AmazonSteps".Localized());

            OutputText("Scenes.Gnollscene.YoDawgIHeardULiekGNollBallzzzdahdakjldh.GaspsStaggering".Localized((noFur ? "glistening" : "covered in a fine chocolate fuzz that glistens")));

            YoDawgHyenaBallz2();
        }

        public void YoDawgHyenaBallz2()
        {
            ClearOutput();

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.RollsOnto".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.YourOwnBalls".Localized());
            else
                OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.SweatSlickThighs".Localized());

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.AmazonGuides".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.ParagraphBreak".Localized());

            ShowImage("gnoll-fuck-butt");

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.BucksAgainst".Localized());

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.BuckingPauses".Localized((noFur ? "" : " fuzzy")));

            if (Player.Balls > 0)
                OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.DistendedSac".Localized());
            else
                OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.SoreLoins".Localized());

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.TremblesCantCum".Localized());

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz2.AgainAndAgain".Localized());

            DoNext(() => YoDawgHyenaBallz3());

            View.Time.Hours = 7;

            View.Time.Days += 1;

        }

        public void YoDawgHyenaBallz3()
        {
            ClearOutput();

            OutputText("Scenes.Gnollscene.YoDawgHyenaBallz3.FinallyFeels".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, -10f));

            CombatScene.CleanupAfterCombat();
        }

    }
}