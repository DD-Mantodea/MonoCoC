using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<JeanClaudeScenes>("coc:jean_claude_scenes")]
    public class JeanClaudeScenes : Scene
    {

        public static void ResumeFromFight()
        {
            CombatController.InCombat = false;

            CombatController.Instance = null;

            UI.Components.Views.GameView.Instance?.HideCombatStats();

            RoomDungeon.ResumeFromFight();
        }

        [Register("coc:keep_hook_jeanclaude_south")]
        public static void KeepHookJeanClaudeSouth() =>

            SceneInstance<JeanClaudeScenes>.Get().GogoFuckTheseBasilisks();

        [Register("coc:keep_hook_jeanclaude_north")]
        public static void KeepHookJeanClaudeNorth() =>

            SceneInstance<JeanClaudeScenes>.Get().GogoFuckTheseBasilisksNorth();

        public override void Entry()
        {

        }

        public void GogoFuckTheseBasilisksNorth()
        {
            ClearOutput();

            var hasLaybans = Player.HasKeyItem("Laybans");

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisksNorth.UnlocksDoor".Localized(
                hasLaybans ? "put your Laybans back on" : "steel yourself",

                hasLaybans ? "deftly hooking your Laybans off with his sickle claw" : "grabbing you with his clawed hand"
            ));

            if (Player.HasPerk(PerkIds.BasiliskResistance))
            {
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisksNorth.NakedEyesGazing".Localized(
                    Player.CanFly() ? "fly" : "run"
                ));

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => RoomDungeon.Move("antechamber"));
            }
            else
            {
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisksNorth.LeadenWeight".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), CapturedByBasilisks);
            }
        }

        public void GogoFuckTheseBasilisks()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.BreathesDeep".Localized());

            if (Player.HasKeyItem("Laybans"))
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.PutsShadesOn".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.StridesGantry".Localized());

            if (Player.DemonScore() > 4)
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.CantBeMistaken".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.IfLizardsRealize".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.NotLookDown".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.ProbablyImagining".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.GodsdamnForm".Localized());
                else
                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.ClackClack".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.AlmostHalf".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.AttentionCall".Localized());

            OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.AttrapezLe".Localized());

            if (!Player.HasKeyItem("Laybans"))
            {
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.BreaksIntoRun".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), CapturedByBasilisks);

                return ;
            }
            else
            {
                OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.BreaksIntoRunAgain".Localized());

                if (Player.Speed < 90 && !Player.HasPerk(PerkIds.BasiliskResistance))
                {
                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.BoothInReach".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), CapturedByBasilisks);

                    return ;
                }
                else
                {
                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.BoothInReachYour".Localized());

                    if (!Player.CanFly())
                        OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.AlmostWord".Localized());
                    else
                        OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.LiterallyWord".Localized());

                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.FlyTowardsDoor".Localized());

                    OutputText("Scenes.Jeanclaudescenes.GogoFuckTheseBasilisks.FreeToContinue".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => RoomDungeon.Move("magpiehalln"));
                }
            }
        }

        public void CapturedByBasilisks()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.StaringUp".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.SaysOne".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.SaysAnother".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.DryHandsGrip".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.WideningSneer".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.StiffeningWord".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.AndFragment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.BeadingExcitement".Localized());

            if (!Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.LoinsAflame".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.SinkFurther".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.GrowlsSoftly".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisks.LastOfCurse".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Taunt".Localized(), CapturedbyBasilisksTaunt);

            AddButton(1, "Common.Button.Run".Localized(), CapturedByBasilisksRun);
        }

        public void CapturedByBasilisksRun()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisksRun.TearsFree".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedByBasilisksRun.ReachesForward".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BasiliskBadEnds);
        }

        public void CapturedbyBasilisksTaunt()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.StaresYellow".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.JeanClaudeDrawsBack".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.RexWhispers".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.DeathlySilence".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.TurnsAwaySnorting".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.SaysSneering".Localized());

            OutputText("Scenes.Jeanclaudescenes.CapturedbyBasilisksTaunt.ScramblesFeet".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "jean_claude")),

                () => ResumeFromFight());
        }

        public void BeatDaFuckhugeBasilisk(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3JeanClaudeDefeated, 1);

            if (hpVictory)
                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.MakesSound".Localized());
            else
                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.CursesWithKind".Localized());

            if (Player.Corruption >= 60)
            {
                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.DefeatedRexStares".Localized());

                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.SnarlsRichVoice".Localized());

                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.MurmursWetChoke".Localized());
            }
            else
            {
                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.DefeatedRexStaresAgain".Localized());

                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.SnarlsRichVoiceAgain".Localized());
            }

            OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.EchoingSilence".Localized());

            if (Player.Mf("m", "f") == "f" && silly)
                OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.RaisesFists".Localized());

            OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.BitOfScuffling".Localized());

            OutputText("Scenes.Jeanclaudescenes.BeatDaFuckhugeBasilisk.HeWhispers".Localized(
                Player.Mf("mon roi", "ma reine")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.GoAway".Localized(), GoAway);
        }

        public void GoAway()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.GoAway.DrawsUp".Localized());

            OutputText("Scenes.Jeanclaudescenes.GoAway.WithinMinute".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.D3BasilisksRemovedFromMagpieHall, 1);

            ResumeFromFight();
        }

        public void FuckhugeBasiliskFuckedYouUp(bool hpVictory)
        {
            ClearOutput();

            if (!hpVictory)
            {
                OutputText("Scenes.Jeanclaudescenes.FuckhugeBasiliskFuckedYouUp.LooksDownAt".Localized());

                OutputText("Scenes.Jeanclaudescenes.FuckhugeBasiliskFuckedYouUp.HeHusks".Localized());
            }
            else
            {
                OutputText("Scenes.Jeanclaudescenes.FuckhugeBasiliskFuckedYouUp.FallsBattered".Localized());

                OutputText("Scenes.Jeanclaudescenes.FuckhugeBasiliskFuckedYouUp.HusksAsGlow".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BasiliskBadEnds);
        }

        public void BasiliskBadEnds()
        {

            if (Player.HasPerk(PerkIds.BasiliskWomb))
                BwombBadEnd();
            else
                RegularBadEnd();
        }

        public void RegularBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.RexGlow".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.VaginaBeads".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.AndFragment".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.EachCockHardens".Localized());

            if (Player.HasCock() || Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.AsFragment".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.BodyReacts".Localized());

            if (Player.HasStatusEffect(Enemies.Vanilla.JeanClaudeEnemy.RemovedArmorId))
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.RealizesVaguely".Localized());
            else
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.UncomfortablyHot".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.BreathesSmell".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.HeMurmurs".Localized());

            if (Player.Body.Get<SkinPart>().Adj == "latex" || Player.Body.Get<SkinPart>().Adj == "rubber")
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SlidesHandDown".Localized());
            else
            {
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SnapsFingers".Localized());

                if (Player.HasFur())
                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.FurDropsAway".Localized());

                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.AnotherBlack".Localized());

                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.WatchesSharing".Localized());
            }

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.MonstrousGrowls".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.VaginaDribbling".Localized());

            if (Player.HasVagina() && Player.HasCock())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.AndFragmentAgain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.CockStraining".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SluttySurrender".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SmirkingGives".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.TransformationIncomplete".Localized());
            else
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.TransformationIncompleteAgain".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.CirclesAdmiring".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SaysAtLast".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.ReachesGrasps".Localized());

                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.RoarsWaggling".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.ReachesBending".Localized());

                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.HeSaysAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.ReachesBendingAgain".Localized());

                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SaysAlmostKindly".Localized());

                    OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.HeSaysAgain".Localized());
                }

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.FinishedGloating".Localized());

            ShowImage("basilisk-vag-fuck");

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SnapsClaws".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.SWord".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.PushRoughly".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.AndFragmentThird".Localized());

            OutputText("Scenes.Jeanclaudescenes.RegularBadEnd.DisappearsDepths".Localized());

            GameOver();
        }

        public void BwombBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.RexGlow".Localized());

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.HeHusksFretful".Localized());

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.MurmursRich".Localized());

            if (Player.HasStatusEffect(Enemies.Vanilla.JeanClaudeEnemy.RemovedArmorId))
            {
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.AlreadyTakenClothes".Localized());

                if (Player.IsBiped())
                    OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.SpreadYourAgain".Localized());
                else
                    OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.PositionYourselfAgain".Localized());

                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.ForBreederBody".Localized());
            }
            else
            {
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.RequireNoDirection".Localized());

                if (Player.IsBiped())
                    OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.SpreadYourAgain".Localized());
                else
                    OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.PositionYourselfAgain".Localized());

                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.ForBreederBodyAgain".Localized());
            }

            ShowImage("basilisk-vag-fuck");

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.HugeAudience".Localized());

            if (AverageVaginalLooseness() < 2)
            {
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.GaspsThrusts".Localized());

                Player.CuntChange(24, true, true, false);
            }
            else
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.GruntsSatisfaction".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.OtherDickBats".Localized());
            else
                OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.OtherDickThrusts".Localized());

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.LaughsRichly".Localized());

            OutputText("Scenes.Jeanclaudescenes.BwombBadEnd.BreathlessMurmur".Localized());

            GameOver();
        }

        public static float AverageVaginalLooseness()
        {
            var total = 0f;

            foreach (var vagina in Player.Vaginas)
                total += vagina.Looseness;

            return Player.Vaginas.Count == 0 ? 0f : total / Player.Vaginas.Count;
        }

    }
}