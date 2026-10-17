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

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<BazaarScene>("coc:bazaar")]
    public class BazaarScene : Scene
    {

        public override void Entry()
        {

        }

        public void FindBazaar()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarEncountered) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BazaarEncountered, CoC.Instance.Flags.Get(GameFlagIds.BazaarEncountered) + 1);

                OutputText("Scenes.Bazaar.FindBazaar.EarthyBreezes".Localized());

                OutputText("Scenes.Bazaar.FindBazaar.RaisesWeapon".Localized());

                OutputText("Scenes.Bazaar.FindBazaar.OneGap".Localized());
            }
            else
                OutputText("Scenes.Bazaar.FindBazaar.SmellsCampfire".Localized());

            OutputText("Scenes.Bazaar.FindBazaar.DoYouApproach".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ApproachBazaarGuard);

            AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
        }

        public void ApproachBazaarGuard()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.ApproachBazaarGuard.StepsConcealment".Localized());

            if (!Player.IsCorruptEnough(33))
            {
                OutputText("Scenes.Bazaar.ApproachBazaarGuard.LeaveAtOnce".Localized());

                ClearButtons();

                AddButton(0, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), InitiateFightGuard);

                AddNextButton("Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), GuardBlowjob);
            }
            else
            {
                OutputText("Scenes.Bazaar.ApproachBazaarGuard.WelcomeBazaar".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Enter".Localized(), EnterTheBazaar);
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void EnterTheBazaar()
        {

            if (View.Time.Hours == 19 || View.Time.Hours == 20)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CountdownToNightRape, CoC.Instance.Flags.Get(GameFlagIds.CountdownToNightRape) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.CountdownToNightRape) % 4 == 0 && (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Player.Mf("m", "f") == "m")))
                {
                    NightBazaarButtfuck();

                    return ;
                }
            }

            EnterTheBazaarAndMenu();
        }

        public void InitiateFightGuard()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.InitiateFightGuard.CombatStance".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.BazaarGatekeeper), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void WinAgainstGuard()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.WinAgainstGuard.KnocksGuard".Localized());

            EnterTheBazaarAndMenu();
        }

        public void GuardBlowjob()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.GuardBlowjob.RedSkinnedGuard".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.LeaveAtOnceAgain".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.StepsCloser".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.LongMoment".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.PantsFlutter".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.LocksClasps".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.ShowPromise".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.ControlSlips".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.RuthlessThrust".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.WelcomeAtLast".Localized());

            OutputText("Scenes.Bazaar.GuardBlowjob.ClearsThroat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f), new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Libido, 3f));

            Player.SlimeFeed();

            Player.RefillHunger(10);

            DoNext(() => EnterTheBazaar());
        }

        public void EnterTheBazaarAndMenu(bool demons = true)
        {
            ClearOutput();

            ShowImage("location-bazaar");

            OutputText("Scenes.Bazaar.EnterTheBazaarAndMenu.BreezesPast".Localized((noFur ? "" : ", paws,")));

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarEntered) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BazaarEntered, 1);

                OutputText("Scenes.Bazaar.EnterTheBazaarAndMenu.BazaarAdded".Localized());
            }

            SceneInstance<FapArenaScene>.Get().FapAppearance(false);

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) == 0 && Random.Shared.Next(4) == 0 && demons)
            {
                OverHearDemonsAboutSyrena();

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Shops".Localized(), ShopMenu);

            AddButton(1, (CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) > 0 ? "Common.Text.FapArena".Localized() : "Common.Text.Tent".Localized()), () => Go<FapArenaScene>());

            if (View.Time.Hours >= 15 && View.Time.Hours <= 20)
                AddButton(5, (CoC.Instance.Flags.Get(GameFlagIds.CinnabarNumberEncounters) > 0 ? "Common.Text.Cinnabar".Localized() : "Common.Text.Rat".Localized()), SceneInstance<CinnabarScene>.Get().CinnabarAppearance(false));

            AddRowButton(1, SceneInstance<LiliumScene>.Get().GetButtonName(), SceneInstance<LiliumScene>.Get().GetButtonFunc());

            AddRowButton(1, CoC.Instance.Flags.Get(GameFlagIds.RoxanneMet) > 0 ? "Common.Text.Roxanne".Localized() : "Common.Text.Lizans".Localized(), () => SceneInstance<RoxanneScene>.Get().RoxanneAppearance());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) > 0 && CoC.Instance.Flags.Get(GameFlagIds.NiamhMovedOutCounter) == -1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 2)
                    OutputText("Scenes.Bazaar.EnterTheBazaarAndMenu.SoundsOfVoices".Localized());

                AddRowButton(1, "Niamh", () => Go<TelAdre.NiamhScene>());
            }

            if ((CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) == 1 || CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) == 2) && demons && Random.Shared.Next(10) == 0)
            {
                OutputText("Scenes.Bazaar.EnterTheBazaarAndMenu.GripingDemons".Localized());

                AddRowButton(1, "GripingDemons", OverHearDemonsAboutSyrena);
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void ShopMenu()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.ShopMenu.GreeneryWagon".Localized());

            SceneInstance<ChillySmithScene>.Get().SmithButton();

            SceneInstance<RoxanneScene>.Get().RoxanneAppearance();

            SceneInstance<BenoitScene>.Get().SetBenoitShop();

            SceneInstance<TellyScene>.Get().TellyPresence();

            ClearButtons();

            SceneInstance<BenoitScene>.Get().SetBenoitShop(true);

            AddButton(1, "Common.Button.GGarments".Localized(), () => Go<GretasGarmentsScene>());

            AddButton(2, "Common.Button.SSqueeze".Localized(), () => Go<SlipperySqueezeScene>());

            SceneInstance<ChillySmithScene>.Get().SmithButton(true);

            SceneInstance<TellyScene>.Get().TellyPresence(true);

            AddButton(14, "Common.Button.Back".Localized(), () => EnterTheBazaarAndMenu());
        }

        public void LeaveShop()
        {

            EnterTheBazaarAndMenu();

            ShopMenu();
        }

        public void OverHearDemonsAboutSyrena()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) == 0)
            {
                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.WhisperCatches".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.ClosesIn".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.FirstVoice".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.BaritoneResponse".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SultryTone".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.MaleLaughs".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SeeTheTwo".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.CuntTested".Localized());

                OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.ExplainsLot".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) == 1)
                {
                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.CollarPopping".Localized());

                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.StillSore".Localized());

                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SmilingCruelly".Localized());

                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SeemsSpeechless".Localized());

                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SlapsAss".Localized());

                    OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SlimingEverywhere".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) >= 2)
                    {
                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SeatedNearFire".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.KnockingBackBeer".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.PregnantGrowls".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.StopByWhilst".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.SureButNext".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.RisesTaunts".Localized());

                        OutputText("Scenes.Bazaar.OverHearDemonsAboutSyrena.UnwillingHelp".Localized());
                    }

            DoNext(() => EnterTheBazaar());

            CoC.Instance.Flags.Set(GameFlagIds.BazaarDemonsListenedIn, CoC.Instance.Flags.Get(GameFlagIds.BazaarDemonsListenedIn) + 1);
        }

        public void NightBazaarButtfuck()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.NightBazaarButtfuck.UneaseGrows".Localized());

            OutputText("Scenes.Bazaar.NightBazaarButtfuck.DismissesFeeling".Localized());

            OutputText("Scenes.Bazaar.NightBazaarButtfuck.GruffVoice".Localized());

            OutputText("Scenes.Bazaar.NightBazaarButtfuck.FigureBeforeYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Escape".Localized(), EscapeFromCertainRape);

            AddButton(1, "Common.Button.Wait".Localized(), WaitOnStuff);

            AddButton(2, "Common.Button.Assault".Localized(), AssaultYoRapistYo);
        }

        public void EscapeFromCertainRape()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.EscapeFromCertainRape.TearsAway".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => EnterTheBazaarAndMenu());
        }

        public void WaitOnStuff()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.WaitOnStuff.UncertaintyPlagues".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CollectSomeButtSmex);
        }

        public void AssaultYoRapistYo()
        {
            ClearOutput();

            if (Player.Strength + Random.Shared.Next(30) < 50)
            {
                OutputText("Scenes.Bazaar.AssaultYoRapistYo.TurnsTables".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), CollectSomeButtSmex);
            }
            else
            {
                OutputText("Scenes.Bazaar.AssaultYoRapistYo.HuffsBreath".Localized((noFur ? "" : ", his fur soft under your fingers"), (noFur ? "Common.Text.Man".Localized() : "Scenes.Rubi.GiveRubiATFItem.FelineWord".Localized()), (noFur ? "Common.Text.RoundTigerLikeEarsCoveredIn".Localized() : "Common.Text.OrangeFurWithBlackStripes".Localized())));

                ClearButtons();

                AddButton(14, "Common.Button.Leave".Localized(), AssaultWinAndLeave);

                if (Player.IsCorruptEnough(66))
                    AddButton(0, "Common.Button.AbuseAss".Localized(), AbuseHisAss);
            }
        }

        public void AssaultWinAndLeave()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.AssaultWinAndLeave.ShovesTiger".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => EnterTheBazaarAndMenu());
        }

        public void AbuseHisAss()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.AbuseHisAss.TongueFlicks".Localized());

            if (!Player.HasCock())
            {
                OutputText("Scenes.Bazaar.AbuseHisAss.HumiliateFool".Localized());

                OutputText("Scenes.Bazaar.AbuseHisAss.WhatsThatRacket".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => EnterTheBazaarAndMenu());

                return ;
            }

            OutputText("Scenes.Bazaar.AbuseHisAss.LittleFun".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Bazaar.AbuseHisAss.StrokesCocks".Localized());
            else
                OutputText("Scenes.Bazaar.AbuseHisAss.MilksPearl".Localized());

            OutputText("Scenes.Bazaar.AbuseHisAss.OiShout".Localized());

            if (Player.Cocks[Player.SmallestCockIndex()].Length >= 16)
            {
                OutputText("Scenes.Bazaar.AbuseHisAss.GruntFrustration".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Bazaar.AbuseHisAss.SqueezeCheeks".Localized());
                else
                    OutputText("Scenes.Bazaar.AbuseHisAss.FattestGrinds".Localized());

                OutputText("Scenes.Bazaar.AbuseHisAss.ButtGlazed".Localized());
            }

            if (!(Player.Cocks[Player.SmallestCockIndex()].Length >= 16))
            {
                OutputText("Scenes.Bazaar.AbuseHisAss.JamsCockhead".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Bazaar.AbuseHisAss.SlipsInside".Localized());
                else
                    OutputText("Scenes.Bazaar.AbuseHisAss.PushesIn".Localized());

                OutputText("Scenes.Bazaar.AbuseHisAss.BurlyWhimpers".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Bazaar.AbuseHisAss.SwatsBalls".Localized());

                OutputText("Scenes.Bazaar.AbuseHisAss.ShoutsDiscomfort".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Bazaar.AbuseHisAss.BeatingRear".Localized());

                OutputText("Scenes.Bazaar.AbuseHisAss.StirsTent".Localized());
            }

            OutputText("Scenes.Bazaar.AbuseHisAss.ContinuesUsing".Localized());

            OutputText("Scenes.Bazaar.AbuseHisAss.GoodForYouMate".Localized());

            OutputText("Scenes.Bazaar.AbuseHisAss.EnjoyingHimself".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => EnterTheBazaarAndMenu());
        }

        public void CollectSomeButtSmex()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.CrashesTable".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.NotBooze".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.MightBeBetter".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.ShutYerTraps".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.VoicesSurround".Localized((noFur ? "Common.Text.EaredManHisChestCoveredIn".Localized() : "Common.Text.CoveredInBrownFurFromHead".Localized())));

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.FollowsGaze".Localized((noFur ? "Common.Text.TigerMan".Localized() : "Common.Text.FelineFigure".Localized()), (noFur ? "Common.Text.AppearanceIsHumanExceptForFurry".Localized() : "Common.Text.FaceLooksHumanoidButHisBody".Localized())));

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.LowTone".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.CommandingVoice".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.WhatYouDoing".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.MyPayment".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.NotEnough".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.TryRelax".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.ArmorDisposed".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.EveryoneWatches".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.GrowsSlick".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.SkinHeats".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.SizesYouUp".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.WrapAround".Localized());
            else
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.RubOver".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.HandTeases".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.HardShaft".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.SLetter".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.SentencePeriod".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.NotLeftOut".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.SatyrPumps".Localized());
            else
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.FingersSlip".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.HandGone".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.SlapsStomach".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.ShouldBeFine".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.UseTheHole".Localized());

            if (Player.Libido < 50)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.FearWord".Localized());
            else
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.ExcitementWord".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.SentencePeriodAgain".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.NoWord".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.GrabsAnkles".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.CloppingHooves".Localized(
                 Player.ButtDescript()
            ));

            if (Player.Libido < 50)
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.HorrifiedWord".Localized());
            else
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.ExcitedWord".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.ExpressionStrokes".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.DraftEffects".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Bazaar.CollectSomeButtSmex.TwitchesLeaks".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.SentencePeriodThird".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.SomethingHotWet".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.BearBellows".Localized());

            OutputText("Scenes.Bazaar.CollectSomeButtSmex.AllTranspires".Localized());

            Player.Orgasm("Anal");

            ClearButtons();

            AddButton(0, "Common.Button.SuckTiger".Localized(), SuckOffATiger);

            AddButton(1, "Common.Button.SuckBear".Localized(), SuckOffABear);

            AddButton(2, "Common.Button.SuckNone".Localized(), SuckOffNone);
        }

        public void SuckOffATiger()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.SuckOffATiger.BarbedLength".Localized((noFur ? "" : " furred")));

            FinalGayFinallee(0);
        }

        public void SuckOffABear()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.SuckOffABear.OpensWide".Localized());

            OutputText("Scenes.Bazaar.SuckOffABear.BitchesKnow".Localized((noFur ? "Common.Text.Hairy".Localized() : "Common.Text.Furred".Localized())));

            FinalGayFinallee(1);
        }

        public void SuckOffNone()
        {
            ClearOutput();

            OutputText("Scenes.Bazaar.SuckOffNone.UnwillingLips".Localized());

            FinalGayFinallee(2);
        }

        public void FinalGayFinallee(int road = 0)
        {

            OutputText("Scenes.Bazaar.FinalGayFinallee.WetPop".Localized(
                 Player.ButtDescript()
            ));

            OutputText("Scenes.Bazaar.FinalGayFinallee.ZugWantHole".Localized());

            OutputText("Scenes.Bazaar.FinalGayFinallee.OrcImpales".Localized(
                 Player.ButtDescript()
            ));

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Bazaar.FinalGayFinallee.PleasuringCocks".Localized());

            if (road == 0)
            {
                OutputText("Scenes.Bazaar.FinalGayFinallee.BarbedCock".Localized());

                OutputText("Scenes.Bazaar.FinalGayFinallee.BearAdvantage".Localized(
                     Player.ButtDescript()
                ));

                OutputText("Scenes.Bazaar.FinalGayFinallee.DontLastLong".Localized((noFur ? "Common.Text.Hairy".Localized() : "Common.Text.Furred".Localized())));

                if (Player.HasCock())
                    OutputText("Scenes.Bazaar.FinalGayFinallee.HandDips".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Bazaar.FinalGayFinallee.FingersRam".Localized(
                             Player.VaginaDescript(0)
                        ));

                OutputText("Scenes.Bazaar.FinalGayFinallee.LicksCheek".Localized());

                if ((int)Player.Gender > 0)
                    OutputText("Scenes.Bazaar.FinalGayFinallee.LipsPartMoan".Localized());

                OutputText("Scenes.Bazaar.FinalGayFinallee.ThirdWave".Localized());

                OutputText("Scenes.Bazaar.FinalGayFinallee.WakesStart".Localized());

                if (Player.Gems > 1)
                {
                    OutputText("Scenes.Bazaar.FinalGayFinallee.FewGemsMissing".Localized());

                    Player.Gems -= 3 + Random.Shared.Next(4);

                    if (Player.Gems < 0)
                        Player.Gems = 0;
                }

                OutputText("Scenes.Bazaar.FinalGayFinallee.RedressesRealizing".Localized());

                Player.Orgasm("Anal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

                View.Time.Days += 1;

                View.Time.Hours = 6;

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.LustDraft), 1);

ReturnToCamp(1);

            }
            else
                if (road == 1)
                {
                    OutputText("Scenes.Bazaar.FinalGayFinallee.GirthyBearCock".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.TigerAdvantage".Localized());

                    Player.ButtChange(45, true, true, false);

                    OutputText("Scenes.Bazaar.FinalGayFinallee.HeftyGirth".Localized(Player.ButtDescript(), (noFur ? "" : " furred")));

                    OutputText("Scenes.Bazaar.FinalGayFinallee.BearLast".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Bazaar.FinalGayFinallee.HandDipsAgain".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Bazaar.FinalGayFinallee.FingersRamAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));

                    OutputText("Scenes.Bazaar.FinalGayFinallee.LicksCheekAgain".Localized());

                    if ((int)Player.Gender > 0)
                        OutputText("Scenes.Bazaar.FinalGayFinallee.LipsPartMoanAgain".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.ThirdWaveAgain".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.WakesStartAgain".Localized());

                    if (Player.Gems > 1)
                    {
                        OutputText("Scenes.Bazaar.FinalGayFinallee.FewGemsMissingAgain".Localized());

                        Player.Gems -= 3 + Random.Shared.Next(4);

                        if (Player.Gems < 0)
                            Player.Gems = 0;
                    }

                    OutputText("Scenes.Bazaar.FinalGayFinallee.RedressesRealizingAgain".Localized());

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

                    View.Time.Days += 1;

                    View.Time.Hours = 6;

                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.LustDraft), 1);

ReturnToCamp(1);

                }
                else
                {
                    OutputText("Scenes.Bazaar.FinalGayFinallee.PantLabored".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.HandsFrantically".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.WarCry".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Bazaar.FinalGayFinallee.HandDrips".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Bazaar.FinalGayFinallee.FingersDip".Localized(
                                 Player.VaginaDescript(0)
                            ));

                    if ((int)Player.Gender > 0)
                        OutputText("Scenes.Bazaar.FinalGayFinallee.LipsPartFeel".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.ThirdWaveThird".Localized());

                    OutputText("Scenes.Bazaar.FinalGayFinallee.WakesStartThird".Localized());

                    if (Player.Gems > 1)
                    {
                        OutputText("Scenes.Bazaar.FinalGayFinallee.OnlyThingMissing".Localized());

                        Player.Gems -= 3 + Random.Shared.Next(4);

                        if (Player.Gems < 0)
                            Player.Gems = 0;
                    }

                    OutputText("Scenes.Bazaar.FinalGayFinallee.RedressesQuickly".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

                    View.Time.Days += 1;

                    View.Time.Hours = 6;

                    InventoryScene.TakeItem(ItemIds.BimboLiqueur, () => ReturnToCamp(1));

                }
        }

    }
}