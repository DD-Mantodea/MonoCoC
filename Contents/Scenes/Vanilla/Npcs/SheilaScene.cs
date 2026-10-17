using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<SheilaScene>("coc:sheila")]
    public class SheilaScene : Scene
    {

        public override void Entry()
        {
            SheilaFirstEncounter();
        }

        public void SheilaPreg()
        {

            if (Player.KnockUpChance(5, 75))
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.ShielaIncubation);
        }

        public int SheilaCorruption(int arg = 0)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) + arg < 0)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, 0);
            else
                CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) + arg);

            return CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption);
        }

        public string SheilaCup()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 10)
                return "tiny, almost-absent";
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 20)
                    return "A-cup";
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 30)
                        return "B-cup";
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 40)
                            return "C-cup";
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 50)
                                return "D-cup";
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 60)
                                    return "DD-cup";
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 70)
                                        return "E-cup";
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 80)
                                            return "EE-cup";
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 90)
                                                return "front-running";
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 100)
                                                    return "FF-cup";
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 110)
                                                        return "G-cup";
                                                    else
                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 120)
                                                            return "GG-cup";
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 130)
                                                                return "H-cup";
                                                            else
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 140)
                                                                    return "HH-cup";
                                                                else
                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 150)
                                                                        return "'Holy Honkin' Hooters!'-cup";
                                                                    else
                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 160)
                                                                            return "I 'want to squeeze them'-cup";
                                                                        else
                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 170)
                                                                                return "II-cup";
                                                                            else
                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 180)
                                                                                    return "Jiggle-cup";
                                                                                else
                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 190)
                                                                                        return "JJ-cup";
                                                                                    else
                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 200)
                                                                                            return "K-cup";
                                                                                        else
                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 210)
                                                                                                return "KK-cup";
                                                                                            else
                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 220)
                                                                                                    return "L-cup";
                                                                                                else
                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 230)
                                                                                                        return "'Ladylove'-cup";
                                                                                                    else
                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 240)
                                                                                                            return "M-cup";
                                                                                                        else
                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 250)
                                                                                                                return "'Mega Milk'-cup";
                                                                                                            else
                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 260)
                                                                                                                    return "N-cup";
                                                                                                                else
                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 270)
                                                                                                                        return "NN-cup";
                                                                                                                    else
                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 280)
                                                                                                                            return "O-cup";
                                                                                                                        else
                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 290)
                                                                                                                                return "'so big it makes you say' OO-cup";
                                                                                                                            else
                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 300)
                                                                                                                                    return "P-cup";
                                                                                                                                else
                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 310 && Player.Cocks.Count > 0)
                                                                                                                                        return "'would feel great around your' PP-cup";
                                                                                                                                    else
                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 310 && Player.Cocks.Count == 0)
                                                                                                                                            return "PP-cup";
                                                                                                                                        else
                                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 320)
                                                                                                                                                return "Q-cup";
                                                                                                                                            else
                                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 330)
                                                                                                                                                    return "QQ-cup";
                                                                                                                                                else
                                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 340)
                                                                                                                                                        return "R-u-serious?-cup";
                                                                                                                                                    else
                                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 350)
                                                                                                                                                            return "RR-cup";
                                                                                                                                                        else
                                                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 360)
                                                                                                                                                                return "S-rank";
                                                                                                                                                            else
                                                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 370)
                                                                                                                                                                    return "'Shooting Star'-cup";
                                                                                                                                                                else
                                                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 380)
                                                                                                                                                                        return "T-cup";
                                                                                                                                                                    else
                                                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 390)
                                                                                                                                                                            return "'damn, those are some big' TT-cup";
                                                                                                                                                                        else
                                                                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 400)
                                                                                                                                                                                return "'bigger than' U-cup";
                                                                                                                                                                            else
                                                                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 410)
                                                                                                                                                                                    return "UU-cup";
                                                                                                                                                                                else
                                                                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 420)
                                                                                                                                                                                        return "V-cup";
                                                                                                                                                                                    else
                                                                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 430)
                                                                                                                                                                                            return "'Va-Va-Voom'-cup";
                                                                                                                                                                                        else
                                                                                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 440)
                                                                                                                                                                                                return "'War Head'-cup";
                                                                                                                                                                                            else
                                                                                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 450)
                                                                                                                                                                                                    return "World-cup";
                                                                                                                                                                                                else
                                                                                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 460)
                                                                                                                                                                                                        return "X-Buster-cup";
                                                                                                                                                                                                    else
                                                                                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 470)
                                                                                                                                                                                                            return "XXXX-cup";
                                                                                                                                                                                                        else
                                                                                                                                                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 480)
                                                                                                                                                                                                                return "'Yay'-cup";
                                                                                                                                                                                                            else
                                                                                                                                                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 490)
                                                                                                                                                                                                                    return "YY-cup";
                                                                                                                                                                                                                else
                                                                                                                                                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 500)
                                                                                                                                                                                                                        return "Z-Saber-cup";
                                                                                                                                                                                                                    else
                                                                                                                                                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 510)
                                                                                                                                                                                                                            return "ZZ-cup";
                                                                                                                                                                                                                        else
                                                                                                                                                                                                                            return "fuckin' magic";
        }

        public void SheilaEncounterRouter()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaClock) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaClock, 0);

            if (SheilaCorruption() >= 100 && CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaDemon, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 0)
            {
                if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0 && CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) >= -2)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) == 0)
                        NormalSheilaPregNotifNumberOne();
                    else
                        NormalSheilaPregNotifREPEATEDED();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 0)
                    SheilaFirstEncounter();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == -1)
                        SheilaApology();
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == -2)
                            SheilaIsSorrySheRapedYou();
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == -3 || CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == -4)
                                SheilaReallyMad();
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 1)
                                    SheilaReconcile();
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 2 || (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 3 && View.Time.Hours != 20))
                                        SheilaGettingFamiliar();
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 3 && View.Time.Hours == 20)
                                            SheilaXPThreeSexyTime();
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 4)
                                                FuckBuddySheilaMeeting();
                                            else
                                            {
                                                OutputText("Scenes.Sheilascene.SheilaEncounterRouter.XpRangeBugAlert".Localized(
                                                    CoC.Instance.Flags.Get(GameFlagIds.SheilaXp)
                                                ));

                                                DoNext(() => ReturnToCamp(1));
                                            }
            }
            else
                if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0)
                    DemonImpChildren();
                else
                    DemonSheilaEncounter();
        }

        public void SheilaCorruptionUpdate()
        {

            if (Player.Corruption > 90)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));

                SheilaCorruption(10);
            }
            else
            {
                var corDiff = Math.Round((double)(SheilaCorruption() - Player.Corruption));

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }
        }

        public void SheilaFirstEncounter()
        {
            ClearOutput();

            var corDiff = Math.Round((double)(SheilaCorruption() - Player.Corruption));

            SheilaCorruption((int)(-corDiff / 2));

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 30f));

            OutputText("Scenes.Sheilascene.SheilaFirstEncounter.StrangeSightBrush".Localized((noFur ? "You can't tell if it's an animal or person, all you can see is a pair of furry legs under human clothing, with a stubby tail sticking out" : "It looks like a dog, though all you can see is a pair of furry hindquarters and a stubby tail draped over them"), (noFur ? ", with" : "; what looked like a dog's shoulders from afar is actually"), (noFur ? "n animal" : " dog"), (noFur ? "" : " actually"), (noFur ? "" : " digitigrade"), (noFur ? "that are somewhat different from any animal you've seen" : "only passingly resembling a canine's"), (noFur ? "knee" : "thigh")));

            ClearButtons();

            AddButton(0, "Common.Button.LookCloser".Localized(), Sheila1ndEncLookCloser);

            AddButton(1, "Common.Button.StayHidden".Localized(), Sheila1stEncStayHidden);
            }
        }

        public void Sheila1stEncStayHidden()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.Sheila1stEncStayHidden.HoldBreathStayStill".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Sheila1ndEncLookCloser()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloser.PokeUpBetterLook".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloser.UnbendAnswerChallenge".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), Sheila1ndEncLookCloserPtII);
        }

        public void Sheila1ndEncLookCloserPtII()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.SubmitToExamination".Localized());

            if (Player.WeaponName == "Scarred Blade")
                OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.BladeVibratesEagerly".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.TakeOwnLook".Localized((noFur ? "feet are extremely long" : "legs are crooked"), (noFur ? "she stands on her toes" : "straightened")));

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.SatisfiedApplesRemark".Localized());

            if (Player.Libido >= 50)
                OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.SmirksEnjoyingLookie".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.NodIntroduceYourself".Localized());

            if (Player.ShortName != "Sheila")
                OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.MatesCallMeSheila".Localized());
            else
                OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.NicknameJokeReply".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.FriendlyStrangerSurprise".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.HardLuckReply".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.FrownsOffersFood".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.AskAboutMates".Localized());

            if (Player.DemonScore() >= 4)
                OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.TakeASquizz".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.DenyAccusationGlare".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.WordsAtOdds".Localized());

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtII.AskTalkMore".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), Sheila1ndEncLookCloserPtIITalk);

            AddButton(1, "Common.Button.Leave".Localized(), Sheila1ndEncLookCloserPtIILeave);
        }

        public void Sheila1ndEncLookCloserPtIILeave()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtIILeave.TakeLeaveGoodOnya".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void Sheila1ndEncLookCloserPtIITalk()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -1);

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtIITalk.TakesSwingEarbashing".Localized());

            Player.HealthChange(-2);

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtIITalk.FeelLikeLeavingFists".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), Sheila1ndEncLookCloserPtIITalkFight);

            AddButton(14, "Common.Button.Leave".Localized(), Sheila1ndEncLookCloserPtIITalkLeave);
        }

        public void Sheila1ndEncLookCloserPtIITalkLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.Sheila1ndEncLookCloserPtIITalkLeave.BackAwayDirtyGlance".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Sheila1ndEncLookCloserPtIITalkFight()
        {
            ClearOutput();

            var sheilaEnemy2 = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy2.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy2.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy2, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);
        }

        public void SheilaApology()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaApology.SnareCatchesLeg".Localized());

            OutputText("Scenes.Sheilascene.SheilaApology.OhItsYouRemark".Localized());

            OutputText("Scenes.Sheilascene.SheilaApology.PeevedCatchingYou".Localized());

            OutputText("Scenes.Sheilascene.SheilaApology.StepAsideRetrieveTrap".Localized());

            OutputText("Scenes.Sheilascene.SheilaApology.ReceptiveEnoughSpeak".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Apologize".Localized(), ApologySheilaApology);

            AddButton(1, "Common.Button.Slapninsult".Localized(), ApologySheilaSlapSult);

            AddButton(2, "Common.Button.Nothing".Localized(), ApologySheilaSayNothing);

            AddButton(3, "Common.Button.AvoidHer".Localized(), ApologyAvoidSheila);
        }

        public void ApologyAvoidSheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.ApologyAvoidSheila.RatherNeverSeeAgain".Localized());

            OutputText("Scenes.Sheilascene.ApologyAvoidSheila.EarsTwitchArrange".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ApologySheilaApology()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.ApologySheilaApology.ScratchEarBroachTopic".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaApology.QuirksBrowAcceptsApology".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaApology.WanSmileResetsSnare".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaApology.BackToSquareOne".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ApologySheilaSlapSult()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.SilenceWhileSnareReset".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.WellQuestionHardLine".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.SurpriseSlapFirst".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.CountersNotTakingIt".Localized(
                Player.Mf("", "ine")
            ));

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.DodgesSecondSwing".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSlapSult.AssumesFightingStance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -3);

            var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void ApologySheilaSayNothing()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.ApologySheilaSayNothing.CrossArmsFrown".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSayNothing.PleaseYourselfTosser".Localized());

            OutputText("Scenes.Sheilascene.ApologySheilaSayNothing.GruntHeadBackCamp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaIsSorrySheRapedYou()
        {
            ClearOutput();

            ShowImage("sheila-talk");

            OutputText("Scenes.Sheilascene.SheilaIsSorrySheRapedYou.StalkingPlainsHunting".Localized());

            OutputText("Scenes.Sheilascene.SheilaIsSorrySheRapedYou.GdayApologyOpening".Localized());

            OutputText("Scenes.Sheilascene.SheilaIsSorrySheRapedYou.TrailsOffRubsElbow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Forgive".Localized(), SheilaIsSorryAndYouForgive);

            AddButton(1, "Common.Button.Fight".Localized(), SheilaPologyFight);

            if (Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                AddButton(2, "Common.Button.Castarouse".Localized(), SheilaPologyCastArouse);

            AddButton(3, "Common.Button.Lemmealone".Localized(), SheilaIsSorryButLeaveMeAlone);
        }

        public void SheilaIsSorryButLeaveMeAlone()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaIsSorryButLeaveMeAlone.DismissiveGestureTurn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaIsSorryAndYouForgive()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaIsSorryAndYouForgive.OkaySingleWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaIsSorryAndYouForgive.JustOkaySurprised".Localized());

            OutputText("Scenes.Sheilascene.SheilaIsSorryAndYouForgive.BonzerLoadOffMind".Localized());

            OutputText("Scenes.Sheilascene.SheilaIsSorryAndYouForgive.NodLopesAway".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);
            else
                CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaPologyFight()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaPologyFight.SwingDodgedWarning".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -3);

            var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void SheilaPologyCastArouse()
        {
            ClearOutput();

            Player.ChangeFatigue(15, FatigueType.Physical);

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.ForgiveRevengeGears".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.MockUnderstandingNod".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.KneesBeginToShake".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.SpellWetSpot".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.FeignedConcernAsk".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.FlinchesAtTouch".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.ShakingStepWetness".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.StepBackwardCalled".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.AskWhatSheWants".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.PleaseLetMeFuck".Localized());

            OutputText("Scenes.Sheilascene.SheilaPologyCastArouse.BetterAndBetter".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((Player.Libido / 5))));

            ClearButtons();

            AddButton(0, "Common.Button.LetHer".Localized(), CastArouseAndLetHer);

            AddButton(14, "Common.Button.Leave".Localized(), CastArouseAndLeaveSheila);
        }

        public void CastArouseAndLeaveSheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.CastArouseAndLeaveSheila.NoTimeRightNow".Localized());

            OutputText("Scenes.Sheilascene.CastArouseAndLeaveSheila.HorrifiedCaughtAlone".Localized());

            OutputText("Scenes.Sheilascene.CastArouseAndLeaveSheila.MelodramaticFine".Localized());

            OutputText("Scenes.Sheilascene.CastArouseAndLeaveSheila.LeaveHerCalling".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, 100);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDemon, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCite, -1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CastArouseAndLetHer()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.CastArouseAndLetHer.BoundsAndJumps".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SheilaLossRapeRouter);
        }

        public void SheilaReallyMad()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReallyMad.RustleBushStanding".Localized());

            OutputText("Scenes.Sheilascene.SheilaReallyMad.ForgotCatchRealize".Localized());

            OutputText("Scenes.Sheilascene.SheilaReallyMad.SulkingWantNothing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) >= 90)
                OutputText("Scenes.Sheilascene.SheilaReallyMad.TaintGoesCrazy".Localized());

            OutputText("Scenes.Sheilascene.SheilaReallyMad.PleaseLeaveAlone".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == -3)
                OutputText("Scenes.Sheilascene.SheilaReallyMad.NotLikeOtherMonsters".Localized());

            OutputText("Scenes.Sheilascene.SheilaReallyMad.SayEndOnly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Stand".Localized(), SheilaReallyMadStandGround);

            AddButton(14, "Common.Button.Leave".Localized(), SheilaReallyMadLeave);
        }

        public void SheilaReallyMadLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReallyMadLeave.HuffThrowAnimals".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaReallyMadStandGround()
        {
            ClearOutput();

            if (!silly)
            {
                OutputText("Scenes.Sheilascene.SheilaReallyMadStandGround.ThrowAnimalsFoldArms".Localized());

                OutputText("Scenes.Sheilascene.SheilaReallyMadStandGround.ShiversRaisesFists".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.SheilaReallyMadStandGround.CallYourStand".Localized());

                OutputText("Scenes.Sheilascene.SheilaReallyMadStandGround.ThrustFistAir".Localized());
            }

            var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            if (silly)
            {
                CombatController.Instance!.Context.Enemy.Health *= 1.2f;

                Player.ChangeFatigue(-10);

                Player.HealthChange(20);
            }
        }

        public void SheilaReconcile()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReconcile.VoiceBehindBackAgain".Localized());

            ShowImage("monster-sheila");

            OutputText("Scenes.Sheilascene.SheilaReconcile.PanningHorizon".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcile.GrinsCatchOnQuick".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Kay".Localized(), SheilaReconcileKay);

            AddButton(1, "Common.Button.DontWanna".Localized(), SheilaReconcileDunWanna);
        }

        public void SheilaReconcileDunWanna()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReconcileDunWanna.PalmsOutDecline".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileDunWanna.PleasantriesJustBusy".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileDunWanna.BodgyBikkieRemark".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaReconcileKay()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReconcileKay.FreshMeatTempting".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SheilaReconcileKay2);
        }

        public void SheilaReconcileKay2()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.FirepitClassicBush".Localized());

            if (Player.Libido < 50)
                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.BeckoningMeatClause".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.BreastsUnderPretext".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.CaughtEyeNoParsley".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.BrimmingQuestionsBiteLip".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.OffersBiscuitAbruptly".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.DevourMeatSilence".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.WhyLiveOutHere".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.StopsHummingDeciding".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.OnlyThreeBlokes".Localized());

            if (Player.Race == "kangaroo-morph")
                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.ExceptYouClause".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.TradeOpShops".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.PressDemonsTakeYou".Localized());

            if (Player.Libido >= 50)
                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.SawYouPerving".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.CorruptionMakesHornier".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.ToreThroughUs".Localized());
            // 原版 SheilaScene.hx:716 — TelAdre.hx:isAllowedInto — 已获准进入 Tel'Adre
            if (SceneInstance<Places.TelAdre.TelAdre>.Get().IsAllowedInto())
            {
                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.MentionDesertCity".Localized());

                OutputText("Scenes.Sheilascene.SheilaReconcileKay2.SpitsGristleKids".Localized());
            }

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.DropsBoneTalking".Localized());

            OutputText("Scenes.Sheilascene.SheilaReconcileKay2.BroadSmileJoking".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaGettingFamiliar()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaGettingFamiliar.HeyItsName".Localized());

            ShowImage("monster-sheila");

            OutputText("Scenes.Sheilascene.SheilaGettingFamiliar.VoiceRingsOutRight".Localized());

            OutputText("Scenes.Sheilascene.SheilaGettingFamiliar.NoCampfireCutLunch".Localized());

            OutputText("Scenes.Sheilascene.SheilaGettingFamiliar.ChildlikeEnthusiasmInfectious".Localized());

            OutputText("Scenes.Sheilascene.SheilaGettingFamiliar.SitPickOffering".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HereOften".Localized(), SheilaFriendlyComeHereOften);

            AddButton(1, "Common.Button.HerPeople".Localized(), SheilaFriendlyHerPeoples);

            AddButton(2, "Common.Button.Curfew".Localized(), SheilaFriendlyCurfew);

            AddButton(3, "Common.Button.NoQuestions".Localized(), SheilaFriendlyNoQuestions);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void SheilaFriendlyNoQuestions()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaFriendlyNoQuestions.HoldTongueMimicPose".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyNoQuestions.PeacefulCompanyRemark".Localized(
                Player.Mf("bloke", "sheila")
            ));

            OutputText("Scenes.Sheilascene.SheilaFriendlyNoQuestions.RaisesHeadSheStands".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyNoQuestions.YouOffThenQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyNoQuestions.DepartsLongLeggedGait".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaFriendlyComeHereOften()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.WhyEnjoyPlains".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.LikeBeingOutHere".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.LakeKneeHighGoblin".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.FrownsChewsAnecdote".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.SmilesOwnWit".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.BushForMost".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyComeHereOften.FoldsHandsStaresClouds".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaFriendlyHerPeoples()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.LieDownAskPeople".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.SingleMindedQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.NotInterestedKnowledge".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.RelievedOrOffended".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.HowAvoidTrouble".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.VillagesFrownPolicy".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.OnlyAtOrgasmQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.FrigidPartnerDonates".Localized());

            if (Player.Femininity < 35)
                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.GlancesAtCrotch".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.SameSexCouplesQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.TakenUnawaresReply".Localized());

            if (Player.Libido < 50)
            {
                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.DemonsFindTransferee".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.ShovedBackDoor".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.SoberingNoMorePrecarious".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.LeanInSalacious".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.FallingForOutsider".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.PigsArseNinny".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.ImpartialAirPretend".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.BeetRedMobShort".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.MeetUpFantasticSex".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.RaiseKidsAtHome".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.QuestionSayEnd".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.SupposeThatllWork".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.TurnKidsOver".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.NoIMeanClause".Localized());

                if (!Player.HasCock())
                    OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.IfIDecideClause".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.NotReadyForClause".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.FuckYouOutburst".Localized());

                OutputText("Scenes.Sheilascene.SheilaFriendlyHerPeoples.GetsUpFlushedLeaves".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaFriendlyCurfew()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.CurfewQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.OrdersFromCap".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.LandmarksPoint".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.EntrancesUniform".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.AdeptAtTunneling".Localized());

            OutputText("Scenes.Sheilascene.SheilaFriendlyCurfew.MolesCantCompare".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaXPThreeSexyTime()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.ProwlTwilitGrasslands".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.WhatWrongQuestion".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.ClingsImpStory".Localized());

            if (Player.Libido < 50)
                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.ImpIncredulity".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.JustOneSpell".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.NoSteamReleased".Localized());
            }

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.ShiversHidesFace".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.HelpMeColonyClosed".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTime.CoupleWaysHelp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), SheilaXPThreeSexyTimeSex);

            AddButton(1, "Common.Button.GuardDuty".Localized(), SheilaXPThreeSexyTimeGuardDuty);

            AddButton(2, "Common.Button.DitchHer".Localized(), SheilaXPThreeSexyTimeDitchHer);
        }

        public void SheilaXPThreeSexyTimeDitchHer()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeDitchHer.RefuseShellBeFine".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeDitchHer.BidFarewellHome".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, 100);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDemon, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCite, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaXPThreeSexyTimeSex()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeSex.EyesWidenPullsBack".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeSex.FrownWhiningNoise".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeSex.MeekCatchingHand".Localized());

            ClearButtons();

            SheilaXP4Sex(false);
        }

        public void SheilaXPThreeSexyTimePostSexLetHerBe()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimePostSexLetHerBe.DressesSatisfiedGlance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            DoNext(() => ReturnToCamp(1));
        }

        public void ShielaXPThreeSexyTimePostSexStay()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.GrabTailCurious".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.ProfessLoveTillMorning".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.NotRealNameBlush".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.ContriveHurtLaughs".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.FrownCloudsEarsDroop".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.LieStrokingHour".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStay.HornyAgainNods".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ShielaXPThreeSexyTimePostSexStayII);
        }

        public void ShielaXPThreeSexyTimePostSexStayII()
        {
            ClearOutput();

            if (Player.HasCockThatFits(32))
            {
                ShowImage("sheila-vag");

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HandSlipsGroinPump".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PlayfullyRollClause".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.OntoBackLegOver".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ROwnWord".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PositionAtopGuideTip".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HoldGazeHalfMinute".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SlumpsSwivelsStirring".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.JustAboveNipple".Localized());
                else
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.OnTheThighClause".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PunishYouDag".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SqueezeHipsSlamDown".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HerWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) < 30)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LittleBreastsRiseFall".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) > 60)
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HugeTitsJiggle".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.BreastsQuiverPumps".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.StrugglesPunishPromise".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.EasyConvulsesFalls".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.BarrelWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ChestWord".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HairHipsPumping".Localized());

                if (Player.Corruption >= 90 || Player.Corruption > CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.RipplingMusclesClamp".Localized());
                else
                    if (Player.Corruption < CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ClampsWouldntPump".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SquirtsSucksAlternately".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.TwitchesSwellsRocks".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.TwitchesInsideHer".Localized());
                else
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SpiesFirstBead".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LonelyPrickWord".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.EachLonelyPricks".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SentencePeriod".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.YoureOnTopRetort".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.AttemptsLiftSpasming".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.JoinedMessyEruption".Localized());

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SentencePeriodAgain".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.RollsOutWaves".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -2f));

                if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                    return ;
                }

                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SlidesForwardWetPatch".Localized());

                if (Player.Corruption < 30)
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.RelaxCaptureSqueeze".Localized());

                SheilaPreg();

                SheilaCorruptionUpdate();
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FingersIntoVagina".Localized(
                        Player.Mf("hermaphrodites unless they have V donger that'll fit", "girls")
                    ));

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LoveOfCourse".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HandStopsUpset".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PullAwayPushDown".Localized());

                    ShowImage("sheila-lickhergina");

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PantsTrickleFuckMe".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.KissInnerThigh".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.StraddleLegSquat".Localized());
                    else
                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SlideUnderLift".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.StraddlePressPussies".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SmearsCrotchMoans".Localized());

                    if (SheilaCorruption() < 30)
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SmallAndHumble".Localized());
                    else
                        if (SheilaCorruption() > 60)
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.WorryGettingMountainous".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HandDriftsPinches".Localized());

                    if (Player.HasTailInsteadOfLegs())
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.TautButtcheekWord".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SentencePeriodThird".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.CupTitsAskLike".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.AdmitsPressSqueeze".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.IntensifyScrapeCheek".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HeatedSlipperyVagina".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LaterMashPlayThem".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.AdmitsTweaksQuakes".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.IntensifyScrapeLabia".Localized());
                    }

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GonnaComeSoon".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ThrustHarderCrossing".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.TailDrawsUpHot".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GrindLikeSexToy".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.OrderAnxiousConvulsing".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SkinRuddyLocksUp".Localized());

                    if (Player.Corruption >= 90 || Player.Corruption > SheilaCorruption())
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SpasmsSuckYouIn".Localized());

                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ScalesWord".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PussyWord".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ComeAsWell".Localized());

                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.BreastsSwellDrool".Localized());
                    }
                    else
                        if (Player.Corruption < SheilaCorruption())
                        {
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.DumpsWaveOrgasm".Localized());

                            if (Player.HasTailInsteadOfLegs())
                                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.BreastsPullAway".Localized());

                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SentencePeriodFourth".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.DroolsSpasmsCoats".Localized());

                            if (Player.HasTailInsteadOfLegs())
                                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.YourTailWord".Localized());
                            else
                                OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.YourOwnWord".Localized());

                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FreshCoatTriggers".Localized());
                        }

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.NeglectedPrefix".Localized());

                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PrickQuakesSpurts".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PricksQuakeSpurt".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.WeaklyDroolsPools".Localized());
                    }

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LeanDownKiss".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.KissSoundsNice".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, -2f));

                    if (SheilaCorruption() >= 90 && Player.Corruption >= 90)
                    {
                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                        return ;
                    }

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.CuddlesArmBeforeSpeaks".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.MaybeDoLoveYou".Localized());
                    else
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.DidntKissMe".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.CallHerHorse".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GaspsDammitStop".Localized());
                    }

                    SheilaCorruptionUpdate();
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HandLowersFingerIn".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SmileAtAdmission".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.StrokesOversizedTool".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ButYouDoQuestion".Localized());

                    ShowImage("sheila-masti");

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ReddensNodsFrigging".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.AndYetPullOut".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.PullsFingerLeverage".Localized());

                        if (!Player.IsTaur())
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FallsIntoLap".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.DriftsBetweenLegs".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.CrawlsStrokesExcited".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.WincesShutUpTosser".Localized());
                    }

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.WorkSoakedPussy".Localized());

                    if (Player.Corruption >= 90 || Player.Corruption > SheilaCorruption())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.HeatsSqueezesFingers".Localized());
                    else
                        if (Player.Corruption < SheilaCorruption())
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GushesClearFluid".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.QuiversPopMouth".Localized());

                    if (SheilaCorruption() >= 90 && Player.Corruption >= 90)
                    {
                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                        return ;
                    }

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ReclaimHandForces".Localized());

                        if (Player.CumQ() > 1000)
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.MouthFillsWrithes".Localized());

                        if (Player.CumQ() >= 2000)
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FinishOnHerFace".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.OtherStrokesDecorate".Localized());

                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.EscapesGagsSeed".Localized());
                    }

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FallsWeaklyBackward".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.FaceGlobbySmears".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GroansYoureJerk".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.CalledMeGoblin".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.SayEndClause".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.YouPrefix".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ActsLikeOneSometimes".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.LaughTugEar".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.AskWorriedNoAnswer".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.WipesFaceStares".Localized());

                    OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ContentLieNext".Localized());

                    if (Player.HasCock())
                    {
                        Player.Orgasm("Dick");

                        Player.ApplyStats(new DynStat(GameStats.Libido, -2f));
                    }
                    else
                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 4)));

                    SheilaCorruptionUpdate();
                }

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.BreaksSilenceCamp".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.GrinDecisionPlay".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.ColorsJustTonight".Localized());

            OutputText("Scenes.Sheilascene.ShielaXPThreeSexyTimePostSexStayII.NodSagelyDress".Localized());

            Player.ChangeFatigue(-20);

            Player.HealthChange(Player.MaxHealth / 2);

            if (View.Time.Hours > 6)
                View.Time.Days += 1;

            View.Time.Hours = 6;

            DoNext(PlayerMenu);
        }

        public void SheilaXPThreeSexyTimeGuardDuty()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.FoldArmsConsent".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.TurnBackWatch".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.ReleasesTurnsKneels".Localized());

            if (Player.Level >= 8)
                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.ImpsBeneathYou".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.StandLongDaydream".Localized());

            if (Player.Libido < 40)
            {
                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.NoneOfYourConcern".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.WetSoundsIncrease".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.TooMuchTurnBack".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.YelpsFailsSoaks".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.QuirkBrowTurnAway".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.InterestingChoiceShoulder".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.StareRedderCough".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.TearsDownDam".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.ApproveMissingOneThing".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.ShiftPelvisForward".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.TongueOutPlayfully".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.GigglesRelaxesInto".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.RightMateSigh".Localized());

                OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.ClosesEyesBlocksView".Localized());
            }

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.UnfinishedThoughtHand".Localized());

            OutputText("Scenes.Sheilascene.SheilaXPThreeSexyTimeGuardDuty.OfferStayBeetRed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            if (Player.LibidoPercent < 40)
                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
            else
                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeeting()
        {
            ClearOutput();

            ShowImage("sheila-talk");

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.HeyGreetingOnly".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.VoiceFromShrub".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.PlunkDownAskHow".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.GoodHangingAround".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.BorrowExpressionHead".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.RestsMinuteThenAsks".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeeting.NoteBreastsSize".Localized(
                SheilaCup()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Chat".Localized(), FuckBuddySheilaMeetingChat);

            AddButton(1, "Common.Button.Sex".Localized(), () => SheilaXP4Sex(true));

            AddButton(2, "Common.Button.JustHang".Localized(), FuckBuddySheilaMeetingJustHangOut);

            AddButton(3, "Common.Button.MaybeLater".Localized(), FuckBuddySheilaMeetingMaybeLater);
        }

        public void FuckBuddySheilaMeetingMaybeLater()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingMaybeLater.NoTimeSqueezeKnee".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingMaybeLater.OkayMaybeLater".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingJustHangOut()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.PinchButtThenScoot".Localized());

            if (!Player.IsTaur() && !Player.IsDrider())
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.ScootOwnForward".Localized());
            else
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.FoldLegsUnder".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.LayInBushShade".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.RatbagNoVenom".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.RelaxMindWander".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.BlinkingGoingAlready".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.RetrievesCatchThanks".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.PoorGirlBlowsKiss".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingJustHangOut.MaybeFunNextTime".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f), new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChat()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChat.FeelingTalkativeAsk".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChat.WhatBringUp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FamnFriends".Localized(), FuckBuddySheilaMeetingChatFamilyAndFriends);

            AddButton(1, "Common.Button.OldLoves".Localized(), FuckBuddySheilaMeetingChatOldLoves);

            AddButton(2, "Common.Button.WhyHide".Localized(), FuckBuddySheilaMeetingChatWhyHide);

            AddButton(3, "Common.Button.Livewithme".Localized(), FuckBuddySheilaMeetingChatLiveWithMe);

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) > 0)
                AddButton(4, "Common.Button.Kids".Localized(), FuckBuddySheilaMeetingChatKids);
        }

        public void FuckBuddySheilaMeetingChatFamilyAndFriends()
        {
            ClearOutput();

            ShowImage("sheila-private-talk");

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.StaresObtuselyDeflect".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.LeastYouCouldDo".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.DadGoodBloke".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.AndYourMom".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.MomNurseBigDeal".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.SoundsNiceYetNot".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.NursesMaintainNursery".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.NicknameSmileReddens".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.LotsOfSiblings".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.LaconicallyAnotherStare".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.JawWorkChangeSubject".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.WorkBuddiesUpgraded".Localized());

            if (Player.Libido >= 50)
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.ConspiratorialSexInterest".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.FoodShelterJob".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.TrueButUnanswered".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.InterestedAsAnyGirl".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.PleasantPictureDistraction".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.SquirmsArmWaist".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.NudgeShirtTeaseSide".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.OkaySeeYouAuntie".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.NeckBurnsEarsTwitch".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.AllDudesNoGirlfriends".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.MoroseGalsMyAge".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.NeverEndedWithOne".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.DrilledDifferentFaith".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.LetsGoHeadingBack".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.TellHattieSureWill".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatFamilyAndFriends.SkinBloomsAboutFaces".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChatOldLoves()
        {
            ClearOutput();

            ShowImage("sheila-private-talk");

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.DontWantToTalk".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.OfferOwnHistoryRefused".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.InheritedPersonalityPoint".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.BoomerAssumption".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.EventuallyComfortableQuestion".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.StiflesLaughNotFunny".Localized());

            if (Player.Libido < 50)
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.CouplesSoundingAlike".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.RichGirlTalk".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.WhatBonzerMeans".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.BonzerMeansGreat".Localized(
                    Player.Mf("bloke", "sheila")
                ));

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.OhIsThatSo".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.DontTeaseDriveCrazy".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.GetUsedToIt".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.JustProposedQuestion".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.BrightRedNo".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.SoonTenYears".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.LooksAwayFoldsArms".Localized());

                if (Player.Corruption >= 60)
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.DefensivePushesTits".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.NotGonnaMarryYou".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.MeansStoppingSeeing".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.DistraughtLogicalConclusion".Localized());

                if (Player.Corruption < 30)
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.HandOnShoulder".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.PullsAwayNoseRed".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.StickItOutQuestion".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatOldLoves.FrownsPlaysWithHand".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChatWhyHide()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.EyesCloudDubious".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.WaveOffOnePlace".Localized());
            // 原版 SheilaScene.hx:1497 — TelAdre.hx:isAllowedInto — 已获准进入 Tel'Adre
            if (SceneInstance<Places.TelAdre.TelAdre>.Get().IsAllowedInto())
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.DesertCityBustling".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.SnortsOneBadApple".Localized());
            }

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.KeepInTouchSwaps".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatWhyHide.ShiversLiftsArm".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChatLiveWithMe()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatLiveWithMe.AskMoveInCamp".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatLiveWithMe.HardlyKnowYou".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatLiveWithMe.RelentsDifferentWorld".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatLiveWithMe.FeetWistfulCatch".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChatKids()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) == 1)
            {
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SheepishAhQuestion".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.RepeatHowKidDoing".Localized());

                ShowImage("sheila-private-talk");

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.WrapsArmSurprised".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.StrongGripAsk".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SeriousAboutThis".Localized());

                if (Player.Femininity > 50)
                {
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.PrettyFaceReaction".Localized());

                    if ((int)Player.Gender >= 2)
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.LesbianClamDiver".Localized());
                    else
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.GirlyBoysBears".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.TeaseBlushDeepens".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.StopItProtest".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ShakeHandOrPunch".Localized());

                    if ((int)Player.Gender <= 1)
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SoundsProtective".Localized());
                    else
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.PunchAGirlTease".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.DaddysGirlFirstChild".Localized());
                }

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SeriousAboutMeeting".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ReadsExpressionHopes".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SoonQuestion".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ShrugDependsQuota".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.CatchingEnoughGame".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.NotOftenBiscuits".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.HideLaughterSmock".Localized());

                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.LeanUntilSheLeaves".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) < 10)
                {
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.HowAreTheKids".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.DowncastSmileFine".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ElaboratesMeasurement".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.FourInchesFigure".Localized());

                    if (Player.Libido > 50)
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.TightHotCuntClause".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.AskGrowUpTime".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.TwoYearsPurebred".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ImplicationsAskAge".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.LaughsWontTellAge".Localized(
                        Player.Mf("gentleman", "lady")
                    ));

                    if ((int)Player.Gender >= 3)
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.NotherWord".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.LadysAgeSayEnd".Localized());

                    if (Player.Libido >= 50)
                    {
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.RetortTickleEar".Localized(
                            Player.Mf("gentleman", "lady")
                        ));

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.ShrieksSlapsRootRat".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.RubCheekApology".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.DontKillMessenger".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.OwnKidAtFour".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.CaptainsDontEncourage".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.KnowsFromExperience".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.MeetsGazeMumNurse".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SighsNothingMore".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.KidsAllRightAgain".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.HollowLinedFace".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SeeItReflections".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.AttemptDemurWavedOff".Localized());

                    OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.HandInYours".Localized());

                    if (Player.Libido < 50)
                    {
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SomeoneElseWatch".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.EyesHardenMumVolunteered".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.SoQuestion".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.NurseryCronesConstant".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.EerilyBitter".Localized());

                        if (Player.Corruption < 40)
                            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.OfferWatchThem".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.CantLeaveColony".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys))
                        ));

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.AskWhyCant".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.DontAskExplainPolicy".Localized());

                        if (silly)
                            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.BodypillowClause".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.NotRightNow".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.TooBadArmAround".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.GoodOilDrooling".Localized());

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.LoveYouSoon".Localized());

                        if (Player.Corruption < 40)
                            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));
                    }
                    else
                    {
                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.WhisperTurnedOn".Localized(
                            SheilaCup()
                        ));

                        OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKids.TrustYouNeverChange".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + 5)));

                        SheilaXP4Sex(false);

                        AddButton(9, "Common.Button.Restfornow".Localized(), FuckBuddySheilaMeetingChatKidsRest4Now);

                        return ;
                    }
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckBuddySheilaMeetingChatKidsRest4Now()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.AlwaysDrawHand".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.KissForeheadLays".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.BackWord".Localized());
            else
                OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.LapWord".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.SweetTakeUpOffers".Localized());

            OutputText("Scenes.Sheilascene.FuckBuddySheilaMeetingChatKidsRest4Now.DozesNudgeAwake".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaXP4Sex(bool display = false)
        {
            ClearOutput();

            if (display)
            {
                OutputText("Scenes.Sheilascene.SheilaXP4Sex.ArmAroundPropose".Localized());

                OutputText("Scenes.Sheilascene.SheilaXP4Sex.FidgetsFingersRove".Localized());

                OutputText("Scenes.Sheilascene.SheilaXP4Sex.LeanCloseEar".Localized());

                OutputText("Scenes.Sheilascene.SheilaXP4Sex.BlushesSaysYes".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Withwang".Localized(), null);

            AddButton(1, "Common.Button.Withcooch".Localized(), null);

            AddButton(2, "Common.Button.FuckHer".Localized(), null);

            if (Player.HasCock())
                AddButton(0, "Common.Button.Withwang".Localized(), () => ConsensualSheila69(true));

            if (Player.HasVagina())
                AddButton(1, "Common.Button.Withcooch".Localized(), () => ConsensualSheila69(false));

            if (Player.HasCockThatFits(32))
                AddButton(2, "Common.Button.FuckHer".Localized(), ConsentacleVagSexForKangarooSlutBitches);

            AddButton(3, "Common.Button.Masturbation".Localized(), SheilaMutualMasturbation);
        }

        public void ConsensualSheila69(bool cock = true)
        {
            ClearOutput();

            ShowImage("sheila-private-talk");

            OutputText("Scenes.Sheilascene.ConsensualSheila69.LeanInBuzzEar".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.RaisedBrowExposedFlesh".Localized());

            if (cock)
            {
                if (Player.HasCockThatFits(32))
                {
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.SmilesAtCockProduce".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.AskPuttingItIn".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.WideEyesPressedLips".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TooFarSizeProtest".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.SLetter".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.RubsLabiaQuestion".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.DropsBottomsSmiles".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Sheilascene.ConsensualSheila69.CuteLittleFlower".Localized());

                if (Player.GetClitLength() > 8)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.StemConclusion".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.SighWetFinger".Localized());
            }

            OutputText("Scenes.Sheilascene.ConsensualSheila69.DrawCloseCoaxForward".Localized());

            if (cock && Player.CockThatFits(32) < 0)
                OutputText("Scenes.Sheilascene.ConsensualSheila69.WontFitProtest".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.PleaseYourselfRemark".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.CommentaryCutsOff".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ConsensualSheila69.TurnHangClause".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.StraddleSquatDangle".Localized());

            if (cock)
                OutputText("Scenes.Sheilascene.ConsensualSheila69.PreSeepingShaft".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.WetPussyWord".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.WagglesOverHead".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.GaspsCouldHaveTold".Localized());

            if (cock)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.HandAroundShaft".Localized());

                if (Player.CockThatFits(32) < 0)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.BendsAgainstChest".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriod".Localized());
            }
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.ReturnsFavorPlunge".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Sheilascene.ConsensualSheila69.PumpsTrickleBringLower".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.ObligesClause".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ConsensualSheila69.FlatteningBack".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.SquattingLower".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.SoYourPrefix".Localized());

            if (cock)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.CockSinksDown".Localized());

                if (Player.HasCockThatFits(32))
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.IntoWarmMouth".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.GrabsBreastsInstead".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriodAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.DanglesOverFace".Localized());

                if (Player.GetClitLength() > 8)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.NipsSucksClit".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.PartsLabiaTongue".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriodThird".Localized());
            }

            OutputText("Scenes.Sheilascene.ConsensualSheila69.OutdoneNotClause".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ConsensualSheila69.BuryTongueInside".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.BothHandsStroke".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.LoverStopsHeadRolls".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.CriesSqueezesNoSemen".Localized());

            if (!cock)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.TongueBackIn".Localized());

                if (Player.GetClitLength() > 8)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.StrokesClitRapidly".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriodFourth".Localized());
            }
            else
                if (Player.HasCockThatFits(32))
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TakesInMouthAgain".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.StrokesCockNippleLine".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.EmbarrassmentKeepsWorking".Localized());

                if (cock && Player.CockThatFits(32) < 0)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TeasingNipplesHeaving".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.HummingInDelight".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriodFifth".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.SureShellKeepWorking".Localized());

                if (cock && Player.CockThatFits(32) < 0)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.HardNipplesWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.ThroatWord".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SentencePeriodSixth".Localized());
            }

            if (cock && Player.CockThatFits(32) < 0)
                OutputText("Scenes.Sheilascene.ConsensualSheila69.ImComingCries".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.MuffledMoansRealization".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.VaginaConvulsesPrefix".Localized());

            if (Player.Corruption >= 90 || Player.Corruption > CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.TryingPullClause".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TongueWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.FingersWord".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.RavenouslyMusclesRipple".Localized());
            }
            else
                if (Player.Corruption < CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                {
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.GushDeluges".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.MouthFilledTaste".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.SoaksHandsOrgasm".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.ThighsTensePelvisAir".Localized());
                }
                else
                    if (Player.Corruption == CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption) && Player.Corruption < 90)
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.DroolsTrickleScreams".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.JamsFaceUpward".Localized());

            if (cock && Player.CockThatFits(32) < 0)
                OutputText("Scenes.Sheilascene.ConsensualSheila69.LipsTongueBaseShaft".Localized());
            else
                if (cock)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.SinksIntoThroat".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.DeepTongueStrokesClit".Localized());

            OutputText("Scenes.Sheilascene.ConsensualSheila69.CrotchOnFace".Localized());

            if (cock)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.StrokeAfterStrokeSemen".Localized());

                if (Player.HasCockThatFits(32))
                {
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.PoursLoverThroatGags".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.WithdrawForcesBackUp".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Sheilascene.ConsensualSheila69.RelentlessDumpsMore".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ConsensualSheila69.StomachRoundBackflow".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.SquirtsIntertwinedBodies".Localized());

                    if (Player.CumQ() > 1000)
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.WellspringMattiedFuzz".Localized((noFur ? "legs" : "thighs")));

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.OtherCockPrefix".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Sheilascene.ConsensualSheila69.SpasmsWord".Localized());
                        else
                            OutputText("Scenes.Sheilascene.ConsensualSheila69.SSpasmSuffix".Localized());

                        OutputText("Scenes.Sheilascene.ConsensualSheila69.AddsToMess".Localized());
                    }
                }
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.QuakesLatchesTongue".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TrickleSuckedGreedy".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.GushFaceSlick".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ConsensualSheila69.OverflowSoaksHair".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.WrapHeadFlattenEars".Localized());
            }

            OutputText("Scenes.Sheilascene.ConsensualSheila69.SpentRollOff".Localized());

            if (cock && Player.HasCockThatFits(32))
                OutputText("Scenes.Sheilascene.ConsensualSheila69.PullFreeHandsKnees".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsensualSheila69.SitsUpWipesFace".Localized());

            if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                return ;
            }
            else
                if ((Player.Corruption > 90 || Player.Corruption > SheilaCorruption()) && SheilaCorruption() >= 80 && CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) != 3)
                {
                    SheilaCorruptionUpdate();

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TitsReadyToFire".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.FoggedBrainStaresBreasts".Localized());

                    OutputText("Scenes.Sheilascene.ConsensualSheila69.TyingUpFriends".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

                    Player.SlimeFeed();

                    ClearButtons();

                    AddButton(0, "Common.Button.Listen2her".Localized(), SheilaCorruptionWarningListenToHer);

                    AddButton(1, "Common.Button.FondleHer".Localized(), SheilaCorruptionWarningsChooseFondle);

                    return ;
                }

            OutputText("Scenes.Sheilascene.ConsensualSheila69.CoughsStaresSlack".Localized());

            if (cock && Player.Balls > 0)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.ExplosiveLaughStinkEye".Localized());

                if (Player.Balls == 2)
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.InfinityWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.DoubleInfinity".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.BallsImprintKangaroo".Localized());
            }

            OutputText("Scenes.Sheilascene.ConsensualSheila69.BloodyWarningRemark".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

            SheilaCorruptionUpdate();

            Player.SlimeFeed();

            OutputText("Scenes.Sheilascene.ConsensualSheila69.UnrufflesPullsClose".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 3)
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.ThanksBetterThanOutside".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SpendsNightOutsideQuestion".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.NotFirstTime".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.SmallHugStaysAwake".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHerBe".Localized(), SheilaXPThreeSexyTimePostSexLetHerBe);

                AddButton(1, "Common.Button.StayWithHer".Localized(), ShielaXPThreeSexyTimePostSexStay);

                return ;
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsensualSheila69.ForgiveYouWanker".Localized());

                if (cock && Player.HasCockThatFits(32))
                    OutputText("Scenes.Sheilascene.ConsensualSheila69.SmearHandOnTits".Localized());

                OutputText("Scenes.Sheilascene.ConsensualSheila69.LieUntilSheLeaves".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void ConsentacleVagSexForKangarooSlutBitches()
        {
            ClearOutput();

            var x = Player.CockThatFits(48);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DoffsHatShy".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SighsTickleBreaksEmbrace".Localized(
                SheilaCup()
            ));

            if (SheilaCorruption() <= 30)
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PertWord".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.JigglyWord".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BreastsFiddlesClothing".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SqueezingFondlingClause".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PlungingTwoFingers".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ShiverPawsNubs".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.WhinesPutItIn".Localized());

            if (Player.Cocks[Player.CockThatFits(32)].Id == BodyPartTypes.Kangaroo)
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.TaperedFleshyPenis".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.CocksToken".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.WhatWrongTease".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BlushUnfastensBelt".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DrongoStopTeasing".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.GrinOnly".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.TakingHoldClause".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ManeuverIntoPosition".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.RubHeadDripping".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.GripThighsLiftBottom".Localized((noFur ? "" : " fuzzy")));
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.LiftsBottomLines".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.HereItComesThrust".Localized(
                Player.CockHead(x)
            ));

            if (SheilaCorruption() <= 30)
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PerkyBreastsRock".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.JigglingBreastsDomes".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DrawInOutPussy".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.RubsSqueezesOwnBreasts".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PauseKissAdjustAngle".Localized());

                if (SheilaCorruption() <= 30)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.FirmWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PillowyWord".Localized());

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ResumeThrustingNipples".Localized());
            }

            if (Player.Balls > 0 || Player.HasVagina())
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.StubbyTailCurling".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BallsToken".Localized());
                else
                    OutputText(Player.VaginaDescript(0));

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.TicklesAsSheWrithes".Localized());

                if (Player.Balls == 0 && Player.HasVagina())
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.CuntTwitchesDrools".Localized());
            }

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.StuffHarder".Localized());

            if (SheilaCorruption() < 30)
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.NipplesTraceArcs".Localized());
            else
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.TitsSlapHerFace".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.MoanClampsWrings".Localized());

            if (Player.Corruption >= 90 || Player.Corruption > CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PainfulIntensityBuilds".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.OtherPrefix".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DickErupts".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DicksErupt".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.FestoonCrotchStrings".Localized());
                }

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SensationAscendsDick".Localized());
            }
            else
                if (Player.Corruption < CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                {
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ResistanceDiesScream".Localized());

                    if (SheilaCorruption() < 30)
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PerkyWord".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PillowyWordAgain".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BreastsHeldTautRams".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SupersedesControlWrings".Localized(
                        Player.SMultiCockDesc()
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.OrOntoThighs".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SentencePeriod".Localized());
                }

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PullOutPregnant".Localized());

            if (!Player.HasKnot(x))
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ObligePullOutWet".Localized(
                    Appearance.CockNoun(Player.Cocks[x].Id)
                ));

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BreastsAndFaceClause".Localized());

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SentencePeriodAgain".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.GlobsPasteGrass".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.KnotSticksLocked".Localized());

                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BellyRoundsGonads".Localized());

                    if (Player.CumQ() >= 3000)
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.StretchedFrothJism".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PeriodSpace".Localized());
                }

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.OtherCocksEruptWhite".Localized((noFur ? "coating" : "matting down the fur on")));
            }

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.OrgasmsFinishUp".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.FinallyPullOut".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.LieBreathingRaggedly".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -0.5f));

            if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                return ;
            }
            else
                if ((Player.Corruption > 90 || Player.Corruption > SheilaCorruption()) && SheilaCorruption() >= 80 && CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) != 3)
                {
                    SheilaCorruptionUpdate();

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.BigPointersExclaim".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.LookAndLook".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.GCupsUglyCow".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SnapOutOfDaze".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DidYouHearTits".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.RightTheyreNice".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.StaggersJiggleHuge".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.HardWord".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.WetWord".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ImagineMoundsPressed".Localized());

                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.KernelOfTruth".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Listen2her".Localized(), SheilaCorruptionWarningListenToHer);

                    AddButton(1, "Common.Button.FondleHer".Localized(), SheilaCorruptionWarningsChooseFondle);

                    return ;
                }

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DipstickCameInMe".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.SorryTooSpent".Localized());

            OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.TeasesKissCheek".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 3)
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.ThankfulnessWritten".Localized());

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.PlansHideAllNight".Localized());

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.DarkNotHard".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHerBe".Localized(), SheilaXPThreeSexyTimePostSexLetHerBe);

                AddButton(1, "Common.Button.StayWithHer".Localized(), ShielaXPThreeSexyTimePostSexStay);

                return ;
            }
            else
            {
                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.LieInSunWhile".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.StrokesChestFur".Localized());
                else
                    OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.WipesOffJizz".Localized());

                OutputText("Scenes.Sheilascene.ConsentacleVagSexForKangarooSlutBitches.CollectGearSeeYou".Localized());

                DoNext(() => ReturnToCamp(1));
            }

            SheilaPreg();

            SheilaCorruptionUpdate();
        }

        public void SheilaMutualMasturbation()
        {
            ClearOutput();

            var target = "";

            var choices = new List<string>();

            if ((Player.HasCock()) && ((Player.IsTaur() && Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length) || Player.HasCock()))
                    choices.Add("cock");

            if (Player.HasVagina() && !Player.IsTaur())
                choices.Add("vagina");

            if (Player.HasFuckableNipples())
                choices.Add("nipplecunt");

            if (Player.Gender == Gender.Genderless || choices.Count == 0)
                choices.Add("ass");

            if (choices.Count == 0)
            {
                CoCSettings.Error("");

                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.MasturbationErrorBug".Localized());

                DoNext(() => ReturnToCamp(1));
            }

            target = choices[Random.Shared.Next(choices.Count)];

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SnatchHatHandShirt".Localized());

            if (silly)
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.ChanSuffix".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.QuestionSayEnd".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.NoAnswerUnfastenBelt".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.GigglesAdmiringNudity".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.LieDownBeside".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.FoldingLegsUnder".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.AndBeginPrefix".Localized());

            if (target == "cock")
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.StrokesPointsSmears".Localized());
            else
                if (target == "cunt")
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SlidesToVaginaParts".Localized());
                else
                    if (target == "nipplecunt")
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.FingersDistendedNipple".Localized());
                    else
                        if (target == "ass" && !Player.IsTaur())
                            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.LickFingerTeaseAsshole".Localized());
                        else
                            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SucksDigitMouth".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.TouchOrPlay".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.LatterSlySmile".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.DubiousCutOffHand".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.KnowHowToFool".Localized());

            if (target == "cock")
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.StrokesShaftDrools".Localized());
            else
                if (target == "cunt")
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.DemonstrateFingersVagina".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SighFollowsLead".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.KissHereTouchThere".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.DoWhateverPleases".Localized());

            if (target == "cock")
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.ReturnHandPumping".Localized());
            else
                if (target == "cunt")
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SlideHandBetweenLegs".Localized());
                else
                    if (target == "nipplecunt")
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.HandReturnsNipple".Localized());
                    else
                        if (!Player.IsTaur())
                            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.ReachesAssholeSpears".Localized());
                        else
                        {
                            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.WishHadSomething".Localized());

                            if (silly)
                                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.EvenPaperclip".Localized());

                            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SentencePeriod".Localized());
                        }

            ShowImage("sheila-masti");

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.HandAlongsideGasps".Localized());

            if (target != "cock" && target != "cunt" && target != "nipplecunt")
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.YoursJoinsEntrance".Localized());

            OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.AlwaysYoursKiss".Localized());

            if (target == "cock")
            {
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.PrickBegins".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.PricksBegin".Localized());

                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.TwitchEjaculateMidriff".Localized());
            }
            else
                if (target == "cunt")
                {
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.PussyConvulses".Localized());

                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.DroolingWord".Localized());
                    else
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SquirtingWord".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.ShakesShiversOrgasm".Localized());
                }
                else
                    if (target == "nipplecunt")
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.NippleConstrictsOozes".Localized());
                    else
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.FingersPulledMuscles".Localized());

            if (Player.Corruption >= 90 || Player.Corruption > CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.LipsDrawYours".Localized());
            else
                if (Player.Corruption < CoC.Instance.Flags.Get(GameFlagIds.SheilaCorruption))
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.KissBreaksForehead".Localized());
                else
                {
                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.BreaksKissEyesSoften".Localized());

                    if (target == "cunt" || target == "cock" || target == "nipplecunt")
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.EmptyReservesFall".Localized());
                }

            if (target == "cunt" || target == "cock" || target == "nipplecunt")
                Player.Orgasm("Generic");
            else
                Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                return ;
            }
            else
                if ((Player.Corruption > 90 || Player.Corruption > SheilaCorruption()) && SheilaCorruption() >= 80 && CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) != 3)
                {
                    SheilaCorruptionUpdate();

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.BlondBimboBreasts".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.CertainlyDoSqueeze".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.DemonicEnergyQuestion".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.NormalPeopleBoring".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Listen2her".Localized(), SheilaCorruptionWarningListenToHer);

                    AddButton(1, "Common.Button.FondleHer".Localized(), SheilaCorruptionWarningsChooseFondle);

                    return ;
                }
                else
                {
                    SheilaCorruptionUpdate();

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.StrokesHairLying".Localized());

                    if (target == "cock")
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.StaringBellybuttonLoad".Localized());
                    else
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.TogetherWord".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.HadSoMuchFun".Localized());

                    if (Player.Gender == Gender.Genderless && silly)
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.KenDollLove".Localized());

                    OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SayEndLabel".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) == 3)
                    {
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.SighsSlipsOut".Localized());

                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.WhatsupSittingUp".Localized());

                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.FindPlaceHide".Localized());

                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.ReallySpendNightAlone".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.LetHerBe".Localized(), SheilaXPThreeSexyTimePostSexLetHerBe);

                        AddButton(1, "Common.Button.StayWithHer".Localized(), ShielaXPThreeSexyTimePostSexStay);

                        return ;
                    }
                    else
                        OutputText("Scenes.Sheilascene.SheilaMutualMasturbation.NuzzlesWishCouldStay".Localized());
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaCorruptionWarningsChooseFondle()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.FuckItReachEntrance".Localized());

            ShowImage("sheila-private-talk");

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.ListenWhenISpeak".Localized());

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.TurnsBackWalksAway".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.ErectWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.WetWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.AchingWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.EdgesOfRack".Localized());

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningsChooseFondle.BitchTease".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -3);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, 90);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaCorruptionWarningListenToHer()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningListenToHer.FeelFrozenMidCop".Localized());

            ShowImage("sheila-private-talk");

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningListenToHer.StaresLongMinute".Localized());

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningListenToHer.PointedLookRemoveHand".Localized());

            OutputText("Scenes.Sheilascene.SheilaCorruptionWarningListenToHer.ThankPresentsBoundary".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            CoC.Instance.Flags.Set(GameFlagIds.SheilaClock, -4);

            DoNext(() => ReturnToCamp(1));
        }

        public void SheilaGotWhomped()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaGotWhomped.SheilaCollapses".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.SpreadEagledInjuries".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.RubbingThighsSmears".Localized());

            OutputText("Scenes.Sheilascene.SheilaGotWhomped.StandOverScootsAway".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) > -4)
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.ForgiveEverythingNotThis".Localized());

            OutputText("Scenes.Sheilascene.SheilaGotWhomped.ImploresSayEnd".Localized());

            if (SheilaCorruption() >= 90)
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.TouchMeAgainTerror".Localized());

            if (Player.Lust >= 33)
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.TakeAdvantageQuestion".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaGotWhomped.NotHornyEnough".Localized());

            ClearButtons();

            if (Player.Lust >= 33)
            {
                if (Player.HasCockThatFits(48))
                    AddButton(0, "Common.Button.FuckPussy".Localized(), RapeSheilasCooter);

                if ((Player.HasCock()) && ((!Player.IsTaur() && Player.ShortestCockLength() <= Player.Tallness * .35f) || (Player.IsTaur() && Player.Tallness * 1.2f <= Player.ShortestCockLength() && Player.ShortestCockLength() <= Player.Tallness * 1.55f)))
                        AddButton(1, "Common.Button.ForcedBj".Localized(), () => ForcedSheilaOral(true));

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.ForcedLick".Localized(), () => ForcedSheilaOral(false));
            }

            AddButton(14, "Common.Button.Leave".Localized(), LetSheilaGo);
        }

        public void LetSheilaGo()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) >= -3)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -1);

            CombatScene.CleanupAfterCombat();
        }

        public void RapeSheilasCooter()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -4);

            var x = Player.CockThatFits(48);

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.GrabLegPinHer".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ForehoovesAboveHips".Localized());

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.RipShirtOpen".Localized());

            if (SheilaCorruption() < 30)
                OutputText("Scenes.Sheilascene.RapeSheilasCooter.TinyWord".Localized());
            else
                if (SheilaCorruption() > 60)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.HugeSoftWord".Localized());

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.BreastsJiggleForce".Localized());

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.UnfastenBeltUnbutton".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sheilascene.RapeSheilasCooter.SlideBetweenLabia".Localized());

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.DragWetFabric".Localized());

            ShowImage("sheila-vag");

            OutputText("Scenes.Sheilascene.RapeSheilasCooter.ExtractCockDrooling".Localized(
                Player.CockHead(x)
            ));

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Sheilascene.RapeSheilasCooter.FrownEscapeBodyEager".Localized(
                    Player.CockHead(x)
                ));

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.SeriousPushCloth".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.DontTouchSkin".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.PushAgainTest".Localized(
                    Player.CockHead(x)
                ));

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.WithBellyClause".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ShyGaspVaginaCoat".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.IsThisOkay".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.KneesClaspForcesApart".Localized(
                    Player.CockHead(x)
                ));

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.WailsDontCumInside".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ShutUpDoSame".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.FilledShallowPanties".Localized());
                else
                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.SagsBowsSpear".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ImagineInsideCloth".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.StandsMidriffUnhappy".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.CumSlipsOutOpacity".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.NoseUpDismissive".Localized());

                SheilaPreg();

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));
            }
            else
            {
                OutputText("Scenes.Sheilascene.RapeSheilasCooter.WhyShouldntFingers".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ShiversCrookFingers".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.PullHarderRipping".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.StrugglesResumePush".Localized());

                if (Player.CockArea(x) > 32)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.PainSlightlyTooLarge".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.RearsScratchCatchHand".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.RepeatShamedThrust".Localized());

                if (SheilaCorruption() > 10)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.JigglesHerBreasts".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ClenchesJawBloom".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.KissLongNeck".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.QuiversLookAway".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.TransferToBreast".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.LowersHandResigned".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.RestAtopYours".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.EyesCloseChinRaises".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.SqueezeKissJaw".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.SentencePeriod".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.RoughFuckWontLast".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.AffectionatelyPrefix".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.HandAlongFaceKiss".Localized());
                else
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.BendCaressesCheek".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.OfCourseWont".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.SqueezeHerBreast".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.ClimaxSeedPour".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.PressesHandUncomfortably".Localized());

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.GaspsKneesWrap".Localized());

                if (Player.Corruption >= 90 || SheilaCorruption() < Player.Corruption)
                {
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.DrinksSemenGreedily".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.BreastHeatsNipple".Localized());

                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.SentencePeriodAgain".Localized());
                }
                else
                    if (Player.Corruption < SheilaCorruption())
                    {
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.FloodsJuicesTwitching".Localized());

                        if (!Player.IsTaur())
                            OutputText("Scenes.Sheilascene.RapeSheilasCooter.SqueezesHandScreams".Localized());
                    }
                    else
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.DroolsMingleThicken".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.OverabundantSquirtsShaft".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.OtherPrefix".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.DickSpits".Localized());
                    else
                        OutputText("Scenes.Sheilascene.RapeSheilasCooter.DicksSpit".Localized());

                    OutputText("Scenes.Sheilascene.RapeSheilasCooter.TrailsAcrossSkin".Localized());
                }

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.LaughWantedThis".Localized());

                if (SheilaCorruption() >= 90 && Player.Corruption >= 90)
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                    return ;
                }

                OutputText("Scenes.Sheilascene.RapeSheilasCooter.PullsAwayButtonsVest".Localized());

                SheilaPreg();

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                SheilaCorruptionUpdate();
            }

            CombatScene.CleanupAfterCombat();
        }

        public void ForcedSheilaOral(bool dick = true)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -4);

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.GrabShirtRaiseKnees".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.ShrieksLetGo".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.CalmHerCaressStomach".Localized());

            if (SheilaCorruption() >= 50)
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SoftPillowyWord".Localized());
            else
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SmallPerkyWord".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.BreastWord".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.ShuddersAreolaRub".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.MisinterpretAnswer".Localized());

            if (SheilaCorruption() < 20)
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.LittleRoundTraces".Localized());
            else
                if (SheilaCorruption() < 50)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.HealthyMoundsFill".Localized());
                else
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.SwollenGlobesBulge".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.PullShirtExpose".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.UnfastenRevealCrotch".Localized());

            if (dick)
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.FlopsFreeHalfErect".Localized(
                    Player.SMultiCockDesc()
                ));
            else
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.DroolsLabiaParting".Localized());

            OutputText("Scenes.Sheilascene.ForcedSheilaOral.WatchesFantasyNipples".Localized());

            if (dick)
            {
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SeizeHairDeposit".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.MouthOpensShovePrick".Localized());

                if (Player.Cocks[Player.ShortestCockIndex()].Thickness <= 6)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PartsLipsOneSided".Localized());
                else
                {
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.JabsTooThick".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.DragFaceAlongTip".Localized());
                }

                ShowImage("sheila-blowjob");

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.ReticentLipsGingerly".Localized());

                if (Player.Cocks[Player.ShortestCockIndex()].Length <= 8)
                {
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.WarmMouthWelcome".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.EncourageUseTongue".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.YourSackPrefix".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.BaseOfShaft".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.OtherHandBreasts".Localized());

                    if (!Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.AFootPrefix".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.TipOfTail".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.KneesToCrotchRubs".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.AskEarsTouched".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.WithdrawsDickHalfSmile".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.InterruptedFuckSake".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.BallsTightenUp".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PushesHandsHold".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.HalfAssedNoGood".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.TerribleAtThis".Localized());

                    if (Player.Cocks[Player.ShortestCockIndex()].Thickness <= 6)
                    {
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.GrabEarsDownThroat".Localized());

                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.WorkOnOralSkills".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Sheilascene.ForcedSheilaOral.BallsBeginRise".Localized());

                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.PanicsRenupsStruggle".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.StrokeImpressiveDollop".Localized());

                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.TauntGoodLook".Localized());

                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.NextDropCoversFace".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Sheilascene.ForcedSheilaOral.BallsDrawCloser".Localized());

                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.SqueezeJawForceOpen".Localized());
                    }
                }

                if (Player.Corruption >= 70 && Player.StatusEffectValue(StatusEffectIds.Infested, 0) == 5)
                {
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.WickedIdeaFlits".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.PackingBallsLust".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.DeepSensationNerves".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.IndulgeMoreRations".Localized());

                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.VolumeWormsStrain".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.FinishPullOutWorms".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.SackToken".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.BodyWord".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.FeelsLighterNoticeable".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PaleHorrifiedWorms".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.SwallowBitchEjaculate".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.LappingAtShaft".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.SentencePeriod".Localized());

                    if (Player.CumQ() >= 3000)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.BodyPushesCum".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PullMouthDragSlimy".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PullGarmentOverBreasts".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Sheilascene.ForcedSheilaOral.WetSeedSeeThrough".Localized());

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.NiceFabricPartingShot".Localized());
                }

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 1f));
            }
            else
            {
                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SeizeHairThrowGround".Localized());

                if (SheilaCorruption() >= 30)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.BreastsJiggleUrge".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.StraddleLowerOrderLick".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.FondleTitsClause".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SentencePeriodThird".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.CompliesTongueTriangle".Localized());

                if ((Player.HasVagina() && Player.VaginalCapacity() >= 100) || (!Player.HasVagina() && Player.AnalCapacity() >= 100))
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.SameSpotNoAdjust".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.GripeSitOnHead".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HellhoundMasterProgress) > 0)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.NiceBoyDogs".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SayEndLabel".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.FaceHeatsShame".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.PinchNippleGrindFace".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) >= 40)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.BetterLoversSeen".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.PeriodSpace".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) > 0 || Player.HasPerk(PerkIds.MinotaurCumAddict))
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.MinotaurTurnOver".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.HomePersonalTraining".Localized());
                // 原版 SheilaScene.hx:2957：Izma 是跟随者且有屌时追加一句
                if (SceneInstance<IzmaScene>.Get().IzmaFollower()
                    && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)

                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.FifteenInchesBalls".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.PeriodSayEnd".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.SqueezesHesitantTongue".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting) || CoC.Instance.Flags.Get(GameFlagIds.LumiMet) > 0)
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.ImaginesGoblinExperiments".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.HoleClenchesDrools".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ForcedSheilaOral.ViciousSqueezeNoCompel".Localized());

                OutputText("Scenes.Sheilascene.ForcedSheilaOral.WorthlessSpatLeaves".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 1f));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void SheilaGetsRapedWithADildont()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -4);

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.CruelIdeaDildo".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.PanickedUnfastenBelt".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.ColdGrabTail".Localized());

            ShowImage("sheila-dildofuck");

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.YelpsLustPotion".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.PerfectTimePullOut".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.HomePretendPutAway".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.WhinesGoCrazy".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.LiftTailAskBadly".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.ComeOnShoveIn".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.SmileUnseenWaggles".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.ScreamsBrutalMarathon".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.ChangeMindPistoning".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.GonnaCumGroans".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.AnalAloneWhisper".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.HumsIntoScales".Localized());

            if (Player.Corruption >= 90 || Player.Corruption > SheilaCorruption())
                OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.FaceHeatsSloppyKiss".Localized());
            else
                if (Player.Corruption < SheilaCorruption())
                    OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.LipsHeatBreastsShrink".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.WetIndecentKiss".Localized());

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.WellDoneButtSlut".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.RealCockNextTime".Localized());

            if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                return ;
            }

            OutputText("Scenes.Sheilascene.SheilaGetsRapedWithADildont.WalksAwayHoldingDildo".Localized());

            if (Player.Corruption >= 90 || SheilaCorruption() < Player.Corruption)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));

                SheilaCorruption(10);
            }
            else
                if (Player.Corruption < SheilaCorruption())
                {
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                    SheilaCorruption(-10);
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 3)));

            CombatScene.CleanupAfterCombat();
        }

        public void GetBeatUpBySheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.GetBeatUpBySheila.SlumpBreathingHeavily".Localized());

            if (CurrentMonster.LustPercent < 75)
            {
                OutputText("Scenes.Sheilascene.GetBeatUpBySheila.GrabsCatchRuns".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Sheilascene.GetBeatUpBySheila.ColorfulSpots".Localized());
                else
                    OutputText("Scenes.Sheilascene.GetBeatUpBySheila.FantasyNakedHat".Localized());

                OutputText("Scenes.Sheilascene.GetBeatUpBySheila.SentencePeriod".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Sheilascene.GetBeatUpBySheila.DesireNerveWar".Localized());

                OutputText("Scenes.Sheilascene.GetBeatUpBySheila.NotADemonQuestion".Localized());

                OutputText("Scenes.Sheilascene.GetBeatUpBySheila.ShakeHeadFogJumps".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) > -4)
                {
                    OutputText("Scenes.Sheilascene.GetBeatUpBySheila.DontHateMeWhisper".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, -2);
                }

                AddButton(0, "Common.Button.Next".Localized(), SheilaLossRapeRouter);
            }
        }

        public void SheilaLossRapeRouter()
        {

            if (!Player.HasCock() || Random.Shared.Next(3) == 0)
                SheilaForcedOralGeneric();
            else
            {
                SheilaCowgirlRapesYou();

                return ;
            }
        }

        public void SheilaCowgirlRapesYou()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PushesYouOntoBack".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HoovesStickAwkward".Localized());

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.BlushStripsVest".Localized(
                SheilaCup()
            ));

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.MoveHandsCupBreast".Localized());

            if (SheilaCorruption() < 30)
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PetitWord".Localized());
            else
                if (SheilaCorruption() > 70)
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.MassiveWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.SqueezeRubsNipples".Localized());

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.EagerLooksAway".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PullsLegsApart".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.StripsArmorOff".Localized());

            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.RevealCocksClause".Localized());

            if (Player.CockThatFits(32) < 0)
            {
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.BobsFreeFrustration".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.DammitMoreDickBrain".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ProblemQuestion".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.BitesLipCantFit".Localized());

                ShowImage("sheila-vag");

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.NotImaginativeEnough".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.AsFlatAsCan".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.SitsOnStomach".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LongLegsBetween".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ScootsPussyAgainstShaft".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ThighsAroundBeginsShifting".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PantHipsAidThrust".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ForelegsFoldTorso".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HandsSearchBreasts".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.GaspsPictureIntensify".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.SuckYourFingers".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.OhNameMoan".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.GruntTooInvolved".Localized());

                if (Player.Sensitivity >= 50)
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.OrderComeSoon".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.NotEvenNearFinishing".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ShiversLeansBackHair".Localized());

                if (Player.Corruption >= 90 || SheilaCorruption() < Player.Corruption)
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PrickHeatsCoolingRest".Localized());
                else
                    if (Player.Corruption < SheilaCorruption())
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.OrgasmWetsCoats".Localized());
                    else
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HipsLockUp".Localized());

                if (Player.Sensitivity >= 50)
                {
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.WarmWetThighsClimax".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.GigglesStopDumbfounded".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.RainPetersPullsApart".Localized());

                    if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
                    {
                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                        return ;
                    }

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LaughsCleansGrass".Localized());

                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

                    CombatScene.CleanupAfterCombat();
                }
                else
                {
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ThrustVigorouslyEdge".Localized());

                    if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
                    {
                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                        return ;
                    }

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.GonnaMakeHorny".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LooksOverShoulderStory".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PartsRedressesLooks".Localized());

                    if (Player.Health < 1)
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.FatigueDesireSleep".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Lust, (float)(50 + Player.Libido / 10)));

                    CombatScene.CleanupAfterCombat();
                }
            }
            else
            {
                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LeansEyesGlimmer".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LookSomewhereElse".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.WhereShallLook".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.EarsPrickNoAnswer".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.SeparatingApartClause".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.StrokesDropPrecum".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.YelpsStaringShock".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.NotLoversStammer".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.SwimExcuseMove".Localized());

                OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.TrailsOffShakesFree".Localized());

                if (Player.Libido < 25)
                {
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.StareGuiltyFace".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.JustMyDick".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.AttemptFrownSlumps".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 70f));

                    CombatScene.CleanupAfterCombat();

                    return ;
                }
                else
                {
                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PantiesStickReveal".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.WhispersBastard".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.LookUpCoySmile".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.FrownStraddlesCrotch".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PushingLegsApart".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.FrownBecomesGrin".Localized(
                        NumberUtils.Num2Text(Player.Cocks[Player.CockThatFits(32)].Length)
                    ));

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.TeaseNeckRed".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HandsToWaistSlams".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.RollsHeadWetterFaster".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.StimulationTooMuch".Localized());

                    if (Player.Corruption >= 90 || Player.Corruption > SheilaCorruption())
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.CumSlickWallsOrgasm".Localized());
                    else
                        if (Player.Corruption < SheilaCorruption())
                            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.GushesPushesOutSemen".Localized());
                        else
                            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HoldsMoansWondering".Localized());

                    if (Player.CumQ() <= 250)
                    {
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.MovesHipsLoftyAscent".Localized());

                        if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.YourTailWord".Localized());
                        else
                            OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.TheGrassWord".Localized());

                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.EyesGoWide".Localized());
                    }
                    else
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.OnlyMatterOfTime".Localized());

                    if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
                    {
                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                        return ;
                    }

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.HowMuchCum".Localized());

                    if (Player.CumQ() <= 250)
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.AllOfIt".Localized());
                    else
                        OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.JustALittle".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.CantBelieveNotice".Localized());

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.ClimbsOffWincing".Localized((noFur ? "legs" : "thighs")));

                    OutputText("Scenes.Sheilascene.SheilaCowgirlRapesYou.PanicsFussesDresses".Localized());

                    SheilaPreg();

                    Player.Orgasm("Dick");

                    SheilaPreg();

                    Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                    CombatScene.CleanupAfterCombat();
                }
            }

            SheilaCorruptionUpdate();
        }

        public void SheilaForcedOralGeneric()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.KissTongueHandArmor".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.HandAroundCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.FingersBetweenLabia".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.RubsBareCrotch".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ReverieBreaksShame".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.WordsCutOffClimbs".Localized());

            if (Player.Strength < 61)
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.WeightBearsDown".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.RubsSlitOnFace".Localized());

            ShowImage("sheila-lickhergina");

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.LickWhisperThreat".Localized());

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.StingersReleasePalm".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ObedientlyTongueOut".Localized());

            if (Player.Corruption > 60)
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.WhimsyOverPain".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ShallowPenetrateVagina".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.JerksSighsRelaxation".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
            {
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.SlyLookTongueUnrolls".Localized());

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.PetulantStareWorking".Localized(
                    SheilaCup()
                ));

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.CantCompleteThought".Localized());

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.AlmostHourFatigue".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.LeverageBasteVulva".Localized());

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.EasyWayConvulse".Localized());

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.EagerSlideLastTime".Localized());
            }

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.PussyClenchesFinishes".Localized());

            if (Player.Corruption >= 90 || Player.Corruption > SheilaCorruption())
            {
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.StranglesTongueMuscles".Localized());

                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.TiredGoAlongRipples".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.FlopsAroundInside".Localized());

                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.GroansVisionFades".Localized());
            }
            else
                if (Player.Corruption < SheilaCorruption())
                {
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.MouthFillsLiquidLove".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.FaceFuckBlowLoad".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.SmearGashGirlJizz".Localized());
                        else
                            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ForceTongueAsshole".Localized());
                }
                else
                    OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.PetsHeadLoving".Localized());

            if (Player.Corruption >= 90 && SheilaCorruption() >= 90)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaGoesDemon);

                return ;
            }

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.TwitchesLastPullApart".Localized());

            if (Player.Strength < 61)
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ClimbingOffKneeling".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.LetsDownFeetGround".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.ContentedThenGuilty".Localized());

            OutputText("Scenes.Sheilascene.SheilaForcedOralGeneric.BacksAwayBottoms".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                Player.ChangeFatigue(15);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 3 + 30)));

            if (Player.Lust >= Player.MaxLust)
                Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            SheilaCorruptionUpdate();

            CombatScene.CleanupAfterCombat();
        }

        public void NormalSheilaPregNotifNumberOne()
        {
            ClearOutput();

            Player.ClearPregnancy();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.SeesFarUnderTree".Localized());

            ShowImage("sheila-talk");

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.SlightlyNervousOpening".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.ObligeSitBeside".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.HandSteadiesHer".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.HadYourBaby".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.ExpressionHurriedExplanation".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOne.QuickGestationBlush".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.YepIssue".Localized(), NormalSheilaPregNotifNumberOneYepIssue);

            AddButton(1, "Common.Button.Nah".Localized(), NormalSheilaPregNotifNumberOneCoolDeal);
        }

        public void NormalSheilaPregNotifNumberOneYepIssue()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneYepIssue.NonplussedSorryFeel".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneYepIssue.StandsWalksAway".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDisabled, 2);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaJoeys, CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifNumberOneCoolDeal()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.ExhalesEmbraceBuries".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.WetSpotTears".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) < 0)
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.SniffsWarmApprobation".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.EarnestEyesHandHead".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.SnifflesThenSpeaks".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.FingerIntoBelt".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifNumberOneCoolDeal.RandyCluckyChores".Localized(
                    Player.Mf("bloke", "sheila")
                ));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) < 4)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaXp, 4);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaJoeys, CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDED()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.ParcelWaitingPlains".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.ProffersSnackBreathes".Localized());

            if (Random.Shared.Next(10) < 7)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.HealthyHellraiserGrip".Localized());
            else
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.WeakNearlyLostIt".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.FatigueLinedEyes".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.SlipsHandIntoYours".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.GiveItSqueeze".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDED.DragsFeetQuota".Localized());

            Player.ClearPregnancy();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaJoeys, CoC.Instance.Flags.Get(GameFlagIds.SheilaJoeys) + 1);

            if (SheilaCorruption() > 80)
                CoC.Instance.Flags.Set(GameFlagIds.SheilaCorruption, 80);

            ClearButtons();

            if ((Player.Speed > 80 && Player.HasPerk(PerkIds.Runner)) || Player.IsTaur() || Player.HasTailInsteadOfLegs() || (Player.CanFly != null && Player.HasKeyItem("Bow")))
                AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOut);

            AddButton(1, "Common.Button.Walkwithher".Localized(), NormalSheilaPregNotifREPEATEDEDWalkWithHer);

            AddButton(2, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), NormalSheilaPregNotifREPEATEDEDLetHerGo);
        }

        public void NormalSheilaPregNotifREPEATEDEDLetHerGo()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.NothingCanDoHidden".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.ImploreEatRest".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.WeakSmileTakesIt".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.WalksStumbleTurns".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.ForgotMyHat".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDLetHerGo.PicksSeatsNods".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDEDWalkWithHer()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.PickHatRefoldLunch".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.HoldsHandNotWorn".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.WatchYourBack".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.KindOfYouStay".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.SlipsHandLead".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDWalkWithHer.SquizzCoverTraps".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOut()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.HatOnTuckLunch".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.UncertainThenHugs".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.InsistIntoEar".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.TwitchesPullsAway".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.DullAmberClimbOn".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.MuzzyStareTurnsRed".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.AsksSureLowers".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.TrotLeansTorpidly".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GruntsDroolLine".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.SlipsDownWipesMouth".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.SupplyFinishingSentence".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.FlinchesWarmSafe".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.WavesOffHugsAnimals".Localized());
            }
            else
                if (Player.HasTailInsteadOfLegs())
                {
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.DoNormallyThinkSomething".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.NodsTakesHandSlither".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GlideTowardAnimals".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.TouchRousesTrance".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.SecuresCatchMoves".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.CirclesDustRetort".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.QuirkBrowTheatrical".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.YetAnotherWoman".Localized());
                }
                else
                    if (Player.CanFly() && Player.HasKeyItem("Bow"))
                    {
                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.WanderFlushGame".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.ConfusedGlumNod".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.QuestionCutBow".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.PointsPheasantFlushed".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.CoyArrowsImpressive".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.FatiguedBrainFlirting".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.NodFlyShootSorry".Localized());

                        Player.ChangeFatigue(20);
                    }
                    else
                        if (Player.Speed >= 80 && Player.HasPerk(PerkIds.Runner))
                        {
                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.DoNormallyHelp".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.BlanklyTakesHandMeander".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.WhatLookingFor".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.MumblesSpoorKills".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.NoUseMoping".Localized());

                            if (Player.Libido < 50)
                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.PinchEarFlick".Localized());
                            else
                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.SlapAssGrabHandful".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.NotNowFrowns".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.QuitStaringGround".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GruntsSobersRustling".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GoThatWayStill".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.QueerFaceCompliesLopes".Localized());

                            if (!Player.IsGoo())
                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.RunClause".Localized());
                            else
                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GlideClause".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.LongLoopDriveCloser".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.PreyJinksCompensate".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.PrettyFastWarmly".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.RaceYouSometime".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.SidelongFinishingQuickly".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.ReadyToTrain".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.FlushHoursBanter".Localized());
                        }

            Player.ApplyStats(new DynStat(GameStats.Corruption, -2f));

            if (View.Time.Hours + 4 < 21)
                DoNext(() => ReturnToCamp(4));
            else
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.GodDammitBold".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.VoiceStartlesSlumped".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOut.WetEyesLostTime".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.StayWithHer".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE);

                if (!false && !false)
                    AddButton(1, "Common.Button.ToCamp".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP);

                AddButton(2, "Common.Button.Welp".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP);
            }
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.WelpSoftly".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.HandShoulderSteady".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.ItllBeAlright".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.SobsGoingSprung".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.NotFirstParent".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDWELP.SnifflesYoureRight".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.PullWomanToFeet".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SuggestFindingHideout".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.NodsThenRealizes".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.AffirmCozyHideout".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SmilesThenStiffens".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.ArrangeGrassNest".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.CoilWord".Localized());
            else
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SeatWord".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.ArmsAroundHerWaist".Localized());

            if (Player.IsGoo())
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.WipesSlimeOffShorts".Localized());

                if (Player.Libido >= 40)
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.LeerNearlyDerailsHer".Localized());
                else
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.YouWord".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.AssentWithBroadSmile".Localized(
                    SheilaCup()
                ));

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.RollEyesAtModesty".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.BlushDeepensUndressing".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 5 + Player.Sensitivity / 3)));
            }
            else
                if (Player.HasTailInsteadOfLegs())
                {
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.WrapTailAroundHer".Localized());

                    if (Player.Libido >= 40)
                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.TightenChestCoil".Localized());

                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SighsHoldItThere".Localized());

                    if (Player.Libido >= 40)
                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 4)));
                }
                else
                    if (Player.IsTaur() && Player.Tallness >= 60)
                    {
                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.FeetPokeOutCanopy".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.AskAboutHorses".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.LovedWatchingPonies".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.RubsYourLowerHalf".Localized());

                        if (Player.Sensitivity >= 40)
                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.DistractedByHerBody".Localized());

                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SentencePeriod".Localized());

                        if (Player.Sensitivity >= 40)
                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 4)));
                    }
                    else
                        if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Harpy && Player.Body.Get<TailPart>().Id == BodyPartTypes.Harpy)
                        {
                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SheWrigglesTooMuch".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.ComplainAboutFeathers".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.CannotStopRustling".Localized());

                            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.RestsThenRisesAgain".Localized());
                        }
                        else
                            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Bee)
                            {
                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.HeadPopsBackUp".Localized());

                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.ChitinTooHardCold".Localized());

                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.BlameGodsDesign".Localized());

                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.BadMoodConfession".Localized());

                                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.WonderIfIntentional".Localized());
                            }
                            else
                                if (Player.IsDrider())
                                {
                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.AskIfCold".Localized());

                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.UnfoldLegsClickClack".Localized());

                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.MumblesWowSnuggling".Localized());

                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.EyebrowAtAssumption".Localized());

                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.WhispersKeepingThis".Localized());

                                    Player.Body.Get<TailPart>().Venom -= 10;

                                    if (Player.Body.Get<TailPart>().Venom < 0)
                                        Player.Body.Get<TailPart>().Venom = 1;
                                }
                                else
                                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.ApologizesAndThanks".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.AdjustToKeepContact".Localized());

            if (!Player.IsGoo())
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.BloodWord".Localized());
            else
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.GooWord".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SheilaSleepsAtOnce".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.BegFifteenMoreMinutes".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.SleepingFifteenMinutes".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.MentionUrgentImp".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.WakesInPanic".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.HerTurnToWatch".Localized());
            }

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE.GrudgingReposition".Localized());

            View.Time.Days += 1;

            View.Time.Hours = 2;

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2);
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2()
        {
            ClearOutput();

            View.Time.Hours = 5;

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.FitfulSleepThenWaking".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.AlreadyDressedOutside".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.SentencePeriod".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.AlmostDawnUntying".Localized());

            if (Player.Lust < 30)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.HelpHerLoadUp".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.RegretHovelDismantled".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.LaughingProtest".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.AskHerToStay".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDERE2.WhyCalledMakingLove".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + 5)));

                ClearButtons();

                SheilaXP4Sex(false);

                AddButton(9, "Common.Button.Leaveherbe".Localized(), NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDEREBUTLEAVEHERBE);
            }

            if (!(Player.Lust >= 30))
                DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDEREBUTLEAVEHERBE()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutANDSTAYDEREBUTLEAVEHERBE.RelentLetHerGo".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.LiftBundleOfGame".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.TakeHerHandHome".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.StumblingAfterYou".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.SayHome".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.BackAtCampsite".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.CallsCampExposed".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.DefenseCanopy))
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.PointAtThorns".Localized());
            else
                if (CampGuards.IsGuard("Jojo"))
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.IntroduceNightWatchman".Localized());
                else
                    if (CampGuards.IsGuard("KidA"))
                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.PointAtBarrelTenant".Localized());
                    else
                        OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.ArgueTradingShifts".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.LightFireSpitBirds".Localized());

            if (Player.Intelligence >= 60)
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.ReassureAboutBaby".Localized());
            else
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.ArmAroundAskOkay".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.NodsPullsFoodOut".Localized());

            if (CampGuards.IsGuard("Jojo"))
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.JojoDeclinesBird".Localized());
            else
                if (CampGuards.IsGuard("KidA"))
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.OfferBirdToTenant".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.SheilaClearsThroat".Localized());

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.AnnounceLeaveBeforeDawn".Localized());

            if (!CampScene.CampGuarded())
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.MovesToBedroll".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.TailsYouToBed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "")
                    OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.DistasteAtSleeper".Localized());

                OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.LiesDownFirstBeside".Localized());
            }

            OutputText("Scenes.Sheilascene.NormalSheilaPregNotifREPEATEDEDHelpABitchOutTOCAMP.GoneByMorning".Localized());

            CampScene.SleepRecovery(false);

            View.Time.Hours = 7;

            View.Time.Days += 1;

            DoNext(PlayerMenu);
        }

        public void SheilaGoesDemon()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.PullsAwayShivering".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.PushedDownAgain".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.ThighsAroundYourFace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaXp) < -2)
                OutputText("Scenes.Sheilascene.SheilaGoesDemon.EagerToFuckTaunt".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaGoesDemon.BestMateTaunt".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.BegForHelp".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.WearilyExtendTongue".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.FeelHerHeatRising".Localized((noFur ? "legs" : "thighs")));

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.GrabsHerOwnEars".Localized((noFur ? "legs" : "thighs")));

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.LegsLockAroundHead".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.StubbyTailThrashing".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.UrgesDeeperTongue".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Sheilascene.SheilaGoesDemon.TongueReachesDeep".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaGoesDemon.OnlyScrapeEntrance".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.ThighsClenchClimax".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.WatchesCrystalHarden".Localized());

            ShowImage("sheila-corrupted");

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.DismissiveReassurance".Localized());

            OutputText("Scenes.Sheilascene.SheilaGoesDemon.CallsAboutNextTime".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 3f));

            Player.CreateKeyItem("Sheila's Lethicite", 0, 0, 0, 0);

            SheilaCorruption(100 - SheilaCorruption());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaCite, 1);

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDemon, 1);

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void DemonSheilaEncounter()
        {
            ClearOutput();

            ShowImage("sheila-corrupted");

            OutputText("Scenes.Sheilascene.DemonSheilaEncounter.ShadowFromTree".Localized());

            OutputText("Scenes.Sheilascene.DemonSheilaEncounter.SashaysWithEarring".Localized(
                SheilaCup()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), DemonSheilaTalk);

            if (Player.Lust >= 33)
                AddButton(1, "Common.Button.Letherfuck".Localized(), SheilaLetHerFuckYou);

            AddButton(2, "Common.Button.Resist".Localized(), DemonSheilaResist);

            AddButton(4, "Common.Button.Run".Localized(), () => ReturnToCamp(1));
        }

        public void DemonSheilaResist()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.DemonSheilaResist.BackAwayRaiseWeapon".Localized());

            if (SheilaCorruption() < 30)
                OutputText("Scenes.Sheilascene.DemonSheilaResist.SpadeSmearsFluid".Localized());
            else
                OutputText("Scenes.Sheilascene.DemonSheilaResist.SeductiveBreastKneading".Localized());

            OutputText("Scenes.Sheilascene.DemonSheilaResist.PostureSoftens".Localized());

            if (SheilaCorruption() < 30)
                OutputText("Scenes.Sheilascene.DemonSheilaResist.DribblesDarkOils".Localized());

            OutputText("Scenes.Sheilascene.DemonSheilaResist.TensesAtGiggles".Localized());

            var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DemonSheilaTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.DemonSheilaTalk.AskToAnswerQuestions".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.DemonSheilaTalk.ArmLinkedIntoArmor".Localized());
            else
                OutputText("Scenes.Sheilascene.DemonSheilaTalk.BetweenYourLegs".Localized());

            OutputText("Scenes.Sheilascene.DemonSheilaTalk.ReachesToTouch".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.DemonSheilaTalk.CockToken".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sheilascene.DemonSheilaTalk.VaginaToken".Localized());
                else
                    OutputText("Scenes.Sheilascene.DemonSheilaTalk.SmoothSkin".Localized());

            if (Player.Libido < 20)
                OutputText("Scenes.Sheilascene.DemonSheilaTalk.ExpressionPetrifies".Localized());

            OutputText("Scenes.Sheilascene.DemonSheilaTalk.AnythingForYouOffer".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(2 + Player.Sensitivity / 5)));

            ClearButtons();

            AddButton(0, "Common.Button.TheChange".Localized(), AskDemonSheilaAboutDahChange);

            AddButton(1, "Common.Button.HerColony".Localized(), AskDemonSheilaAboutHerColony);

            AddButton(2, "Common.Button.WhatNow".Localized(), TalkToDemonSheilaAboutWhatNow);

            AddButton(3, "Common.Button.FuckOff".Localized(), TellSheilaDemonToFuckOff);
        }

        public void TellSheilaDemonToFuckOff()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.TellSheilaDemonToFuckOff.EyesGlossOverPlea".Localized());

            if (Random.Shared.Next(10) < 3)
            {
                OutputText("Scenes.Sheilascene.TellSheilaDemonToFuckOff.DemandsSayItBack".Localized());

                var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

                return ;
            }
            else
            {
                OutputText("Scenes.Sheilascene.TellSheilaDemonToFuckOff.NoWorriesRecovery".Localized());

                DoNext(() => ReturnToCamp(1));
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
        }

        public void AskDemonSheilaAboutDahChange()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCite) >= 1)
            {
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.BlushesYouWereThere".Localized());

                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.RecallsFeelingMonumental".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.YourOwnMonument".Localized());

                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.SqueezeSharpenedFocus".Localized());

                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.CaptainWasRight".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SheilaCite) <= -1)
                {
                    OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.WistfulWish".Localized());

                    OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.ShyRecountingCaptain".Localized());
                }

            if (Player.Femininity < 50)
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutDahChange.LoweredVoiceConfession".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 12f));

            ClearButtons();

            AddButton(0, "Common.Button.AnotherQ".Localized(), DemonSheilaTalkAnotherQuestion);

            if (Player.Lust >= 33)
                AddButton(1, "Common.Button.Letherfuck".Localized(), SheilaLetHerFuckYou);

            AddButton(2, "Common.Button.Resist".Localized(), DemonSheilaResist);
        }

        public void AskDemonSheilaAboutHerColony()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.StealsKissAfterOffer".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.SharedGiftAtHome".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.HorrifiedChoice".Localized());
            else
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.InterestedDespiteYourself".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.AskAboutHisFate".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.TugsLethiciteEarring".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.SoulsHarvestedCount".Localized());

            if (Player.Corruption >= 75)
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.WistfulSighHarvest".Localized());
            else
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.WinceAtStolenSouls".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.OneEyedPeek".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.DefiantEyesMeetYours".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.TiesDownAndRides".Localized());
            else
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.BindsHandsAndSits".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.YouWord".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.BreakEyeContactBlushing".Localized());
            else
                OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.StareIntoShiningEyes".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.SentencePeriod".Localized());

            OutputText("Scenes.Sheilascene.AskDemonSheilaAboutHerColony.AsksIfLikedIdea".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 3 + 5)));

            ClearButtons();

            AddButton(0, "Common.Button.AnotherQ".Localized(), DemonSheilaTalkAnotherQuestion);

            if (Player.Lust >= 33)
                AddButton(1, "Common.Button.Letherfuck".Localized(), SheilaLetHerFuckYou);

            AddButton(2, "Common.Button.Resist".Localized(), DemonSheilaResist);
        }

        public void TalkToDemonSheilaAboutWhatNow()
        {
            ClearOutput();

            ShowImage("sheila-corrupted");

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.LooksOutOverPlains".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.ConfidentialLean".Localized());

            if (Player.Libido >= 30)
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.EyebrowsRise".Localized());
            else
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.MouthHardens".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.SaveForHoneymoon".Localized(
                Player.Mf("knight-champion", "great lady")
            ));

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.TakesYourChin".Localized());

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.RatherStainMyColors".Localized(
                    Player.Mf("lord", "dame"),

                    Player.Mf("squire", "lady-in-waiting")
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.MyPussy".Localized());
                else
                    OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.TheDickIGrow".Localized());

                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.AnythingItalicBreak".Localized());
            }
            else
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.PointersSoBig".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.SayEnd".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.WantsYouAndChildren".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.GiveThemToMe".Localized());
            else
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.HaveThemForMe".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.NothingCouldBeSimpler".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.FixatedOnChildren".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOfferAccepted) > 0)
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.TwoAreReminiscent".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.DemonsMakeImps".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.ImpsTrustworthyLikeDogs".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.SeenItLurking".Localized());

            OutputText("Scenes.Sheilascene.TalkToDemonSheilaAboutWhatNow.FlyYourColors".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 15)));

            ClearButtons();

            AddButton(0, "Common.Button.AnotherQ".Localized(), DemonSheilaTalkAnotherQuestion);

            if (Player.Lust >= 33)
                AddButton(1, "Common.Button.Letherfuck".Localized(), SheilaLetHerFuckYou);

            AddButton(2, "Common.Button.Resist".Localized(), DemonSheilaResist);
        }

        public void DemonSheilaTalkAnotherQuestion()
        {
            ClearOutput();

            if (Player.LustPercent <= 99)
            {
                OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.ReachesFartherDown".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.FingerAndThumbGrip".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.PartingYourLips".Localized());
                    else
                        OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.RubbingBareGroin".Localized());

                OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.AmuseYourselfInvite".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 10)));

                ClearButtons();

                AddButton(0, "Common.Button.TheChange".Localized(), AskDemonSheilaAboutDahChange);

                AddButton(1, "Common.Button.HerColony".Localized(), AskDemonSheilaAboutHerColony);

                AddButton(2, "Common.Button.WhatNow".Localized(), TalkToDemonSheilaAboutWhatNow);

                AddButton(3, "Common.Button.FuckOff".Localized(), TellSheilaDemonToFuckOff);
            }
            else
            {
                OutputText("Scenes.Sheilascene.DemonSheilaTalkAnotherQuestion.GrinsAsYouStammer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => LoseToSheila(true));
            }
        }

        public void SheilaLetHerFuckYou()
        {
            ClearOutput();

            ShowImage("sheila-corrupted-prepare");

            OutputText("Scenes.Sheilascene.SheilaLetHerFuckYou.SheReadsYourDaydreams".Localized());

            OutputText("Scenes.Sheilascene.SheilaLetHerFuckYou.OpensPostureForHer".Localized());

            if (SheilaCorruption() >= 40)
                OutputText("Scenes.Sheilascene.SheilaLetHerFuckYou.PillowyWord".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaLetHerFuckYou.PertWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaLetHerFuckYou.BreastsAgainstYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => LoseToSheila(true));
        }

        public void DemonImpChildren()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.DemonImpChildren.SwishGrassThenSheila".Localized());

            if (SheilaCorruption() < 40)
                OutputText("Scenes.Sheilascene.DemonImpChildren.PerkyNipplesOnArm".Localized());
            else
                OutputText("Scenes.Sheilascene.DemonImpChildren.PendulousBreastsWrap".Localized());

            OutputText("Scenes.Sheilascene.DemonImpChildren.SentencePeriod".Localized());

            OutputText("Scenes.Sheilascene.DemonImpChildren.AnnouncesYourBaby".Localized());

            OutputText("Scenes.Sheilascene.DemonImpChildren.SurpriseAsPermission".Localized(
                Player.Mf("daddy", "other mommy")
            ));

            if (!Player.HasCock())
                OutputText("Scenes.Sheilascene.DemonImpChildren.TrailsOffFrowning".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.DemonImpChildren.NotSameAsInside".Localized());

                if (SheilaCorruption() <= 80)
                    OutputText("Scenes.Sheilascene.DemonImpChildren.FingersSpreadOpen".Localized());
                else
                    OutputText("Scenes.Sheilascene.DemonImpChildren.SplitTailCradlesTits".Localized());

                OutputText("Scenes.Sheilascene.DemonImpChildren.SpadeAtTheHole".Localized());
            }

            Player.ClearPregnancy();

            CoC.Instance.Flags.Set(GameFlagIds.SheilaImps, CoC.Instance.Flags.Get(GameFlagIds.SheilaImps) + 1);

            if (Player.HasCock())
                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            ClearButtons();

            AddButton(0, "Common.Button.OtherSex".Localized(), PregDemonSheilaOtherSex);

            if (Player.HasCock())
                AddButton(1, "Common.Button.KnockUp".Localized(), PregDemonSheilaKnockUp);

            AddButton(14, "Common.Button.Leave".Localized(), LeavePregDemonSheila);
        }

        public void LeavePregDemonSheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.LeavePregDemonSheila.ShakeHeadBackingOff".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sheilascene.LeavePregDemonSheila.RefusalIgnoredFinish".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void PregDemonSheilaOtherSex()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.EyebrowsKnitAtAlternative".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.InsistsOnHerSay".Localized());

                if (SheilaCorruption() >= 80)
                    OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.HeavesOntoFeet".Localized());
                else
                    OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.HopsToClawedFeet".Localized());

                OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.HandsAlongHerHips".Localized());

                var sheilaEnemy = EnemyRegistry.Create(new Identifier("coc", "sheila"));

            sheilaEnemy.OnDefeated = _ => WinAgainstDemoNSheilaForVaginas();

            sheilaEnemy.OnWon = _ => LoseToSheila();

            CombatScene.Start(sheilaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Sheilascene.PregDemonSheilaOtherSex.SnugglesCloseAnyway".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                    BeatUpDemonSheila(false);
                }
                else
                    DoNext(() => ReturnToCamp(1));
        }

        public void PregDemonSheilaKnockUp()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.PregDemonSheilaKnockUp.SmearsEntranceWithSlime".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(56))
                AddButton(0, "Common.Button.Next".Localized(), MissionaryForThePurposesOfCreatingImpsWithSheila);
            else
                AddButton(0, "Common.Button.Next".Localized(), BigDickAndThighs);
        }

        public void LoseToSheila(bool consensual = false)
        {
            ClearOutput();

            if (Player.Health < 1 && !consensual)
            {
                OutputText("Scenes.Sheilascene.LoseToSheila.OpponentEyesGlimmer".Localized());

                View.Time.Hours += 8;

                if (View.Time.Hours > 23)
                {
                    View.Time.Hours -= 24;

                    View.Time.Days += 1;
                }

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), LoseToDemonSheila);
            }
            else
            {
                var choices = new List<Action>();

                if (Player.HasCockThatFits(56))
                    choices.Add(() => LoseToNormalSheilaAndGetRidden());

                if (Player.HasCock() && Player.BiggestCockArea() > 56)
                    choices.Add(() => TailSpadeSoundingForFuckHugeDongsWithDemonSheila());

                if (Player.HasVagina())
                    choices.Add(() => ClitSwellingDemonSheilaClitSoundingAnal());

                if (Player.Gender == Gender.Genderless)
                    choices.Add(() => AintGotNoGenderAndKangarooRaped());

                choices[Random.Shared.Next(choices.Count)]();
            }
        }

        public void LoseToDemonSheila()
        {
            ClearOutput();

            ShowImage("sheila-corrupted-loss");

            OutputText("Scenes.Sheilascene.LoseToDemonSheila.WakesEightHoursLater".Localized());

            OutputText("Scenes.Sheilascene.LoseToDemonSheila.FogClearsInDisbelief".Localized());

            OutputText("Scenes.Sheilascene.LoseToDemonSheila.HemsAndHaws".Localized());

            OutputText("Scenes.Sheilascene.LoseToDemonSheila.NervesHerselfUp".Localized());

            OutputText("Scenes.Sheilascene.LoseToDemonSheila.StandsStillShaken".Localized());

            Player.Orgasm("Generic");

            Player.Health = Player.MaxHealth;

            Player.ChangeFatigue(-50);

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToNormalSheilaAndGetRidden()
        {
            ClearOutput();

            var x = Player.CockThatFits(56);

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.PushedOntoGround".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ForelegsBracedOnHaunches".Localized());
            else
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.LegsSpreadArmorStripped".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.CocksExposed".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ParagraphBreak".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ChoosesPreferredCock".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.HandPumpsPrecum".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.WatchesDickThrob".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ForelegsOnShoulders".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.BreastsPressedAgainstYou".Localized(
                SheilaCup()
            ));

            if (SheilaCorruption() < 100)
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.NipplesDraggedAgainstYours".Localized());
            else
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.BreastsPartAroundChest".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.WhispersAboutDong".Localized(
                Player.Mf("handsome bloke", "pretty sheila")
            ));

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.GrinsAndSinksDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.RemainderBetweenThighs".Localized());

            OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FeelsEveryInch".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.MocksRollingEyes".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.SpadeHoversAboveSack".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FacePressedReassurance".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.CoosWhileJerkingTail".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.QueasinessSubsides".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.LaughsAboutSpermies".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.GroansInAgreement".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.PussySpasmsCompulsion".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.LapContinuation".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ScreamsForBabies".Localized());

                ShowImage("sheila-loss-male");

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.CannotHelpIt".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ForelegsWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.HandsWord".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.GlobOfCumDisplaced".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.TenMinutesTwoOrgasms".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ThirtyMinutesFourOrgasms".Localized());
                    else
                        OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FiftyMinutesEightOrgasms".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.BallsFinallyEmptied".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.DreamilyHoldsStomach".Localized());

                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.ShielaIncubation, 100, 1);

                Player.BallSize += 1;

                if (Player.BallSize < 10)
                    Player.BallSize += 1;

                Player.CumMultiplier += 1;

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                SheilaCorruption(-10);

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.PausesAtBase".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FlatlyAsksMeaning".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ClaimsNoBalls".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.NativeWouldKnow".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ForcesHerDownward".Localized());
                else
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.GrabsHerHips".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.HelpsHerImpaleHerself".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ShuddersAtSheathBump".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.InterruptsIrritably".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.TwinklesMischievously".Localized());

                if (Player.Corruption < 70)
                {
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.StiffensInEmbarrassment".Localized());

                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FucksHarderInReply".Localized());

                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.HissesThroughTeeth".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.FrownsAtRevelation".Localized());

                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.KneadsBreastsWhileBerated".Localized());
                }

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.RoughFuckContinues".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.TopPositionRemark".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.GravityForcesHerDown".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.LapContinuationAgain".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.CapacityThenPullsOff".Localized());

                if (Player.CumQ() >= 6000)
                    OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.CoveredInBubbles".Localized());

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.SentencePeriod".Localized());

                ShowImage("sheila-loss-male");

                OutputText("Scenes.Sheilascene.LoseToNormalSheilaAndGetRidden.ScootsBackLeavingTrail".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                SheilaPreg();

                SheilaCorruption(-10);

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
        }

        public void TailSpadeSoundingForFuckHugeDongsWithDemonSheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.UndressesAndInspectsCrotch".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.PrickHardensToAttention".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.StepsForwardBending".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.WarnsHoldOnTight".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.TailGripsUrethra".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.SqueezesGooOut".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.PumpsFluidConstantly".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.MoansWhileComing".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.LaughsOutOfBreath".Localized());

            ShowImage("sheila-corrupted-vag");

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.EyesGlossWithDesire".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.UnguidedLeadIn".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.OtherTwitchesAndSprays".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.OthersTwitchAndSpray".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.CoatingHerBack".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.AnythingTheyPointAt".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.AnythingItPointsAt".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.HipsPushUnbalancingHer".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.SighsAndStandsUp".Localized());

            OutputText("Scenes.Sheilascene.TailSpadeSoundingForFuckHugeDongsWithDemonSheila.PassesOutWondering".Localized());

            Player.CumMultiplier += 1;

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            SheilaCorruption(-10);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();

            DoNext(() => ReturnToCamp(1));
        }

        public void ClitSwellingDemonSheilaClitSoundingAnal()
        {
            ClearOutput();

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LowerBackHand".Localized());
            else
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.HandAtopWithers".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.UrgesToLieDown".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.BefoggedButReserved".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.StraddlesYourGroin".Localized());
            else
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.CirclesToYourRump".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailSnakesTowardPussy".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.CaressingWord".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.YourBalls".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.UndersideOfCock".Localized());
            }

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.SpreadsLipsAndLingers".Localized());

            if (Player.GetClitLength() <= 6)
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LewdLittleQualifier".Localized());
            else
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GrotesqueWord".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.PerfectSizeForFun".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.EyesOpenInAlarm".Localized());

            if (Player.GetClitLength() > 6)
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.SeveralInchesDeep".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.PleasureTurnsToPain".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailSqueezesTighter".Localized());

            if (Player.GetClitLength() <= 6)
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.RealizesClitMutated".Localized());
            else
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ClitLeftInPlace".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GaspsOnElbows".Localized());

            OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.BurningTapersOff".Localized());

            if (Player.GetClitLength() <= 6)
            {
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.SitsUpOnKnees".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ClosesJawThenKisses".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.EyesGlitterWithMirth".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.QuestionsTheLook".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GrassTraversal".Localized());
                else
                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.PlaceholderInterpolation".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.FlatChestStroke".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.KissAfterKiss".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ChestToken".Localized());
                else
                    OutputText(Player.ButtDescript());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ShallowPumpsClit".Localized());

                ShowImage("sheila-corrupted-anal");

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TurnsAroundAgain".Localized());

                if (Player.Corruption < 50)
                {
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.HotLickAtTip".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.StalkThrashesWeakly".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.DemonLegsFragment".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GrassWord".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ClitSwellsInside".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.RestrainsStrainingTail".Localized());
                }
                else
                {
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.HotFluidSpatter".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.DrizzlingWord".Localized());
                    else
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.CopiouslySoaking".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GirlJizzIntoPussy".Localized());
                }

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LeansPantingOnYou".Localized());
            }
            else
            {
                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailFreedFromClit".Localized());

                ShowImage("sheila-corrupted-anal");

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.StaresAsSheTurns".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.RollsOntoBack".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailsTwistAroundClit".Localized());

                if (Player.GetClitLength() <= 12)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.BuriedInHerBowels".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TakesAllSheCan".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.HalfSmileHalfWince".Localized());

                if (Player.GetClitLength() <= 12)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LipsDragAcrossYours".Localized());
                else
                {
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailsSlipIntoYou".Localized());

                    if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TeasesIntactVirtue".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ShiftsAroundYourClit".Localized());
                }

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TalonedFeetPlaced".Localized());

                if (!Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LegsSpreadWideOpen".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LeveragePumpsClit".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.GetClitLength() > 12)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailDrippingWithJuice".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.SpadeSmearsGoo".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.CoatingToSatisfaction".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.GetClitLength() <= 12)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LabiaLapFluid".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailsHoldLabiaOpen".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.StopsMidStroke".Localized());

                if (Player.GetClitLength() <= 12)
                {
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.TailsSinkInside".Localized());

                    if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                        OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.VirginityLostNotice".Localized());

                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.SpadeDroolsFluids".Localized());
                }

                if (Player.Corruption < 50)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.PussyGushesFluids".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.DemonSkinParches".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ButtFeelsGood".Localized());

                if (Player.GetClitLength() > 12 && Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.ApologizesForHymen".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.LaughsAtHerState".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GrassStainedTits".Localized());
                else
                    OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GreenWetAsscheeks".Localized());

                OutputText("Scenes.Sheilascene.ClitSwellingDemonSheilaClitSoundingAnal.GripesThenShovesFace".Localized());
            }

            if (Player.GetClitLength() > 12 || Player.Corruption < 50)
            {
                Player.CuntChange(12, false, false, false);

                Player.SlimeFeed();
            }

            Player.Vaginas[0].ClitLength += 1;

            if (Player.HasPerk(PerkIds.BigClit))
                Player.Vaginas[0].ClitLength += .5f;

            if (Player.Corruption < 50)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                SheilaCorruption(-10);
            }
            else
            {
                SheilaCorruption(10);

                Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -2f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void AintGotNoGenderAndKangarooRaped()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.HandInFeaturelessCrotch".Localized());

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.ShowsConfusion".Localized());

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.IrritatedAboutKids".Localized());

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.RefusesBeingSheila".Localized());

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.IrisesFlareLavender".Localized());

            OutputText("Scenes.Sheilascene.AintGotNoGenderAndKangarooRaped.DriftsIntoDoze".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 4)));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void BeatUpDemonSheila(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Sheilascene.BeatUpDemonSheila.AdmitsDefeat".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Sheilascene.BeatUpDemonSheila.WoundsCloseGlowing".Localized());
                else
                    OutputText("Scenes.Sheilascene.BeatUpDemonSheila.VigorousFriggingOffer".Localized());
            }

            if (Player.Lust <= 33 && output)
            {
                OutputText("Scenes.Sheilascene.BeatUpDemonSheila.UnwillingToFuck".Localized());

                CombatScene.CleanupAfterCombat();

                return ;
            }

            if (Player.Gender == Gender.Genderless)
            {
                CombatScene.CleanupAfterCombat();

                return ;
            }

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), null);

            AddButton(1, "Common.Button.Analhatefuck".Localized(), null);

            AddButton(2, "Common.Button.Useherthighs".Localized(), null);

            AddButton(3, "Common.Button.ClitFuck".Localized(), null);

            if (Player.HasCockThatFits(56))
            {
                AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), MissionaryForThePurposesOfCreatingImpsWithSheila);

                AddButton(1, "Common.Button.Analhatefuck".Localized(), AnalHateFuck);
            }

            if (Player.HasCock() && Player.BiggestCockArea() > 56)
                AddButton(2, "Common.Button.Useherthighs".Localized(), BigDickAndThighs);

            if (Player.HasVagina())
                AddButton(3, "Common.Button.ClitFuck".Localized(), WinAgainstDemoNSheilaForVaginas);
        }

        public void MissionaryForThePurposesOfCreatingImpsWithSheila()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.UndressFully".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.MelodramaticShriek".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ZealFalters".Localized());
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ImpatienceAtPlayacting".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SheDropsPretense".Localized());

            ShowImage("sheila-corrupted-vag");

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.HairTossInvitation".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ShouldersWord".Localized());
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.FlanksWord".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.EnthusiasmReflected".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.GentlePenetration".Localized());

            if (SheilaCorruption() >= 40)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SoftBreastsJiggle".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ThrustForcesAdjustment".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ChestKissUrges".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ArmsWord".Localized());
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ForelegsWord".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.KneesWobbleFocus".Localized((Player.IsTaur() ? ", practically lifting herself off of the ground as she impales her cunt on your [cockFit 56] to prevent you from pulling out" : "")));

            if (silly)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.YouthfulNostalgiaOutburst".Localized());
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.NoSpillDemand".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ResistanceCrumbles".Localized());

            if (SheilaCorruption() < 40)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.TightWord".Localized());
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SoftWord".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.BodyPressedClose".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PostClimaxSlump".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.CockWord".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ItsWord".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.BrotherWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.BrethrenWord".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SwellSquirtPlural".Localized());
            }
            else
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SwellSquirtSingular".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SemenPouredDeep".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.GlazeOnCrotch".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SoulSuckingGaze".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.FilledOverflowDribble".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.GentleKiss".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.HerForelegsSuffix".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.TenderDevotion".Localized());

            if (Player.CockTotal() > 1 || Player.CumQ() >= 500)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ExcessSeedDrips".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.EyeContactAfter".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.YouWord".Localized());

            if (Player.CumQ() >= 500 || Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.AccusesSpilling".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.TooSoonComplaint".Localized());

            OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.JokeRevealedGrin".Localized());

            if (Random.Shared.Next(10) <= 6)
            {
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PowerHelpedFinish".Localized(
                    Player.Mf("productive as his father", "sexy as his mother")
                ));

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                SheilaPreg();

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.CravesSeedInside".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ImpsSpawnBehind".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ImpsGatherStroking".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PeriodSpace".Localized());

                if (Player.Libido < 40)
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ShockedExpression".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.FlusteredBlush".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.BreaksMindLink".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.HairPlayQuestion".Localized());

                ShowImage("sheila-corrupted-vag");

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.Overwhelmed".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.DeepKissLingering".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.HandOnCheek".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PartwayWithdrawal".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.GroanOfApproval".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.DemandsSharedPeak".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SpasmHeatWave".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SuckingPrecumWaves".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.LimpMilking".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.NearlyFitsFully".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.WeakenedSecondRound".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.BackWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.SidesWord".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PostClimaxCooling".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.PokeInSide".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.ReadsRevengePoke".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.CongratulatoryPeck".Localized());

                OutputText("Scenes.Sheilascene.MissionaryForThePurposesOfCreatingImpsWithSheila.TenderFarewellGaze".Localized());

                SheilaPreg();

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                Player.SlimeFeed();

                if (Player.Corruption < 50)
                {
                    Player.SlimeFeed();

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                    SheilaCorruption(-10);
                }
                else
                {
                    Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));

                    SheilaCorruption(10);
                }

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
        }

        public void AnalHateFuck()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.AnalHateFuck.ContemptAtDefiance".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Tearherup".Localized(), SheilaAnalHateFuckAGoGo);

            if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                AddButton(1, "Common.Button.CallJojo".Localized(), AnalHateFucksWithJojo);
        }

        public void SheilaAnalHateFuckAGoGo()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.ArousalSuppressed".Localized());

            ShowImage("sheila-corrupted-anal");

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.PresentingRear".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.SilentAgreement".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.HandGripsShaft".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.MountingStance".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.AssholeFirstThrust".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.RefusalWord".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.LeverageHerLegs".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.ForcesAngryWoman".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.TailsPushBack".Localized());

            if (SheilaCorruption() >= 90)
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.BreastsCushionWeight".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.PoundsEarthFists".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.BitternessSettles".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.StopsInConfusion".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.QuestionsHerConsent".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.PleadsForRoughness".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.ForcefulPunctuatedReply".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.PanickedConfession".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.ResumesTeasingPumps".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGo.BegsForWomb".Localized());

            if (Player.Corruption >= 80 && Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                ClearButtons();

                AddButton(0, "Common.Button.No".Localized(), SheilaAnalHateFuckAGoGoNO);

                AddButton(1, "Common.Button.Wormssuityou".Localized(), SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS);
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SheilaAnalHateFuckAGoGoNO);
            }
        }

        public void SheilaAnalHateFuckAGoGoNO()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.HatefulFinalThrust".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.DominanceRuling".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.FloodedOverflowLeak".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.CurseOnWithdrawal".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.TrickleWord".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.StreamWord".Localized());
                else
                    OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.WaterfallWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.BodyExpelsSeed".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoNO.CruelPoemTaunt".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, 2f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.NoncommittalAnswer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.BallsToken".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.InsidesWord".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.PullsOutDoubt".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.CuntRubbedPleading".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.ReachingToTouch".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.LegsWrapTight".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.YourWord".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.SackToken".Localized());
            else
                OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.CockFitToken".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.PentUpPets".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.ParasiteFilledShiver".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.SitsUpCurious".Localized());

            OutputText("Scenes.Sheilascene.SheilaAnalHateFuckAGoGoGETYOUSOMEWORMS.EagerFingerReach".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.Infested);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 5)));

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDisabled, 3);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void AnalHateFucksWithJojo()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.WatchesHerDefiance".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.SummonedServantArrives".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.OrdersSlaveObedience".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.MouseTakesPosition".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.InstructsLiftingHer".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.TailsReleaseMouse".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.MockSolemnRecital".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.RejectsHerPlea".Localized());

            ShowImage("sheila-corrupted-jojo");

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.ShoutsDisgraceViolence".Localized());

            if (SheilaCorruption() < 40)
                OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.MocksTinyBreasts".Localized());
            else
                OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.MocksGiantBreasts".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.NippleTwistFoot".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.HumiliationDeepensBlush".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.DemandsRealApology".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.DefiantBitterness".Localized());

            if (Player.Corruption >= 80)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => AnalHateFucksWithJojoNo(true));

                OutputText("Scenes.Sheilascene.AnalHateFucksWithJojo.RuinThemWarning".Localized());

                AddButton(1, "Common.Button.RuinThem".Localized(), () => JojoRuinsTheAnalHateFuck(true));
            }
            else
                AnalHateFucksWithJojoNo(false);
        }

        public void AnalHateFucksWithJojoNo(bool clear)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.ParagraphBreak".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.RefusesDemand".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.DismayAtRejection".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.AsksForOrders".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.SeedOnGroundOrder".Localized());

            OutputText("Scenes.Sheilascene.AnalHateFucksWithJojoNo.FallsIntoFormation".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 3)), new DynStat(GameStats.Corruption, 2f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void JojoRuinsTheAnalHateFuck(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.ParagraphBreak".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.SicklySmileConsider".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.EarsPerkAttention".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.OrdersPussySwitch".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.SurprisedCompliance".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.DryEntryWince".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.WhinesAtRoughness".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.CaressAndRemind".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.ShyGlanceReddening".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.GripsMouseSack".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.EyesWidenAtTaunt".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.SobbingTaintedBody".Localized());

            if (SheilaCorruption() >= 90)
                OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.TitsMashedThickening".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.MouseGrowingNails".Localized((noFur ? "" : "-furred")));

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.ShriekWombPressure".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.WombPowerFails".Localized());

            if (SheilaCorruption() >= 40)
                OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.BreastsShrinkCups".Localized());
            else
                OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.TLetter".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.TesticlesSwellHuge".Localized());

            ShowImage("sheila-corrupted-superjojo");

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.CollapseAfterEjection".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.MockWeddingPreside".Localized());

            OutputText("Scenes.Sheilascene.JojoRuinsTheAnalHateFuck.GrimExitWondering".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)), new DynStat(GameStats.Corruption, 10f));

            CoC.Instance.Flags.Set(GameFlagIds.SheilaDisabled, 4);

            CoC.Instance.Flags.Set(GameFlagIds.JojoDeadOrGone, 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void BigDickAndThighs()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.BigDickAndThighs.WatchesHerDiddle".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Sheilascene.BigDickAndThighs.EyebrowsClimbReveal".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.PricksStackedUp".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.SentencePeriod".Localized());
            }
            else
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ApproachesCocksBobbing".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.AwedAtSize".Localized());

            if (Player.Corruption < 40)
            {
                OutputText("Scenes.Sheilascene.BigDickAndThighs.LilliputianComparison".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.RollsOntoAllFours".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.AimsUnderCunt".Localized());

                if (Player.CockTotal() > 1)
                {
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.BothWord".Localized());
                    else
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.AllWord".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.InsistsAllInside".Localized());
                }

                OutputText("Scenes.Sheilascene.BigDickAndThighs.CoaxingTugGuidance".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.BorrowsSensation".Localized());

                if (!Player.IsTaur() && Player.Tallness >= 60)
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.CheekKissFuzz".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.DemonExperiments".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.EmbarrassedShudder".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.TeasesQuickShot".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.UnseenWord".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.SentencePeriodAgain".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.LeansOnCocks".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.GripsBothHands".Localized());
                else
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.OneInEachHand".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.AngledPrecumDrool".Localized());
            }
            else
                if (Player.Corruption >= 40)
                {
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.DismissiveAdvance".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.HeedlessVaginalThrust".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.GripesAtUselessness".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.FrustratedProtest".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.TailsBreakAlignment".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.ShudderAtSkill".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Sheilascene.BigDickAndThighs.SlidesBetweenLegs".Localized());
                        else
                            OutputText("Scenes.Sheilascene.BigDickAndThighs.PricksThroughThighs".Localized());
                    }
                    else
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.ParagraphBreak".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.ScootsBackChoking".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.EnjoysStrokingToo".Localized(
                        SheilaCup()
                    ));
                }

            OutputText("Scenes.Sheilascene.BigDickAndThighs.LostInOwnWorld".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetter".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.WeakThrustGuidance".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ReleasesPricksPlural".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ReleasesPrickSingular".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.MilksMoreDrops".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.LossWhenFocusShifts".Localized());

            if (SheilaCorruption() < 50)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ModestBreastsCleavage".Localized());
            else
                if (SheilaCorruption() < 150)
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.LargeBreastsSheath".Localized(
                        SheilaCup()
                    ));
                else
                {
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.EnormousBreastsCover".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetterAgain".Localized());

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.WholeBodyFuckhole".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Sheilascene.BigDickAndThighs.FrameSuitsPricks".Localized(
                            NumberUtils.Num2Text(Player.CockTotal())
                        ));

                    OutputText("Scenes.Sheilascene.BigDickAndThighs.SentencePeriodThird".Localized());
                }

            OutputText("Scenes.Sheilascene.BigDickAndThighs.ForeplayNotEnough".Localized());

            if (SheilaCorruption() >= 150)
            {
                OutputText("Scenes.Sheilascene.BigDickAndThighs.BreastsSecondHole".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetterThird".Localized());

                OutputText("Scenes.Sheilascene.BigDickAndThighs.PeriodSpace".Localized());
            }

            OutputText("Scenes.Sheilascene.BigDickAndThighs.MumbledAgreement".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.ObliviousIgnoring".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetterFourth".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.ThighViseSqueeze".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.TopmostWord".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.FinalThrustEjaculate".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ReawakensHerUrgency".Localized());
            else
                OutputText("Scenes.Sheilascene.BigDickAndThighs.CriesComingEverywhere".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.PullsSoakedShaft".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetterFifth".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.FreedFromThighs".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.SLetterSixth".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.WistfulFarewellLook".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ItWord".Localized());
            else
                OutputText("Scenes.Sheilascene.BigDickAndThighs.ThemWord".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.GoWord".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.AsksWhyNotGrow".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.GiggleMeetsGaze".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Sheilascene.BigDickAndThighs.GrowPussyForMe".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.WantsInsideAnytime".Localized());

            OutputText("Scenes.Sheilascene.BigDickAndThighs.DepartsShakingHead".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Corruption < 50)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

                SheilaCorruption(-10);
            }
            else
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));

                SheilaCorruption(10);
            }

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void WinAgainstDemoNSheilaForVaginas()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.DropsOntoLap".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.MaulsHerBreasts".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.GrindsDesperately".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.ThumbInMouthCorner".Localized());
            else
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.TurnsHeadBeside".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.TransmitsFantasy".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.CaughtOffGuardBlush".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.HerCockFirming".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.CrawlsAwayHands".Localized());

                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None && Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.RaisesTail".Localized());
            }
            else
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.BendsForPresenting".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.PresentsVagina".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.WarnsOfPain".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.SuddenPenetration".Localized());

            Player.CuntChange(10, true, true, false);

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.BurningFluidShock".Localized());

            if (SheilaCorruption() > 0)
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.TitsPressedAgainst".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.BarksCalmDown".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.NeckWord".Localized());
            else
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.WithersWord".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.TailShaftDrooling".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.TrickleIntensifies".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.PromisesRoughUse".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.FantasyOfOwnership".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.DebasedThoughtClimax".Localized());

            if (Player.Libido < 20)
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.PrideWavers".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.SubmissionAppeals".Localized());

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.PumpsMoreMuck".Localized());

                if (SheilaCorruption() > 0)
                    OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.ArchJamsDeep".Localized());

                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.HowlsFinalStream".Localized());

                if (Player.Fertility < 50)
                {
                    OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.FertilityTingle".Localized());

                    Player.Fertility += 1;
                }
            }
            else
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.CollapsesPanting".Localized());

            OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.WithdrawsWithSplat".Localized());

            if (Player.Wetness() < 5)
            {
                OutputText("Scenes.Sheilascene.WinAgainstDemoNSheilaForVaginas.NoticesWetness".Localized());

                Player.Vaginas[0].Wetness += 1;
            }

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.Corruption < 50)
            {
                SheilaCorruption(-10);

                Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));
            }
            else
            {
                SheilaCorruption(10);

                Player.ApplyStats(new DynStat(GameStats.Corruption, -10f));
            }

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void BadEndScarredBlade()
        {

            OutputText("Scenes.Sheilascene.BadEndScarredBlade.InjuredCollapseDread".Localized());

            if (silly)
                OutputText("\n\n[DATA EXPUNGED]");

            if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                OutputText("Scenes.Sheilascene.BadEndScarredBlade.MarbleScreamsNo".Localized());

            CombatController.InCombat = false;

            CoC.Instance.Flags.Set(GameFlagIds.ScarredBladeStatus, 1);

            GameOver();
        }

        public void BreakScarredBlade()
        {

            OutputText("Scenes.Sheilascene.BreakScarredBlade.BladeFallsInanimate".Localized());

            if (Player.Strength >= 85)
                OutputText("Scenes.Sheilascene.BreakScarredBlade.BendsUntilCracks".Localized());
            else
            {
                OutputText("Scenes.Sheilascene.BreakScarredBlade.BendAttemptFails".Localized());
                // 原版 SheilaScene.hx:5504 — Helia 是跟随者时由她出手
                if (SceneInstance<HelScene>.Get().FollowerHel())
                    OutputText("Scenes.Sheilascene.BreakScarredBlade.SummonsHelia".Localized());
                else
                    if (MarblePurification.KihaFollower())
                        OutputText("Scenes.Sheilascene.BreakScarredBlade.SummonsKiha".Localized());
                    else
                        if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                            OutputText("Scenes.Sheilascene.BreakScarredBlade.SummonsMarble".Localized());

                OutputText("Scenes.Sheilascene.BreakScarredBlade.ThrowsSwordAtTree".Localized());
            }

            OutputText("Scenes.Sheilascene.BreakScarredBlade.InspectsBrokenSaber".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void RebellingScarredBlade(bool wieldAttempt = false)
        {

            if (!wieldAttempt)
                OutputText("Scenes.Sheilascene.RebellingScarredBlade.SwordJerksFree".Localized());
            else
                OutputText("Scenes.Sheilascene.RebellingScarredBlade.JerksWhenWielded".Localized());

            if (!wieldAttempt)
            {
                var dmg = 20;

                dmg -= (int)(Player.ArmorDefense);

                if (dmg < 1)
                    dmg = 1;

                Player.HealthChange(-dmg);

                Player.SetUnarmed();

                CoC.Instance.Flags.Set(GameFlagIds.ScarredBladeStatus, 1);
            }

            DoNext(PlayerMenu);

            AddButton(1, "Common.Button.Fight".Localized(), () => CombatScene.Start(EnemyRegistry.Create(ItemIds.ScarredBlade), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false));
        }

        public void FindScarredBlade()
        {

            OutputText("Scenes.Sheilascene.FindScarredBlade.GlimmerRevealsBlade".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), TakeScarredBlade);

            AddButton(1, "Common.Button.No".Localized(), LeaveScarredBlade);
        }

        public void TakeScarredBlade()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.TakeScarredBlade.TakesBloodthirstySaber".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ScarredBladeStatus, 0);

            InventoryScene.TakeItem(ItemIds.ScarredBlade, () => ReturnToCamp(1));
        }

        public void LeaveScarredBlade()
        {
            ClearOutput();

            OutputText("Scenes.Sheilascene.LeaveScarredBlade.LeavesItToRust".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ScarredBladeStatus, -1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}