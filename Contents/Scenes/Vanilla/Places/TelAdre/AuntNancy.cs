using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<AuntNancy>("coc:aunt_nancy")]
public class AuntNancy : Scene
    {

        public override void Entry()
        {
            AuntNancyMenu();
        }

        public bool AuntNancyMenu(bool display = true)
        {

            if (View.Time.Hours >= 6 && View.Time.Hours <= 14)
            {
                if (display)
                {
                    OutputText("Scenes.AuntNancy.AuntNancy.BuxomWoman".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.NancyMet) == 0)
                        OutputText("Scenes.AuntNancy.AuntNancy.SheWord".Localized());
                    else
                        OutputText("Scenes.AuntNancy.AuntNancy.AuntNancyName".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancy.GlancesUpSmiles".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.NancyMet) == 0)
                        OutputText("Scenes.AuntNancy.AuntNancy.AdditionalEyes".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancy.SentencePeriod".Localized());
                }

                return true;
            }
            else
                return false;
        }

        public void InteractWithAuntNancy()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) >= 30 && View.Time.Hours == 14 && CoC.Instance.Flags.Get(GameFlagIds.NancyTimesBoned) == 0)
            {
                OutputText("Scenes.AuntNancy.InteractWithAuntNancy.EntersBar".Localized());

                OutputText("Scenes.AuntNancy.InteractWithAuntNancy.AfternoonFriend".Localized());

                OutputText("Scenes.AuntNancy.InteractWithAuntNancy.SorryBoreYou".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                ClearButtons();

                AddButton(0, "Common.Button.Agree".Localized(), TimeForAuntNancySpiderCooch);

                AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), DeclineAuntNancyMassage);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) >= 30 && View.Time.Hours >= (14 - (CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) / 30)) && CoC.Instance.Flags.Get(GameFlagIds.NancyTimesBoned) > 0)
                {
                    OutputText("Scenes.AuntNancy.InteractWithAuntNancy.LooksUp".Localized());

                    OutputText("Scenes.AuntNancy.InteractWithAuntNancy.CouldntStayAway".Localized());

                    ClearButtons();

                    if (Player.Gems < 5)
                    {
                        AddButton(0, "Common.Button.Strong".Localized(), null);

                        AddButton(1, "Common.Button.Light".Localized(), null);
                    }
                    else
                    {
                        AddButton(0, "Common.Button.Strong".Localized(), StrongStuff);

                        AddButton(1, "Common.Button.Light".Localized(), LightStuff);
                    }

                    AddButton(5, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), TimeForAuntNancySpiderCooch);

                    AddButton(14, "Common.Button.Back".Localized(), DeclineAuntNancyMassage);
                }
                else
                {
                    OutputText("Scenes.AuntNancy.InteractWithAuntNancy.TalkBartender".Localized());

                    OutputText("Scenes.AuntNancy.InteractWithAuntNancy.WelcomeWetBitch".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.NancyMet) < 1)
                        CoC.Instance.Flags.Set(GameFlagIds.NancyMet, 1);

                    ClearButtons();

                    if (Player.Gems < 5)
                    {
                        AddButton(0, "Common.Button.Strong".Localized(), null);

                        AddButton(1, "Common.Button.Light".Localized(), null);
                    }
                    else
                    {
                        AddButton(0, "Common.Button.Strong".Localized(), StrongStuff);

                        AddButton(1, "Common.Button.Light".Localized(), LightStuff);
                    }

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go<TelAdre>());
                }
        }

        public void StrongStuff()
        {
            ClearOutput();

            if (Player.Gems < 5)
            {
                OutputText("Scenes.AuntNancy.StrongStuff.AskStrongDrink".Localized());

                DoNext(() => InteractWithAuntNancy());

                return ;
            }

            Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Lust, 15f));

            Player.Gems -= 5;

            OutputText("Scenes.AuntNancy.StrongStuff.StrongAnyway".Localized());

            OutputText("Scenes.AuntNancy.StrongStuff.HeavyMead".Localized());

            OutputText("Scenes.AuntNancy.StrongStuff.PaysGems".Localized());

            if (Player.Toughness >= 30)
            {
                OutputText("Scenes.AuntNancy.StrongStuff.WentToHead".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.NancyRelationshipLevel, CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) + 10);
            }
            else
            {
                OutputText("Scenes.AuntNancy.StrongStuff.TookItOut".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.NancyRelationshipLevel, CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) + 5);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void LightStuff()
        {
            ClearOutput();

            if (Player.Gems < 5)
            {
                OutputText("Scenes.AuntNancy.LightStuff.AskLightDrink".Localized());

                DoNext(() => InteractWithAuntNancy());

                return ;
            }

            Player.Gems -= 5;

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Lust, 7f));

            OutputText("Scenes.AuntNancy.LightStuff.NotHeavy".Localized());

            if (Player.Intelligence >= 20)
                OutputText("Scenes.AuntNancy.LightStuff.ShoulderLength".Localized());

            OutputText("Scenes.AuntNancy.LightStuff.ParagraphBreak".Localized());

            OutputText("Scenes.AuntNancy.LightStuff.GivesDrink".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NancyTimesTalked) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.NancyTimesTalked, CoC.Instance.Flags.Get(GameFlagIds.NancyTimesTalked) + 1);

                OutputText("Scenes.AuntNancy.LightStuff.SettlesArms".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.NodsHesitantly".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.SaysWonderful".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.RecentExploits".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.ListensEntranced".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.PoursGlass".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.WasntAlways".Localized());

                if (Player.HumanScore() >= 4)
                    OutputText("Scenes.AuntNancy.LightStuff.LikeYouWord".Localized());
                else
                    OutputText("Scenes.AuntNancy.LightStuff.PoorLittleYara".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.OneThingLed".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.SighsDreamily".Localized());

                OutputText("Scenes.AuntNancy.LightStuff.AnotherDrink".Localized());

                if (Random.Shared.Next(2) == 0)
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1 + Random.Shared.Next(2)));

                CoC.Instance.Flags.Set(GameFlagIds.NancyRelationshipLevel, CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) + 20);
            }
            else
            {
                OutputText("Scenes.AuntNancy.LightStuff.SwapStories".Localized());

                if (Random.Shared.Next(2) == 0)
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1 + Random.Shared.Next(2)));

                CoC.Instance.Flags.Set(GameFlagIds.NancyRelationshipLevel, CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) + 20);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineAuntNancyMassage()
        {
            ClearOutput();

            OutputText("Scenes.AuntNancy.DeclineAuntNancyMassage.PolitelyTurnsDown".Localized());

            OutputText("Scenes.AuntNancy.DeclineAuntNancyMassage.RaisesHand".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TimeForAuntNancySpiderCooch()
        {
            ClearOutput();

            OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.SpiderLadyEye".Localized());

            OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.LeadsStreets".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.GropesBalls".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.PeriodBreak".Localized());

            OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.ComeIntoParlor".Localized());

            OutputText("Scenes.AuntNancy.TimeForAuntNancySpiderCooch.SkittersDoor".Localized());

            Player.Lust += 29;

            Player.ApplyStats(new DynStat(GameStats.Lust, 1f));

            DoNext(() => AuntNancyPoonPartII());
        }

        public void AuntNancyPoonPartII()
        {
            ClearOutput();

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.FollowsHome".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ClimbBackUp".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LightsMatch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NancyTimesBoned) == 0)
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LongLongTime".Localized());
            else
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ReadyForMe".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MesmerizedNod".Localized());

            if (Player.Intelligence > 40)
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SpitChemical".Localized());
            else
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.KissResponsible".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PeriodBreak".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ManyArms".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.UtterRelaxation".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TailTop".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PeriodBreakAgain".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PeelsFold".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LicksLips".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SightOfBalls".Localized());

                if (Player.BallSize < 15)
                {
                    if (Player.BallSize > 5 && Player.BallSize < 15)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TakingOne".Localized());
                    else
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.BothOfThem".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.IntoMouth".Localized());

                    if (Player.BallSize <= 5)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ThemWord".Localized());
                    else
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ItWord".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.DexterousTongue".Localized());
                }
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.KissingLicking".Localized());
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TheSpiderLady".Localized());

                if (Player.Balls > 0 && Player.HasCock())
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LiftsEquipment".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ExaminesFolds".Localized(
                     Player.VaginaDescript(0)
                ));
            }

            if (Player.HasCock())
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.CaressesCocks".Localized(
                     Player.MultiCockDescriptLight()
                ));

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.WellWellWell".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.WrapsCheeks".Localized(
                 Player.ButtDescript()
            ));

            if (Player.HasCock())
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.RisesAbove".Localized());

            var x = Player.ShortestCockIndex();

            if (Player.HasCock())
            {
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.BringsToPussy".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SighsOooooh".Localized());

                var titFucked = false;

                var y = Player.BiggestCockIndex();

                if (Player.CockTotal() >= 2)
                {
                    if (Player.Cocks[y].Length >= 22)
                    {
                        titFucked = true;

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.RidesEnthusiastically".Localized());

                        if (Player.Cocks.Count > 2)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.AnotherOneOf".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.YourWord".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.OtherCock".Localized());
                        else
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.CocksWord".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.HeavingBosom".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.RisingFalling".Localized(
                             Player.CockDescript(y)
                        ));

                        if (Player.Cocks[y].Length >= 30)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.HeadSlip".Localized());
                        else
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TongueDangle".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.StaringDown".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PillowyBosom".Localized(
                             Player.CockDescript(y)
                        ));

                        if (Player.Cocks[y].Length >= 30)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.AndMouth".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.CoosHappily".Localized());
                    }

                    if (Player.CockTotal() > 2)
                    {
                        if (Player.CockTotal() == 3)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.WrapsOtherCock".Localized());
                        else
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.HerOtherHands".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MilkingStroking".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PulsatingPrick".Localized());

                        if (Player.CockTotal() > 3)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SLetter".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ForAll".Localized());

                        if (Player.CockTotal() == 3)
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ItIs".Localized());
                        else
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TheyAre".Localized());

                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.WorthWord".Localized());
                    }
                }

                if (!titFucked && Player.BiggestLactation() >= 1)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MilkLeaks".Localized(
                         Player.NippleDescript(0),

                         Appearance.AllBreastsDescript(Player)
                    ));

                if (!titFucked || (titFucked && Player.CockTotal() <= 2))
                {
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.RemainingHands".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.KneadingBalls".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ExploringFolds".Localized(
                                 Player.VaginaDescript(0)
                            ));

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MassagingChest".Localized());
                }
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.StrokesFace".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SpotsSomething".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ThoughtsInterrupted".Localized());

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TailBeforeRising".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.TurnsYourHead".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LeftHandFree".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.FeelsWonderful".Localized());

                    if (Player.BiggestLactation() >= 1)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MilkLeaksAgain".Localized(
                             Player.NippleDescript(0),

                             Appearance.AllBreastsDescript(Player)
                        ));
                }
                else
                {
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.StrokesFaceAgain".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.SpotsSomethingAgain".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ThoughtsInterruptedAgain".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.PuzzleBeforeHer".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.ClimbsAtop".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.LeftHandFreeAgain".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.FeelsWonderfulAgain".Localized());

                    if (Player.BiggestLactation() >= 1)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartII.MilkLeaksThird".Localized(
                             Player.NippleDescript(0),

                             Appearance.AllBreastsDescript(Player)
                        ));
                }

            DoNext(() => AuntNancyPoonPartIII());
        }

        public void AuntNancyPoonPartIII()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                var x = Player.ShortestCockIndex();

                var y = Player.BiggestCockIndex();

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.PussyQuivers".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.TenseChurning".Localized());
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.BodyTensing".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ParagraphBreak".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.CumsHard".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.CumQ() >= 750)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SoonerOrLater".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ParagraphBreakAgain".Localized());

                if (Player.Cocks[y].Length >= 22 && Player.CockTotal() > 1)
                {
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.UnwrapsTongue".Localized(
                         Player.CockDescript(y),

                         Player.CockDescript(y)
                    ));

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SpasmsRopes".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ReachingCeiling".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.PeriodBreak".Localized());
                }

                if ((Player.CockTotal() > 2) && (Player.CockTotal() > 1))
                        if ((Player.Cocks[y].Length < 22 && Player.CockTotal() == 2) || (Player.Cocks[y].Length >= 22 && Player.CockTotal() == 2))
                            OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.OtherDickFires".Localized());
                        else
                            if ((Player.Cocks[y].Length < 22 && Player.CockTotal() > 2) || (Player.Cocks[y].Length >= 22 && Player.CockTotal() > 2))
                                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.RestFireOff".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ClenchesTightly".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Balls == 0 && (Player.Cocks[y].Length < 22 || (Player.Cocks[y].Length >= 22 && Player.CockTotal() <= 2)))
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SqueezingFingers".Localized());
                    else
                        OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.LongForInside".Localized());

                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SpasmsEcstasy".Localized());
                }

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.OrgasmsEnd".Localized());

                if (Player.CockTotal() >= 2 && Player.CumQ() >= 700)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.LowerBodyMess".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ParagraphBreakThird".Localized());

                if (Player.CockTotal() >= 2 && Player.CumQ() >= 700)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.RunsHandUp".Localized());
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.HandBeneath".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SnakingTongue".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.BodyWord".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ThrowsHeadBack".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.ShePurrs".Localized());
            }
            else
            {
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.PussyQuiversAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.AndWord".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.AssholeBreak".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.HowlsPleasure".Localized());

                if (Player.Wetness() >= 5 || Player.BiggestLactation() >= 1)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.HandsDrenched".Localized());
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.YourWord".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.FingersSoaked".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.SensationSubsides".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.AndWordSecond".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.WornOut".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.LiftingHand".Localized(
                         Player.VaginaDescript(0)
                    ));
            }

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.PressesLips".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.NotAChance".Localized());

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartIII.DamnedIfIDo".Localized());

            DoNext(() => AuntNancyPoonPartIV());
        }

        public void AuntNancyPoonPartIV()
        {
            ClearOutput();

            OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.TwoStickyHours".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Strength, .3f), new DynStat(GameStats.Toughness, .3f));

            Player.ChangeFatigue(35);

            CoC.Instance.Flags.Set(GameFlagIds.NancyRelationshipLevel, CoC.Instance.Flags.Get(GameFlagIds.NancyRelationshipLevel) + 20);

            if (CoC.Instance.Flags.Get(GameFlagIds.NancyTimesBoned) == 0)
            {
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.AwakensOnBed".Localized());

                if ((int)Player.Gender > 0)
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.GenitalsWord".Localized());
                else
                    OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.JawWord".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.LastHourBlur".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.SoftBreath".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.ThankYou".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.HotTears".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.PullsHeadBack".Localized());

                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.TentativelySmile".Localized());
            }
            else
                OutputText("Scenes.AuntNancy.AuntNancyPoonPartIV.WornOutSticky".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NancyTimesBoned, CoC.Instance.Flags.Get(GameFlagIds.NancyTimesBoned) + 1);

            DoNext(() => ReturnToCamp(2));
        }

    }
}