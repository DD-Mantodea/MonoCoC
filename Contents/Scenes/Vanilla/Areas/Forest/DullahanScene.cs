using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Npcs;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{

    [Register<DullahanScene>("coc:dullahan")]
    public class DullahanScene : Scene, INpc
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Dullahan)!;

        public NpcState State => Player.GetNpcState(Definition);

        public static SaveContent Save => SaveContent(SceneIds.Forest.Dullahan);

        public override void Entry()
        {

            if (CombatScene.LastEnemy is Enemy last && CombatScene.LastOutcome is { } outcome)
            {
                CombatScene.LastEnemy = null;

                CombatScene.LastOutcome = null;

                HandleCombatResult(last, outcome);

                return;
            }

            ExecEncounter();
        }

        public void HandleCombatResult(Enemy enemy, CombatOutcome outcome)
        {
            var won = outcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory;

            if (enemy is DullahanHorseEnemy)
            {
                if (won)
                    DullahanPt2();
                else
                    DullahanVictory();

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Spar))
            {
                if (won)
                    DefeatedDullahanFriendly(outcome == CombatOutcome.HpVictory);
                else
                    DefeatedDullahanVictoryFriendly();

                return;
            }

            var rude = CoC.Instance.Flags.Get(GameFlagIds.DullahanRude);

            if (rude == 2)
            {
                if (won)
                    DefeatedDullahanFinishHerOff(outcome == CombatOutcome.HpVictory);
                else
                    DullahanFinishesYouOff();

                return;
            }

            if (won)
                DefeatedDullahan(outcome == CombatOutcome.HpVictory);
            else
                DullahanVictory();
        }

        public static Action<bool> AfterCombat(Action<bool> handler) => hpVictory =>
        {
            CombatScene.LastEnemy = null;

            CombatScene.LastOutcome = null;

            handler(hpVictory);
        };

        public void Talk() => ExecEncounter();

        public void ExecEncounter()
        {

            Save.SetInt("encounterDay", View.Time.Days);

            var flags = CoC.Instance.Flags;

            if (flags.Get(GameFlagIds.DullahanRude) == 1)
                DullahanIntroRude();
            else if (flags.Get(GameFlagIds.DullahanMet) == 0)
                DullahanIntro();
            else
                DullahanIntro2();
        }

        public void DullahanIntro()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanIntro.HoovesSound".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.WalkCautiously".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.HearRustling".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.InterloperKnight".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.LongPause".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.HarbingerOfDeath".Localized());

            StartKnightCombat();
        }

        public void DullahanIntroRude()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanIntroRude.FamiliarHooves".Localized());

            OutputText("Scenes.Dullahan.DullahanIntroRude.WalkCautiously".Localized());

            OutputText("Scenes.Dullahan.DullahanIntroRude.HearRustling".Localized());

            StartKnightCombat();
        }

        public void StartKnightCombat()
        {
            var knight = EnemyRegistry.Create(EnemyIds.DullahanHorse);

            knight.OnDefeated = AfterCombat(_ => DullahanPt2());

            knight.OnWon = AfterCombat(_ => DullahanVictory());

            CombatScene.Start(knight, () => Go<CampScene>());
        }

        public void DullahanIntro2()
        {
            ClearOutput();

            var flags = CoC.Instance.Flags;

            OutputText("Scenes.Dullahan.DullahanIntro.FamiliarThunder".Localized());

            OutputText("Scenes.Dullahan.DullahanIntro.StopsRecognizes".Localized());

            if (flags.Get(GameFlagIds.TimesBeatenDullahanSpar) < 3)
            {
                OutputText("Scenes.Dullahan.DullahanIntro.MoreReceptive".Localized());

                DullMenu();

                return;
            }

            if (flags.Get(GameFlagIds.AcceptedDullRequest) == 0)
            {
                ClearOutput();

                OutputText("Scenes.Dullahan.DullahanIntro.NearClearing".Localized());

                OutputText("Scenes.Dullahan.DullahanIntro.MadeDecision".Localized());

                OutputText("Scenes.Dullahan.DullahanIntro.ExpectedOldAge".Localized());

                OutputText("Scenes.Dullahan.DullahanIntro.PleaseGoManor".Localized());

                OutputText("Scenes.Dullahan.DullahanIntro.CertainlySudden".Localized());

                AddButton(0, "Common.Button.Accept".Localized(), () => ManorChoice(1));

                AddButton(1, "Common.Button.No".Localized(), () => ManorChoice(2));

                return;
            }

            DullMenu();
        }

        public void ManorChoice(int choice)
        {
            ClearOutput();

            var flags = CoC.Instance.Flags;

            if (choice == 1)
            {
                OutputText("Scenes.Dullahan.ManorChoice.ThinkOverAccept".Localized());

                OutputText("Scenes.Dullahan.ManorChoice.HowToFind".Localized());

                flags.Set(GameFlagIds.AcceptedDullRequest, 1);
            }
            else
            {
                OutputText("Scenes.Dullahan.ManorChoice.NotReadyQuest".Localized());

                OutputText("Scenes.Dullahan.ManorChoice.UnderstandNotReady".Localized());

                flags.Set(GameFlagIds.AcceptedDullRequest, 2);
            }

            ManorIntro();
        }

        public void DullMenu()
        {
            ClearOutput();

            switch (Random.Shared.Next(4))
            {
                case 0:
                    OutputText("Scenes.Dullahan.DullMenu.LazilyBreaksBranches".Localized());

                    break;

                case 1:
                    OutputText("Scenes.Dullahan.DullMenu.BlowsHair".Localized());

                    break;

                case 2:
                    OutputText("Scenes.Dullahan.DullMenu.HorseMakesKnown".Localized());

                    break;

                default:
                    OutputText("Scenes.Dullahan.DullMenu.UncrossesLegs".Localized());

                    break;
            }

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DullMenu.ButtonAppearance".Localized(), DullAppearance);

            AddButton(1, "Scenes.Dullahan.DullMenu.ButtonTalk".Localized(), () => DullTalkMenu(null));

            if (Player.Lust >= 33)
                AddButton(2, "Scenes.Dullahan.DullMenu.ButtonSex".Localized(), AskDullSex);
            else
                AddButtonDisabled(2, "Scenes.Dullahan.DullMenu.ButtonSex".Localized(),

                    "Scenes.Dullahan.DullMenu.NotArousedEnough".Localized());

            if (Player.Health > 1)
                AddButton(3, "Scenes.Dullahan.DullMenu.ButtonSpar".Localized(), SparDull);
            else
                AddButtonDisabled(3, "Scenes.Dullahan.DullMenu.ButtonSpar".Localized(),

                    "Scenes.Dullahan.DullMenu.NoShapeToFight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenDullahanSpar) >= 3 &&
                !Player.HasPerk(PerkIds.CounterAB))

                AddButton(10, "Scenes.Dullahan.DullMenu.ButtonHerSkills".Localized(), LearnSkill);

            if (CorruptedCoven.Has(CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven), CorruptedCovenFlags.ReturnedDullahanPostdream))
                AddButton(5, "Scenes.Dullahan.DullMenu.ButtonDates".Localized(), DullDates);

            if (CoC.Instance.Flags.Get(GameFlagIds.AcceptedDullRequest) == 2)
                AddRowButton(1, "Scenes.Dullahan.DullMenu.ButtonManor".Localized(), () => ManorChoice(1));

            AddButton(14, "Scenes.Dullahan.DullMenu.ButtonLeave".Localized(), DullLeave);
        }

        public void DullAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullAppearance.TallDullahan".Localized());

            OutputText("Scenes.Dullahan.DullAppearance.ECupBreasts".Localized());

            var seenBody = Save.GetBool("seenBody");

            OutputText("Scenes.Dullahan.DullAppearance.TextFragment".Localized());

            if (seenBody)
                OutputText("Scenes.Dullahan.DullAppearance.BetweenButtCheeks".Localized());

            AddButton(0, "Scenes.Dullahan.DullAppearance.ButtonAppearance".Localized(), DullMenu);
        }

        public void DullTalkMenu(string? lastFunc)
        {
            ClearButtons();

            if (lastFunc == null)
            {
                ClearOutput();

                OutputText("Scenes.Dullahan.DullTalkMenu.ChanceToTalk".Localized());
            }

            AddTalkButton(0, "Spooking", lastFunc);

            AddTalkButton(1, "Story", lastFunc);

            if (CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) == 7 &&
                CoC.Instance.Flags.Get(GameFlagIds.AcceptedDullRequest) == 3)

                if (lastFunc == "Horse")
                    AddButtonDisabled(2, "Scenes.Dullahan.DullTalkMenu.ButtonHorseName".Localized(),

                        "Scenes.Dullahan.DullTalkMenu.AlreadyAsked".Localized());
                else
                    AddButton(2, "Scenes.Dullahan.DullTalkMenu.ButtonHorseName".Localized(), AskDullHorse);

            var flags = CoC.Instance.Flags;

            var coven = flags.Get(GameFlagIds.CorrWitchCoven);

            if (flags.Get(GameFlagIds.CodexEphraimJournal) == 7 && flags.Get(GameFlagIds.AcceptedDullRequest) == 3)
            {
                AddButton(3, "Scenes.Dullahan.DullTalkMenu.ButtonFuture".Localized(), DullahanFuture);

                AddButton(4, "Scenes.Dullahan.DullTalkMenu.ButtonCurse".Localized(), DullahanCurse);
            }

            if (CorruptedCoven.Has(coven, CorruptedCovenFlags.TalkedManorDullahan) &&
                !CorruptedCoven.Has(coven, CorruptedCovenFlags.BroughtDullahan))

                AddButton(5, "Scenes.Dullahan.DullTalkMenu.ButtonCirce".Localized(), TalkCirceOffer);

            if (CorruptedCoven.Has(coven, CorruptedCovenFlags.ReturnedDullahanPostdream))
                if (lastFunc == "Miss")
                    AddButtonDisabled(6, "Scenes.Dullahan.DullTalkMenu.ButtonMissMe".Localized(), "Scenes.Dullahan.DullTalkMenu.AlreadyAsked".Localized());
                else
                    AddButton(6, "Scenes.Dullahan.DullTalkMenu.ButtonMissMe".Localized(), AskDullMiss);

            AddButton(14, "Common.Back".Localized(), DullMenu);
        }

        public void AskDullHorse()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullAskHorse.AskAboutHorse".Localized());

            OutputText("Scenes.Dullahan.DullAskHorse.TurnsToSteed".Localized());

            OutputText("Scenes.Dullahan.DullAskHorse.ShrugDoubt".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DullahanHorseName, 1);

            DullTalkMenu("Horse");
        }

        public void AddTalkButton(int slot, string kind, string? lastFunc)
        {
            var disabled = lastFunc == kind;

            var label = kind switch
            {
                "Spooking" => "Scenes.Dullahan.DullTalkMenu.ButtonAboutSpooking".Localized(),

                _ => "Scenes.Dullahan.DullTalkMenu.ButtonStory".Localized()
            };

            if (disabled)
                AddButtonDisabled(slot, label, "Scenes.Dullahan.DullTalkMenu.AlreadyAsked".Localized());
            else if (kind == "Spooking")
                AddButton(slot, label, () => AskAboutDullSpooks());
            else
                AddButton(slot, label, () => AskAboutDullStory());
        }

        public void AskAboutDullSpooks()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.AskAboutDullSpooks.WhySpook".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullSpooks.WhyNotFun".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullSpooks.BodyMorphs".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullSpooks.GreatFun".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullSpooks.NotSureEnjoy".Localized());

            DullTalkMenu("Spooking");
        }

        public void AskAboutDullStory()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.AskAboutDullStory.AskHerStory".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.NotSureShould".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.ChosenToEnter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryOmnibusDefeated) == 1)
                OutputText("Scenes.Dullahan.AskAboutDullStory.VillageDiscovery".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.NoteworthyFights".Localized());

            var days = View.Time.Days;

            OutputText((days switch
            {
                < 30 => "Scenes.Dullahan.AskAboutDullStory.RatherRecent",

                < 50 => "Scenes.Dullahan.AskAboutDullStory.MonthLong",

                < 300 => "Scenes.Dullahan.AskAboutDullStory.MonthsLong",

                < 600 => "Scenes.Dullahan.AskAboutDullStory.YearLong",

                _ => "Scenes.Dullahan.AskAboutDullStory.YearsLong"
            }).Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.Adventure".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.BitMoreSure".Localized());

            OutputText("Scenes.Dullahan.AskAboutDullStory.TakesBySurprise".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.AskAboutDullStory.ButtonYes".Localized(), () => DullStory(true));

            AddButton(1, "Scenes.Dullahan.AskAboutDullStory.ButtonNo".Localized(), () => DullStory(false));
        }

        public void DullStory(bool miss)
        {
            ClearOutput();

            if (miss)
            {
                OutputText("Scenes.Dullahan.DullStory.BreatheDeeplyAnswer".Localized());

                OutputText("Scenes.Dullahan.DullStory.NodsSmilesShyly".Localized());
            }
            else
            {
                OutputText("Scenes.Dullahan.DullStory.RaiseBrowAnswer".Localized());

                OutputText("Scenes.Dullahan.DullStory.NodsSmilesShyly2".Localized());
            }

            OutputText("Scenes.Dullahan.DullStory.BreathesDeeply".Localized());

            OutputText("Scenes.Dullahan.DullStory.NarrowEyesHaventTold".Localized());

            OutputText("Scenes.Dullahan.DullStory.ForgettingOldLife".Localized());

            OutputText("Scenes.Dullahan.DullStory.NodUnderstanding".Localized());

            DullTalkMenu("Story");
        }

        public void DullLeave()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullLeave.LookAtNightSky".Localized());

            OutputText("Scenes.Dullahan.DullLeave.HaveGoodNight".Localized());

            ReturnToCamp(1);
        }

        public void SparDull()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.SparDull.InviteSpar".Localized());

            OutputText("Scenes.Dullahan.SparDull.GoodIdeaExcited".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.Dullahan);

            enemy.CreateStatusEffect(StatusEffectIds.Spar);

            enemy.Gems = 0;

            enemy.OnDefeated = AfterCombat(hpVictory =>
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DullahanRude) == 2)
                    DefeatedDullahanFinishHerOff(hpVictory);
                else
                    DefeatedDullahanFriendly(hpVictory);
            });

            enemy.OnWon = AfterCombat(_ =>
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DullahanRude) == 2)
                    DullahanFinishesYouOff();
                else
                    DefeatedDullahanVictoryFriendly();
            });

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void LearnSkill()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.LearnSkill.AskWhereLearned".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.ThoughtYoudNotice".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.YouNod".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.Silence".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.TeachTechnique".Localized());

            if (Player.Speed < 90)
            {
                OutputText("Scenes.Dullahan.LearnSkill.YoureSlow".Localized());

                OutputText("Scenes.Dullahan.LearnSkill.CantForceHer".Localized());

                DullMenu();

                return;
            }

            Player.ChangeFatigue(40);

            Player.CreatePerk(PerkIds.CounterAB);

            OutputText("Scenes.Dullahan.LearnSkill.ThatCanWork".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.UnsheathesSaber".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.TwoHoursLearning".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.ExhaustedExcited".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.RestSoundsGreat".Localized());

            OutputText("Scenes.Dullahan.LearnSkill.PerkCounterStance".Localized());

            ReturnToCamp(4);
        }

        public void DefeatedDullahanFriendly(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesBeatenDullahanSpar,

                CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenDullahanSpar) + 1);

            if (!hpVictory)
            {
                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.FacePurplish".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.TakeABreak".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.MoreReceptive2".Localized());

                if (Player.Lust > 33)
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.FollowAskHelp".Localized());

                        OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.CanYouJustGo".Localized(
                            Save.GetBool("talkedAboutCurse")

                                ? "Scenes.Dullahan.DefeatedDullahanFriendly.TalkedRestrain".Localized()

                                : "Scenes.Dullahan.DefeatedDullahanFriendly.NoPenetration".Localized()));

                        if (!Save.GetBool("talkedAboutCurse"))
                            if (IsSilly)
                                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.VaginaDentata2".Localized());
                            else if ((CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) & 2) != 0)
                                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.KnowWhy2".Localized());
                            else
                                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.BetterNotPry2".Localized());

                        if (Player.HasCock())
                            AddButton(0, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonThighjob".Localized(),

                                DullThighjob, "Scenes.Dullahan.DefeatedDullahanFriendly.HintThighjob".Localized());

                        if (Player.HasVagina())
                            AddButton(1, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonCunnilingus".Localized(),

                                DullahanCunnilingus2, "Scenes.Dullahan.DefeatedDullahanFriendly.HintCunnilingus".Localized());

                        if (Player.HasKeyItem(DemonicStrapOn))
                            AddButton(2, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonStrapOn".Localized(),

                                DullahanStrapOn, "Scenes.Dullahan.DefeatedDullahanFriendly.HintStrapOn".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.FollowAskHelp".Localized());

                        RegisterMacro("talkedAboutCurse", Save.GetInt("talkedAboutCurse", 0) > 0);

                        OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.CanYouJustGo2".Localized());

                        OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.OftenRefuses".Localized());

                        if (Player.Corruption + Player.CorruptionTolerance() > 60 && Player.HasCock())
                            AddButton(0, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonRape".Localized(),

                                () => DullOhYouFuckedUp(false),

                                "Scenes.Dullahan.DefeatedDullahanFriendly.HintRape".Localized());

                        AddButton(1, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonLeave".Localized(),

                            DullSexRefused, "Scenes.Dullahan.DefeatedDullahanFriendly.HintLeave".Localized());
                    }

                AddButton(2, "Scenes.Dullahan.DefeatedDullahanFriendly.ButtonNah".Localized(),

                    DullMenu, "Scenes.Dullahan.DefeatedDullahanFriendly.HintNah".Localized());

                return;
            }

            OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.DealFinalBlow".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.NodSitRest".Localized());

            if (Player.Speed >= Player.GetMaxStats("spe"))
                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.LimitAgility".Localized());
            else
                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.DontNoticeHits".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenDullahanSpar) == 4)
                OutputText("Scenes.Dullahan.DefeatedDullahanFriendly.WhileRestingNotice".Localized());

            Player.ApplyStats(new DynStat(GameStats.Speed, 2));

            DullMenu();
        }

        public void DefeatedDullahanVictoryFriendly()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DefeatedDullahanVictoryFriendly.CollapseOverwhelmed".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanVictoryFriendly.KnightApproachesScythe2".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanVictoryFriendly.MorphsSpecter2".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanVictoryFriendly.JustAsYouBelieve".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanVictoryFriendly.DuelsNotLightly".Localized());

            DullMenu();
        }

        public void DefeatedDullahan(bool hpVictory)
        {
            ClearOutput();

            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.DullahanMet, 1);

            if (flags.Get(GameFlagIds.DullahanRude) == 1)
            {
                OutputText("Scenes.Dullahan.DefeatedDullahan.LandFinalBlow".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahan.ApproachStrikeDown".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahan.ProtectSpecterPhases".Localized());

                flags.Set(GameFlagIds.DullahanRude, 2);

                ReturnToCamp(1);

                return;
            }

            if (hpVictory)
            {
                OutputText("Scenes.Dullahan.DefeatedDullahan.LandFinalBlow2".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahan.HaventBeatenMe".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahan.SufferedWorse".Localized());

                OutputText("Scenes.Dullahan.DefeatedDullahan.LeaveOrStay".Localized());

                var canKick = !Player.HasTailInsteadOfLegs() && Player.Strength >= 60;

                if (canKick)
                    OutputText("Scenes.Dullahan.DefeatedDullahan.KickHeadForest".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), ListenToDull);

                AddButton(14, "Scenes.Dullahan.DefeatedDullahan.ButtonLeave".Localized(), DontListenToDull);

                if (canKick)
                    AddButton(10, "Scenes.Dullahan.DefeatedDullahan.ButtonKick".Localized(), KickTheHead);

                return;
            }

            OutputText("Scenes.Dullahan.DefeatedDullahan.ContinuesToStare".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahan.HeadShoutsStop".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahan.HeadStaresAngrily".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahan.WhenImDone".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahan.BizarreSituation".Localized());

            var canKickLust = !Player.HasTailInsteadOfLegs() && Player.Strength >= 60;

            ClearButtons();

            if (Player.HasCock() && !Player.IsTaur() && Player.Lust >= 33)
                AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonBlowjob".Localized(), DullahanBlowjob);

            if (Player.HasVagina() && !Player.IsTaur() && Player.Lust >= 33)
                AddButton(2, "Scenes.Dullahan.DefeatedDullahan.ButtonCunnilingus".Localized(), DullahanCunnilingus);

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonWaitItOut".Localized(), WaitOutDull);

            AddButton(14, "Scenes.Dullahan.DefeatedDullahan.ButtonLeave".Localized(), DontListenToDull);

            if (canKickLust)
                AddButton(10, "Scenes.Dullahan.DefeatedDullahan.ButtonKick".Localized(), KickTheHead);
        }

        public void ListenToDull()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.ListenToDull.StareHeadUnderstand".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.TrailsOffSighing".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.BodyBack2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.LongPause".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.KnightDustsOff2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.SmirkLaugh2".Localized());

            OutputText(CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) >= 1

                ? "Scenes.Dullahan.ListenToDull.NotFirstTime2".Localized()

                : "Scenes.Dullahan.ListenToDull.FirstTimeMet2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.FidgetsFlustered2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.Distressed2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.HorseAppears2".Localized());

            OutputText("Scenes.Dullahan.ListenToDull.EnoughForOneNight2".Localized());

            ReturnToCamp(2);
        }

        public void WaitOutDull()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.WaitoutDull.StareDisembodied".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.TrailsOffMoaning".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.BodyBack".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.LongPause".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.KnightDustsOff".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.SmirkLaugh".Localized());

            OutputText(CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) >= 1

                ? "Scenes.Dullahan.WaitoutDull.NotFirstTime".Localized()

                : "Scenes.Dullahan.WaitoutDull.FirstTimeMet".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.FidgetsFlustered".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.Distressed".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.HorseAppears".Localized());

            OutputText("Scenes.Dullahan.WaitoutDull.EnoughForOneNight".Localized());

            ReturnToCamp(2);
        }

        public void DontListenToDull()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DontListenToDull.LeaveAnnoyingHead".Localized());

            ReturnToCamp(2);
        }

        public void KickTheHead()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.KickTheHead.TiredChildishRamblings".Localized());

            OutputText("Scenes.Dullahan.KickTheHead.EnoughInsanity".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DullahanRude, 1);

            ReturnToCamp(2);
        }

        public void DullahanVictory()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanVictory.CollapseOverwhelmed".Localized());

            OutputText("Scenes.Dullahan.DullahanVictory.KnightApproachesScythe".Localized());

            OutputText("Scenes.Dullahan.DullahanVictory.MorphsSpecter".Localized());

            OutputText("Scenes.Dullahan.DullahanVictorypt.ForestClosesIn".Localized());

            OutputText(Player.Intelligence >= Player.Strength

                ? "Scenes.Dullahan.DullahanVictorypt.LaughAndCry".Localized()

                : "Scenes.Dullahan.DullahanVictorypt.StrengthFades".Localized());

            OutputText("Scenes.Dullahan.DullahanVictorypt.ScytheCutsAir".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DullahanRude) == 1)
            {
                OutputText("Scenes.Dullahan.DullahanVictorypt.ReapedSoulClaimed".Localized());

                GameOver();

                return;
            }

            OutputText("Scenes.Dullahan.DullahanVictorypt.WakeScreaming".Localized());

            OutputText("Scenes.Dullahan.DullahanVictorypt.ShakeThoughts".Localized());

            ReturnToCamp(4);
        }

        public void DullahanPt2()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanPt.DealFinalStrike".Localized());

            OutputText("Scenes.Dullahan.DullahanPt.BusyGloating".Localized());

            OutputText("Scenes.Dullahan.DullahanPt.SheatheWeapons".Localized());

            OutputText("Scenes.Dullahan.DullahanPt.SpeechlessTakeOpportunity".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.Dullahan);

            enemy.OnDefeated = AfterCombat(hpVictory =>
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DullahanRude) == 2)
                    DefeatedDullahanFinishHerOff(hpVictory);
                else
                    DefeatedDullahan(hpVictory);
            });

            enemy.OnWon = AfterCombat(_ =>
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DullahanRude) == 2)
                    DullahanFinishesYouOff();
                else
                    DullahanVictory();
            });

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void DullahanFuture()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanFuture.AskFuturePlans".Localized());

            OutputText("Scenes.Dullahan.DullahanFuture.LaughsWeakly".Localized());

            OutputText("Scenes.Dullahan.DullahanFuture.SmileWithHer".Localized());

            OutputText("Scenes.Dullahan.DullahanFuture.NarrowsEyes".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DullahanFuture.ButtonBackHome".Localized(),

                () => DullahanFutureAnswer(0));

            AddButton(1, "Scenes.Dullahan.DullahanFuture.ButtonStayHere".Localized(),

                () => DullahanFutureAnswer(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                AddButton(2, "Scenes.Dullahan.DullahanFuture.ButtonRevenge".Localized(),

                    () => DullahanFutureAnswer(2));
            else
                AddButtonDisabled(2, "Scenes.Dullahan.DullahanFuture.ButtonUnknown".Localized(),

                    "Scenes.Dullahan.DullahanFuture.UnknownHint".Localized());

            AddButton(3, "Scenes.Dullahan.DullahanFuture.ButtonRuleMareth".Localized(),

                () => DullahanFutureAnswer(3));
        }

        public void DullahanFutureAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.AnswerGoHome".Localized());

                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.SimpleAnswer".Localized());

                    break;

                case 1:
                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.AnswerKeepAdventuring".Localized());

                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.KeepSpiritAlive".Localized());

                    break;

                case 2:
                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.LowerBrowFulfill".Localized());

                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.SupportPayback".Localized());

                    break;

                default:
                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.AnswerTakeDownLethice".Localized());

                    OutputText("Scenes.Dullahan.DullahanFutureAnswer.WellSureMakesSense".Localized());

                    break;
            }

            OutputText("Scenes.Dullahan.DullahanFutureAnswer.KeepWordsInMind".Localized());

            DullTalkMenu("Future");
        }

        public void DullahanCurse()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanCurse.KnowOfCurse".Localized());

            OutputText("Scenes.Dullahan.DullahanCurse.CrossesArmsSad".Localized());

            OutputText("Scenes.Dullahan.DullahanCurse.LookApprehension".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DullahanCurse.ButtonCure".Localized(), DullCurseCure);

            AddButton(1, "Scenes.Dullahan.DullahanCurse.ButtonJustice".Localized(), DullCurseJustice);
        }

        public void DullCurseCure()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullCurseCure.TellRelax".Localized());

            OutputText("Scenes.Dullahan.DullCurseCure.DetachesHeadBriefly".Localized());

            OutputText("Scenes.Dullahan.DullCurseCure.EnjoyLiving".Localized());

            OutputText("Scenes.Dullahan.DullCurseCure.HandOnShoulder".Localized());

            OutputText("Scenes.Dullahan.DullCurseCure.SmilesBack".Localized());

            DullTalkMenu("Curse");
        }

        public void DullCurseJustice()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullCurseJustice.LookDownDangerous".Localized());

            OutputText("Scenes.Dullahan.DullCurseJustice.LooksSadWeakly".Localized());

            OutputText("Scenes.Dullahan.DullCurseJustice.FightingDullahan".Localized());

            DullTalkMenu("Curse");
        }

        public void TalkCirceOffer()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.TalkCirceOffer.CirceRequest".Localized());

            OutputText("Scenes.Dullahan.TalkCirceOffer.TooLongNotices".Localized());

            OutputText("Scenes.Dullahan.TalkCirceOffer.SmootherThanExpected".Localized());

            if (Player.IsBiped())
            {
                OutputText("Scenes.Dullahan.TalkCirceOffer.ExtendsHand".Localized());

                OutputText("Scenes.Dullahan.TalkCirceOffer.LegsSpreadSaddle".Localized());

                OutputText("Scenes.Dullahan.TalkCirceOffer.DontWorry".Localized());
            }
            else
            {
                OutputText("Scenes.Dullahan.TalkCirceOffer.ExtendsHand2".Localized());

                OutputText("Scenes.Dullahan.TalkCirceOffer.SorryGoingToBe".Localized());

                if (Player.IsTaur() && Player.Speed >= 100)
                    OutputText("Scenes.Dullahan.TalkCirceOffer.LaughSmugly".Localized());
                else
                    OutputText("Scenes.Dullahan.TalkCirceOffer.FineCompromise".Localized());
            }

            ForestPlaceholder.Show("Dullahan");
        }

        public void AskDullMiss()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullAskMiss.AskIfMisses".Localized());

            OutputText("Scenes.Dullahan.DullAskMiss.SurprisedQuestion".Localized());

            OutputText("Scenes.Dullahan.DullAskMiss.NeverThoughtAfter".Localized());

            OutputText("Scenes.Dullahan.DullAskMiss.ClearThought".Localized());

            DullTalkMenu("Miss");
        }

        public void DullahanFinishesYouOff()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.FallTooWeak".Localized());

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.WalksSheathing".Localized());

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.GroanAttemptUp".Localized());

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.FrownsBladeWobbles".Localized());

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.WhimpersSheathes".Localized());

            OutputText("Scenes.Dullahan.DullahanFinishesYouOff.WhistlesForHorse".Localized());

            ReturnToCamp(2);
        }

        public void DefeatedDullahanFinishHerOff(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.KnightFallsWounded".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.GroansWhatALife".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.ApproachFinalBlow".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.RaiseWeaponChest".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.DullahanDead".Localized());

            OutputText("Scenes.Dullahan.DefeatedDullahanFinishHerOff.LookForHorse".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DullahanDead, 1);

            CoC.Instance.Flags.Set(GameFlagIds.BeautifulSwordLevel,

                CoC.Instance.Flags.Get(GameFlagIds.BeautifulSwordLevel) + 1);

            ReturnToCamp(1);
        }

        public void AskDullSex()
        {
            ClearOutput();

            var flags = CoC.Instance.Flags;

            if (Player.Lust >= 33 && flags.Get(GameFlagIds.AcceptedDullRequest) == 3)
            {
                ClearButtons();

                if (!Player.IsTaur())
                {
                    var talkedCurse = Save.GetBool("talkedAboutCurse");

                    OutputText("Scenes.Dullahan.AskDullSex.AskWillingSex".Localized(
                        talkedCurse ? "" : "Scenes.Dullahan.AskDullSex.NoPenetrationWarning".Localized()));

                    if (!talkedCurse)
                        if (IsSilly)
                            OutputText("Scenes.Dullahan.AskDullSex.VaginaDentata".Localized());
                        else if ((flags.Get(GameFlagIds.CodexEphraimJournal) & 2) != 0)
                            OutputText("Scenes.Dullahan.AskDullSex.KnowWhy".Localized());
                        else
                            OutputText("Scenes.Dullahan.AskDullSex.BetterNotPry".Localized());

                    if (Player.HasCock())
                    {
                        AddButton(0, "Scenes.Dullahan.AskDullSex.ButtonThighjob".Localized(), DullThighjob);

                        AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), DullBlowjobTease);
                    }

                    if (Player.HasVagina())
                        AddButton(2, "Scenes.Dullahan.AskDullSex.ButtonCunnilingus".Localized(), DullahanCunnilingus2);

                    if (Player.HasKeyItem(DemonicStrapOn))
                        AddButton(3, "Scenes.Dullahan.AskDullSex.ButtonStrapOn".Localized(), DullahanStrapOn);

                    if (TentacleCockCount() >= 4)
                        AddButton(4, "Scenes.Dullahan.AskDullSex.ButtonTentacleFun".Localized(), DullTentacleFun);

                    if (Player.Gender != Gender.Genderless)
                        AddButton(5, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), CuddleDull);

                    if (Player.HasVagina() && Save.GetInt("rimmingProgress", 0) > 0)
                        AddButton(6, "Scenes.Dullahan.AskDullSex.ButtonRimming".Localized(), DullahanRimming);

                    AddButton(14, "Common.Back".Localized(), DullMenu);

                    return;
                }

                if (TentacleCockCount() >= 4)
                    AddButton(0, "Scenes.Dullahan.AskDullSex.ButtonTentacleFun".Localized(), DullTentacleFun);
                else
                    OutputText("Scenes.Dullahan.AskDullSex.AskWillingSex2".Localized());

                if (Player.Corruption + Player.CorruptionTolerance() > 60 && Player.HasCock())
                    AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonRape".Localized(), () => DullOhYouFuckedUp(false),

                        "Scenes.Dullahan.DefeatedDullahanFriendly.HintRape".Localized());

                AddButton(2, "Scenes.Dullahan.AskDullSex.ButtonNah".Localized(), DullSexRefused);

                return;
            }

            RegisterMacro("talkedAboutCurse", Save.GetInt("talkedAboutCurse", 0) > 0);

            OutputText("Scenes.Dullahan.AskDullSex.AskWillingSex3".Localized());

            OutputText("Scenes.Dullahan.AskDullSex.NotInterestedBody".Localized());

            OutputText("Scenes.Dullahan.AskDullSex.SlumpsDepression".Localized());

            ClearButtons();

            AddButtonDisabled(2, "Scenes.Dullahan.AskDullSex.ButtonSex".Localized(),

                "Scenes.Dullahan.AskDullSex.NotAnOption".Localized());

            if (Player.HasCock() && Player.Corruption + Player.CorruptionTolerance() > 60)
            {
                OutputText("Scenes.Dullahan.AskDullSex.CouldTakeForce".Localized());

                AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonRape".Localized(), () => DullOhYouFuckedUp(false),

                    "Scenes.Dullahan.AskDullSex.HintRape2".Localized());
            }

            AddButton(14, "Common.Back".Localized(), DullMenu);
        }

        public void DullSexRefused()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullSexRefused.AfterAwkwardConversation".Localized());

            ReturnToCamp(1);
        }

        public static bool IsSilly => OptionStore.Instance.Get("silly", 0) > 0;

        public static readonly Identifier DemonicStrapOn = new("coc", "demonic_strap_on");

        public static int TentacleCockCount() =>

            Player.Cocks.FindAll(cock => cock.Id == new Identifier("coc", "tentacle")).Count;

        public void CuddleCock()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleCock.AlluringBut".Localized());

            if (Player.Strength > 80)
            {
                OutputText("Scenes.Dullahan.CuddleCock.TakeByArms".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.HeadOnSide".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.AskStillTrusts".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.AskContinue".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.HeadPurple".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.HeadMumbling".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.BodyFreezes".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.ReachBack".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.BringFaceUp".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.BreakKiss".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.WhoIsMaster".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.WhoIsSlut".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.SecretlyEnjoyed".Localized());

                OutputText("Scenes.Dullahan.CuddleCock.MoveHeadAway".Localized());

                ClearButtons();

                if (Player.HasCock())
                    AddButton(0, "Scenes.Dullahan.CuddleCock.ButtonCock".Localized(), CuddleFinishCock);

                if (Player.HasVagina())
                    AddButton(1, "Scenes.Dullahan.CuddleCock.ButtonVagina".Localized(), CuddleFinishVagina);

                return;
            }

            OutputText("Scenes.Dullahan.CuddleCock.FailToBudge".Localized());

            OutputText("Scenes.Dullahan.CuddleCock.HeadlessBody".Localized());

            DullOhYouFuckedUp(true);
        }

        public void CuddleNoCock()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleNoCock.BodyArches".Localized());

            if (Player.Strength > 80)
            {
                OutputText("Scenes.Dullahan.CuddleNoCock.SpeedButNotStrength".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.TakeByArms2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.HeadOnSide2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.AskStillTrusts".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.AskContinue".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.HeadPurple2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.HeadMumbling2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.BodyFreezes2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.ReachBack2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.BringFaceUp2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.BreakKiss2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.WhoIsMaster2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.WhoIsSlut2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.SecretlyEnjoyed2".Localized());

                OutputText("Scenes.Dullahan.CuddleNoCock.MoveHeadAway2".Localized());

                ClearButtons();

                if (Player.HasVagina())
                    AddButton(0, "Scenes.Dullahan.CuddleNoCock.ButtonVagina".Localized(), CuddleFinishVagina);

                return;
            }

            OutputText("Scenes.Dullahan.CuddleNoCock.BodyOverpowers".Localized());

            OutputText("Scenes.Dullahan.CuddleNoCock.LooksSadlyAlone".Localized());

            ReturnToCamp(2);
        }

        public void CuddleFinishVagina()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleFinishVagina.PlaceHeadBetweenLegs".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishVagina.StillWorkingUp".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishVagina.NoticingResolve".Localized());

            CuddlePostSex();
        }

        public void CuddlePostSex()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddlePostSex.WhileTwoOfYou".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.TurnHeadReply".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.Oh".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.WrySmile".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.PlayfullyRemark".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.BlushRevives".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.ScoochesCloser".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.AskAnyLovers".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.QuestionAnswered".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.FaceSerious".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.ClosesEyesRecount".Localized());

            RegisterMacro("seenBody", Save.GetInt("seenBody", 0) > 0);

            OutputText("Scenes.Dullahan.CuddlePostSex.OpensEyesAfter".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.FirstTimeLongTime".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.LayInSilence".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.CouldHaveDefeated".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.RollsOverSad".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.TearRolls".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.PullIntoHug".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.KissBreaks".Localized());

            OutputText("Scenes.Dullahan.CuddlePostSex.BeforeAnswer".Localized());

            CuddleEnd();
        }

        public void CuddleFinishCock()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleFinishCock.RockHard".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishCock.AsMuchAsYoudLove".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishCock.AmusedAtIdea".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishCock.Stabilize".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishCock.NoLongerWait".Localized());

            OutputText(IsSilly

                ? "Scenes.Dullahan.CuddleFinishCock.Something".Localized()

                : "Scenes.Dullahan.CuddleFinishCock.WatchYouPump".Localized());

            OutputText("Scenes.Dullahan.CuddleFinishCock.EyesOpen".Localized());

            CuddlePostSex();
        }

        public void DullThighjob()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullThighjob.LookAtLegs".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.SoundsInteresting".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.StrippingOff".Localized(
                Player.ClothedOrNakedLower(
                    "Scenes.Dullahan.DullThighjob.StrippingArmor".Localized(),

                    "Scenes.Dullahan.DullThighjob.StrokingCock".Localized())));

            OutputText("Scenes.Dullahan.DullThighjob.ClosesGap".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.ConsiderIgnoring".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.KissesNeck".Localized());

            var longCock = Player.BiggestCockLength() >= 10;

            if (longCock)
                OutputText("Scenes.Dullahan.DullThighjob.ThrustingSpeedsBreeze".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.HugTightly".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.FinallyOverwhelms".Localized());

            var cum = Player.CumQ();

            if (cum <= 100)
                OutputText("Scenes.Dullahan.DullThighjob.StainingThighHighs".Localized());
            else if (cum <= 500)
                OutputText("Scenes.Dullahan.DullThighjob.SplatteringClothes".Localized());
            else
                OutputText("Scenes.Dullahan.DullThighjob.DrenchingClothes".Localized());

            if (longCock)
                OutputText("Scenes.Dullahan.DullThighjob.SomeOnFace".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.ImpotentThrusts".Localized());

            if (longCock)
                OutputText("Scenes.Dullahan.DullThighjob.ReattachesHead".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.HasntOrgasmed".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.AskHelpDenied".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.LotOfFun".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.NowSayingThisSilly".Localized());

            OutputText("Scenes.Dullahan.DullThighjob.StepsBackRushes".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(2);
        }

        public void DullTentacleFun()
        {
            ClearOutput();

            var tentacles = TentacleCockCount();

            OutputText("Scenes.Dullahan.DullTentacleFun.AskTryDifferent".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.StripShowCocks".Localized(
                Player.ClothedOrNakedLower(
                    "Scenes.Dullahan.DullTentacleFun.StripArmor".Localized(),

                    "Scenes.Dullahan.DullTentacleFun.PointToCocks".Localized())));

            OutputText("Scenes.Dullahan.DullTentacleFun.BitHorrified".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.YeepFeltThat".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.JustDo".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.FaceDistinctlyPurple".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.SwallowsPrepLicks".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.SucksEagerly".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.EyesOpenJerks".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.PullsFirstCock".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.EffortHasEffect".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.ClosesLegsTrap".Localized());

            if (tentacles > 4)
                OutputText("Scenes.Dullahan.DullTentacleFun.ReluctantCock".Localized());

            if (Player.HasVagina() && tentacles <= 5)
                OutputText("Scenes.Dullahan.DullTentacleFun.MultitudeWorks".Localized());

            if (Player.HasVagina() && tentacles > 5)
                OutputText("Scenes.Dullahan.DullTentacleFun.CuntTingling".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.BodyCoveredPreCum".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.PreBodyGround".Localized());

            var cum = Player.CumQ();

            if (cum <= 100)
                OutputText("Scenes.Dullahan.DullTentacleFun.EachCockStrings".Localized());
            else if (cum <= 500)
                OutputText("Scenes.Dullahan.DullTentacleFun.EachCockPaints".Localized());
            else
                OutputText("Scenes.Dullahan.DullTentacleFun.EachCockDrenches".Localized());

            if (Player.HasVagina() && tentacles > 5)
                OutputText("Scenes.Dullahan.DullTentacleFun.InsideCockOrgasm".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.FallOverwhelmed".Localized());

            OutputText(Player.Libido + Player.Corruption > 60

                ? "Scenes.Dullahan.DullTentacleFun.WayPastCaring".Localized()

                : "Scenes.Dullahan.DullTentacleFun.SnugglingSemen".Localized());

            OutputText("Scenes.Dullahan.DullTentacleFun.NotBadNight".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(2);
        }

        public void DullahanBlowjob()
        {
            ClearOutput();

            var index = Player.SmallestCockIndex();

            var cock = Player.CockDescriptShort(index);

            OutputText("Scenes.Dullahan.DullahanBlowjob.ApproachMoaning".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.ShowStiffeningCock".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.YesAnswer".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.GrabColdHead".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Dullahan.DullahanBlowjob.WeirdestThings".Localized());
            else if (Player.Corruption < 60)
                OutputText("Scenes.Dullahan.DullahanBlowjob.LandGotToYou".Localized());
            else
                OutputText("Scenes.Dullahan.DullahanBlowjob.SeenShareBizarre".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.LowerHeadIntoCock".Localized());

            var eldritch = Player.Intelligence > 60

                ? "Scenes.Dullahan.DullahanBlowjob.Eldritch".Localized()

                : "Scenes.Dullahan.DullahanBlowjob.Bizarre".Localized();

            if (Player.Cocks[index].Length < 9)
                OutputText("Scenes.Dullahan.DullahanBlowjob.PushIntoMouth".Localized(cock));
            else
                OutputText("Scenes.Dullahan.DullahanBlowjob.PushIntoMouth2".Localized(cock));

            OutputText("Scenes.Dullahan.DullahanBlowjob.CloseEyesFocus".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.FeelHugBehind".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.EroticDisplayRekindles".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.ThisTimeNotLong".Localized());

            OutputText(Player.CumQ() > 500

                ? "Scenes.Dullahan.DullahanBlowjob.GargantuanLoads".Localized()

                : "Scenes.Dullahan.DullahanBlowjob.SwallowsEagerly".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.BreatheDeeplyFades".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.ShuffleReattach".Localized());

            OutputText("Scenes.Dullahan.DullahanBlowjob.WhatANight".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(2);
        }

        public void DullahanCunnilingus()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanCunnilingus.ApproachMoaning".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.ShowVagina".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.YesAnswer".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.GrabColdHead".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Dullahan.DullahanCunnilingus.WeirdestThings".Localized());
            else if (Player.Corruption < 60)
                OutputText("Scenes.Dullahan.DullahanCunnilingus.LandGotToYou".Localized());
            else
                OutputText("Scenes.Dullahan.DullahanCunnilingus.SeenShareBizarre".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.PushHeadInto".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.DisbeliefColdTongue".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.HearRustling".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.TongueFuckingEffect".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.BothReachLimits".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.WakeDullahanGone".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(4);
        }

        public static float AverageVaginalWetness()
        {
            if (Player.Vaginas.Count == 0)
                return 0f;

            var total = 0f;

            foreach (var vagina in Player.Vaginas)
                total += vagina.Wetness;

            return total / Player.Vaginas.Count;
        }

        public void DullahanCunnilingus2()
        {
            ClearOutput();

            var wet = AverageVaginalWetness() > 3f;

            OutputText("Scenes.Dullahan.DullahanCunnilingus.DontWorryAboutThat".Localized());

            if (Player.Libido > 50)
                OutputText("Scenes.Dullahan.DullahanCunnilingus.CanTakeHerOn".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.StripsCompletely".Localized());

            OutputText(
                (Player.ClothedOrNaked(
                    "Scenes.Dullahan.DullahanCunnilingus.OpenUpClothedText",

                    "Scenes.Dullahan.DullahanCunnilingus.OpenUpNakedText").Localized()) +

                (wet

                    ? "Scenes.Dullahan.DullahanCunnilingus.Drenched2"

                    : "Scenes.Dullahan.DullahanCunnilingus.Moist2").Localized() +

                " " +

                "Scenes.Dullahan.DullahanCunnilingus.Yourself".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.TwoHugTightly".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.CantSitBack".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.DoBestPleasure".Localized());

            OutputText(wet

                ? "Scenes.Dullahan.DullahanCunnilingus.OrgasmFirstSplattering".Localized()

                : "Scenes.Dullahan.DullahanCunnilingus.OrgasmFirstCoating".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Dullahan.DullahanCunnilingus.CockPressedBetween".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.AssaultDoesntStop".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.LustStillStirring".Localized());

            OutputText("Scenes.Dullahan.DullahanCunnilingus.SomeTimePassesWake".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(4);
        }

        public void DullBlowjobTease()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullBlowjobTease.OldFashionedBlowjob".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.LookAtPiercingEyes".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.CantMakeMeSuck".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.WhatIsBodyDoing".Localized());

            DullBlowjobTease2();
        }

        public void DullBlowjobTease2()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullBlowjobTease2.AlreadyGrowingErect".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.BodyStops".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.PlungesFingers".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.LowerHeadCrotch".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.BodyNoticesFingers".Localized());

            var thickness = Player.Cocks.Count > 0 ? Player.Cocks[0].Thickness : 0f;

            if (thickness >= 4f)
                OutputText("Scenes.Dullahan.DullBlowjobTease.HeadAllSheCan".Localized());
            else if (thickness >= 3f)
                OutputText("Scenes.Dullahan.DullBlowjobTease.StrugglingOpenMouth".Localized());
            else
                OutputText("Scenes.Dullahan.DullBlowjobTease.EngulfingMost".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.OverwhelmedRecent".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.BreatheHardThrust".Localized());

            DullBlowjobTease3();
        }

        public void DullBlowjobTease3()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullBlowjobTease.ProstateTingling".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.EjaculateInside".Localized());

            if (Player.CumQ() > 1500)
                OutputText("Scenes.Dullahan.DullBlowjobTease.ManagesObsceneLoad".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.SpentKeepsSucking".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.SitReleaseHead".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.SlowlyOpenEyes".Localized());

            OutputText("Scenes.Dullahan.DullBlowjobTease.SmileGrabHead".Localized());

            Save.SetBool("seenBody", true);

            ReturnToCamp(2);
        }

        public void ManorIntro() => ForestPlaceholder.Show("Dullahan");

        public void DullahanGift() => ForestPlaceholder.Show("Dullahan");

        public void DullahanIntroPostDream() => ForestPlaceholder.Show("Dullahan");

        public void DullDates()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullDates.AskOut".Localized());

            OutputText("Scenes.Dullahan.DullDates.ADateHowRomantic".Localized());

            OutputText("Scenes.Dullahan.DullDates.WhereToGo".Localized());

            ClearButtons();

            if (Player.Gender != Gender.Genderless)
                AddButton(0, "Scenes.Dullahan.DullDates.ButtonWoods".Localized(), DullDateWoods);
            else
                AddButtonDisabled(0, "Scenes.Dullahan.DullDates.ButtonWoods".Localized(),

                    "Scenes.Dullahan.DullDates.NeedGenitals".Localized());

            AddButton(14, "Scenes.Dullahan.DullDates.ButtonNah".Localized(), DullNoDate);
        }

        public void DullNoDate()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullNoDate.ChangedMind".Localized());

            OutputText("Scenes.Dullahan.DullNoDate.LastLaughTease".Localized());

            DullMenu();
        }

        public void DullDateWoods()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullDateWoods.DateSoundsGood".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.HeadBackSupplies".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.WarmSmirk".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.RubAbdomen".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.DidJokeRomantic".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.CantLetSlide".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.SkewerFish".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.FinishCooking".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.OfferFish".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.GazeDistant".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.NecromancerFeeling".Localized());

            DullDateWoods2();
        }

        public void DullDateWoods2()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullDateWoods.FinishMeat".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.ReplyNotListening".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.LookAwayFormulate".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.WhyAsk".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.DetachesHeadEar".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.KissHerChest".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.HandsDownBack".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.GropingGrinding".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods.BringToLips".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Dullahan.DullDateWoods.PussyWetDrip".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.ColdChillRealize".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.EllipsisBreak".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.NoMonsterAgain".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.RelaxLustReturns".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.TongueLulls".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.ShudderThrash".Localized());
            }
            else if (Player.HasVagina())
            {
                OutputText("Scenes.Dullahan.DullDateWoods.BaskBodies".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.FrustratedStimulation".Localized());

                OutputText("Scenes.Dullahan.DullDateWoods.LongPleasure".Localized());
            }

            OutputText("Scenes.Dullahan.DullDateWoods.CalmsDown".Localized());

            Save.SetBool("seenBody", true);

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DullDateWoods.ButtonYes".Localized(), DullDateWoods2Yes);

            AddButton(1, "Scenes.Dullahan.DullDateWoods.ButtonNo".Localized(), DullDateWoods2No);
        }

        public void DullDateWoods2Yes()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullDateWoods2Yes.WhateverCausesFall".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods2Yes.SilentThinking".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods2Yes.SlightFrown".Localized());

            DullFinishForestDate(true);
        }

        public void DullDateWoods2No()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullDateWoods2No.DifficultToJudge".Localized());

            OutputText("Scenes.Dullahan.DullDateWoods2No.PleasedAnswer".Localized());

            DullFinishForestDate(false);
        }

        public void DullFinishForestDate(bool answeredYes)
        {
            ClearOutput();

            View.Time.PassTime(6);

            OutputText("Scenes.Dullahan.DullFinishForestDate.SilentMinutes".Localized());

            RegisterMacro("answeredYes", answeredYes);

            OutputText("Scenes.Dullahan.DullFinishForestDate.WakeGrabbed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DullDates,

                CoC.Instance.Flags.Get(GameFlagIds.DullDates) | 1);

            ReturnToCamp(1);
        }

        public void DullahanStrapOn()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.DullahanStrapOn.TellPerfectTool".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.TakingStrapCreative".Localized());

            if (!Player.HasVagina())
                DullahanStrapOnAnal();
            else
                DullahanStrapOnVaginal();

            ReturnToCamp(1);
        }

        public void DullahanStrapOnAnal()
        {
            var anal = Player.Body.Get<BodyParts.Vanilla.AnalPart>();

            OutputText("Scenes.Dullahan.DullahanStrapOn.DontHaveSlot".Localized(Player.ClothedOrNakedLower(" strip off your [armor]", " get into position")));

            OutputText("Scenes.Dullahan.DullahanStrapOn.BreathingRapidly".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.WalksPointsPenis".Localized());

            var lubed = anal.Wetness > 1

                ? "Scenes.Dullahan.DullahanStrapOn.SlidesSmoothly".Localized()

                : "Scenes.Dullahan.DullahanStrapOn.MeetsResistance".Localized();

            OutputText(anal.Looseness > 1

                ? "Scenes.Dullahan.DullahanStrapOn.ToHerSurprise".Localized(lubed)

                : "Scenes.Dullahan.DullahanStrapOn.NoticesResistance".Localized(lubed));

            OutputText("Scenes.Dullahan.DullahanStrapOn.NineInches".Localized());

            OutputText(Player.HasCock()

                ? "Scenes.Dullahan.DullahanStrapOn.WeightOnProstate".Localized()

                : "Scenes.Dullahan.DullahanStrapOn.Period".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.SlowlyClumsily".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.ClumsinessAdvantages".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.GetsTheHang".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.FasterHarder".Localized((Player.HasCock() ? " your [cocks] throbbing and pulsing, welling up with cum" : "")));

            OutputText("Scenes.Dullahan.DullahanStrapOn.SlapCausesPause".Localized());

            var cum = Player.CumQ();

            if (!Player.HasCock())
                OutputText("Scenes.Dullahan.DullahanStrapOn.ShudderingPleasure".Localized());
            else if (cum < 50)
                OutputText("Scenes.Dullahan.DullahanStrapOn.ShootingStrings".Localized());
            else if (cum < 300)
                OutputText("Scenes.Dullahan.DullahanStrapOn.PaintingColdGround".Localized());
            else
                OutputText("Scenes.Dullahan.DullahanStrapOn.JetAfterJet".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.WatchesOrgasm".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.AfterEcstasy".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.AgreePanting".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.GoodbyesHeadBack".Localized());
        }

        public void DullahanStrapOnVaginal()
        {
            var vagina = Player.Body.Get<BodyParts.Vanilla.VaginaPart>();

            OutputText("Scenes.Dullahan.DullahanStrapOn.NeverFuckedLikeThat".Localized(Player.ClothedOrNakedLower(" strip off your [armor]", " get into position")));

            OutputText("Scenes.Dullahan.DullahanStrapOn.BreathingRapidly2".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.WalksPointsPenis2".Localized());

            var wet = AverageVaginalWetness() > 2f;

            var lubed = wet

                ? "Scenes.Dullahan.DullahanStrapOn.SlidesSmoothlyVaginal".Localized()

                : "Scenes.Dullahan.DullahanStrapOn.MeetsResistanceVaginal".Localized();

            OutputText(vagina.Looseness > 2

                ? "Scenes.Dullahan.DullahanStrapOn.ToHerSurprise".Localized(lubed)

                : "Scenes.Dullahan.DullahanStrapOn.NoticesResistance".Localized(lubed));

            OutputText("Scenes.Dullahan.DullahanStrapOn.NineInches2".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.SlowlyClumsily".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.ClumsinessAdvantages".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.GetsTheHang2".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.FasterHarder2".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.SlapCausesPause".Localized());

            if (AverageVaginalWetness() > 3f)
                OutputText("Scenes.Dullahan.DullahanStrapOn.SplatteringCrotch".Localized());

            var cum = Player.CumQ();

            if (!Player.HasCock())
                OutputText("Scenes.Dullahan.DullahanStrapOn.ShudderingPleasure".Localized());
            else if (cum < 50)
                OutputText("Scenes.Dullahan.DullahanStrapOn.AndShootingStrings".Localized());
            else if (cum < 300)
                OutputText("Scenes.Dullahan.DullahanStrapOn.AndPaintingGround".Localized());
            else
                OutputText("Scenes.Dullahan.DullahanStrapOn.AndJetAfterJet".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.WatchesOrgasm".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.AfterEcstasy2".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.AgreePanting".Localized());

            OutputText("Scenes.Dullahan.DullahanStrapOn.GoodbyesHeadBack".Localized());
        }

        public void DullahanRimming()
        {
            ClearOutput();

            var firstTime = Save.GetInt("rimmingProgress", 0) < 2;

            RegisterMacro("rimmingProgress", Save.GetInt("rimmingProgress", 0) < 2);

            OutputText("Scenes.Dullahan.DullahanRimming.TellSatisfyNeed".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.SitEvelynPlacesHead".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.NowPleasureHer".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.LovingTongueRear".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.IncreasingTempo".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.PantingRemoveFace".Localized());

            RegisterMacro("rimmingProgress", Save.GetInt("rimmingProgress", 0) < 2);

            OutputText("Scenes.Dullahan.DullahanRimming.AfterPostOrgasm".Localized());

            OutputText("Scenes.Dullahan.DullahanRimming.NotWantingToGo".Localized());

            Save.SetInt("rimmingProgress", 2);

            ReturnToCamp(1);
        }

        public void DullOhYouFuckedUp(bool skipStart = false)
        {
            ClearOutput();

            if (!skipStart)
            {
                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.ApproachAlreadyAroused".Localized(
                    Player.ClothedOrNakedLower(
                        "Scenes.Dullahan.DullOhYouFuckedUp.StrippingArmor".Localized(),

                        "Scenes.Dullahan.DullOhYouFuckedUp.StrokingCock".Localized())));

                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.TerribleIdea".Localized());

                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.BodyPushesDown".Localized());

                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.LowersOnTop".Localized());
            }

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.MoanLoudly".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.ClawingGround".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.ItDoesntTakeLong".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.SmileEngorges".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.PerkyBreastsBounce".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.BreastsCuntMilking".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.HasntEnded".Localized());

            if (Player.Libido < 30)
                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.UnusualLibido".Localized());
            else if (Player.Libido < 60)
                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.UsedToNotTiring".Localized());
            else
                OutputText("Scenes.Dullahan.DullOhYouFuckedUp.InsaneLibidoResult".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.AndAgain".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.DozensOfOrgasms".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUp.VisionBlurs".Localized());

            DullOhYouFuckedUpPt2();
        }

        public void DullOhYouFuckedUpPt2()
        {
            ClearOutput();

            if (Player.Libido >= 80 && Player.Toughness >= 95 && Player.Health - 700 > 0)
            {
                OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.WakeSoulTorn".Localized());

                OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.GroanGetUp".Localized());

                OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.MoveBackCamp".Localized());

                Player.ApplyStats(
                    new DynStat(GameStats.Strength, -15),

                    new DynStat(GameStats.Toughness, -15),

                    new DynStat(GameStats.Speed, -15));

                Player.HealthChange(-700);

                CoC.Instance.Flags.Set(GameFlagIds.DullahanRude, 2);

                ReturnToCamp(4);

                return;
            }

            OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.HourHundredsEjaculations".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.GetsUpPanties".Localized());

            OutputText("Scenes.Dullahan.DullOhYouFuckedUpPT.BodyLifeless".Localized());

            GameOver();
        }

        public void CuddleDull()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleDull.QuickLookAround".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.RecoilNoOpportunity".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.BeginApproach".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.WouldAcceptInvitation".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) > 30)
                OutputText("Scenes.Dullahan.CuddleDull.MuchTimeMareth".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.GrantPermission".Localized());

            CuddleDull2();
        }

        public void CuddleDull2()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleDull2.GatherStones".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.LookAtFace".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.InviteLayDown".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.LookIntoNeck".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.FeelsLikeIngnam".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.ScoopsHead".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.FeelAtHome".Localized(Player.Gender switch
            {
                Gender.Male => "man",

                Gender.Female => "woman",

                _ => "eunuch"
            }));

            OutputText("Scenes.Dullahan.CuddleDull.FeelingCuddled".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.TakesNoticeHeat".Localized());

            OutputText("Scenes.Dullahan.CuddleDull2.WhilstDoingThat".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.LoosenCorset".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.HandsStopHalfway".Localized());

            OutputText("Scenes.Dullahan.CuddleDull.CatchesBreath".Localized());

            Save.SetBool("seenBody", true);

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.CuddleDull.ButtonStop".Localized(), CuddleStop);

            AddButton(1, "Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), CuddleInsist);
        }

        public void CuddleStop()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleStop.CloseEyesNod".Localized());

            CuddleEnd();
        }

        public void CuddleEnd()
        {
            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleEnd.DreamNice".Localized());

            OutputText("Scenes.Dullahan.CuddleEnd.WakeInCamp".Localized());

            ReturnToCamp(8);
        }

        public void CuddleInsist()
        {

            if (Save.GetInt("rimmingProgress", 0) < 1)
                Save.SetInt("rimmingProgress", 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AcceptedDullRequest) < 3)
            {
                ClearOutput();

                OutputText("Scenes.Dullahan.CuddleInsist.PullsTwistsNipple".Localized());

                OutputText("Scenes.Dullahan.CuddleInsist.BodyGetsUp".Localized());

                OutputText("Scenes.Dullahan.CuddleInsist.SpursHorseRidesOff".Localized());

                Player.Lust += 10;

                ReturnToCamp(2);

                return;
            }

            ClearOutput();

            OutputText("Scenes.Dullahan.CuddleInsist.PickHeadUp".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.AddressByName".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.BeGentle".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.NoWasteTime".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.BlissAtLast".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.HandUnderSkirt".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.HowOftenPussy".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.StartHittingHome".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.PleaseStopNow".Localized());

            OutputText("Scenes.Dullahan.CuddleInsist.TooLateBodyStops".Localized());

            if (Player.HasCock())
                CuddleCock();
            else
                CuddleNoCock();
        }

    }
}