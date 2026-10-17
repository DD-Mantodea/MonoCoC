using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<BasiliskScene>("coc:basilisk")]
    public class BasiliskScene : Scene
    {

        public override void Entry()
        {
            BasiliskGreeting();
        }

        public void BasiliskGreeting()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredBasilisk) == 0)
            {
                OutputText("Scenes.Basiliskscene.BasiliskGreeting.TrailblazingUpA".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskGreeting.HarpyFlapsIntoView".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskGreeting.TopOfThePass".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskGreeting.EveryVestigeOfWillpower".Localized((noFur ? "abdomen" : "underbelly"), (noFur ? "human-like head" : "snub head")));

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryBasilisks);

                var basilisk = EnemyRegistry.Create(EnemyIds.Basilisk);

                CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredBasilisk, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredBasilisk) + 1);

                CombatScene.Start(basilisk, () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Basiliskscene.BasiliskGreeting.NoticeLargeBoulder".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskGreeting.YouWord".Localized());

                if (!(Player.HasPerk(PerkIds.BasiliskResistance) || Player.CanUseStare()))
                    OutputText("Scenes.Basiliskscene.BasiliskGreeting.LookAwayInTime".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskGreeting.ReadyYourselfToFight".Localized());

                if (Player.HasKeyItem("Laybans"))
                    OutputText("Scenes.Basiliskscene.BasiliskGreeting.PutOnLaybans".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryBasilisks);

                CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredBasilisk, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredBasilisk) + 1);

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.Basilisk), () => Go<CampScene>());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredBasilisk, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredBasilisk) + 1);
        }

        public void DefeatBasilisk()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Basiliskscene.DefeatBasilisk.UnableToStandAnymore".Localized());
            else
                OutputText("Scenes.Basiliskscene.DefeatBasilisk.NoLongerControlHis".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Tongue".Localized(), null);

            AddButton(1, "Common.Button.AssGentle".Localized(), null);

            AddButton(2, "Common.Button.AssCruel".Localized(), null);

            AddButton(3, "Common.Button.LayEggs".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Basiliskscene.DefeatBasilisk.CertainCreatureWont".Localized());

                AddButton(0, "Common.Button.Tongue".Localized(), TongueBasiliskSmex);

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                {
                    AddButton(1, "Common.Button.AssGentle".Localized(), () => DefeatBasiliskAndAnal(false));

                    if (Player.IsCorruptEnough(66) || Player.HasPerk(PerkIds.Sadist))
                        AddButton(2, "Common.Button.AssCruel".Localized(), () => DefeatBasiliskAndAnal(true));
                }
            }

            if (Player.CanOvipositSpider())
                AddButton(3, "Common.Button.LayEggs".Localized(), DriderPCEggLaysBasilisk);

            if (Player.CanOvipositBee() && !Player.IsGenderless())
                AddButton(3, "Common.Button.LayEggs".Localized(), LayBeeEggsInABasilisk);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void TongueBasiliskSmex()
        {
            ClearOutput();

            ShowImage("monster-basilisk");

            OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.TapYourJaw".Localized());

            OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.AsYouAreTaking".Localized());

            OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.FirstThingsFirst".Localized(
                 Player.ClothedOrNakedLower(", as you tear away your clothing")
            ));

            if (Player.HasCock())
            {
                OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.DemandGrufflyPoking".Localized(
                     Player.CockDescript(0)
                ));

                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.OnceHasWrappedAround".Localized());

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.LastAsLongAs".Localized());
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.WrapsHimselfAround".Localized(
                         Player.CockDescript(0),

                         Player.CockDescript(1),

                         Player.CockDescript(1)
                    ));

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.LastAsLongAgain".Localized(
                         Player.CockDescript(1)
                    ));
                }

                OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.LookBeatificallyDown".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.TellBrusquelyAs".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.SlowlyDoesAsYou".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.FindsYourAndAfter".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.MakeBasiliskPleasureYou".Localized((noFur ? "face" : "scaly snout")));

                    OutputText("Scenes.Basiliskscene.TongueBasiliskSmex.LookBeatificallyDownAgain".Localized());
                }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatBasiliskAndAnal(bool corrupt)
        {
            ClearOutput();

            ShowImage("monster-basilisk");

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.StandOverKneeling".Localized((corrupt ? "really" : "")));

            if (corrupt)
            {
                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.WhateverHeSees".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.SlapHandOn".Localized());
            }
            else
            {
                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.WhateverHeSeesAgain".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GentlyGuideBasilisk".Localized());
            }

            if (corrupt)
            {
                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GetOffTheBasilisk".Localized(Player.CockDescript(x), (noFur ? "skin" : "scales"), (noFur ? "" : ", leathery"), Player.CockDescript(x)));

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GoingToughAtFirst".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ClearlySomethingOf".Localized());

                if (Player.Cocks[x].Length <= 13)
                {
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.SoonClappingStaccato".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.YourBallsSlapping".Localized());

                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.TakingEveryInchOf".Localized());
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ForceAsMuchIn".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.SwingHeavilyUnderneath".Localized());

                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.AtEndOfReach".Localized(
                         Player.CockDescript(x)
                    ));
                }

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.AcceptYourEvery".Localized());

                if (Player.Cocks[x].Length <= 13)
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GrippingHipsPushingEvery".Localized());
                else
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GrippingHipsPushingAs".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ContinueToFuckCreature".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ShakenOutOfHim".Localized((noFur ? "" : "scaled ")));
            }
            else
            {
                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GetOffBasiliskAgain".Localized(Player.ClothedOrNakedLower("Knowing your opponent is going nowhere, you undress at leisure, allowing your " + Player.CockDescript(x) + " to slide out and feel the fresh air. "), (noFur ? "skin" : "scales"), (noFur ? "" : ", leathery"), (Player.Strength < 60 ? "a lot of effort to move" : "some effort to move"), Player.CockDescript(x), Player.CockDescript(x)));

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ToughAtFirstAgain".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ClearlySomethingOfAgain".Localized());

                if (Player.Cocks[x].Length <= 13)
                {
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.SoonClappingStaccatoAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.YourBallsSlappingAgain".Localized());

                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.TakingEveryInchAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ForceMuchInAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.SwingHeavilyUnderneathAgain".Localized());

                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.EndOfReachAgain".Localized(
                         Player.CockDescript(x)
                    ));
                }

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.AcceptYourEveryAgain".Localized());

                if (Player.Cocks[x].Length <= 13)
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GrippingHipsEveryInch".Localized());
                else
                    OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.GrippingHipsPushing".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.FuckCreatureAgain".Localized());

                OutputText("Scenes.Basiliskscene.DefeatBasiliskAndAnal.ShakenOutAgain".Localized(
                     Player.ClothedOrNakedLower(", dress yourself,")
                ));
            }

            Player.Orgasm("Dick");

            if (corrupt)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToBasilisk()
        {
            ClearOutput();

            if (Player.Speed <= 1)
                OutputText("Scenes.Basiliskscene.LoseToBasilisk.MovingIntenselyDifficult".Localized());
            else
                if (Player.Health < 1)
                {
                    OutputText("Scenes.Basiliskscene.LoseToBasilisk.FallToHandsAnd".Localized());

                    if (Player.HasKeyItem("Laybans"))
                        OutputText("Scenes.Basiliskscene.LoseToBasilisk.RemovesYourGlassesAgain".Localized());

                    OutputText("Scenes.Basiliskscene.LoseToBasilisk.ForcesYouToLook".Localized());
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.LoseToBasilisk.CantHelpYourself".Localized());

                    if (Player.HasKeyItem("Laybans"))
                        OutputText("Scenes.Basiliskscene.LoseToBasilisk.RemovesYourGlassesAgain".Localized());

                    OutputText("Scenes.Basiliskscene.LoseToBasilisk.ForcedToLookAgain".Localized());
                }

            OutputText("Scenes.Basiliskscene.LoseToBasilisk.StareDeepIntoCreature".Localized());

            OutputText("Scenes.Basiliskscene.LoseToBasilisk.TakesSeveralMoments".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.BasiliskSlow);

            Player.ApplyStats(new DynStat(GameStats.Speed, !Player.HasPerk(PerkIds.BasiliskResistance) ? -3 : -1), new DynStat(GameStats.Lust, 399f));

            if (Player.Speed < 5 && !Player.HasPerk(PerkIds.BasiliskResistance) && !Player.CanUseStare())
            {
                BasiliskBadEnd();

                return ;
            }

            if (Player.HasVagina() && (Player.InHeat || Player.HasPerk(PerkIds.Oviposition) || Player.HasPerk(PerkIds.BasiliskWomb) || Player.Pregnancy.Type == PregnancyIds.OvielixirEggs))
                BasiliskHasVagEggStuff();
            else
                DefaultBasiliskRape();
        }

        public void DefaultBasiliskRape()
        {

            OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.WorkingBrisklyTearsOff".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.SurprisingGentleness".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.SticksSmaller".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.AnglingSickleClawAway".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.SurprisingGentlenessAgain".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Basiliskscene.DefaultBasiliskRape.StaringIntoYourEyes".Localized());

            float scene = Random.Shared.Next(5);

            if (scene == 0)
                BasiliskAdvantageNobody();
            else
                if (scene == 1)
                    BasiliskAdvantageHarpy();
                else
                    if (scene == 2)
                        BasiliskAdvantageImp();
                    else
                        if (scene == 3)
                            BasiliskAdvantageGoblin();
                        else
                            BasiliskAdvantageMinotaur();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void BasiliskHasVagEggStuff()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.BreathingHeavilyAs".Localized());

            if (Player.InHeat)
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.MoreAwareThanEver".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.SuddenlyBreaksAway".Localized());

            if (Player.Pregnancy.Incubation > 1 && Player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.LicksBulgingBelly".Localized());

            if (Player.InHeat || Player.HasPerk(PerkIds.Oviposition) || Player.HasPerk(PerkIds.BasiliskWomb))
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.LicksBellyHungrily".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.ParagraphBreak".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.GetsUpAndAgain".Localized());

            ShowImage("basilisk-vag-fuck");

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.RapsSomethingOut".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.PinionedThe".Localized());

            if (Player.Vaginas[0].Looseness < 4)
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.NotGirthyIt".Localized());
            else
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.SensationIsNot".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.RockWallBasilisk".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.RutMeansIt".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.CollectsItself".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.WhispersSomethingDryly".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.AfterAboutAnHour".Localized());

            Player.KnockUp(PregnancyIds.Basilisk, PregnancyIds.BasiliskIncubation);

            if (Player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
            {
                OutputText("Scenes.Basiliskscene.BasiliskHasVagEggStuff.WombGurglesInstinctively".Localized());

                Player.KnockUp(PregnancyIds.Basilisk, PregnancyIds.BasiliskIncubation - 150, 100, 1);
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void BasiliskBirth()
        {

            OutputText("Scenes.Basiliskscene.BasiliskBirth.NewlineBreak".Localized());

            ShowImage("birth-basilisk");

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Basiliskscene.BasiliskBirth.FeelTerriblePressure".Localized());

                Player.CreateVagina();
            }

            if (Player.HasPerk(PerkIds.BasiliskWomb))
            {
                OutputText("Scenes.Basiliskscene.BasiliskBirth.SuddenPressureInBelly".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.ThickGreenSlime".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.StartingLeakPrecum".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.FindYourselfMoved".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.AndDownpourOfHermcum".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.SentencePeriod".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.WhenAbleTo".Localized());

                OutputText(NumberUtils.Num2Text((float)(int)Math.Floor(Player.TotalFertility() / 10)));

                OutputText("Scenes.Basiliskscene.BasiliskBirth.LargeJadeColoredEggs".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) + CoC.Instance.Flags.Get(GameFlagIds.BenoitGenericEggs) == 0)
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.SecondsDragBy".Localized());
                else
                {
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.SecondsDragByAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskBirth.PlaceEggBackDown".Localized());
                }

                OutputText("Scenes.Basiliskscene.BasiliskBirth.NothingElseToBe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) > 0)
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.AsWellWord".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.PlaceEggDownAgain".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.LightOfDayBreaks".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.SnarlsDisplayingFangs".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitGenericEggs) == 0)
                {
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.ApprehensivelyExplain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskBirth.SaysHeavilyBreathe".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) + CoC.Instance.Flags.Get(GameFlagIds.BenoitGenericEggs) == 0)
                    {
                        OutputText("Scenes.Basiliskscene.BasiliskBirth.FeelsAroundClutter".Localized());

                        OutputText("Scenes.Basiliskscene.BasiliskBirth.SpendTimeArranging".Localized());

                        OutputText("Scenes.Basiliskscene.BasiliskBirth.SaysHesitantlyShrug".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Basiliskscene.BasiliskBirth.BenoitPlacesEggs".Localized());

                        OutputText("Scenes.Basiliskscene.BasiliskBirth.BlindBasiliskReaches".Localized());

                        OutputText("Scenes.Basiliskscene.BasiliskBirth.UngrySay".Localized());

                        OutputText("Scenes.Basiliskscene.BasiliskBirth.LingerLongEnough".Localized());
                    }
                }
                else
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.PutsLatestBatch".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.NewlineBreakAgain".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.BenoitGenericEggs, CoC.Instance.Flags.Get(GameFlagIds.BenoitGenericEggs) + (int)Math.Floor(Player.TotalFertility() / 10));
            }
            else
            {
                OutputText("Scenes.Basiliskscene.BasiliskBirth.SuddenShiftInWeight".Localized());

                Player.CuntChange(20, true, true, false);

                OutputText("Scenes.Basiliskscene.BasiliskBirth.ExperienceTurnsYouOn".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

                OutputText("Scenes.Basiliskscene.BasiliskBirth.SlowlyDragYourself".Localized((noFur ? "" : " reptilian")));

                OutputText("Scenes.Basiliskscene.BasiliskBirth.InFrontOfYou".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskBirth.QuicklyAdaptTo".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Basiliskscene.BasiliskBirth.FindYourselfShaken".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Basiliskscene.BasiliskBirth.PickYourselfUp".Localized());
                    else
                        OutputText("Scenes.Basiliskscene.BasiliskBirth.SoftSmileYouGet".Localized());
            }

            OutputText("Scenes.Basiliskscene.BasiliskBirth.NewlineBreakThird".Localized());
        }

        public void BasiliskBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.MovingIntenselyDifficult".Localized());

            ShowImage("badend-basilisk-statue");

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.StareDeepIntoCreature".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Basiliskscene.BasiliskBadEnd.MoanThroughStillThroat".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Basiliskscene.BasiliskBadEnd.MoanStillThroatAgain".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Basiliskscene.BasiliskBadEnd.MoanStillThroatThird".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.EveryEroticThing".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.SuddenlyStopsSteps".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.HoursGoByNight".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.PaleBlueFingerTraces".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskBadEnd.NoVoiceAndYou".Localized());

            GameOver();
        }

        public void BasiliskAdvantageNobody()
        {

            ShowImage("badend-basilisk-statue");

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageNobody.TimeStretchesAgonizingly".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageNobody.GushingOverwhelmingJoy".Localized());
        }

        public void BasiliskAdvantageImp()
        {

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.TimeStretchesAgonizingly".Localized());

            ShowImage("monster-imp");

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.WhirringOfSmall".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.EvilCreatureWastesNo".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.SufferThisIndignity".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.CantPleasureTheImp".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageImp.AfterManyHours".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();
        }

        public void BasiliskAdvantageHarpy()
        {

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.TimeStretchesAgonizingly".Localized());

            ShowImage("monster-harpy");

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.ShrillCryFrom".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.HarpyEyesZeroIn".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.WonderVaguelyHow".Localized());

                if (Player.IsBiped())
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.TeeterHorribly".Localized());
                else
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.SlowlyAndHorribly".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.UnableToBrace".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.BangOnTheHead".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.PluralS".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.SexDriveOrWhat".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.InabilityToSatisfy".Localized());
                else
                {
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.ThreeOfThemEventually".Localized(
                         Player.CockDescript(1)
                    ));

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.CrowdedOutByBodies".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.OtherTwoBeginTo".Localized());
                }

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.EventuallyNoticeDick".Localized());

                Player.ChangeFatigue(20);

                Npcs.SophieLuststick.Apply(20);

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.HarpyComesToHalt".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.SuddenlyClosesIn".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.FeelSomethingBuild".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.TakesLongTimeFor".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.ManagesToCumTwice".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.AnotherTenMinutes".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.HarpyHaltsBehindAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.SuddenlyClosesInAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.FeelSomethingBuildAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.LongToCalmAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.CumsTwiceAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageHarpy.AnotherTenMinutesAgain".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
                }
        }

        public void BasiliskAdvantageGoblin()
        {

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.TimeStretchesAgonizingly".Localized());

            ShowImage("monster-goblin");

            if (Player.HasCock())
            {
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.AtCornerOfVision".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.SheYelpsThen".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.ShePurrsYouDoubt".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.GoblinInNoHurry".Localized(
                     Player.Mf("stud", "bitch")
                ));

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.FeelHerPickUp".Localized());

                int x = Player.CockThatFits(60);

                if (x < 0)
                    x = 0;

                if (Player.CockArea(x) >= 60)
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.SheCoosWetWarmth".Localized());
                else
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.WetWarmthSwallows".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.CannotLastLong".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.GigglingVoiceReaches".Localized());

                OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.AfterSeemsLikeHours".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.CornerOfVisionAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.SheYelpsHesitantly".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.GoblinPurrsFromBehind".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.MomentLaterFeelA".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.HugeBullMan".Localized());

                    Player.ButtChange(60, true, true, false);

                    if (Player.AnalCapacity() < 60)
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.GiantCockStretches".Localized());
                    else
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.WellWornAssFits".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.PantFromExertion".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.MinotaurBallsSwell".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.MinotaurHoldsOntoYou".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0 || Player.HasPerk(PerkIds.MinotaurCumAddict))
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.IncrediblyFrustrated".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.ParagraphBreak".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.AfterAnotherThirty".Localized());

                    Player.Orgasm("Anal");

                    Player.SlimeFeed();
                }
                else
                {
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.CornerOfVisionThird".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.SheYelpsHesitantlyAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.ShePurrsDoubtAgain".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.CoosTheGoblin".Localized(
                         Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(30, true, true, false);

                    if (Player.VaginalCapacity() < 30)
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.DidntGetTheWet".Localized());
                    else
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.DidntGetWetAgain".Localized());

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.FeelGoblinCrawlOn".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageGoblin.CummingOverAndOver".Localized());

                    Player.Orgasm("Vaginal");
                }
        }

        public void BasiliskAdvantageMinotaur()
        {

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.TimeStretchesAgonizingly".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.DeepRumblingSnuffling".Localized());

            ShowImage("minotaur-loss-butt");

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.HugeBullMan".Localized());

            Player.ButtChange(60, true, true, false);

            if (Player.AnalCapacity() < 60)
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.GiantCockStretches".Localized());
            else
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.WellWornAssFits".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.PantFromExertion".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.MinotaurBallsSwell".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.SensualHighAlready".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.SensualHighAlreadyAgain".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.SensualHighAlreadyThird".Localized());
                    else
                        OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.SensualHighAlreadyFourth".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.ParagraphBreak".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.MinotaurHoldsOntoYou".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0 || Player.HasPerk(PerkIds.MinotaurCumAddict))
                OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.IncrediblyFrustrated".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Basiliskscene.BasiliskAdvantageMinotaur.AfterAnotherThirty".Localized());

            Player.Orgasm("Anal");

            Player.MinoCumAddiction(10);

            Player.SlimeFeed();
        }

        public void DriderPCEggLaysBasilisk()
        {
            ClearOutput();

            if (Player.Gender == Gender.Female)
                ShowImage("player-drider-female");
            else
                ShowImage("player-drider-male");

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.LeaningBackCarapace".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.ExhaustionWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.ArousalWord".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.StillStrivingTo".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.ReachingBehindYou".Localized());

            if (silly)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.PinningHimBetween".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.ShovingHimUp".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.PlasterTheWeb".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.FeelBreathQuicken".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.AlreadyDrippingPussy".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.AndWord".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.EachCockWord".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.HangsWord".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.JustOverHisMouth".Localized());

            if (Player.HasPerk(PerkIds.BasiliskWomb) || Player.HasPerk(PerkIds.Oviposition) || Player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.WatchWithIdleAmusement".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.FeelEggsSliding".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.SegmentedShaftWorks".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.CockWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.CuntWord".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.UpAgainstHisMouth".Localized((noFur ? "" : "scaled ")));

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.CreatureHesitatesBelow".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.WrapAroundWaitingPrick".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.BurrowIntoNeedySlit".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.CooInDelight".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.MonsterStomachBloats".Localized());

            if (Player.FertilizedEggs() == 0)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.UnPrefix".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.FertilizedOrbs".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.DickWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.CuntWordAgain".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.AgainstHisFace".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.SemenWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.FemcumWord".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.AcrossPartnerFace".Localized());

            OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.FeelHisStomach".Localized());

            if (Player.Corruption >= 50)
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.BeforeHeadingOff".Localized());
            else
                OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.ReachUpAndSlice".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void LayBeeEggsInABasilisk()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
                ShowImage("player-bee-male");
            else
                ShowImage("player-bee-herm");

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.CompletelyDestroyed".Localized());

            if (Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeSmall || Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeLarge)
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.BuzzLightlyOver".Localized());
            else
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.SaunterArrogantlyOver".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.LightlyNudgeHim".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.PainWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.ArousalWord".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.CurrentlySuffering".Localized((noFur ? "head" : "scaly visage")));

            if (Player.IsTaur() || Player.Gender == Gender.Genderless)
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.DroolMessily".Localized());
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.RemoveJustEnough".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Basiliskscene.DriderPCEggLaysBasilisk.PluralS".Localized());

                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.RubbingEnthusiastically".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.PluralS".Localized());

                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.LandsWithPitters".Localized());
                }
                else
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.TapFingerAgainst".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.VictimSquirms".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.AboveWord".Localized());
            else
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.BehindWord".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.DesperatelySquirming".Localized());

            if (Player.FertilizedEggs() != 0)
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.FertilizedOffspring".Localized());
            else
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.BurdensomeLoad".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.SentencePeriod".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.TwitchesEachTime".Localized());

            if (Player.IsTaur() || Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.NothingToDistract".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.NoNeedyCockCunt".Localized());
                else
                    if (Player.IsTaur())
                        OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.EvenIfWanted".Localized());
            }

            if (Player.HasCock())
            {
                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.YourSeepingMember".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.PluralSAgain".Localized());

                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.DemandWord".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.PluralSThird".Localized());

                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.AttentionDontDeny".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.AndInsideYourBalls".Localized());

                OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.MoreThanYouTake".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.BodyDemandingMore".Localized());

            OutputText("Scenes.Basiliskscene.LayBeeEggsInABasilisk.RemoveDepletedAppendage".Localized((noFur ? "" : " but for the scales in the way")));

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

    }
}