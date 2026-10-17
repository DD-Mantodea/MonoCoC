using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<FollowerInteractionsScene>("coc:follower_interactions")]
    public class FollowerInteractionsScene : Scene
    {

        public override void Entry()
        {
            AmilyMeetsPureJojo();
        }

        public void AmilyMeetsPureJojo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyMetPureJojo, CoC.Instance.Flags.Get(GameFlagIds.AmilyMetPureJojo) + 1);

            OutputText("Scenes.Followerinteractions.AmilyMeetsPureJojo.LocatedJojo".Localized());

            OutputText("Scenes.Followerinteractions.AmilyMeetsPureJojo.JojoColors".Localized());

            OutputText("Scenes.Followerinteractions.AmilyMeetsPureJojo.SighsHugs".Localized());

            OutputText("Scenes.Followerinteractions.AmilyMeetsPureJojo.NoticesYou".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            SceneInstance<JojoScene>.Get().JojoCamp();
        }

        public void PureJojoAndAmilySpar()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilySparWithPureJojo, CoC.Instance.Flags.Get(GameFlagIds.AmilySparWithPureJojo) + 1);

            OutputText("Scenes.Followerinteractions.PureJojoAndAmilySpar.AppearSparring".Localized());

            SceneInstance<AmilyScene>.Get().AmilyFollowerEncounter();
        }

        public void AmilyIsPissedAtYouForRuiningJojo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyPissedPcCorrupedJojo, CoC.Instance.Flags.Get(GameFlagIds.AmilyPissedPcCorrupedJojo) + 1);

            OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.PersonalFuckPet".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.WhatDidYouDo".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.AmilyScreams".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIsPissedAtYouForRuiningJojo.AtCrossroads".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FixHim".Localized(), AgreeToHelpAmilyFixJojo);

            AddButton(1, "Common.Button.FuckOff".Localized(), TellAmilyToFuckOffRuinedJojo);
        }

        public void TellAmilyToFuckOffRuinedJojo()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.BetterFuck".Localized());

            OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.SadnessPalpable".Localized());

            OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.RanBackRuins".Localized());

            if (Player.Intelligence >= 45)
                OutputText("Scenes.Followerinteractions.TellAmilyToFuckOffRuinedJojo.GoblinAle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            Go<CampScene>();
        }

        public void AgreeToHelpAmilyFixJojo()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.YouNod".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.GenuinelySorry".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.UnsureRightChoice".Localized());
                else
                    OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.EmptyPromise".Localized());

            OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.YouSay".Localized());

            OutputText("Scenes.Followerinteractions.AgreeToHelpAmilyFixJojo.TearfullyNods".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWaitForPcFixJojo, 1);

            Go<CampScene>();
        }

        public void FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.YouBroughtIt".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney));

            OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.FewSips".Localized());

            OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.NodAndLeave".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.AlreadyRegretting".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.LittleConfused".Localized());
                else
                    OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.GladDamage".Localized());

            OutputText("Scenes.Followerinteractions.FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM.SentencePeriod".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JojoDeadOrGone, 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo, 4);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWaitForPcFixJojo, 0);

            CoC.Instance.Flags.Set(GameFlagIds.JojoFixedStatus, 1);

            ReturnToCamp(1);
        }

        public void FindJojosNote()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.JojoFixedStatus, 2);

            OutputText("Scenes.Followerinteractions.FindJojosNote.NoteOnBedroll".Localized());

            OutputText("Scenes.Followerinteractions.FindJojosNote.AfterTranspired".Localized());

            Go<CampScene>();
        }

        public void AmilyDiscoversJojoWithTentaclesAndShitOhBoy()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyDiscoveredTentatleJojo, CoC.Instance.Flags.Get(GameFlagIds.AmilyDiscoveredTentatleJojo) + 1);

            OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.SettleDownFuck".Localized());

            OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.WhatDidYouDo".Localized());

            OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.AmilyScreams".Localized());

            if (!Player.IsPureEnough(66))
            {
                OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.FlipHerBird".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.RanBackRuins".Localized());

                if (Player.Intelligence >= 45)
                    OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.GoblinAle".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

                Go<CampScene>();
            }
            else
            {
                OutputText("Scenes.Followerinteractions.AmilyDiscoversJojoWithTentaclesAndShitOhBoy.ThisIsBad".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Explain".Localized(), AerisDies);

                AddButton(1, "Common.Button.FlipBird".Localized(), AmilyLeavesBirdJojoTentacles);
            }
        }

        public void AmilyLeavesBirdJojoTentacles()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AmilyLeavesBirdJojoTentacles.FlipHerBird".Localized());

            OutputText("Scenes.Followerinteractions.AmilyLeavesBirdJojoTentacles.RanBackRuins".Localized());

            if (Player.Intelligence >= 45)
                OutputText("Scenes.Followerinteractions.AmilyLeavesBirdJojoTentacles.GoblinAle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Go<CampScene>();
        }

        public void AerisDies()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AerisDies.ListensImpassively".Localized());

            OutputText("Scenes.Followerinteractions.AerisDies.KillJojo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.StopHer".Localized(), StopJojoDeathInTheNameOfLove);

            AddButton(1, "Common.Button.LetHer".Localized(), WhyWouldDoThis);
        }

        public void StopJojoDeathInTheNameOfLove()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.StopJojoDeathInTheNameOfLove.GrabsShoulders".Localized());

            if (Player.Intelligence < 40)
            {
                OutputText("Scenes.Followerinteractions.StopJojoDeathInTheNameOfLove.StopThinkThis".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -99f));

                CoC.Instance.Flags.Set(GameFlagIds.JojoDeadOrGone, 2);
            }
            else
            {
                OutputText("Scenes.Followerinteractions.StopJojoDeathInTheNameOfLove.ThisIsWrong".Localized());

                OutputText("Scenes.Followerinteractions.StopJojoDeathInTheNameOfLove.SlowlyNods".Localized());

                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));
            }

            ReturnToCamp(1);
        }

        public void WhyWouldDoThis()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.WhyWouldDoThis.AvertEyes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -99f));

            CoC.Instance.Flags.Set(GameFlagIds.JojoDeadOrGone, 2);

            ReturnToCamp(1);
        }

        public void AmilyIntroducesSelfToRathazul()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyMetRathazul, CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) + 1);

            OutputText("Scenes.Followerinteractions.AmilyIntroducesSelfToRathazul.ElderlyRodent".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIntroducesSelfToRathazul.StrokesWhiskers".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIntroducesSelfToRathazul.FriendlyHug".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIntroducesSelfToRathazul.StutterKiss".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIntroducesSelfToRathazul.TakesOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu();
        }

        public void AmilyIngredientDelivery()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AmilyIngredientDelivery.MakeWayLab".Localized());

            if (!SceneInstance<AmilyScene>.Get().AmilyFurry())
                OutputText("Scenes.Followerinteractions.AmilyIngredientDelivery.BigEaredBlur".Localized());
            else
                OutputText("Scenes.Followerinteractions.AmilyIngredientDelivery.BlurOfFur".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIngredientDelivery.SprintsLab".Localized());

            OutputText("Scenes.Followerinteractions.AmilyIngredientDelivery.BobsHead".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 4f));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyMetRathazul, CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) + 1);

            SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu();
        }

        public void AmilyAsksAboutRathazulsVillage()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyMetRathazul, CoC.Instance.Flags.Get(GameFlagIds.AmilyMetRathazul) + 1);

            OutputText("Scenes.Followerinteractions.AmilyAsksAboutRathazulsVillage.OnceAgainLab".Localized());

            OutputText("Scenes.Followerinteractions.AmilyAsksAboutRathazulsVillage.ShufflesAbout".Localized());

            OutputText("Scenes.Followerinteractions.AmilyAsksAboutRathazulsVillage.NodsSolemnly".Localized());

            SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu();
        }

        public void RathazulFreaksOverJojo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RathazulCorruptJojoFreakout, CoC.Instance.Flags.Get(GameFlagIds.RathazulCorruptJojoFreakout) + 1);

            OutputText("Scenes.Followerinteractions.RathazulFreaksOverJojo.SeriousWorried".Localized());

            OutputText("Scenes.Followerinteractions.RathazulFreaksOverJojo.StraightFace".Localized());

            OutputText("Scenes.Followerinteractions.RathazulFreaksOverJojo.MineCanHandle".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Mine".Localized(), TellRathazulYouOwnJojo);

            AddButton(1, "Common.Button.HandleIt".Localized(), TellRathazulYouCanHandleIt);

            AddButton(2, "Common.Button.Harmless".Localized(), TellRathazulJojoIsHarmless);
        }

        public void TellRathazulYouOwnJojo()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.TellRathazulYouOwnJojo.YouTellRathazul".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulYouOwnJojo.ScratchesHead".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulYouOwnJojo.BlushesLurches".Localized());

            ReturnToCamp(1);
        }

        public void TellRathazulYouCanHandleIt()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.TellRathazulYouCanHandleIt.YouTellRathazul".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulYouCanHandleIt.LooksYouOver".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulYouCanHandleIt.YouAnswer".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulYouCanHandleIt.SeemsSatisfied".Localized());

            ReturnToCamp(1);
        }

        public void TellRathazulJojoIsHarmless()
        {
            ClearOutput();

            SceneInstance<JojoScene>.Get().JojoSprite();

            OutputText("Scenes.Followerinteractions.TellRathazulJojoIsHarmless.ExplainToRathazul".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulJojoIsHarmless.LooksIncredulous".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulJojoIsHarmless.SnortsDisdainfully".Localized());

            OutputText("Scenes.Followerinteractions.TellRathazulJojoIsHarmless.ConversationConcluded".Localized());

            ReturnToCamp(1);
        }

        public void JojoOffersRathazulMeditation()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.JojoRathazulInteractionCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoRathazulInteractionCounter) + 1);

            OutputText("Scenes.Followerinteractions.JojoOffersRathazulMeditation.JojoAtLab".Localized());

            OutputText("Scenes.Followerinteractions.JojoOffersRathazulMeditation.TurnsGivesBow".Localized());

            SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu();
        }

        public void CatchRathazulNapping()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.JojoRathazulInteractionCounter, CoC.Instance.Flags.Get(GameFlagIds.JojoRathazulInteractionCounter) + 1);

            OutputText("Scenes.Followerinteractions.CatchRathazulNapping.IntensiveMeditation".Localized());

            OutputText("Scenes.Followerinteractions.CatchRathazulNapping.QuietlyRises".Localized());

            SceneInstance<JojoScene>.Get().JojoCamp();
        }

        public void MarbleVsAmilyFreakout()
        {
            ClearOutput();

            SceneInstance<AmilyScene>.Get().AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNotFreakedOut, CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFreakedOut) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleOrAmilyFirstForFreakout) <= 1)
                OutputText("Scenes.Followerinteractions.MarbleVsAmilyFreakout.BringAmily".Localized());
            else
                OutputText("Scenes.Followerinteractions.MarbleVsAmilyFreakout.BringMarbleBack".Localized());

            OutputText("Scenes.Followerinteractions.MarbleVsAmilyFreakout.WithinMoments".Localized());

            OutputText("Scenes.Followerinteractions.MarbleVsAmilyFreakout.HowRespond".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Staysilent".Localized(), MarbleIsPissyAndYourTooDumbToTalk);

            AddButton(1, "Common.Button.Pimp".Localized(), BeAPimpMarbleLovesIt);

            AddButton(2, "Common.Button.Explain".Localized(), LucyYouGotSomeSplainingToDo);
        }

        public void MarbleIsPissyAndYourTooDumbToTalk()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.MarbleIsPissyAndYourTooDumbToTalk.CantThinkAnything".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
            {
                OutputText("Scenes.Followerinteractions.MarbleIsPissyAndYourTooDumbToTalk.ProtectivelyFront".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0)
                {
                    OutputText("Scenes.Followerinteractions.MarbleIsPissyAndYourTooDumbToTalk.BurstsTears".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.CampMarble);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);
                }
                else
                {
                    OutputText("Scenes.Followerinteractions.MarbleIsPissyAndYourTooDumbToTalk.HammerRaised".Localized(
                        Player.ShortName
                    ));

                    Player.RemoveStatusEffect(StatusEffectIds.CampMarble);
                }

            ReturnToCamp(1);
        }

        public void BeAPimpMarbleLovesIt()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.BeAPimpMarbleLovesIt.LookBothEyes".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
            {
                OutputText("Scenes.Followerinteractions.BeAPimpMarbleLovesIt.TryAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Staysilent".Localized(), MarbleIsPissyAndYourTooDumbToTalk);

                AddButton(1, "Common.Button.Explain".Localized(), LucyYouGotSomeSplainingToDo);
            }
            else
            {
                OutputText("Scenes.Followerinteractions.BeAPimpMarbleLovesIt.SimplyStare".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Serious".Localized(), SrslyPimpinGuyz);

                AddButton(1, "Common.Button.Joking".Localized(), JokeAboutPimpularness);
            }
        }

        public void SrslyPimpinGuyz()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.YesImSerious".Localized());

            Player.HealthChange(-Player.Health - 1);

            OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.WakeHoursLater".Localized());

            if (Player.ArmorName != "goo armor")
                OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.ComfortableClothes".Localized());
            else
                OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.ArmorGoo".Localized());

            OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.NoteRoughScript".Localized());

            Player.Gems *= 0;

            if (Player.Armor.Id != ArmorIds.VineArmorBehavior)
                Player.SetArmor(Armor.Nothing);

            Player.SetUnarmed();

            Player.RemoveStatusEffect(StatusEffectIds.CampMarble);

            OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.RanBackRuins".Localized());

            if (Player.Intelligence >= 45)
                OutputText("Scenes.Followerinteractions.SrslyPimpinGuyz.GoblinAle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void JokeAboutPimpularness()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.JokeAboutPimpularness.ThoughtAJoke".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Staysilent".Localized(), MarbleIsPissyAndYourTooDumbToTalk);

            AddButton(1, "Common.Button.Explain".Localized(), LucyYouGotSomeSplainingToDo);
        }

        // Haxe LucyYouGotSomeSplainingToDo()   outputText 3 
        public void LucyYouGotSomeSplainingToDo()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.LucyYouGotSomeSplainingToDo.TryingToExplain".Localized());

            if (Player.Intelligence > 50)
            {
                OutputText("Scenes.Followerinteractions.LucyYouGotSomeSplainingToDo.NotReallySure".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, -100);

                ReturnToCamp(1);

                return ;
            }
            else
            {
                OutputText("Scenes.Followerinteractions.LucyYouGotSomeSplainingToDo.BecomesApparent".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Staysilent".Localized(), MarbleIsPissyAndYourTooDumbToTalk);

                if (Player.HasPerk(PerkIds.MarblesMilk))
                    AddButton(1, "Common.Button.Blamemarble".Localized(), null);
                else
                    AddButton(1, "Common.Button.Blamemarble".Localized(), BlameMarblezSweetVagoozle);

                AddButton(2, "Common.Button.Blameamily".Localized(), BlameAmilysDumbMouseCunt);

                return ;
            }
        }

        public void BlameMarblezSweetVagoozle()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.BlameMarblezSweetVagoozle.PointBlankly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                OutputText("Scenes.Followerinteractions.BlameMarblezSweetVagoozle.AndChild".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                    OutputText("Scenes.Followerinteractions.BlameMarblezSweetVagoozle.AndHerChildren".Localized());

            OutputText("Scenes.Followerinteractions.BlameMarblezSweetVagoozle.SlowlyWalking".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.CampMarble);

            Player.RemoveStatusEffect(StatusEffectIds.NoMoreMarble);

            ReturnToCamp(1);
        }

        public void BlameAmilysDumbMouseCunt()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.BlameAmilysDumbMouseCunt.PointBlank".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            ReturnToCamp(1);
        }

        public void AmilyUrtaReaction()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AmilyUrtaReaction.SurprisesYou".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaReaction.BoundsOutCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNeedToFreakAboutUrta, 0);

            Go<CampScene>();
        }

        public void AskAboutAmily()
        {
            ClearOutput();

            if (View.Time.Hours > 14)
            {
                OutputText("Scenes.Followerinteractions.AskAboutAmily.AskBarPatrons".Localized());

                SceneInstance<TelAdre>.Get().BarTelAdre();

                return ;
            }
            else
            {
                OutputText("Scenes.Followerinteractions.AskAboutAmily.AskBarPatronsAgain".Localized());

                OutputText("Scenes.Followerinteractions.AskAboutAmily.MixtureConfusion".Localized());

                OutputText("Scenes.Followerinteractions.AskAboutAmily.UrtaGroan".Localized());

                OutputText("Scenes.Followerinteractions.AskAboutAmily.ChitteringSqueal".Localized());

                OutputText("Scenes.Followerinteractions.AskAboutAmily.EaseOpenDoor".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Followerinteractions.AskAboutAmily.HalfErect".Localized(
                        SceneInstance<AmilyScene>.Get().AmilyCock()
                    ));

                OutputText("Scenes.Followerinteractions.AskAboutAmily.StomachSwollen".Localized());

                if (SceneInstance<AmilyScene>.Get().Pregnancy.Event >= 3)
                    OutputText("Scenes.Followerinteractions.AskAboutAmily.AlmostTwice".Localized());

                OutputText("Scenes.Followerinteractions.AskAboutAmily.MouseHolding".Localized(
                    SceneInstance<AmilyScene>.Get().AmilyTits()
                ));

                OutputText("Scenes.Followerinteractions.AskAboutAmily.LeaveThem".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LeaveThem".Localized(), AskAboutAmilyPt2);

                AddButton(1, "Common.Button.Interrupt".Localized(), CheatingSloots);
            }
        }

        public void AskAboutAmilyPt2()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AskAboutAmilyPt2.BreaksKiss".Localized());

            OutputText("Scenes.Followerinteractions.AskAboutAmilyPt2.ShutTheDoor".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 75f));

            ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaAmilyFollowupNeeded, 1);
        }

        public void CheatingSloots()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.CheatingSloots.KickDoorOpen".Localized());

            OutputText("Scenes.Followerinteractions.CheatingSloots.TurnToAmily".Localized());

            OutputText("Scenes.Followerinteractions.CheatingSloots.MouseMayHavePoint".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LetItBe".Localized(), LetTheSlootsFuck);

            AddButton(1, "Common.Button.EndIt".Localized(), EndThisMadness);
        }

        public void LetTheSlootsFuck()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.LetTheSlootsFuck.ChuckleUnderstand".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 75f));

            ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 4);
        }

        public void EndThisMadness()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.EndThisMadness.SlamFist".Localized());

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Followerinteractions.EndThisMadness.DidThisForYou".Localized());

                OutputText("Scenes.Followerinteractions.EndThisMadness.MouseScurries".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);
            }
            else
            {
                OutputText("Scenes.Followerinteractions.EndThisMadness.UrtaScowls".Localized());

                OutputText("Scenes.Followerinteractions.EndThisMadness.AngryVixen".Localized());

                SceneInstance<UrtaScene>.Get().UrtaLove(-100);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 3);

            OutputText("Scenes.Followerinteractions.EndThisMadness.PutsEnd".Localized());

            ReturnToCamp(1);
        }

        public void AmilyUrtaMorningAfter()
        {

            OutputText("Scenes.Followerinteractions.AmilyUrtaMorningAfter.LeaveBedding".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaMorningAfter.SheGroans".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaMorningAfter.WaitRelax".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.BeNice".Localized(), SmileAndNodToAmilyXUrta);

            AddButton(1, "Common.Button.BeADick".Localized(), BeADickToAmily);
        }

        public void BeADickToAmily()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.BeADickToAmily.ShitEatingGrin".Localized());

            OutputText("Scenes.Followerinteractions.BeADickToAmily.FuckYouName".Localized());

            OutputText("Scenes.Followerinteractions.BeADickToAmily.ExactlyWhy".Localized());

            OutputText("Scenes.Followerinteractions.BeADickToAmily.ThatsBullshit".Localized());

            OutputText("Scenes.Followerinteractions.BeADickToAmily.GrabsGear".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 3);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            Go<CampScene>();
        }

        public void SmileAndNodToAmilyXUrta()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.SmileAndNodToAmilyXUrta.NotSurprised".Localized());

            OutputText("Scenes.Followerinteractions.SmileAndNodToAmilyXUrta.BlushingEmbarrassment".Localized());

            OutputText("Scenes.Followerinteractions.SmileAndNodToAmilyXUrta.LeaveSleep".Localized());

            OutputText("Scenes.Followerinteractions.SmileAndNodToAmilyXUrta.UrtaUnlocked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 4);

            Go<CampScene>();
        }

        public void AmilyUrtaSex()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaAmilyFollowupNeeded, 2);

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.KnowingSmirk".Localized());

            if (!Player.IsPureEnough(50) && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Followerinteractions.AmilyUrtaSex.PrivatelyNote".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.ParagraphBreak".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.WithGesture".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Followerinteractions.AmilyUrtaSex.BlushesSoftly".Localized());
            else
                OutputText("Scenes.Followerinteractions.AmilyUrtaSex.TurnsSideways".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.PeriodBreak".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.SmilesShyly".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.FaceLightsUp".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSex.EmptyRoom".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.UseDick".Localized(), SceneInstance<AmilyScene>.Get().ThreesomeAmilUrtaCAWKS);
            else
                AddButton(0, "Common.Button.UseDick".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UseVag".Localized(), SceneInstance<AmilyScene>.Get().UrtaXAmilyCuntPussyVagSQUICK);
            else
                AddButton(1, "Common.Button.UseVag".Localized(), null);

            AddButton(3, "Common.Button.Watch".Localized(), AmilyUrtaSexWatch);
        }

        public void AmilyUrtaSexWatch()
        {
            ClearOutput();

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.ContentWatch".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.DeepBreath".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.NotSureHow".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.AssureFine".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.RollsEyes".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.ApproachesUrta".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.GropingUrta".Localized(
                SceneInstance<AmilyScene>.Get().AmilyTits()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.UndersidePenis".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.BetweenTits".Localized(
                SceneInstance<AmilyScene>.Get().AmilyTits()
            ));

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.SlidesCleavage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.BeadPreCum".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.StraddlingUrta".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.WatchesUrta".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.QuickGlance".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.GiveMeSecond".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.HuffsGrowls".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.BreaksKissReaches".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.SqueaksMoans".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.BestToMilk".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.CollapsesUrta".Localized());

            OutputText("Scenes.Followerinteractions.AmilyUrtaSexWatch.TurnedOn".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            ReturnToCamp(1);
        }

    }
}