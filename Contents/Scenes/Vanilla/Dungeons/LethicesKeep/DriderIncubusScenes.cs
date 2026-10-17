using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<DriderIncubusScenes>("coc:drider_incubus_scenes")]
    public class DriderIncubusScenes : Scene
    {

        public override void Entry()
        {
            EncounterDriderIncbutt();
        }

        public void EncounterDriderIncbutt()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.EncounterDriderIncbutt.DoorSwingsFreely".Localized());

            OutputText("Scenes.Driderincubusscenes.EncounterDriderIncbutt.ClicksTeethChittering".Localized());

            OutputText("Scenes.Driderincubusscenes.EncounterDriderIncbutt.TorchlightGlittersOnCoal".Localized());

            OutputText("Scenes.Driderincubusscenes.EncounterDriderIncbutt.LooksLikeYoull".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "drider_incubus")), RoomDungeon.ResumeFromFight);
        }

        public void SpooderbuttGetsANewCockSleeve(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            var m = (DriderIncubusEnemy)CurrentMonster!;

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.FloorOfThroneRoom".Localized());

            if (hpVictory)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.WoundedWord".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.FlushedWord".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.CheekWord".Localized());

            if (hpVictory)
            {
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.DontRememberDropping".Localized());

                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.InsectileLegsClatter".Localized());
            }
            else
            {
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.TooTurnedOnTo".Localized());

                if (!Player.Weapon.IsAttached())
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.ClatteredDullyNext".Localized());

                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.InsectileLegsClatterAgain".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.SoundBetweenHiss".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.SurroundedByCage".Localized());

            if (hpVictory)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.BreatheThroughCovering".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.PantNeedilyUnder".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.IfOnlyYouMove".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.EasilyPassesYouTo".Localized());

            if (!Player.IsTaur() && !Player.IsDrider())
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.AcrossTopPortion".Localized());

            if (m.GoblinFree)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.GoblinSlaveRemounts".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.SkittersThroughThrong".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeve.DeathlySilenceFalls".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpooderbuttGetsANewCockSleeveII);
        }

        public void SpooderbuttGetsANewCockSleeveII()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.WayYoureBeingHeld".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.LongNailedFingersDig".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.HandsomeDriderResponds".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.VoiceLiltsPleasantly".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.MostGraciousMyQueen".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.SoftFingersStrokeHair".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveII.BowingDemonLordsShadow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpooderbuttGetsANewCockSleeveIII);
        }

        public void SpooderbuttGetsANewCockSleeveIII()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.BindingsCutFromYou".Localized());

            if (!Player.HasPlainSkin())
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.ThroughYourSkinWord".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.DoesntLethiceIsToo".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.WebbingFallsAway".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.EyesCaptureYourOwn".Localized());

            if (Player.HumanScore() < 5)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.NoMatterHowMutated".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.DrowningInThose".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.PinkSkinnedQueenJostles".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.WantToDenyDesperately".Localized());

            if (Player.Corruption >= 50)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.LetYourselfBecomeCorrupted".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.SorelyTemptedBy".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.SentencePeriod".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.CouldntEvenSaveOne".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.OnlyAMatterOf".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 0)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.WhitneysFarmConsumed".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.ThanksForPreparingWhitney".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.GoLimpDefeated".Localized());

            if (Player.LustPercent >= 90)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.AndHornyWord".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.UnwillingToAdmit".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.StrokesHairIgnoring".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.LookBackUp".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIII.VoiceRingsWithAs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpooderbuttGetsANewCockSleeveIV);
        }

        public void SpooderbuttGetsANewCockSleeveIV()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.WillPressesOnYou".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.NoMatterHowRepugnant".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.NoMatterWhatObjections".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.WhatSheTastesLike".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.StruggleUpAndWipe".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.SpreadsLipsWetnessDrips".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.WayWetterNow".Localized());

            if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.JudgingByTheWay".Localized());

                if (silly)
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.CouldDrownSomeone".Localized());
            }
            else
                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.JudgingByPreIs".Localized());
                else
                    if (Player.LustPercent >= 95)
                        OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.JudgingByHowMuch".Localized());
                    else
                        OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.JudgingByHowHot".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.EachDropletSpills".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.DoTheOnlyThing".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.SmellHitsYouFirst".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.DontTryToWipe".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.TongueFizzesOnContact".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.GigglesLikeAGirl".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.CheeksNoseChinSoon".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.RiverOfHoneyGushes".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.SmellPussyTastePussy".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.YourCocksSpurt".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.PluralS".Localized());

                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.UselesslyIntoArmor".Localized());
            }

            if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.YourVaginaWord".Localized());

                if (Player.Cocks.Count == 0)
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.DrenchesUndergarments".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.MixesRightIn".Localized());

                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.SentencePeriod".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.FeelLikeGuitarString".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveIV.HandPushesYourHead".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpooderbuttGetsANewCockSleeveV);
        }

        public void SpooderbuttGetsANewCockSleeveV()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveV.ComeToWhenPair".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveV.ExhaustedSleepClaims".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpooderbuttGetsANewCockSleeveVI);
        }

        public void SpooderbuttGetsANewCockSleeveVI()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveVI.TrueToWordsConquered".Localized());

            if (Player.Vaginas.Count == 0)
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveVI.AfterBeingOutfitted".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveVI.AndWord".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveVI.ServedInBreedingPits".Localized());

            OutputText("Scenes.Driderincubusscenes.SpooderbuttGetsANewCockSleeveVI.DarkQueenNotForget".Localized());

            GameOver();
        }

        public void BeatTheSpooderbutt(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DriderincubusDefeated, 1);

            var m = (DriderIncubusEnemy)CurrentMonster!;

            if (hpVictory)
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.CorruptedDriderCries".Localized());
            else
            {
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.CorruptedDriderCriesAgain".Localized());

                if (!m.GoblinFree)
                    OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.PushesGoblinOff".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.GrabsHoldOfHis".Localized());

                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.MasturbatingLikeWild".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.ThuddingVoiceBetween".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.AnotherVoiceHighPitched".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.SilenceHangsOverDemons".Localized(
                Player.IsChild() ? "young " : ""));

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.ParagraphBreak".Localized());

            if (!m.GoblinFree)
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.GoblinSeparatesFromHer".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.GoblinDropsToKnees".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.EyesFullAndLuminous".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.MitziWord".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.WigglesHerWord".Localized());

                if (m.GoblinOiled)
                    OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.OiledUpWord".Localized());

                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.ButtAtYouWord".Localized());
            }
            else
                OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.UnspoolsLongTongue".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.WinkingMitziWillHelp".Localized());

            OutputText("Scenes.Driderincubusscenes.BeatTheSpooderbutt.LooksLikeDemonsIntend".Localized());

            DriderDefeatMenu();
        }

        public void DriderDefeatMenu(bool doneDrider = false)
        {

            var doneDriderbus = false;

            var doneGoblin = false;

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.DriderincubusKilled) != 1 && CoC.Instance.Flags.Get(GameFlagIds.DriderincubusFucked) != 1)
            {
                AddButton(0, "Common.Button.KillDrider".Localized(), KillDrider);

                if (Player.Vaginas.Count > 0)
                    AddButton(1, "Common.Button.RideDrider".Localized(), RideDrider);
                else
                    AddButtonDisabled(1, "Ride Drider", "Scenes.Driderincubusscenes.DriderDefeatMenu.NeedsVagina".Localized());

                if (Player.Cocks.Count > 0)
                    AddButton(2, "Common.Button.ButtfuckDrider".Localized(), ButtfuckDrider);
                else
                    AddButtonDisabled(2, "Buttfuck Drider", "Scenes.Driderincubusscenes.DriderDefeatMenu.NeedsCock".Localized());

                AddButton(3, "Common.Button.LetGo".Localized(), LetDriderGo);
            }
            else
                doneDriderbus = true;

            if (CoC.Instance.Flags.Get(GameFlagIds.MitziRecruited) != 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MitziFucked) != 1)
                {
                    if (Player.Cocks.Count > 0)
                    {
                        AddButton(5, "Common.Button.FuckGoblin".Localized(), FuckMitzi);

                        AddButton(6, "Common.Button.GoblinTittyfuck".Localized(), TitfuckMitzi);
                    }
                    else
                    {
                        AddButtonDisabled(5, "Fuck Goblin", "Scenes.Driderincubusscenes.DriderDefeatMenu.NeedsCock".Localized());

                        AddButtonDisabled(6, "Goblin Tittyfuck", "Scenes.Driderincubusscenes.DriderDefeatMenu.NeedsCock".Localized());
                    }

                    if (Player.Vaginas.Count > 0)
                        AddButton(7, "Common.Button.GoblinLicks".Localized(), MitziEatsPussy);
                    else
                        AddButtonDisabled(7, "Goblin Licks", "Scenes.Driderincubusscenes.DriderDefeatMenu.NeedsVagina".Localized());
                }

                AddButton(8, "Common.Button.RecruitGoblin".Localized(), RecruitMitzi);

                if (doneDriderbus)
                    AddButton(9, "Common.Button.LeaveGoblin".Localized(), AfterDriderbuttFight);
            }
            else
                doneGoblin = true;

            if (doneDriderbus && doneGoblin)
                AfterDriderbuttFight();
        }

        public void AfterDriderbuttFight()
        {
            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void KillDrider()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.KillDrider.LaughAtLethice".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.DemonicHighHeels)
                OutputText("Scenes.Driderincubusscenes.KillDrider.HeelWord".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.KillDrider.FootWord".Localized());

            OutputText("Scenes.Driderincubusscenes.KillDrider.DownOnCorruptedDrider".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DriderincubusKilled, 1);

            DriderDefeatMenu();
        }

        public void LetDriderGo()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.LetDriderGo.LookingUponFallenDrider".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DriderincubusKilled, 1);

            DriderDefeatMenu();
        }

        public void FuckMitzi()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MitziFucked, 1);

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ShrugAndStartStripping".Localized());

            if (silly)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.AndSmutWriters".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ItsHubris".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.GapesAtTheSight".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.AskTheGoblinPivoting".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.YouGrabYour".Localized());

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Driderincubusscenes.FuckMitzi.SecondWord".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.FuckMitzi.SmallestWord".Localized());

                OutputText("Scenes.Driderincubusscenes.FuckMitzi.DickAndShakeIt".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.GreenSkinnedGirlShivers".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.EsSuffix".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.BitingLowerLipShe".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.GrabLockOfPurple".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.LicksPurplePaintedLips".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.LayYourPipeAcross".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.WouldntBeANeed".Localized());

            if (Player.BiggestCockArea() > 100)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.ThroatBulgingUnholy".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.PolishingYourKnob".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.DontHaveTheTime".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.GoblinLooksLikeCry".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.GuideHerDown".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ElfinEarsPerkUp".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.WrapsTailAroundYour".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.TailWrapsLegAgain".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.DoesntNeedToPull".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.KnowingGirlsPussy".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.WhatSheLooksLike".Localized());

            if (Player.BiggestCockArea() > 250)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.BestPartIsThat".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ParagraphBreak".Localized());

            if (Player.BiggestCockArea() <= 250)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.BottomingOut".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.FillingHerMuch".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.TooMuchForMitzi".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.DontSeeAnyPoint".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.DemonsInAreaHave".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.JustLookingAtGoblin".Localized());

            if (CampScene.FollowersCount() + CampScene.SlavesCount() + CampScene.LoversCount() > 0)
            {
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.YourOtherFollower".Localized());

                if (CampScene.FollowersCount() + CampScene.SlavesCount() + CampScene.LoversCount() > 1)
                    OutputText("Scenes.Driderincubusscenes.FuckMitzi.PluralS".Localized());

                OutputText("Scenes.Driderincubusscenes.FuckMitzi.WouldntMindHavingA".Localized());
            }
            else
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.CertainlyWouldntMind".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ParagraphBreakAgain".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.CouldTryToHelp".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.NoHarmInWaking".Localized());

            if (Player.CumQ() >= 15000)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.ThoughtYouProduceCum".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.MaybeShedEvenGive".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.ThoughtOfMitziPregnant".Localized());
            // 原版 DriderIncubusScenes.hx:382 — CeraphScene.hx：已解锁 Ceraph 的露出剧情
            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.ThatThoughtProbablyJust".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.VictoriousSlapOnMitzi".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.QuakePullingTaut".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.HoldYourselfPumping".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.PullingOut".Localized());

            if (Player.HasKnot())
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.WetSoundingPop".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.SatisfiedGrabYourAnd".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.WithCumPregnantBelly".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Driderincubusscenes.FuckMitzi.SpoogeDrippingDown".Localized());

            OutputText("Scenes.Driderincubusscenes.FuckMitzi.SlowlyRousingBack".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.RecruitMitzi".Localized(), RecruitMitzi);

            AddButton(1, "Common.Button.Next".Localized(), () => DriderDefeatMenu());
        }

        public void RecruitMitzi()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.RecruitMitzi.GiveMitziFriendlyPat".Localized());

            OutputText("Scenes.Driderincubusscenes.RecruitMitzi.BeamsEcstaticallyBows".Localized());

            OutputText("Scenes.Driderincubusscenes.RecruitMitzi.ExcitableThingIsGone".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MitziRecruited, 1);

            ClearButtons();

            DriderDefeatMenu();
        }

        public void MitziEatsPussy()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.ShrugAndStartStripping".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.GoblinLikeHerKind".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.WhileYouHaveOne".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.WhileYouMayLack".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.ThatTongueIsJust".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.LiftYourBallsBefore".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.Comma".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.StretchingCunnyWithPair".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.LittleLoveSlaveCrawls".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.GreenskinTartLicks".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.SimpleContactOfFlesh".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.EagerToPleaseLets".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.MightAsWellBe".Localized());

            if (Player.Wetness() >= 3)
                OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.CopiousWord".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.LubricationDownCrease".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.SwallowsNoisily".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.TooMuchYouDrop".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.FingersDriftFromHer".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussy.MeltAgainstYourBody".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MitziEatsPussyII);
        }

        public void MitziEatsPussyII()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussyII.VisionClearsMitziIs".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussyII.HarlotWithTooLong".Localized());

            OutputText("Scenes.Driderincubusscenes.MitziEatsPussyII.DontHaveTimeTo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RecruitMitzi".Localized(), RecruitMitzi);

            AddButton(1, "Common.Button.Next".Localized(), () => DriderDefeatMenu());
        }

        public void TitfuckMitzi()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.GrinAtTheGoblin".Localized());

            if (Player.Balls > 0 && Player.LustPercent >= 75)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.AndTooFullBalls".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.OnlyFittingThatShe".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.WastesNoTimeDropping".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.PurrsAfterSlobbery".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitzi.ThinksSheCanGet".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Quick".Localized(), TitfuckMitziQuick);

            AddButton(1, "Common.Button.Slow".Localized(), TitfuckMitziSlow);
        }

        public void TitfuckMitziSlow()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.QuickLookAroundAssembled".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.CooesDelightedlyKisses".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.MoanNoPointIn".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.GreenskinSlutPleads".Localized());

            if (Player.BiggestCockArea() <= 20)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.TinyWord".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.TowerOfMeat".Localized());

            if (Player.BiggestCockArea() <= 20)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.NearlyObscuring".Localized());
            else
                if (Player.BiggestCockArea() >= 100)
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.BarelyAbleToSupport".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SentencePeriod".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.KeepsTheOther".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.PluralS".Localized());

                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AngledOutOfWay".Localized());

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ItWord".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ThemWord".Localized());

                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.GentleEncouragingStroke".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.BrushesPurpleDyedHair".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.KissSlowYetSearing".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ViridianVixenCollects".Localized(
                Player.Mf("master's", "mistress's")
            ));

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ArchingBackDrags".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.OverYourBallsAnd".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.UpToEitherSide".Localized());

            if (Player.Balls > 0 && Player.BallSize > 5)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.DribbleYetMorePrecum".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.DribblePrecumAgain".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.VoiceAboveThoseTwo".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AcknowledgementHalf".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AsksSheSlowlyForces".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.LazilyNodAndGrab".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SensationsAssaulting".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.JustLikeThatSqueezing".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.GaspFeelingMomentStart".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ChewsBottomLipStill".Localized(
                Player.Mf("masters", "mistresses")
            ));

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.PussyPromiseWhatIs".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.PlushDickMassagersRoll".Localized());

            if (Player.BiggestCockArea() >= 100)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SometimesPlantsAFew".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.HeadIsSwimming".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.WorstOrBestPart".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.DuskyEyesFlickUp".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.MusclesTwitch".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.YourFeelLikeGoing".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.EdgeIsRightThere".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.OnceAgainPullsBack".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.TapsItAgain".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.InvoluntaryContractions".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniMet) > 0)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SinceMeetingTamani".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.EvenBreezesFeelGood".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.LookOverWhereOiled".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SnarlsAtCorrupted".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.DemonBacksOffFading".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.BreastsCompressingAbout".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AnnouncesWithDelight".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.IfItWasntBefore".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.MoansHerSkinFeels".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.ItsTwitchingPowerfully".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.TheyWord".Localized());

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.BothWord".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AllWord".Localized());

                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.AreEvenTheOne".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.PluralSAgain".Localized());

                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.NotGettingMuchAttention".Localized());
            }
            else
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.FlingingRopesOfPre".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SoCloseYouCan".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.PluralSThird".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.CuspOfFiring".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.GreenSlutMoansGingerly".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.GodsShesRight".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.HusksSheDrags".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.SlipsYouRightBack".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.JizzBubblesOut".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlow.LittleDoesMitziKnow".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TitfuckMitziSlowII);
        }

        public void TitfuckMitziSlowII()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlowII.FinallyExhaustYourself".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlowII.SitUpNotingShe".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziSlowII.YouveGotADemon".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RecruitMitzi".Localized(), RecruitMitzi);

            AddButton(1, "Common.Button.Next".Localized(), () => DriderDefeatMenu());
        }

        public void TitfuckMitziQuick()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.AnswerPetiteGreenSlut".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.PressesYourAgainstSide".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.ConfidentSlutRises".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.NipplesDraggingOver".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.MatterOfFactlyGrabs".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.PluralS".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.SlickSoftBoobflesh".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.KeenLittleSlutPicks".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.PluralSAre".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.IsWord".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.ExposedToPurpleLips".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.TrembleALittleBit".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.MovesToNextStage".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.FeelPleasureSpasms".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.LipsNeverLeaveTip".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.TriesToKeepLips".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.OnlyHasOneMouth".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.CockHungryNever".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.PluralSAgain".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.RecklessAbandon".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.LuckyManageTo".Localized());

            if (Player.Cocks.Count > 1 || Player.CumQ() > 500)
                OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.CummyWord".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.EyelashesAndPushes".Localized());

            OutputText("Scenes.Driderincubusscenes.TitfuckMitziQuick.ShouldProbablyDecide".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.RecruitMitzi".Localized(), RecruitMitzi);

            AddButton(1, "Common.Button.Next".Localized(), () => DriderDefeatMenu());
        }

        public void RideDrider()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.RideDrider.PushGoblinAsideAnd".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Driderincubusscenes.RideDrider.SlitherWord".Localized());
            else
                if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed)
                    OutputText("Scenes.Driderincubusscenes.RideDrider.ClopWord".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.RideDrider.StrideWord".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDrider.VictoriouslyTowardPrize".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Driderincubusscenes.RideDrider.FeelALittleGuilty".Localized());
            else
                if (Player.Corruption <= 75)
                    OutputText("Scenes.Driderincubusscenes.RideDrider.OnlyGotThisOne".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.RideDrider.ThisDriderGloriousCock".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDrider.KinarielPleads".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDrider.KneelingOverConsider".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHim".Localized(), ActuallyRideDriderDick);

            AddButton(1, "Common.Button.UseGoblin".Localized(), RideDriderToGoblin);

            AddButton(2, "Scenes.CorruptedGlade.NeverMind".Localized(), () => DriderDefeatMenu());
        }

        public void RideDriderToGoblin()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.RideDriderToGoblin.GrinOutlandishlyAtDemon".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDriderToGoblin.NodsTooEnthusiastically".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDriderToGoblin.NowAboutThisGoblin".Localized());

            OutputText("Scenes.Driderincubusscenes.RideDriderToGoblin.QuiteTheClaim".Localized());

            if (Player.Cocks.Count > 0)
            {
                AddButton(5, "Common.Button.FuckMitzi".Localized(), FuckMitzi);

                AddButton(6, "Common.Button.Mitzititfuck".Localized(), TitfuckMitzi);
            }

            if (Player.Vaginas.Count > 0)
                AddButton(7, "Common.Button.MitziLicks".Localized(), MitziEatsPussy);
        }

        public void ActuallyRideDriderDick()
        {
            ClearOutput();

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.HissInDriderDemon".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.DeserveWorseThan".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.InFrontOfCohorts".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.LooksUpMixTerror".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.NoAmountOfHeckling".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.CooInHisEar".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.ShuddersAndMoans".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.StrokeHisUnderside".Localized());

            if (Player.Wetness() >= 3)
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.SoakedWord".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.ArousedWord".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.LipsSpreadingAgainst".Localized());

            if (Player.GetClitLength() > 3)
            {
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.BestPartTheWay".Localized());

                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.MountingHungerChannel".Localized());

                if (Player.Pregnancy.Type.Path != "none")
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.CuntWord".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.WombWord".Localized());

                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.StuffingSatisfaction".Localized());
            }
            else
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.BestPartWayAgain".Localized());

                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.IsWord".Localized());
                    else
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.AreWord".Localized());

                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.SmashedAgainstThick".Localized());

                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.MountingHungerAgain".Localized());

                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.YourWord".Localized());
                    else
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.AWord".Localized());

                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.DickAgainstHis".Localized());

                    if (Player.Pregnancy.Type.Path != "none")
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.CuntWordAgain".Localized());
                    else
                        OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.WombWordAgain".Localized());

                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.StuffingSatisfactionAgain".Localized());
                }

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.LiftingYourEdgeCloser".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.SlipsInSoEasily".Localized());

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.VirginityAnAfterthought".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.AlmostRegretFighting".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.GrabHoldOfShoulders".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.JeersFromAudience".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.BounceUpAndDown".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.InsectLegsSplay".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.ButterslickHeatSlams".Localized());

            if (Player.GetClitLength() > 3 || Player.Cocks.Count > 0)
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.OneOfWord".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.HisNipplesWord".Localized());

            if (Player.GetClitLength() > 3 || Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.WhileStillTryingTo".Localized());

                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.PumpYourClit".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.GiveYouAHandjob".Localized());
            }

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.FloodsYouWithTainted".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.GoOffWithHim".Localized());

            if ((Player.Vaginas.Count > 0 && Player.Vaginas.Any(vagina => vagina.Virgin))
                || (Player.Corruption <= 33 && !SceneInstance<CeraphScene>.Get().HasExhibition()))

                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.NUnexpectedly".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.WonderfulFeelingCum".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.RideTheWaves".Localized());

            if (!Player.HasPerk(PerkIds.HistorySlut) && !Player.HasPerk(PerkIds.HistoryWhore))
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.ChasteWord".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.DeterminedWord".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.ChampionEnteredMareth".Localized());

            OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.OnlyWhenYourNewly".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.StaggerUpMoreThan".Localized());
            else
                if (Player.Corruption <= 75)
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.StaggerUpBetterThan".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.ActuallyRideDriderDick.MakeSureToKiss".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DriderincubusFucked, 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

            Player.Orgasm("Vaginal");

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            DriderDefeatMenu();
        }

        public void ButtfuckDrider()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DriderincubusFucked, 1);

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.LookDefeatedDriderOver".Localized());

            if (((DriderIncubusEnemy)CurrentMonster!).GoblinFree)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.SeeHisTurgidRod".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GoblinSlungUnder".Localized());

            if (Player.Corruption > 75)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.MagnificentCreature".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.MakeYourWayBehind".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GrowlsGlancingBetween".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.FollowHisGazeUp".Localized());

            if (Player.Corruption > 75)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.WatchCloselyBitch".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.EnjoyThisShow".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.YouShoutWord".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.LungingUpOnto".Localized());
            else
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.ClamberingUpInto".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.ShiftOfYourHips".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.VoiceEchoingAgainst".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.AndItIsTight".Localized());

            if (Player.BiggestCockArea() >= 1200)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.FeelMammothMember".Localized());
            else
                if (Player.BiggestCockArea() >= 500)
                    OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.AnimalisticPrickStretches".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.PrickSinksDeep".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.SurprisinglyMoist".Localized());

            if (Player.IsDrider())
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.ReachDownBestial".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.SlapDriderAss".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.DemonWailsGrabbing".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.AnotherCalls".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.HandsRoamHisHumanoid".Localized());

            if (((DriderIncubusEnemy)CurrentMonster!).GoblinFree)
            {
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.LeftToHerDevices".Localized());

                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GruntBreaksFree".Localized());

                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GoblinMuffledCries".Localized());
            }
            else
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GoblinFirmlyMounted".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.LetDriderHaveHis".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.AddedBonusOf".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.GreenSlutScreams".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.FollowOverSexedGoblin".Localized());

            if (Player.CumQ() <= 1000)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.SquirtLoadDeep".Localized());
            else
                if (Player.CumQ() <= 5000)
                    OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.PumpThickCreamyLoad".Localized());
                else
                    OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.FloodDriderAss".Localized());

            OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.SpunkBloatedDemon".Localized());

            if (Player.Corruption >= 75)
                OutputText("Scenes.Driderincubusscenes.ButtfuckDrider.CantWaitToSee".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

            Player.Orgasm("Dick");

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            DriderDefeatMenu();
        }

        [Register("coc:keep_hook_drider")]
        public static void KeepHookDrider() =>

            SceneInstance<DriderIncubusScenes>.Get().EncounterDriderIncbutt();

    }
}