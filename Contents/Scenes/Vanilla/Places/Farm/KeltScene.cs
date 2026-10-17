using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Farm
{
    [Register<KeltScene>("coc:kelt_scene")]
    public class KeltScene : Scene
    {

        public override void Entry()
        {
            KeltEncounter();
        }

        public int BowSkill(int diff)
        {
            for (var i = 0; i < diff; i++)
                Player.MasteryXP(new Identifier("coc", "Bow"), 5 + Random.Shared.Next(11));

            return Player.MasteryLevel(new Identifier("coc", "Bow"));
        }

        public void KeltEncounter()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.Kelt))
                Player.CreateStatusEffect(StatusEffectIds.Kelt, 0, 0, 0, 0);

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 2, 1);

            if (Player.StatusEffectValue(StatusEffectIds.Kelt, 2) <= 1)
                KeltFirstTime();
            else
            {
                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 2) <= 3)
                {
                    KeltMainEncounter();

                    return ;
                }

                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 130)
                {
                    if (((Player.Libido + Player.Lust) < 30 && Player.Intelligence >= 50) || !Player.HasStatusEffect(StatusEffectIds.KeltBadEndWarning))
                    {
                        Player.CreateStatusEffect(StatusEffectIds.KeltBadEndWarning, 0, 0, 0, 0);

                        ShowImage("kelt-dream");

                        OutputText("Scenes.Keltscene.KeltEncounter.RaceTowardsTheFarm".Localized());

                        OutputText("Scenes.Keltscene.KeltEncounter.AnotherThoughtCrosses".Localized());

                        AddButton(0, "Common.Button.Yes".Localized(), KeltSubmissiveBadEnd);

                        AddButton(1, "Common.Button.No".Localized(), DefySubmission);

                        AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), DefySubmissionAndFight);
                    }
                    else
                        KeltSubmissiveBadEnd();

                    return ;
                }

                if (Player.IsTaur() && Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 100 && (int)Player.Gender > 1)
                {
                    if (Player.Intelligence > Random.Shared.Next(40) && Player.StatusEffectValue(StatusEffectIds.Kelt, 1) < 130 && !Player.HasStatusEffect(StatusEffectIds.KeltBadEndWarning))
                    {
                        Player.CreateStatusEffect(StatusEffectIds.KeltBadEndWarning, 0, 0, 0, 0);

                        ShowImage("kelt-dream");

                        OutputText("Scenes.Keltscene.KeltEncounter.ApproachFarmReadyFor".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 5 + 10)));

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                        KeltCentaurBadEnd();

                    return ;
                }

                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 2) == 4 && !Player.HasStatusEffect(StatusEffectIds.NakedOn))
                {
                    KeltRequiresNakedness();

                    return ;
                }

                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 40 && !Player.HasStatusEffect(StatusEffectIds.KeltBJ))
                {
                    KeltRequiresBlowjobs();

                    return ;
                }

                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 2) > 4)
                    KeltMainEncounter();
            }
        }

        public void KeltFirstTime()
        {

            OutputText("Scenes.Keltscene.KeltFirstTime.ApproachWhitneysFarm".Localized());

            ShowImage("kelt-encounter");

            OutputText("Scenes.Keltscene.KeltFirstTime.GetCloserHowever".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.WarinessGrowingSlowDown".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.LooksScornfullyAtYou".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.DontBeStupid".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.TouchesLongbowThat".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.LikeMyBow".Localized());

            OutputText("Scenes.Keltscene.KeltFirstTime.LaughsAgainDerisively".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DefySubmission()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.DefySubmission.InDefianceDefy".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DefySubmissionAndFight()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.DefySubmissionAndFight.InDefianceAndFight".Localized());

            SceneInstance<KellyScene>.Get().FightToBeatKelt();
        }

        public void FightToBeatKeltVictoryHP()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.FightToBeatKeltVictoryHP.TooWeakToContinue".Localized());

            ChooseToFinishKelt();
        }

        public void FightToBeatKeltVictoryLust()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.FightToBeatKeltVictoryLust.OverwhelmedByDesires".Localized());

            ChooseToFinishKelt();
        }

        public void ChooseToFinishKelt()
        {

            CombatScene.CleanupAfterCombat();

            OutputText("Scenes.Keltscene.ChooseToFinishKelt.StepOverDefeated".Localized());

            AddButton(0, "Common.Button.KillHim".Localized(), YouBadEndKeltForGood);

            AddButton(1, "Common.Button.RapeHim".Localized(), FuckKeltsShitUp);

            AddButton(4, "Common.Button.TakebowLeave".Localized(), LeaveKelt4Good);
        }

        public void YouBadEndKeltForGood()
        {
            ClearOutput();

            ShowImage("item-kelt'sBow");

            OutputText("Scenes.Keltscene.YouBadEndKeltForGood.WithoutASecondThought".Localized());

            OutputText("Scenes.Keltscene.YouBadEndKeltForGood.AbruptTwistSnap".Localized());

            OutputText("Scenes.Keltscene.YouBadEndKeltForGood.GotKeltsBow".Localized());

            Player.RemoveKeyItem("Bow");

            Player.CreateKeyItem("Kelt's Bow", 0, 0, 0, 0);

            Player.CreateStatusEffect(StatusEffectIds.KeltOff, 0, 0, 0, 0);

            Player.UpgradeBeautifulSword();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.KeltKilled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void LeaveKelt4Good()
        {
            ClearOutput();

            ShowImage("item-kelt'sBow");

            OutputText("Scenes.Keltscene.LeaveKelt4Good.DeterminingNotWorth".Localized());

            OutputText("Scenes.Keltscene.LeaveKelt4Good.AlsoFindTheBow".Localized());

            OutputText("Scenes.Keltscene.LeaveKelt4Good.GotKeltsBow".Localized());

            Player.RemoveKeyItem("Bow");

            Player.CreateKeyItem("Kelt's Bow", 0, 0, 0, 0);

            Player.CreateStatusEffect(StatusEffectIds.KeltOff, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltRequiresNakedness()
        {
            ClearOutput();

            ShowImage("kelt-encounter");

            OutputText("Scenes.Keltscene.KeltRequiresNakedness.OnceMoreEncounterKelt".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresNakedness.HeSneers".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresNakedness.SlapsHandOnHis".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresNakedness.CowardTooFuckOff".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresNakedness.ObeyHisDemand".Localized());

            if (Player.Corruption + Player.Libido + (Player.Lust * 100 / Player.MaxLust) >= 180)
            {
                OutputText("Scenes.Keltscene.KeltRequiresNakedness.OfCourseYouDo".Localized());

                DoNext(() => KeltEagerlyGetNaked());

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Reluctantly".Localized(), KeltReluctantlyGetNaked);

            AddButton(1, "Common.Button.Eagerly".Localized(), KeltEagerlyGetNaked);

            if (Player.Intelligence > 40 && (Player.IsCorruptEnough(70) || Player.HasPerk(PerkIds.Sadist) || Player.HasPerk(PerkIds.Pervert)) && !Player.IsTaur())
            {
                OutputText("Scenes.Keltscene.KeltRequiresNakedness.IfYouFightBack".Localized());

                AddButton(3, "Common.Button.TurnTables".Localized(), KeltResistance);
            }
            else
                AddButtonDisabled(3, "Turn Tables", "");

            AddButton(4, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), KeltRefuseNakedness);
        }

        public void KeltRefuseNakedness()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltRefuseNakedness.AdamantlyRefuse".Localized());

            OutputText("Scenes.Keltscene.KeltRefuseNakedness.SomehowYouKnow".Localized());

            Player.CreateStatusEffect(StatusEffectIds.KeltOff, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltEagerlyGetNaked()
        {
            ClearOutput();

            DoNext(() => ReturnToCamp(1));

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.NoProblemNakedIn".Localized(
                Player.ClothedOrNaked("stripping down", "being")
            ));

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.KeltPacesAroundYou".Localized(
                Player.ClothedOrNaked("remove your top first, slowly revealing your " + Appearance.AllBreastsDescript(Player), "show off your " + Appearance.AllBreastsDescript(Player))
            ));

            if (Player.Gender == Gender.Male && Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.WhatAreYouGirl".Localized());
            else
                if (Player.BiggestBreastSize() == 0)
                    OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.SwornPairOfTits".Localized());
                else
                    if (Player.BiggestBreastSize() <= 5)
                        OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.NoWonderCoverUp".Localized(
                            NumberUtils.Num2Text(Player.BreastRows.Count())
                        ));
                    else
                        if (Player.BiggestBreastSize() <= 11)
                            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.UhOhBoysLook".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.DoesntThatLookYummy".Localized());

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.DespiteHisHarshComments".Localized(
                Player.ClothedOrNaked("As you remove the lower half of your clothes, stripping completely naked", "As you perform a pelvic thrust and slowly spin around")
            ));

            if (Player.Body.Butt < 6)
                OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.SomeoneWorksOut".Localized());
            else
                if (Player.Body.Butt < 13)
                    OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.NiceLittleCushion".Localized());
                else
                    OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.RearEndGetBehind".Localized());

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.SlapsYourAss".Localized());

            if (Player.Cocks.Count == 0)
                OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.PrettyLittleThing".Localized());
            else
            {
                OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.InevitablyEyesDrop".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Human)
                    OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.SportingNormalModel".Localized());
                else
                    if (new[] { BodyPartTypes.Dog, BodyPartTypes.Wolf }.Contains(Player.Cocks[0].Id))
                        OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.LookieHereWeGot".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.NowThatsJustSad".Localized());
                        else
                            if (Player.Cocks[0].TypeIndex >= 3)
                                OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.IllBiteWhatThe".Localized());

                if (Player.Cocks[0].Length <= 10)
                    OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.PokeAWoman".Localized());
                else
                    if (Player.Cocks[0].Length <= 20)
                        OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.AtLeastYouveGot".Localized());
                    else
                        OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.IfTheyCouldFind".Localized());
            }

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.LooksYouOverLast".Localized());

            OutputText("Scenes.Keltscene.KeltEagerlyGetNaked.NodAlmostGrateful".Localized());

            Player.CreateStatusEffect(StatusEffectIds.NakedOn, 0, 0, 0, 0);

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 10);

            Player.ApplyStats(new DynStat(GameStats.Lust, (int)(Player.Libido / 10) + 5));

            DoNext(() => KeltMainEncounter2());
        }

        public void KeltReluctantlyGetNaked()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.UncomfortableWithIdea".Localized(
                Player.ArmorDescript()
            ));

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.KeltPacesAroundYou".Localized(
                Player.ClothedOrNaked("remove your top first, slowly revealing your " + Appearance.AllBreastsDescript(Player), "show off your " + Appearance.AllBreastsDescript(Player))
            ));

            if (Player.Gender == Gender.Male && Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.WhatAreYouGirl".Localized());
            else
                if (Player.BiggestBreastSize() == 0)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.SwornPairOfTits".Localized());
                else
                    if (Player.BiggestBreastSize() <= 5)
                        OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.NoWonderCoverUp".Localized(
                            NumberUtils.Num2Text(Player.BreastRows.Count())
                        ));
                    else
                        if (Player.BiggestBreastSize() <= 11)
                            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.UhOhBoysLook".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.DoesntThatLookYummy".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.EvenHisHarshComments".Localized(
                Player.ClothedOrNaked("As you remove the lower half of your clothes, stripping completely naked", "As you perform a pelvic thrust and slowly spin around")
            ));

            if (Player.Body.Butt < 6)
                OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.SomeoneWorksOut".Localized());
            else
                if (Player.Body.Butt < 13)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.NiceLittleCushion".Localized());
                else
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.RearEndGetBehind".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.SlapsYourWithOpen".Localized());

            if (Player.Cocks.Count == 0)
                OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.PrettyLittleThing".Localized());
            else
            {
                OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.InevitablyEyesDrop".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Human)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.SportingNormalModel".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Dog)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.LookieHereWeGot".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.NowThatsJustSad".Localized());

                if (Player.Cocks[0].TypeIndex >= 3)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.IllBiteWhatThe".Localized());

                if (Player.Cocks[0].Length <= 10)
                    OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.PokeAWoman".Localized());
                else
                    if (Player.Cocks[0].Length <= 20)
                        OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.AtLeastYouveGot".Localized());
                    else
                        OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.IfTheyCouldFind".Localized());
            }

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.LooksYouOverLast".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantlyGetNaked.NodALittleIrritated".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 7);

            Player.ApplyStats(new DynStat(GameStats.Lust, (int)(Player.Libido / 10) + 5));

            DoNext(() => KeltMainEncounter2());
        }

        public void KeltRequiresBlowjobs()
        {
            ClearOutput();

            Player.CreateStatusEffect(StatusEffectIds.KeltBJ, 0, 0, 0, 0);

            ShowImage("kelt-encounter");

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.CrossingFieldOfFarm".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.GrinCrossesYourFace".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.EnjoyingTheSights".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.BlinkStartled".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.BetFiringShafts".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.PacesAroundYou".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.SelfInTheFoot".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.NodAutomatically".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.DontWantYourMoney".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.PushesYouOver".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.CantHelpButStare".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.KnowThePrice".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.SwallowHardYouDont".Localized());

            OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.DoYouSubmit".Localized());

            if (Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 200 && Player.Intelligence < 60)
            {
                OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.OfCourseSlut".Localized());

                DoNext(() => KeltBlowjobRequirementEagerly());

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Shamefully".Localized(), () => KeltBlowjobRequirementShamefully());

            AddButton(1, "Common.Button.Eagerly".Localized(), () => KeltBlowjobRequirementEagerly());

            AddButton(2, "Common.Button.Never".Localized(), () => KeltBlowjobRequirementNever());

            if (Player.Intelligence > 40 && (Player.IsCorruptEnough(70) || Player.HasPerk(PerkIds.Sadist) || Player.HasPerk(PerkIds.Pervert)) && !Player.IsTaur())
            {
                OutputText("Scenes.Keltscene.KeltRequiresBlowjobs.IfYouFightBack".Localized());

                AddButton(3, "Common.Button.TurnTables".Localized(), KeltResistance);
            }
            else
                AddButtonDisabled(3, "Turn Tables", "");

            AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => SceneInstance<KellyScene>.Get().FightToBeatKelt());
        }

        public void KeltBlowjobRequirementNever(bool newl = true)
        {
            ClearOutput();

            ShowImage("kelt-encounter");

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.StruggleHoldOnto".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.FeetMoveUncertainly".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.SpendsFewMomentsCursing".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.HaveJustAMoment".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.MovesTowardsEyesBlazing".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.WalkedInOnYou".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.WordsHurtMoreThan".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementNever.FeelingWeakerNow".Localized());

            Player.CreateStatusEffect(StatusEffectIds.KeltOff, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltBlowjobRequirementShamefully(bool newl = true)
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.ThoughtOfGoingThrough".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.FaceFlushedWithShame".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.LeaningForwardLapMeekly".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.MassiveCockHead".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.DoesNotGiveLong".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.KnewItFromMoment".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.TryBestToIgnore".Localized());

            ShowImage("kelt-farm-shamefulbj");

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.FeelPrettyGood".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.HurtsToAdmitIt".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.DiligenceRewarded".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.ByTheTimeFinishes".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.MovesAsideLookingDown".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.GetUsedBitch".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.HeresTheRule".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.LeavesQuitePleased".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementShamefully.UnhealthyToKeep".Localized());

            Player.Orgasm("Lips", false);

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 7);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltBlowjobRequirementEagerly(bool newl = true)
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.LikeSomeoneAnswered".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.HeSaysCruelly".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.ThoughtSendsQuiver".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.SaltyOozeSetsYour".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.NurseAtItHungrily".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.DoesNotGiveLong".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.KnewItFromMoment".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.InsultingWordsCause".Localized());

            ShowImage("kelt-farm-eagerbj");

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.FeelPrettyGood".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.BarelyHearTheWords".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.DiligenceRewarded".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.ByTheTimeFinishes".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.InTimeMovesAside".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.StillHornyBitch".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.LeavesQuitePleased".Localized());

            OutputText("Scenes.Keltscene.KeltBlowjobRequirementEagerly.PartFeelsUnsteady".Localized());

            Player.Orgasm("Lips", false);

            Player.CreateStatusEffect(StatusEffectIds.BlowjobOn, 0, 0, 0, 0);

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 15);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltMainEncounter()
        {
            ClearOutput();

            ShowImage("kelt-encounter");

            OutputText("Scenes.Keltscene.KeltMainEncounter.OnceMoreEncounterKelt".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 30)
                OutputText("Scenes.Keltscene.KeltMainEncounter.GrindTeethInIrritation".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 70)
                    OutputText("Scenes.Keltscene.KeltMainEncounter.NodReluctantlyKeltGrins".Localized());
                else
                    OutputText("Scenes.Keltscene.KeltMainEncounter.NodEnthusiastically".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Kelt, 2) <= 3)
                OutputText("Scenes.Keltscene.KeltMainEncounter.SeemsToFindIdea".Localized());
            else
            {
                KeltMainEncounterAfterNakedReq();

                return ;
            }

            DoNext(() => KeltMainEncounter2());
        }

        public void KeltMainEncounterAfterNakedReq()
        {

            if (Player.HasStatusEffect(StatusEffectIds.NakedOn))
            {
                OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.NodsSmirkingGestures".Localized());

                Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 3);
            }
            else
                if (Random.Shared.Next(10) <= 5)
                {
                    OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.LooksDownBodyScornfully".Localized());

                    if ((Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 180 && Player.Intelligence < 30) || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 60)
                    {
                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.IdeaTurnsYouOn".Localized());

                        if ((Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 220 && Player.Intelligence < 40) || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 70)
                        {
                            OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.WithALustySmile".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.NakedOn, 0, 0, 0, 0);
                        }

                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.ParagraphBreak".Localized());

                        Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 7);
                    }
                    else
                    {
                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.NotCertainYouWant".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Yes".Localized(), KeltReluctantlyGetNaked);

                        AddButton(1, "Common.Button.No".Localized(), KeltRefuseNakedness);

                        if (Player.Intelligence > 40 && (Player.IsCorruptEnough(70) || Player.HasPerk(PerkIds.Sadist) || Player.HasPerk(PerkIds.Pervert)) && !Player.IsTaur())
                        {
                            OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.IfYouFightBack".Localized());

                            AddButton(2, "Common.Button.TurnTables".Localized(), KeltResistance);
                        }
                        else
                            AddButtonDisabled(2, "Turn Tables", "");

                        AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => SceneInstance<KellyScene>.Get().FightToBeatKelt());

                        return ;
                    }
                }
                else
                {
                    OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.LooksAtClothesSourly".Localized());

                    if ((Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 180 && Player.Intelligence < 40) || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 60)
                    {
                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.ThoughIdeaTurnsOn".Localized());

                        if ((Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 220 && Player.Intelligence < 40) || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 75)
                        {
                            OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.CastASeductiveSmile".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.NakedOn, 0, 0, 0, 0);
                        }

                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.ParagraphBreakAgain".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (int)(Player.Libido / 10) + 5));
                    }
                    else
                        OutputText("Scenes.Keltscene.KeltMainEncounterAfterNakedReq.IgnoreBarbedComments".Localized());
                }

            DoNext(() => KeltMainEncounter2());
        }

        public void KeltMainEncounter2()
        {

            if (!Player.HasKeyItem("Bow"))
            {
                ShowImage("item-bow");

                OutputText("Scenes.Keltscene.KeltMainEncounter2.SaysTossingSpare".Localized());

                OutputText("Scenes.Keltscene.KeltMainEncounter2.DespiteMockingDescription".Localized());

                if (!Player.HasKeyItem("Bow"))
                    Player.CreateKeyItem("Bow", 0, 0, 0, 0);
            }
            else
            {
                OutputText("Scenes.Keltscene.KeltMainEncounter2.SneersAsHeLooks".Localized());

                OutputText("Scenes.Keltscene.KeltMainEncounter2.TogetherHeadTo".Localized());
            }

            if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 60 && Random.Shared.Next(4) == 0 && Player.HasStatusEffect(StatusEffectIds.KeltBJ))
            {
                DoNext(() => KeltMainEncounterPostBlowjob());

                return ;
            }

            DoNext(() => KeltMainEncounter3());
        }

        public void KeltMainEncounter3()
        {
            ClearOutput();

            float temporary = 0;

            if (!Player.HasStatusEffect(StatusEffectIds.NakedOn))
            {
                if ((int)Player.Gender > 1)
                    ShowImage("kelt-archery-female");
                else
                    ShowImage("kelt-archery-male");

                OutputText("Scenes.Keltscene.KeltMainEncounter3.ArrogantCrudeAndAll".Localized());

                var temp = Random.Shared.Next(4);

                if (temp == 0)
                    OutputText("Scenes.Keltscene.KeltMainEncounter3.ShapingUpPretty".Localized());

                if (temp == 1)
                    OutputText("Scenes.Keltscene.KeltMainEncounter3.WantToBecomeArcher".Localized());

                if (temp == 2)
                    OutputText("Scenes.Keltscene.KeltMainEncounter3.SeeTheRedDot".Localized());

                if (temp == 3)
                    if (Player.Race != "centaur")
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.BestOptionIsFuck".Localized());
                    else
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.RecommendSuicide".Localized());

                if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 5)
                    BowSkill(5 + Random.Shared.Next(4));
                else
                    BowSkill(1);
            }
            else
            {
                if (Player.InHeat && (int)Player.Gender > 1)
                {
                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LineUpAsNormal".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.OneShotGoesWide".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LeanOverToPluck".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.DidntThinkNoticeSlut".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.ForelegsRearUp".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 30)
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.StruggleAsBestAs".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 70)
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.PutForthTokenEffort".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.MasculineScentOfHim".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.CentaursCockHasTruly".Localized());

                    if (Player.VaginalCapacity() <= 16)
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.FitSeemsImpossible".Localized());
                    else
                        if (Player.VaginalCapacity() <= 40)
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.KeltCockOversized".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.SeemsLikePerfectFit".Localized());

                    Player.CuntChange(50, true, true, false);

                    if (Player.IsTaur())
                        ShowImage("kelt-farm-female-inheat-taur");
                    else
                        ShowImage("kelt-farm-female-inheat");

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.RideGetsRougher".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.NotTooBad".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 30)
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.ShiverAndGroan".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 70)
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.ThoughtOfThatFills".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.WhimperInAffirmative".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.HereComesYourBaby".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.BloomOfWarmth".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.SomeTimeLaterEnormous".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.GoodLookForYou".Localized());

                    Player.SlimeFeed();

                    Player.Orgasm("Vaginal");

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LeavesWithoutAnotherWord".Localized());

                    Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 5);

                    Player.KnockUp(PregnancyIds.Kelt, PregnancyIds.CentaurIncubation, 50);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

                temporary = Random.Shared.Next(5);

                if (temporary <= 2)
                {
                    ShowImage("kelt-archery-naked");

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LessonProceedsAsNormal".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 30)
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.TryToIgnoreFoul".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) <= 70)
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.DespiteYourselfCruder".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.WordsOnlyServe".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                    if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 5)
                        BowSkill(4);
                    else
                        BowSkill(1);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

                if (Player.BiggestBreastSize() == 0 && temporary == 3)
                    temporary = 4;

                if (temporary == 3)
                {
                    ShowImage("kelt-boobs");

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.PracticeBeginsAsNormal".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.AsksGrinningDown".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.IndignantlyCoverBreasts".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.WomenArentWarriors".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.SurpriseSuddenlyLeans".Localized());

                    if (Player.BiggestBreastSize() <= 5)
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.EvenWithIttyBitty".Localized());
                    else
                        if (Player.BiggestBreastSize() <= 11)
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.LookAtMommyMelons".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounter3.MiracleOfScience".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.DespiteCruelWords".Localized());

                    if (Player.BiggestLactation() > 1)
                    {
                        OutputText("Scenes.Keltscene.KeltMainEncounter3.BeadsOfMilkAppear".Localized());

                        OutputText("Scenes.Keltscene.KeltMainEncounter3.BroughtSnacksForAll".Localized());

                        OutputText("Scenes.Keltscene.KeltMainEncounter3.WithoutHesitationLowers".Localized());

                        OutputText("Scenes.Keltscene.KeltMainEncounter3.NotBadCow".Localized());

                        OutputText("Scenes.Keltscene.KeltMainEncounter3.ReleasedTeatContinues".Localized());

                        Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                        Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
                    }

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.KnowYourPlace".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.FlickingErectTeats".Localized());

                    DoNext(() => ReturnToCamp(1));

                    Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 5);

                    if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 5)
                        BowSkill(4);
                    else
                        BowSkill(1);

                    return ;
                }

                if (temporary == 4)
                {
                    ShowImage("kelt-archery-naked");

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.NothingYouDoToday".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.AggressiveBehaviorDistracts".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.RoarsOutBackhanding".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.ThrownOffGuard".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.ThatWhatYouWanted".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LiftsUpWithFingers".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.NowThatOurBitch".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.GroaningDistracted".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.WithASnortShoves".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.LessonsOverForNow".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounter3.WalksAwayWithoutWord".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                    Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 5);

                    if (Player.MasteryLevel(new Identifier("coc", "Bow")) < 5)
                        BowSkill(4);
                    else
                        BowSkill(1);

                    BowSkill(4);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltMainEncounterPostBlowjob()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.BlowjobOn))
            {
                if (Player.LustPercent >= 75 || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 90 && Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.AlmostTooMuch".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.SurpriseKeltLeads".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.GetInThereBitch".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ScramblingToYourFeet".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.LyingOnRoughTable".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.YourCockRockHard".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.FingersTraceAMoment".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.LikeThatDoYou".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.FlinchAsHisTwo".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.DoesntHesitateWithAn".Localized());

                    if (Player.Tallness < 112)
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.SmallerWord".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.BodyWithNoConcern".Localized());

                    Player.ButtChange(70, true, true, false);

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ParagraphBreak".Localized());

                    ShowImage("kelt-farm-smallbarn");

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.CantHelpItAt".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.BeneathPressedFirmly".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.NeglectedPussyQuivers".Localized());

                    if (Player.Gender == Gender.Genderless)
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.NeverCompleteLack".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.OrgasmOnlySeems".Localized());

                    if (Player.Anal.Looseness < 3)
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.TightLittleAss".Localized());
                    else
                        if (Player.Anal.Looseness < 5)
                            OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.NiceAssBackHere".Localized());
                        else
                            OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.StretchedOutLittleAss".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.LoseTrackOfTime".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.TorrentOfJizz".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.PullsOutHeLeaves".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.StayThereWhile".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.SomeHoursLater".Localized());

                    DoNext(() => ReturnToCamp(2));

                    Player.SlimeFeed();

                    if (Player.ButtChange(70, true))
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ParagraphBreakAgain".Localized());

                    Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 10);

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    return ;
                }
                else
                {
                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.MoveOutToThe".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ExcitementRushing".Localized());

                    ShowImage("kelt-farm-eagerbj");

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.SwallowMyCock".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.NothingCouldGiveMore".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ThoughHeLasts".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ReasonIKeptYou".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.HardlyRememberRest".Localized());

                    Player.SlimeFeed();

                    Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 5);

                    BowSkill(3);

                    Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 1f));

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
            }
            else
                if (Random.Shared.Next(10) <= 3)
                {
                    ShowImage("event-archery");

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.MoveTowardsPracticeField".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.OnceAtTheField".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.NotTodayBitch".Localized());

                    OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.ShiverOfDesire".Localized());

                    if (Player.Corruption + Player.LibidoPercent + Player.LustPercent >= 220 && Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 80)
                    {
                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.TryToResistNeed".Localized());

                        DoNext(() => KeltSubmitGivingBJ());

                        return ;
                    }
                    else
                    {
                        ShowImage("kelt-dream");

                        OutputText("Scenes.Keltscene.KeltMainEncounterPostBlowjob.DespiteTheNeedDesire".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Submit".Localized(), KeltSubmitGivingBJ);

                        AddButton(1, "Common.Button.Resist".Localized(), KeltResistGivingBJ);

                        AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => SceneInstance<KellyScene>.Get().FightToBeatKelt());

                        return ;
                    }
                }

            DoNext(() => KeltMainEncounter3());
        }

        public void KeltResistGivingBJ()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltResistGivingBJ.SummoningWhatRemains".Localized());

            OutputText("Scenes.Keltscene.KeltResistGivingBJ.SurpriseKeltDoes".Localized());

            OutputText("Scenes.Keltscene.KeltResistGivingBJ.KeepFoolingYourself".Localized());

            OutputText("Scenes.Keltscene.KeltResistGivingBJ.LeavesRefusingToTeach".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, -5);

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltSubmitGivingBJ()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Keltscene.KeltSubmitGivingBJ.TheresAGoodWhore".Localized());

            OutputText("Scenes.Keltscene.KeltSubmitGivingBJ.ReluctantlyShameBurning".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 90)
                Player.CreateStatusEffect(StatusEffectIds.BlowjobOn, 0, 0, 0, 0);

            KeltReluctantGivingBJ();

            DoNext(() => ContinueAfterBJ());
        }

        public void ContinueAfterBJ()
        {
            ClearOutput();

            ShowImage("event-archery");

            OutputText("Scenes.Keltscene.ContinueAfterBJ.AfterBriefRest".Localized());

            KeltMainEncounter3();
        }

        public void KeltReluctantGivingBJ()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.ImmediatelyMovesOver".Localized());

            ShowImage("kelt-farm-reluctantbj");

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.SwallowMyCock".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.NothingCouldGiveMore".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.DoesNotHesitate".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.ReasonIKeptYou".Localized());

            OutputText("Scenes.Keltscene.KeltReluctantGivingBJ.MeeklyNodHumiliated".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltCentaurBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltCentaurBadEnd.TrotTowardsFarmPleased".Localized());

            OutputText("Scenes.Keltscene.KeltCentaurBadEnd.OnceYouSpotHim".Localized());

            OutputText("Scenes.Keltscene.KeltCentaurBadEnd.HadToGetTaste".Localized());

            OutputText("Scenes.Keltscene.KeltCentaurBadEnd.PacesAroundYou".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.StillTechnicallyMale".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.RoughlySeizesHold".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.NoUseForMales".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.LegsRearWrapping".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.WasForHimTo".Localized());

                ShowImage("kelt-lost-male");

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.DoesNotHesitateAsserting".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.SneersThrustingHard".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.ThrustsInRuthlessly".Localized());
            }

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.SupposedToBe".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.RoughlyShovesCouple".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.ManOrWomanFine".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.LegsRearWrappingAgain".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.WasForHimAgain".Localized());

                ShowImage("kelt-lost-male");

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.AssertingDominanceAgain".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.SneersThrustingHardAgain".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.ThrustsInRuthlesslyAgain".Localized());
            }

            if ((int)Player.Gender >= 2)
            {
                if (Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Keltscene.KeltCentaurBadEnd.TheSlutIsPacking".Localized());

                    OutputText("Scenes.Keltscene.KeltCentaurBadEnd.IsntOfCourseNot".Localized());

                    OutputText("Scenes.Keltscene.KeltCentaurBadEnd.WhatICameFor".Localized());
                }

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.CrudelyPushesThreeFingers".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.PrettyLittleFilly".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.LegsRearWrappingThird".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.WasForHimThird".Localized());

                ShowImage("kelt-lost-female");

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.AssertingDominanceThird".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.SneersThrustingHardThird".Localized());

                OutputText("Scenes.Keltscene.KeltCentaurBadEnd.ThrustsInRuthlesslyThird".Localized());
            }

            DoNext(() => KeltBadEndEpilogue());
        }

        public void KeltSubmissiveBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.RaceTowardsTheFarm".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.AlmostSoonArrive".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.FaceCloudsOverDarkly".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.EnoughOfThisGame".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.SenseOfDreadFills".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.NoMoreArchery".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.WhimperWithJoy".Localized());

            ShowImage("kelt-farm-smallbarn");

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.FromNowYoureMine".Localized());

            OutputText("Scenes.Keltscene.KeltSubmissiveBadEnd.ScreamButWithPleasure".Localized());

            DoNext(() => KeltBadEndEpilogue());
        }

        public void KeltBadEndEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.NextYearFew".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.HeroineMovedForward".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.SuddenSoundCaught".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.WhatSheFound".Localized((Player.HasPerk(PerkLib.BroBody) || Player.HasPerk(PerkLib.FutaForm) || Player.Gender == Gender.HERM ? "hermaphrodite" : "female"), (Player.HasPerk(PerkLib.BroBody) || Player.HasPerk(PerkLib.FutaForm) || Player.Gender == Gender.HERM ? "and cum " : ""), (Player.HasPerk(PerkLib.BroBody) || Player.HasPerk(PerkLib.FutaForm) || Player.Gender == Gender.HERM ? "were" : "was")));

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.InTimeMaleObliged".Localized((Player.HasPerk(PerkLib.BroBody) || Player.HasPerk(PerkLib.FutaForm) || Player.Gender == Gender.HERM ? " At the same time, she spurts a large amount of futa-cum, an obvious sign that she's a hermaphrodite." : "")));

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.CentaurDismounted".Localized());

            ShowImage("badend-kelt");

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.EnjoyingTheShowFucker".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.HeroinePaledTurned".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.LittleBitchHuman".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.TouchedBowStrung".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.CentaurNoticedLooking".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.WavedConfidentlyAtMare".Localized());

            OutputText("Scenes.Keltscene.KeltBadEndEpilogue.GrinnedKnowinglyMusk".Localized());

            GameOver();
        }

        public void KeltResistance()
        {
            ClearOutput();

            OutputText("Scenes.Keltscene.KeltResistance.YouCloseYourEyes".Localized());

            if (Player.HasMuzzle())
                OutputText("Scenes.Keltscene.KeltResistance.LowGrowlBuilding".Localized());
            else
                OutputText("Scenes.Keltscene.KeltResistance.FightingAngerSpasms".Localized());

            OutputText("Scenes.Keltscene.KeltResistance.AsInsultsGoToo".Localized());

            OutputText("Scenes.Keltscene.KeltResistance.IdeaHowToPut".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), FuckKeltsShitUp);

            AddButton(1, "Common.Button.No".Localized(), KeltResistancePussyOut);
        }

        public void KeltResistancePussyOut()
        {

            OutputText("Scenes.Keltscene.KeltResistancePussyOut.SuppressYourAnger".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Kelt, 1, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckKeltsShitUp()
        {
            ClearOutput();

            if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs && Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake && Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeigningCoySmileLick".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.OpenMouthAndInstead".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.MomentToIndulge".Localized());

                ShowImage("kelt-farm-naga-subkelt");

                if (Player.HasCock())
                {
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SaysSighingAsPart".Localized(
                            Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.UnpracticedAtFirstUnsure".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighingAsPartAgain".Localized(
                            Player.CockDescript(0),

                            Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.UnpracticedUnsureAgain".Localized());
                    }

                    if (Player.Cocks[0].Length <= 10)
                    {
                        if (Player.Balls > 0)
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.YourBumpIntoChin".Localized());
                        else
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.GenitalSlitBumps".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.NowEntirelyEngaged".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeelMightyGroan".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.HissWickedIdeaStrikes".Localized());

                        if (Player.Balls > 0)
                            OutputText(Player.BallsDescriptLight());
                        else
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.CrotchWord".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.BeginToRoughlyPush".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighAsHeDrinks".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.PunctuateTheseWords".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SlowlyButSurelyFeed".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeelMightyGroanAgain".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.HissWickedIdeaAgain".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighHeDrinksAgain".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.PunctuateTheseWordsAgain".Localized());
                    }
                }
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighingAsPartThird".Localized(
                            Player.VaginaDescript(0),

                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.UnpracticedUnsureThird".Localized(
                            Player.ClitDescript(),

                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.GirlCumDripsDown".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeelMightyGroanAnd".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.WickedIdeaStrikesThird".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighAsHeAccepts".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.PunctuateTheseWordsThird".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SaysSneersKeltTrails".Localized(
                            Player.AssholeDescript()
                        ));

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.UnpracticedAtFirstUnwilling".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.TeaseAtHisHorsecock".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeelMightyGroanReverberate".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.HissWickedIdeaFourth".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighHeAcceptsAgain".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.PunctuateTheseWordsFourth".Localized());
                    }

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.TakeYourTimeUnknotting".Localized());
            }
            else
            {
                OutputText("Scenes.Keltscene.FuckKeltsShitUp.FeigningCoySmileDrop".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.GripDickBelow".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.CheeksColorAnger".Localized());

                if (Player.Tallness < 50 || Player.Cocks.Count == 0)
                {
                    if (Player.Tallness < 50)
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.RealizingTooShort".Localized());

                    OutputText("Scenes.Keltscene.FuckKeltsShitUp.GiveAssAHard".Localized());
                }
                else
                    OutputText("Scenes.Keltscene.FuckKeltsShitUp.GiveAssHardAgain".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.PullingOutWetSounding".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.PullTheRopeUp".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Keltscene.FuckKeltsShitUp.TurnAroundBendingOver".Localized(
                        Player.AssDescript()
                    ));
                else
                    if (Player.HasVagina() && (Player.Cocks.Count == 0 || Random.Shared.Next(2) == 0))
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SpreadYourAndExpose".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.Vaginas[0].Wetness < 3)
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.TastingThemExperimentally".Localized());
                        else
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.BecomingSlick".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SighAndHeComplies".Localized());

                        if (Player.Cocks.Count > 0)
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.SplattersCockCream".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.ParagraphBreak".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.SpreadAndExposeAgain".Localized());

                        if (Player.Cocks[0].Length < 10)
                            OutputText("Scenes.Keltscene.FuckKeltsShitUp.TakingEntireThingEasily".Localized());
                        else
                            if (Player.Cocks[0].Length < 25)
                                OutputText("Scenes.Keltscene.FuckKeltsShitUp.TakingEntireThingDeep".Localized());
                            else
                                OutputText("Scenes.Keltscene.FuckKeltsShitUp.TakingAsMuchAs".Localized());

                        OutputText("Scenes.Keltscene.FuckKeltsShitUp.ExplosiveOrgasmDeposit".Localized());
                    }

                ShowImage("kelt-farm-subkelt");

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.TakingPityOnHim".Localized());

                OutputText("Scenes.Keltscene.FuckKeltsShitUp.RedressBeforeComatose".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Corruption, 4f));

            Player.CreateStatusEffect(StatusEffectIds.KeltOff, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

    }
}