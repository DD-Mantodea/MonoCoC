using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<DemonFistFighterScene>("coc:demon_fist_fighter")]
    public class DemonFistFighterScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Bazaar.DemonFistFighter);

        public override void Entry()
        {

        }

        public bool TimeChange()
        {
            Save.SetInt("demonfistTimeAway", Math.Max(0, Save.GetInt("demonfistTimeAway", 0) - 1));

            return false;
        }

        public bool TimeChangeLarge() => false;

        public void Reset()
        {

            Save.SetInt("timesLost", 0);

            Save.SetInt("timesWon", 0);

            Save.SetInt("timesBrokenRules", 0);

            Save.SetString("playerName", "");

            Save.SetBool("beatDemonfist", false);

            Save.SetBool("learnedOfDemonFist", false);

            Save.SetInt("consecutiveLosses", 0);

            Save.SetInt("demonfistTimeAway", 0);

            Save.SetBool("newRulesExplained", true);

            Save.SetInt("timesFoughtDog", 0);

            Save.SetBool("shookDogHand", false);
        }

        public float TimesFoughtDemonfist()
        {

            return Save.GetInt("timesLost", 0) + Save.GetInt("timesWon", 0);
        }

        public void DemonFistTentDescription()
        {

            if (View.Time.Hours <= 18)
                if (!Save.GetBool("learnedOfDemonFist"))
                    OutputText("Scenes.Demonfistfighterscene.DemonFistTentDescription.DemonsBeeline".Localized());
                else
                    OutputText("Scenes.Demonfistfighterscene.DemonFistTentDescription.ConstantFlow".Localized());
        }

        public void FollowTheLine()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.FollowTheLine.FollowsLine".Localized());

            ClearButtons();

            AddNextButton("Common.Button.EnterTheLine".Localized(), EnterTheLine);

            AddNextButton("Common.Button.BailOut".Localized(), BackToBazaar);
        }

        public void BackToBazaar()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.BackToBazaar.ShakesHead".Localized());

            SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();
        }

        public void EnterTheLine()
        {
            ClearOutput();

            Save.SetBool("learnedOfDemonFist", true);

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.EntersLine".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.StupidOrNot".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.WhichOfTwo".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.GoesSilent".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.WhyRiskMoney".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine.CanDoIt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDefeatedAndKilled) != 0)
                OutputText("Scenes.Demonfistfighterscene.EnterTheLine.MentionsVacancy".Localized());
            else
                OutputText("Scenes.Demonfistfighterscene.EnterTheLine.WishesLuck".Localized());

            EnterTheLine2();
        }

        public void EnterTheLine2()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.LongWait".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.StuffyAir".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.CrimsonFalls".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.WavesHand".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.GoodOne".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.ScreamsSeven".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.AudienceSilent".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.MiffixIsIt".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.MiffixEnters".Localized());

            OutputText("Scenes.Demonfistfighterscene.EnterTheLine2.BetterFool".Localized());

            MiffixFight();
        }

        public void MiffixFight()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.MiffixTerrified".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.GotTheGems".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.MiffixSwallows".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.FightStarts".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.GathersCourage".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.QuickJab".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.ComingDown".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.WastesNoTime".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight.DoesNotGetUp".Localized());

            MiffixFight2();
        }

        public void MiffixFight2()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.MiffixFight2.CrowdSilent".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight2.GoodSmartOne".Localized());

            OutputText("Scenes.Demonfistfighterscene.MiffixFight2.CheersUnconscious".Localized());

            ReturnToCamp(2);
        }

        public void ReturnToTent()
        {
            ClearOutput();

            if (Save.GetBool("beatDemonfist"))
                TentDescriptionDemonfistOut();
            else
                TentDescriptionDemonfistIn();
        }

        public void TentDescriptionDemonfistOut()
        {

            if (Save.GetBool("newRulesExplained"))
            {
                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.BackToArena".Localized());

                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.ShutYourMouth".Localized());

                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.SlapsAway".Localized());

                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.DeepVoice".Localized());

                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.LiftsHead".Localized());

                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.NodsEntrance".Localized());

                RhinoExplanation();
            }
            else
                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistOut.BackToArenaAgain".Localized());
        }

        public void RhinoExplanation()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.DemonfistLeaves".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.OpensFlap".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.CutsYouOff".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.OpensPath".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.TalksSpectators".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.FinishesSentence".Localized());

            OutputText("Scenes.Demonfistfighterscene.RhinoExplanation.WillConsider".Localized());

            Save.SetBool("newRulesExplained", true);

            ClearButtons();

            AddNextButton("Common.Button.DefendTitle".Localized(), DefendTitle);
        }

        public void LeaveTent()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.LeaveTent.NotFighting".Localized());

            OutputText("Scenes.Demonfistfighterscene.LeaveTent.NodsExit".Localized());

            BackToBazaar();
        }

        public void DefendTitle()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Demonfistfighterscene.DefendTitle.WantsToFight".Localized());

            if (Save.GetInt("timesWon", 0) == 1)
            {
                OutputText("Scenes.Demonfistfighterscene.DefendTitle.PursesLips".Localized());

                if (Save.GetInt("timesFoughtDog", 0) == 0)
                {
                    OutputText("Scenes.Demonfistfighterscene.DefendTitle.DogNameIntro".Localized());

                    OutputText("Scenes.Demonfistfighterscene.DefendTitle.FlashesGrin".Localized());

                    OutputText("Scenes.Demonfistfighterscene.DefendTitle.AssuresHim".Localized());

                    AddNextButton("Common.Button.Shake".Localized(), () => IntroDogFight(true));

                    AddNextButton("Common.Button.DontShake".Localized(), () => IntroDogFight(false));
                }
                else
                {
                    OutputText("Scenes.Demonfistfighterscene.DefendTitle.KnowEachOther".Localized());

                    AddNextButton("Common.Button.Fight".Localized(), () => IntroDogFight(true, true));
                }
            }
        }

        public void IntroDogFight(bool shake, bool skip = false)
        {
            ClearOutput();

            if (skip)
            {
                Save.SetBool("shookDogHand", shake);

                if (shake)
                    OutputText("Scenes.Demonfistfighterscene.IntroDogFight.ShakesHand".Localized());
                else
                    OutputText("Scenes.Demonfistfighterscene.IntroDogFight.RefusesHand".Localized());
            }

            OutputText("Scenes.Demonfistfighterscene.IntroDogFight.LeadToRing".Localized());

            OutputText("Scenes.Demonfistfighterscene.IntroDogFight.AddressesCrowd".Localized());

            OutputText("Scenes.Demonfistfighterscene.IntroDogFight.NoMoreDelays".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "placeholder_dodgy_monster")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void TentDescriptionDemonfistIn()
        {

            var possibleOpponents = new[] { "a huge minotaur", "a stocky-looking imp", "an athletic incubus", "a mean-looking rat-morph", "a surprisingly muscled succubus" };

            OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.BackToArena".Localized());

            OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.FinallyInside".Localized(
                possibleOpponents[Random.Shared.Next(possibleOpponents.Length)]
            ));

            OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.ChampionRitual".Localized());

            ClearButtons();

            if (Player.Gems >= 500)
            {
                if (Save.GetString("playerName") == "")
                {
                    OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.YourChance".Localized());

                    AddButton(0, "Common.Button.CustomName".Localized(), () => NameYourself());

                    AddButton(1, "Common.Button.YourName".Localized(), () => NameYourself(Player.ShortName));

                    if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDefeatedAndKilled) != 0)
                        AddNextButton("Common.Button.ZetazsDoom".Localized(), () => NameYourself("Zetaz's Doom"));

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzumiTimesGrabbedTheHorn) > 2)
                        AddNextButton("Common.Button.TheOniObliterator".Localized(), () => NameYourself("The Oni Obliterator"));

                    if (CoC.Instance.Flags.Get(GameFlagIds.HeliaSparVictories) > 5)
                        AddNextButton("Common.Button.TheSalamanderStomper".Localized(), () => NameYourself("The Salamander Stomper"));

                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenDullahanSpar) > 3 || CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenShouldra) > 3)
                        AddNextButton("Common.Button.TheUndertaker".Localized(), () => NameYourself("The Undertaker"));

                    if (Player.IsReligious() && Player.Corruption < 40)
                        AddNextButton("Common.Button.Angelfist".Localized(), () => NameYourself("Angelfist"));
                    else
                        AddNextButton("Common.Button.TheCorruptedCrusader".Localized(), () => NameYourself("The Corrupted Crusader"));

                    if (Player.Weapon.Id == ItemIds.LightRailAvenger)
                        AddNextButton("Common.Button.PerfectStorm".Localized(), () => NameYourself("Perfect Storm"));
                }
                else
                {
                    OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.AnotherRound".Localized());

                    AddButton(0, "Common.Button.Challenge".Localized(), ChallengeBegins);
                }
            }
            else
                OutputText("Scenes.Demonfistfighterscene.TentDescriptionDemonfistIn.LacksGems".Localized());

            AddButton(14, "Common.Button.JustWatch".Localized(), JustWatchFight);
        }

        public void JustWatchFight()
        {
            ClearOutput();

            var he = Random.Shared.Next(2) != 0 ? "he" : "she";

            var his = he == "he" ? "his" : "her";

            var him = he == "he" ? "him" : "her";

            OutputText("Scenes.Demonfistfighterscene.JustWatchFight.KeepSpectating".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                OutputText("Scenes.Demonfistfighterscene.JustWatchFight.PoorThing".Localized(
                    his
                ));
                }

                break;

                case 1:
                {
                OutputText("Scenes.Demonfistfighterscene.JustWatchFight.DecentShow".Localized(
                    he
                ));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Demonfistfighterscene.JustWatchFight.NewChallenger".Localized(
                    he
                ));

                OutputText("Scenes.Demonfistfighterscene.JustWatchFight.CrowdShock".Localized(
                    him,

                    he
                ));

                OutputText("Scenes.Demonfistfighterscene.JustWatchFight.CrowdErupts".Localized());
                }

                break;
            }

            OutputText("Scenes.Demonfistfighterscene.JustWatchFight.HadFairShare".Localized());

            SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();
        }

        public void NameYourself(string name = "")
        {
            ClearOutput();

            var input = GetInput();

            if (name == "" && input == "")
            {
                OutputText("Scenes.Demonfistfighterscene.NameYourself.MustNameYourself".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => NameYourself());

                return ;
            }

            if (input != "")
                name = input;

            Save.SetString("playerName", name);

            ChallengeBegins();
        }

        public void ChallengeBegins()
        {

            Player.Gems -= 500;

            OutputText("Scenes.Demonfistfighterscene.ChallengeBegins.YellsRingName".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChallengeBegins.TakesGemPouch".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChallengeBegins.NodsAccepts".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChallengeBegins.StretchesArm".Localized());

            var monster = EnemyRegistry.Create(new Identifier("coc", "demon_fist_fighter"));

            monster.CreateStatusEffect(StatusEffectIds.GenericRunDisabled, 0, 0, 0, 0);

            CombatScene.Start(monster, () => Go<DemonFistFighterScene>());
        }

        public void LustKO(bool angry)
        {

            OutputText("Scenes.Demonfistfighterscene.LustKO.MovementsSlow".Localized());

            OutputText("Scenes.Demonfistfighterscene.LustKO.LethiceTits".Localized());

            OutputText("Scenes.Demonfistfighterscene.LustKO.CoylyAsks".Localized());

            OutputText("Scenes.Demonfistfighterscene.LustKO.StopsRambling".Localized());

            OutputText("Scenes.Demonfistfighterscene.LustKO.SpotsOpening".Localized());

            OutputText("Scenes.Demonfistfighterscene.LustKO.GrabsJaw".Localized());

            PlayerWins(angry);
        }

        public void PlayerWins(bool angry)
        {

            OutputText("Scenes.Demonfistfighterscene.PlayerWins.CrowdSilent".Localized());

            OutputText("Scenes.Demonfistfighterscene.PlayerWins.IncubusBreaks".Localized());

            OutputText("Scenes.Demonfistfighterscene.PlayerWins.ShutHellUp".Localized());

            OutputText("Scenes.Demonfistfighterscene.PlayerWins.ApproachesGrabsWrist".Localized(
                Player.Mf("King", "Queen")
            ));

            Save.SetBool("beatDemonfist", true);

            CombatScene.CleanupAfterCombat();
        }

        public void RegularPlayerLoss()
        {

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.StumblesBack".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.CheersChange".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.BreathesDeeply".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.ReadiesWeapon".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.DropsGround".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.CrowdDisappointed".Localized());

            var hpRatio = CurrentMonster.HPRatio();

            var lustRatio = CurrentMonster.LustRatio();

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.PullingYouUp".Localized());

            if (hpRatio < 0.25f || lustRatio > 0.75f)
                OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.HellOfShow".Localized());
            else
                if (hpRatio < 0.5f || lustRatio > 0.5f)
                    OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.TrainAndReturn".Localized());
                else
                    OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.TrainMore".Localized());

            OutputText("Scenes.Demonfistfighterscene.RegularPlayerLoss.SlapsBack".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((0.2) * 60)));

            CombatScene.CleanupAfterCombat();
        }

        public void AngryDemonfistPlayerLoss()
        {

            Save.SetInt("timesBrokenRules", Save.GetInt("timesBrokenRules", 0) + 1);

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.BarelyStanding".Localized());

            if (TimesFoughtDemonfist() > 1 && Save.GetInt("timesBrokenRules", 0) == 1)
            {
                OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.PunchesTurned".Localized());

                OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.ShrugsCrowd".Localized());
            }
            else
                if (TimesFoughtDemonfist() == 0)
                    OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.WhatWasPoint".Localized());
                else
                    if (Save.GetInt("timesBrokenRules", 0) > 1)
                        OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.FuckedOrStupid".Localized());

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.CrowdRoars".Localized());

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss.WorldShifting".Localized());

            AngryDemonfistPlayerLoss2();
        }

        public void AngryDemonfistPlayerLoss2()
        {
            ClearOutput();

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss2.BodilyFluids".Localized());

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss2.ThrownInCell".Localized());

            OutputText("Scenes.Demonfistfighterscene.AngryDemonfistPlayerLoss2.MaybeLater".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void WhatNext()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.WhatNext.AsksIntentions".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.StopsSwig".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.SlamsMug".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.SwipesFinger".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.NoncommittalNod".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.HeShrugs".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.MakesSense".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.SmilesAgain".Localized());

            OutputText("Scenes.Demonfistfighterscene.WhatNext.VoicePalpable".Localized());
        }

        public void ChatOrigins()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.AsksOrigins".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.HeScoffs".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.AgreesPointsOut".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.KeepGoing".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.GroansLightly".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.ProdsMore".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.HeChuckles".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.BoredOfRunning".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.LaughsRemembering".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.InItForFight".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.HeSmiles".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.AnotherGulp".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.YourStory".Localized());

            OutputText("Scenes.Demonfistfighterscene.ChatOrigins.NotSureTelling".Localized());

            ClearButtons();

            AddNextButton("Common.Button.NoBusiness".Localized(), () => ChatOriginsAnswer(0));

            AddNextButton("Common.Button.Ingnam".Localized(), () => ChatOriginsAnswer(1));

            AddNextButton("Common.Button.Wanderer".Localized(), () => ChatOriginsAnswer(2));

            AddNextButton("Common.Button.Lie".Localized(), () => ChatOriginsAnswer(3));

            ReturnToCamp(1);
        }

        public void ChatOriginsAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.NoneOfBusiness".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.WaitressArrives".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.DeepBreath".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.QuarterDrunk".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.GoneWorse".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.NotMuchInteresting".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.HeShrugs".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.WaitressArrivesAgain".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.NotAboutToTell".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.HuhAmazing".Localized());

                OutputText("Scenes.Demonfistfighterscene.ChatOriginsAnswer.WaitressArrivesThird".Localized());
                }

                break;
            }

            ReturnToCamp(1);
        }

        public void QuestionTheBazaar()
        {
            ClearOutput();

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.LooksAround".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.HowPossible".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.NarrowsEyes".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.OrganizesThoughts".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.HeFrowns".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.EveryNonDemon".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.QuickToPointOut".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.GroansConcedes".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.Repositioning".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.FervorFaded".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.JustVanished".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.ItDawns".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.NotTired".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.RepeatsBelieve".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.ThinksWords".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.NoFuckingWay".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.StaresDeep".Localized());

            OutputText("Scenes.Demonfistfighterscene.QuestionTheBazaar.ImpossibleAfterAll".Localized());

            ReturnToCamp(1);
        }

    }
}