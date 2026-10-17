using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<RogarScene>("coc:rogar")]
    public class RogarScene : Scene
    {

        public override void Entry()
        {

        }

        public void EncounterRogarSwamp()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.RogarPhase) == 0)
            {
                OutputText("Scenes.Rogar.EncounterRogarSwamp.WanderingThroughSwamp".Localized((Player.ArmorDescript() != "gear" ? "You can feel the wet seeping through your clothes, making the fabric stick to your form. " : "")));

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryOrcs, false, true);

                if (Player.SexOrientation > 20 && (Player.SexOrientation < 50 ? Player.Libido + Player.Corruption < 120 : Player.Libido + Player.Corruption < 70))
                    OutputText("Scenes.Rogar.EncounterRogarSwamp.NodAtHimWiping".Localized());
                else
                {
                    OutputText("Scenes.Rogar.EncounterRogarSwamp.SmileSweetlyThrough".Localized());

                    if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Rogar.EncounterRogarSwamp.TailFlicksHappily".Localized());

                    OutputText("Scenes.Rogar.EncounterRogarSwamp.ShouldYouReallyWait".Localized());
                }

                AddButton(0, "Common.Button.Yes".Localized(), WaitForChunkyOrcLoe);

                AddButton(1, "Common.Button.No".Localized(), DontWaitForRogar);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RogarPhase) == 1)
                {
                    OutputText("Scenes.Rogar.EncounterRogarSwamp.MapInHandWander".Localized());

                    if (Player.Intelligence <= 30)
                    {
                        OutputText("Scenes.Rogar.EncounterRogarSwamp.HoursPassWithEfforts".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.OneComesOutTop".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.GitSay".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.EyesWidenRecognize".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.SnickersLettingYouGo".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.NodExplainingSituation".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Rogar.EncounterRogarSwamp.TakesSomeTimeBut".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.CallOutIntoHut".Localized());

                        if (Player.Libido >= 50)
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.YourLoinsBurn".Localized());
                        else
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.YourNoseCrinkles".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.AtRemainsOfRogar".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.DoorSlamsOpen".Localized());
                    }

                    OutputText("Scenes.Rogar.EncounterRogarSwamp.ExtendsHisArmsOut".Localized());

                    if (Player.BiggestBreastSize() >= 2 && Player.Libido >= 50)
                    {
                        OutputText("Scenes.Rogar.EncounterRogarSwamp.DrinkMoreYourMind".Localized());

                        if (Player.Intelligence <= 30)
                        {
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.FeelHeatRising".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.WitnessedHimMakeThat".Localized());

                            if (Player.HasCock())
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.FeelShiftAsRemember".Localized());

                            OutputText("Scenes.Rogar.EncounterRogarSwamp.PleasureYourselfDiscreetly".Localized());
                        }
                        else
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.SeeTheDriedCum".Localized());

                        OutputText("Scenes.Rogar.EncounterRogarSwamp.RogarLooksAtYou".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 2);

                        Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                        DoNext(() => ReturnToCamp(2));

                        return ;
                    }
                    else
                        if (Player.BiggestBreastSize() >= 2)
                        {
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.GrinAsYouChat".Localized());

                            if (Player.Intelligence < 30)
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.ApologizesForWrestling".Localized());
                            else
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.ApologizesForMessy".Localized());

                            OutputText("Scenes.Rogar.EncounterRogarSwamp.JustGrinAndShake".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 2);

                            DoNext(() => ReturnToCamp(2));

                            return ;
                        }
                        else
                            if (Player.Libido < 50)
                            {
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.GrinChattingAgain".Localized());

                                if (Player.Intelligence < 30)
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.ApologizesForWrestlingAgain".Localized());
                                else
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.ApologizesForMessyAgain".Localized());

                                OutputText("Scenes.Rogar.EncounterRogarSwamp.GrinAndShakeAgain".Localized());

                                OutputText("Scenes.Rogar.EncounterRogarSwamp.LookAroundTheHut".Localized());

                                if (Player.Intelligence < 30)
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.LeftoversFromFight".Localized());
                                else
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.MakesYouWonderWhat".Localized());

                                OutputText("Scenes.Rogar.EncounterRogarSwamp.FollowsYourGaze".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.DrinkMindWandersAgain".Localized());

                                if (Player.Intelligence < 30)
                                {
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.FeelHeatRisingAgain".Localized());

                                    if (Player.HasVagina())
                                        OutputText("Scenes.Rogar.EncounterRogarSwamp.WitnessedHimSubmit".Localized());

                                    if (Player.HasCock())
                                        OutputText("Scenes.Rogar.EncounterRogarSwamp.ShiftRememberingAgain".Localized());

                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.PleasureDiscreetly".Localized());
                                }
                                else
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.SeeDriedCumAgain".Localized());
                            }

                    SexyChoices();
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RogarPhase) == 2)
                        if (Player.BiggestBreastSize() >= 2)
                        {
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.TravelToRogars".Localized());

                            DoNext(() => ReturnToCamp(1));

                            CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 3);
                        }
                        else
                        {
                            OutputText("Scenes.Rogar.EncounterRogarSwamp.RememberingYourWay".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RogarDirt) == 2)
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.YallTookOff".Localized());

                            OutputText("Scenes.Rogar.EncounterRogarSwamp.ExtendsHandWelcoming".Localized());

                            if (Player.Libido > 50)
                            {
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.OverMuskFueled".Localized());

                                if (Player.HasCock())
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.BulgeWord".Localized());
                                else
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.NipplesWord".Localized());
                            }

                            OutputText("Scenes.Rogar.EncounterRogarSwamp.StartsFromAcross".Localized());

                            if (!SceneInstance<Places.TelAdre.TelAdre>.Get().IsAllowedInto())
                            {
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.ApologizeToRogar".Localized());

                                DoNext(() => ReturnToCamp(1));
                            }
                            else
                            {
                                OutputText("Scenes.Rogar.EncounterRogarSwamp.MentionACity".Localized());

                                if (Player.Intelligence >= 30)
                                {
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.AfterBrainstorming".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                                }
                                else
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.StumpedForBrightIdeas".Localized());

                                OutputText("Scenes.Rogar.EncounterRogarSwamp.GetsUpFromChair".Localized());

                                OutputText("Scenes.Rogar.EncounterRogarSwamp.AsksAsHePuts".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.RogarDirt) == 1)
                                {
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.LeaveRogarToFinish".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 3);

                                    DoNext(() => ReturnToCamp(1));
                                }
                                else
                                {
                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.ExitTheDoor".Localized());

                                    if (Player.Tallness < 48)
                                        OutputText("Scenes.Rogar.EncounterRogarSwamp.HandsHookUnderArmpits".Localized());

                                    if (Player.Tallness >= 84)
                                        OutputText("Scenes.Rogar.EncounterRogarSwamp.HandOnShoulderTugs".Localized());

                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.LipsPressToYours".Localized());

                                    OutputText("Scenes.Rogar.EncounterRogarSwamp.WanderTowardCamp".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                                    CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 3);

                                    DoNext(() => ReturnToCamp(1));
                                }
                            }
                        }
        }

        public void DontWaitForRogar()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.DontWaitForRogar.TimePassesNoSign".Localized());

            OutputText("Scenes.Rogar.DontWaitForRogar.FindOnlyMoreMud".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RogarDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void WaitForChunkyOrcLoe()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.RogarPhase) == 0)
            {
                OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.FeelsLikeSeveralHours".Localized());

                if (Player.Libido < 50)
                    OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.HulkingOrcGentle".Localized());
                else
                {
                    OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.SomethingAboutBurlyOrc".Localized());

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.StartsToQuiver".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.EvenYourAndFingers".Localized());
                    }
                    else
                        if (Player.HasCock())
                            OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.StirsToLife".Localized(
                                Player.SMultiCockDesc()
                            ));

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20 + Player.Sensitivity / 20)));

                    OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.ParagraphBreak".Localized());
                }

                OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.ChucklesAsHeDrops".Localized(
                    Player.Mf("feller", "lass")
                ));

                OutputText("Scenes.Rogar.WaitForChunkyOrcLoe.CanNowFindRogar".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 1);

                DoNext(() => ReturnToCamp(2));
            }
        }

        public void SexyChoices()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Ewww".Localized(), EwwwRogarIsGay);

            AddButton(1, "Common.Button.Nothnxbro".Localized(), NoSlowBroIDontWantPokeSex);

            AddButton(2, "Common.Button.LickClean".Localized(), OkayBroLetsHaveAGayCarwash);
        }

        public void EwwwRogarIsGay()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.EwwwRogarIsGay.DecliningInClippedManner".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RogarDisabled, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void NoSlowBroIDontWantPokeSex()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.NoSlowBroIDontWantPokeSex.NervousChuckle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RogarDirt, 1);

            CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 2);

            DoNext(() => ReturnToCamp(2));
        }

        public void OkayBroLetsHaveAGayCarwash()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.WithHasteRetrieveBucket".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.SetTheTowelDown".Localized((Player.ArmorDescript() != "gear" ? " beneath your clothes" : "")));

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.ShovesYourHeadDown".Localized(
                Player.FaceDescript()
            ));

            if (Player.HasLongTail() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.TailBrushesAlong".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.PressesHisFatLength".Localized((Player.ArmorDescript() != "gear" ? " under your clothes" : "")));

            if (Player.HasCock())
                OutputText(Player.SMultiCockDesc());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.YourVagina".Localized(
                        Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.YourAsshole".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.PenetratesYourMouth".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.PantsHeavilyAs".Localized());

            if (Player.Intelligence < 30)
                OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.FightBetweenHim".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.SmileWarmlyAndPlant".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.ErectionWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.PussyWord".Localized());
                else
                    OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.AssWord".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.AchesEachStep".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.DroolingWord".Localized());

            OutputText("Scenes.Rogar.OkayBroLetsHaveAGayCarwash.PartsAgainst".Localized((Player.ArmorDescript() != "gear" ? " the fabric of your clothes" : " your hand")));

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            CoC.Instance.Flags.Set(GameFlagIds.RogarDirt, 2);

            CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 2);

            if (Player.Intelligence < 30)
                DoNext(() => ReturnToCamp(2));
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void RogarThirdPhase()
        {
            ClearOutput();

            if (Player.BiggestBreastSize() >= 2)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.RogarWarning) == 0)
                {
                    OutputText("Scenes.Rogar.RogarThirdPhase.JudgingPatronFigure".Localized());

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryOrcs, false, true);

                    OutputText("Scenes.Rogar.RogarThirdPhase.GreetHimWarmly".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 1);
                }
                else
                    OutputText("Scenes.Rogar.RogarThirdPhase.RogarIsHere".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
            }
            else
            {
                OutputText("Scenes.Rogar.RogarThirdPhase.AfterTakingFewMoments".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RogarWarning) >= 1)
                    OutputText("Scenes.Rogar.RogarThirdPhase.TapOnTheShoulder".Localized());
                else
                    if (Player.Tallness >= 60)
                        OutputText("Scenes.Rogar.RogarThirdPhase.CarefullyWrapArms".Localized());
                    else
                        OutputText("Scenes.Rogar.RogarThirdPhase.GentlyTugOnFigure".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryOrcs, false, true);

                OutputText("Scenes.Rogar.RogarThirdPhase.ReturningTheGrin".Localized());

                if (Player.Body.Get<SkinPart>().Tone == "green")
                    OutputText("Scenes.Rogar.RogarThirdPhase.MuttersQuietly".Localized());

                OutputText("Scenes.Rogar.RogarThirdPhase.ParagraphBreak".Localized());

                OutputText("Scenes.Rogar.RogarThirdPhase.ArtlesslyChangingSubject".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RogarDirt) == 0)
                {
                    OutputText("Scenes.Rogar.RogarThirdPhase.ChatAWhileLonger".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 4);

                    CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 0);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RogarDirt) == 1)
                    {
                        OutputText("Scenes.Rogar.RogarThirdPhase.GivesSatisfiedHuff".Localized(
                            Player.ShortName
                        ));

                        CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 4);

                        CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 0);

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.BroBrew), 1);

ReturnToCamp(1);
                    }
                    else
                    {
                        OutputText("Scenes.Rogar.RogarThirdPhase.HeartFluttersRealize".Localized());

                        if (Player.Gender == Gender.Female || Player.Gender == Gender.Genderless)
                        {
                            OutputText("Scenes.Rogar.RogarThirdPhase.StopsOnBareMons".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RogarDisabled, 1);

                            DoNext(() => ReturnToCamp(1));
                        }
                        else
                        {
                            OutputText("Scenes.Rogar.RogarThirdPhase.GetsACaress".Localized(
                                Player.SMultiCockDesc()
                            ));

                            if (Player.Corruption < 50)
                                OutputText("Scenes.Rogar.RogarThirdPhase.BlushWord".Localized());
                            else
                                OutputText("Scenes.Rogar.RogarThirdPhase.FantasizeStrongHands".Localized());

                            OutputText("Scenes.Rogar.RogarThirdPhase.DrinkHereWhenever".Localized());

                            if (Player.HasLongTail() && Player.HasTailInsteadOfLegs())
                                OutputText("Scenes.Rogar.RogarThirdPhase.FeelYourTailFlicking".Localized());

                            OutputText("Scenes.Rogar.RogarThirdPhase.PeriodBreak".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RogarPhase, 4);

                            CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 0);

                            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                            DoNext(() => ReturnToCamp(1));
                        }
                    }
            }
        }

        public void RogarLeaveBar()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarLeaveBar.OnlyLightCasualExchange".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void RogarBarChat()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarBarChat.UndeterredByDiscomfort".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RogarDisabled, 1);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void RogarBarFlirt()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarBarFlirt.GoesWithoutSaying".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirt.CoughsAwkwardly".Localized());

            AddNextButton("Common.Button.DropIt".Localized(), RogarBarFlirtDropIt);

            AddNextButton("Common.Button.TrickHim".Localized(), RogarBarFlirtTrickHim);
        }

        public void RogarBarFlirtDropIt()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarBarFlirtDropIt.LittleToGainPressing".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void RogarBarFlirtTrickHim()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHim.FarBeIt".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHim.BoastsHeQuietsDown".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHim.SweepAwayHisRemark".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHim.OverCourseOfChat".Localized());

            DoNext(() => RogarBarFlirtTrickHimPt2());
        }

        public void RogarBarFlirtTrickHimPt2()
        {
            ClearOutput();

            var virgin = true;

            var stretched = false;

            if (Player.HasVagina())
            {
                virgin = Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin;

                stretched = Player.CuntChangeNoDisplay(25);
            }
            else
            {
                virgin = !Player.LostVirginity;

                stretched = Player.ButtChangeNoDisplay(25);
            }

            RegisterMacro("didStretch", stretched);

            RegisterMacro("wasVirgin", virgin);

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.DespiteDrunkenGuidance".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.OrcRollsOntoBack".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.FabricEasilyRemoved".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.GitOffIRather".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.AfterPullingYourselfUp".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.SlursInDisbelief".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.AgonizingMomentsSuspense".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.SaysInABreathy".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.RiseAllowingAllBut".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.MuscularManBeneathYou".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.RoarsPelvisThrusts".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.TiredAndDefeated".Localized());

            OutputText("Scenes.Rogar.RogarBarFlirtTrickHimPt2.WhenYouClimbOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            Player.Orgasm("VaginalAnal");

            DoNext(() => ReturnToCamp(1));
        }

        public void RogarPhaseFour()
        {
            ClearOutput();

            if ((Player.BiggestBreastSize() >= 2) && CoC.Instance.Flags.Get(GameFlagIds.RogarWarning) == 0)
            {
                OutputText("Scenes.Rogar.RogarPhaseFour.RogarIsDrinking".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Rogar.RogarPhaseFour.AndYourChest".Localized());

                OutputText("Scenes.Rogar.RogarPhaseFour.AgreeCautiously".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 1);

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryOrcs);

                DoNext(() => ReturnToCamp(1));
            }
            else
                if ((Player.BiggestBreastSize() >= 2) && CoC.Instance.Flags.Get(GameFlagIds.RogarWarning) == 1)
                {
                    OutputText("Scenes.Rogar.RogarPhaseFour.RogarIsHere".Localized());

                    AddNextButton("Common.Button.Flirt".Localized(), RogarBarFlirt);

                    AddNextButton("Common.Button.Chat".Localized(), RogarBarChat);

                    AddNextButton("Common.Button.Leave".Localized(), RogarLeaveBar);
                }
                else
                {
                    OutputText("Scenes.Rogar.RogarPhaseFour.TippingHisDrink".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.RogarWarning) == 1)
                        OutputText("Scenes.Rogar.RogarPhaseFour.YerLookinMuchBetter".Localized());
                    else
                        OutputText("Scenes.Rogar.RogarPhaseFour.YouWord".Localized());

                    OutputText("Scenes.Rogar.RogarPhaseFour.WaveBartenderOver".Localized());

                    if (Player.Gender == Gender.Female || Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Rogar.RogarPhaseFour.ReachesCenter".Localized());

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                    {
                        OutputText("Scenes.Rogar.RogarPhaseFour.TremblesUnderTouch".Localized(
                            Player.SMultiCockDesc()
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.RogarDirt) == 1)
                            OutputText("Scenes.Rogar.RogarPhaseFour.KnowYaProbablyAint".Localized());

                        OutputText("Scenes.Rogar.RogarPhaseFour.ClearlyInTheMood".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RogarWarning, 0);

                        RogarFuckMenu();

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                    }
                }
        }

        public void RogarFuckMenu()
        {

            var hasCock = Player.HasCock();

            ClearButtons();

            AddButton(0, "Common.Button.Getanal".Localized(), LoseButtGinity);

            AddButton(1, "Common.Button.GiveAnal".Localized(), FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim);

            AddButton(2, "Common.Button.Frot".Localized(), FrotWithRogar);

            AddButton(3, "Common.Button.Brodrink".Localized(), Brobrobrobro);

            AddButton(4, "Common.Button.Gettiedanal".Localized(), KinkyWithDaOrc);

            AddButton(5, "Common.Button.Orcrubdown".Localized(), RubdubWithDaOrcWithoutDucky);

            AddButton(6, "Common.Button.NoThanks".Localized(), NoTHanksRogarIAintGayDudeIjustLickedThatMudToBeNice);
        }

        public void NoTHanksRogarIAintGayDudeIjustLickedThatMudToBeNice()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.NoTHanksRogarIAintGayDudeIjustLickedThatMudToBeNice.SmileAtOrcBut".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void LoseButtGinity()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            if (Player.Anal.Looseness == 0)
            {
                OutputText("Scenes.Rogar.LoseButtGinity.NodEagerlyAndOrc".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Rogar.LoseButtGinity.ShylyWord".Localized());
                else
                    if (Player.Corruption > 66)
                        OutputText("Scenes.Rogar.LoseButtGinity.EagerlyWord".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.OverTheThreshold".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.BendOverTheBed".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.YanksWristsBehind".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.AbruptGruntFeel".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.AsIfTimeFreezes".Localized(
                    Player.ButtDescript()
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Rogar.LoseButtGinity.DarkGreenCockhead".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.ParagraphBreak".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.GutturalHissesEcho".Localized());

                if (Player.HasLongTail() || Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Rogar.LoseButtGinity.TailWrapsAround".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.ProducesHarshSnarls".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.AndCurlInPleasure".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.HearBottlePop".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.GroanEscapesYourLips".Localized());

                Player.ButtChange(25, true, true, false);

                OutputText("Scenes.Rogar.LoseButtGinity.AnotherWhimperEscapes".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.MindRacesWithRogar".Localized());

                if (Player.HasLongTail() || Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Rogar.LoseButtGinity.TailStillGripping".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Rogar.LoseButtGinity.ClenchSharplyAsFeel".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.SnarlsAsHeNears".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.SighOfExhaustion".Localized());
            }
            else
            {
                OutputText("Scenes.Rogar.LoseButtGinity.NodEagerlyAgain".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.CantHelpButFeel".Localized());

                if (Player.Tallness >= 84)
                    OutputText("Scenes.Rogar.LoseButtGinity.LeanDownAsRogar".Localized());
                else
                    OutputText("Scenes.Rogar.LoseButtGinity.LeanUpIntoRogar".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.FeelHisDemandingTongue".Localized((Player.ArmorDescript() != "gear" ? "[say: Let's get you out of these clothes.] His voice rumbles deep in his chest, the resonations vibrating against your skin. He takes the initiative and pulls off your garments, leaving you completely nude in his lap. " : "")));

                OutputText("Scenes.Rogar.LoseButtGinity.LeftPantingForBreath".Localized(
                    Player.ButtDescript()
                ));

                OutputText("Scenes.Rogar.LoseButtGinity.BuckOfRogar".Localized(
                    Player.SMultiCockDesc()
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Rogar.LoseButtGinity.LinesOfPreCum".Localized());

                OutputText("Scenes.Rogar.LoseButtGinity.SpunkDribblesOver".Localized(
                    Player.ButtDescript()
                ));

                if ((Player.Thickness >= 75 && Player.Tone < 33) || Player.Tone >= 75)
                {
                    OutputText("Scenes.Rogar.LoseButtGinity.RidingStiffManhood".Localized(
                        Player.AssholeDescript(),

                        Player.SMultiCockDesc()
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Rogar.LoseButtGinity.AndYourClenchTight".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.SentencePeriod".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.CurlYourFingersIn".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.BucksBecomeRagged".Localized());
                }
                else
                {
                    OutputText("Scenes.Rogar.LoseButtGinity.SnarlsAsHisArms".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.LaysOutOverChest".Localized(
                        Player.SMultiCockDesc()
                    ));

                    if (Player.HasLongTail()  || Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Rogar.LoseButtGinity.TailFlicksHelplessly".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.CryOutAsRogar".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Rogar.LoseButtGinity.BallsPullUpTight".Localized());

                    OutputText("Scenes.Rogar.LoseButtGinity.ContinuesSavageThrusts".Localized());
                }

                OutputText("Scenes.Rogar.LoseButtGinity.ExhaustedPanting".Localized((Player.ArmorDescript() != "gear" ? " beneath your garments" : "")));
            }

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.NodEagerlyAndOrc".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ReachHandUpTo".Localized());
            else
                if (Player.Tallness > 84)
                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ReachDownAndCup".Localized());

            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.TonguePressesAgainst".Localized());

            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.PushingHandsAgainst".Localized());

            if (Player.Corruption > 70 && Player.HasVagina())
                OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.FlicksTongueToward".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.Corruption > 70)
                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.FlicksTongueOver".Localized(
                        Player.MultiCockDescriptLight(),

                        Player.AssholeDescript()
                    ));

            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.PullingBackOff".Localized());

            if (Player.BiggestCockLength() < 4)
            {
                OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.AsksWithQuirked".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (Player.ShortestCockLength() > 18)
                {
                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ActuallyStutters".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.GrindsOverEveryInch".Localized(
                            Player.SMultiCockDesc()
                        ));
                    else
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.BucksAgainstRogar".Localized(
                            Player.MultiCockDescriptLight()
                        ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.SquirmsUnderYou".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ReachesDownSweatyPalm".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ParagraphBreak".Localized());
                }
                else
                {
                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ActuallyStuttersAgain".Localized());

                    if (Player.Cocks[0].Id == BodyPartTypes.Cat)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.GrindAroundInGreen".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.FlaredHeadNestles".Localized());
                        else
                            if (Player.HasKnot(0))
                                OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.PointedTipSlipsIn".Localized());
                            else
                                OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.BreakingOrcHole".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.GripHipsAndBegin".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.OtherCocksGrindOver".Localized());
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.OtherCockGrindsOver".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.SeeHowLoud".Localized());

                    if (Player.HasKnot(0))
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.KnotForcesPast".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ShortJerksOfHips".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.WaveAfterWave".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.OtherMeatRubbing".Localized());

                    OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.ParagraphBreakAgain".Localized());
                }

            OutputText("Scenes.Rogar.FuckRogarsButtPussyBoyCuntManMoundSissySlitQueerQuim.CumCoatedBody".Localized((Player.ArmorDescript() != "gear" ? " by the time you are dressed" : ""), (Player.ArmorDescript() != "gear" ? " and finish putting on your clothes" : "")));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void Brobrobrobro()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.Brobrobrobro.FollowRogarHomeAnd".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fukkingrabit".Localized(), TakeDatBroBrewFromDaBigMeanOlOrc);

            AddButton(1, "Common.Button.Reversepsych".Localized(), RogarIsDumb);
        }

        public void RogarIsDumb()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.RogarIsDumb.RelinquishTheCan".Localized());

            OutputText("Scenes.Rogar.RogarIsDumb.PlayfulMoodBroken".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeDatBroBrewFromDaBigMeanOlOrc()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BroBrew));

            var changed = false;

            var cocked = false;

            OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.ReachingForTheCan".Localized());

            OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.DipHeadDownSharply".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.FacePressesInto".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.GettingToYourFeet".Localized());

            OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.LeavingRogarsApartment".Localized());

            if (!Player.HasPerk(PerkIds.BroBody))
            {
                if (Player.Balls == 0)
                {
                    Player.Balls = 2;

                    Player.BallSize = 3;
                }

                if (Player.Intelligence > 35)
                {
                    Player.Intelligence = 35;

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -0.1f));
                }

                if (Player.Libido < 50)
                {
                    Player.Libido = 50;

                    Player.ApplyStats(new DynStat(GameStats.Libido, .1f));
                }

                OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.ParagraphBreak".Localized());

                if (Player.BiggestBreastSize() >= 1)
                {
                    if (Player.BreastRows.Count > 1)
                        while (Player.BreastRows.Count > 1)
                        {
                            Player.RemoveBreastRow(Player.BreastRows.Count - 1, 1);
                        }

                    Player.BreastRows[0].BreastRating = 0;

                    Player.BreastRows[0].NipplesPerBreast = 1;

                    Player.BreastRows[0].Fuckable = false;

                    if (Player.NippleLength > .5f)
                        Player.NippleLength = .25f;

                    Player.BreastRows[0].Lactation = 0;
                }

                if (Player.HasCock())
                {
                    if (Player.Cocks[0].Length < 10)
                    {
                        if (Player.Cocks[0].Thickness < 2.75f)
                            Player.Cocks[0].Thickness = 2.75f;

                        cocked = true;

                        Player.Cocks[0].Length = 10;
                    }

                    if (Player.Balls == 0)
                    {
                        Player.Balls = 2;

                        Player.BallSize = 3;
                    }
                }

                if (Player.HasVagina())
                    Player.RemoveVagina(0, 1);

                if (Player.Femininity > 0)
                    Player.ModFem(0, 100);

                if (Player.Tone < 100)
                    Player.ModTone(100, 100);

                if (Player.Thickness < 100)
                    Player.ModThickness(100, 50);

                if (Player.Intelligence > 21)
                    Player.Intelligence = 21;

                Player.ApplyStats(new DynStat(GameStats.Strength, 33f), new DynStat(GameStats.Toughness, 33f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Lust, 40f));

                changed = true;

                Player.RemovePerk(PerkIds.Feeder);
            }

            if (changed)
            {
                OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.LookMuchStronger".Localized());

                if (cocked)
                    OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.SmileToYourself".Localized((Player.ArmorDescript() != "gear" ? " rubs against garments not suited to it" : " flops freely in the air")));

                OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.WhoNeedsBookSmarts".Localized());

                Player.CreatePerk(PerkIds.BroBrains, 0, 0, 0, 0);

                Player.CreatePerk(PerkIds.BroBody, 0, 0, 0, 0);

                OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.BroBodyPerkGained".Localized());

                OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.BroBrainsPerkGained".Localized());

                if (Player.HasPerk(PerkIds.Feeder))
                {
                    OutputText("Scenes.Rogar.TakeDatBroBrewFromDaBigMeanOlOrc.PerkLostFeeder".Localized());

                    Player.RemovePerk(PerkIds.Feeder);
                }
            }

            Player.Orgasm("Dick");

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void FrotWithRogar()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            OutputText("Scenes.Rogar.FrotWithRogar.AllowToBeLed".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Rogar.FrotWithRogar.LengthsStiffenAgainst".Localized(
                    Player.MultiCockDescriptLight(),

                    Player.MultiCockDescriptLight(),

                    Player.MultiCockDescriptLight()
                ));
            else
            {
                OutputText("Scenes.Rogar.FrotWithRogar.CockPressesTight".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Rogar.FrotWithRogar.SlapAudiblyAgainst".Localized());

                OutputText("Scenes.Rogar.FrotWithRogar.EachThrustSlicker".Localized());
            }

            OutputText("Scenes.Rogar.FrotWithRogar.PairOfYouPant".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KinkyWithDaOrc()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            OutputText("Scenes.Rogar.KinkyWithDaOrc.NodEagerlyAndOrc".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.ShylyWord".Localized());
            else
                if (Player.Corruption > 66)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.EagerlyWord".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.OverTheThreshold".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.BendOverTheBed".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.YanksWristsBehind".Localized());

            if (Player.Libido < 33)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.FeignedConfusionWord".Localized());
            else
                if (Player.Libido > 66)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.ClearLustWord".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.AtRogarOnlySee".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.AbruptGruntFeel".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.YoureSoTight".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.FavoriteBoyReady".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.RollsYouOverBy".Localized(
                Player.ButtDescript()
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.DarkGreenCockhead".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.ParagraphBreak".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.GutturalHissesEcho".Localized());

            if (Player.HasLongTail() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Rogar.KinkyWithDaOrc.TailWrapsAround".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.ProducesHarshSnarls".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.SomeResistanceRelentless".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.RubsOverYourWalls".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.AndCurlInPleasure".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.HearBottlePop".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.GroanEscapesYourLips".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.TightHoleTeethClench".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.WellUsedHoleMoan".Localized());

            Player.ButtChange(25, true, true, false);

            OutputText("Scenes.Rogar.KinkyWithDaOrc.AnotherWhimperEscapes".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.CantHelpButFocus".Localized());

            if (Player.HasLongTail() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Rogar.KinkyWithDaOrc.TailStillGripping".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.ClenchSharplyAsFeel".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Rogar.KinkyWithDaOrc.SnarlsAsHeNears".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Rogar.KinkyWithDaOrc.VoiceChestRumble".Localized());

            OutputText("Scenes.Rogar.KinkyWithDaOrc.SighOfExhaustion".Localized((Player.ArmorDescript() != "gear" ? " beneath your garments" : "")));

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void RubdubWithDaOrcWithoutDucky()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RogarFuckedToday, 1);

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.ByTheTimeReach".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.OrcLoverMotionsTo".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.BottlesPopOpen".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.SkinUnderYour".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.GruntsAsYouWork".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.AskToFlip".Localized());

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.WithoutHesitationStraddle".Localized(
                Player.ButtDescript(),

                Player.ButtDescript()
            ));

            OutputText("Scenes.Rogar.RubdubWithDaOrcWithoutDucky.DoneThisBefore".Localized(
                Player.ButtDescript()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.PlayWithPits".Localized(), SexyOrcPitsAreSexy);

            AddButton(1, "Common.Button.IgnorePits".Localized(), LickSomeMoreOrcButNoPits);
        }

        public void SexyOrcPitsAreSexy()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.LeanForwardEnticed".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.LeansBackAndLets".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.TiltOfYourHead".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.WinksPlayfully".Localized(
                Player.MultiCockDescriptLight()
            ));

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.OverflowingPoolOf".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.OneHandPumpsRogar".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.JustTooMuch".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.StrainsHisFace".Localized());

            OutputText("Scenes.Rogar.SexyOrcPitsAreSexy.BendsForwardToTug".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void LickSomeMoreOrcButNoPits()
        {
            ClearOutput();

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.TiltOfYourHead".Localized());

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.WinksPlayfully".Localized(
                Player.MultiCockDescriptLight()
            ));

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.OverflowingPoolOf".Localized());

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.OneHandPumpsRogar".Localized());

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.JustTooMuch".Localized());

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.StrainsHisFace".Localized());

            OutputText("Scenes.Rogar.LickSomeMoreOrcButNoPits.BendsForwardToTug".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

    }
}