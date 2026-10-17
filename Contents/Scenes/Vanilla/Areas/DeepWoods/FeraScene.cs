using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.DeepWoods
{
    [Register<FeraScene>("coc:fera")]
    public class FeraScene : Scene
    {

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            switch (FeraStage())
            {
                case 0:
                {

                }

                break;

                case 1:
                {

                }

                break;

                case 2:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.FerasTrapSprung) > 0)
                    SceneInstance<FeraScene>.Get().FeraAkbalQuestIntro(false);
                else
                {

                }
                }

                break;
            }

            if (true )
            {

            }
        }

        public int FeraStage()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.FeraReleased) > 3)
            {
                if (Player.HasPerk(PerkIds.FerasBoonMilkingTwat) || Player.HasPerk(PerkIds.FerasBoonSeeder) || Player.HasPerk(PerkIds.FerasBoonWideOpen))
                    CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 2);
                else
                    if (Player.HasPerk(PerkIds.FerasBoonBreedingBitch) || Player.HasPerk(PerkIds.FerasBoonAlpha))
                        CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 1);
                    else
                        CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 0);

                CoC.Instance.Flags.Set(GameFlagIds.FeraEncounterDate, 0);

                if (CoC.Instance.Flags.Get(GameFlagIds.FerasTrapSprung) > 1)
                    CoC.Instance.Flags.Set(GameFlagIds.FerasTrapSprung, 1);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.FeraReleased);
        }

        public void PumpkinFuckEncounter()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PumpkinSeedsEaten, 0);

            CoC.Instance.Flags.Set(GameFlagIds.FeraEncounterDate, View.Time.Days);

            AwardAchievement(AchievementIds.HolidayHalloweenI);

            OutputText("Scenes.Fera.PumpkinFuckEncounter.JustAheadTheMiddle".Localized());

            OutputText("Scenes.Fera.PumpkinFuckEncounter.StartledByVegetation".Localized());

            OutputText("Scenes.Fera.PumpkinFuckEncounter.AdvanceCautiouslyFamiliar".Localized());

            OutputText("Scenes.Fera.PumpkinFuckEncounter.StemOfThePumpkin".Localized());

            OutputText("Scenes.Fera.PumpkinFuckEncounter.OneOfManyIndentations".Localized());

            OutputText("Scenes.Fera.PumpkinFuckEncounter.WhereOncePumpkinHad".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckIt".Localized(), PumpkinFuck);

            AddButton(1, "Common.Button.MountIt".Localized(), MountPumpkin);

            AddButton(2, "Common.Button.InsertHand".Localized(), FistPumpkin);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void FistPumpkin()
        {
            ClearOutput();

            OutputText("Scenes.Fera.FistPumpkin.RelaxedByVapors".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartII(0));
        }

        public void PumpkinFuck()
        {
            ClearOutput();

            OutputText("Scenes.Fera.PumpkinFuck.WellFuckYoureHorny".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.KneelDownBeforeTarget".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.TentativelyRaiseOne".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.GushyGourdInterior".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.PumpkinFuck.BallsDeepWord".Localized());
            else
                OutputText("Scenes.Fera.PumpkinFuck.HiltDeepWord".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.OrNotNotMind".Localized());

            if (Player.Cocks[0].Length >= 20)
                OutputText("Scenes.Fera.PumpkinFuck.AfterYouveFedOver".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.ParagraphBreak".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.PumpkinFuck.YourBallsSlap".Localized());
            else
                OutputText("Scenes.Fera.PumpkinFuck.YourGroinSlaps".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.IntoOrangeSkinWetly".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.SlapSlapRingsOut".Localized());

            OutputText("Scenes.Fera.PumpkinFuck.TriumphantRoarClimax".Localized());

            if (Player.CumQ() < 200)
                OutputText("Scenes.Fera.PumpkinFuck.WeakLoadDisappears".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Fera.PumpkinFuck.ThickLoadDisappears".Localized());
                else
                    OutputText("Scenes.Fera.PumpkinFuck.MassiveLoadFloods".Localized());

            if (Player.CumQ() < 1000)
            {
                OutputText("Scenes.Fera.PumpkinFuck.TakeABreather".Localized());

                OutputText("Scenes.Fera.PumpkinFuck.SharpPinchOnYour".Localized());

                OutputText("Scenes.Fera.PumpkinFuck.BeforeConsciousnessFades".Localized());
            }
            else
            {
                OutputText("Scenes.Fera.PumpkinFuck.OverOverYouEmpty".Localized());

                OutputText("Scenes.Fera.PumpkinFuck.ThankTheGods".Localized());

                OutputText("Scenes.Fera.PumpkinFuck.FaintRumblingIsOnly".Localized());

                if (Player.Weapon.IsUnarmed())
                    OutputText("Scenes.Fera.PumpkinFuck.BrandishingYourWeapon".Localized());
                else
                    OutputText("Scenes.Fera.PumpkinFuck.PickingUpYourWeapon".Localized());

                OutputText("Scenes.Fera.PumpkinFuck.AsWouldProtect".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartII(1));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));
        }

        public void MountPumpkin()
        {
            ClearOutput();

            OutputText("Scenes.Fera.MountPumpkin.WellFuckYoureHorny".Localized());

            OutputText("Scenes.Fera.MountPumpkin.KneelDownBeforeTarget".Localized());

            OutputText("Scenes.Fera.MountPumpkin.TentativelyRaiseJuiceSoaked".Localized());

            Player.CuntChange(50, true, false, false);

            OutputText("Scenes.Fera.MountPumpkin.GourdOozingStem".Localized());

            OutputText("Scenes.Fera.MountPumpkin.YourSlapOrangeSkin".Localized());

            OutputText("Scenes.Fera.MountPumpkin.WetSlappingNoisesRing".Localized());

            if (Player.Pregnancy.Incubation > 0 || (Player.Wetness() < 2 && Player.TotalFertility() < 30))
            {
                OutputText("Scenes.Fera.MountPumpkin.TriumphantRoarClimax".Localized());

                OutputText("Scenes.Fera.MountPumpkin.RidingOutYourOrgasm".Localized());

                OutputText("Scenes.Fera.MountPumpkin.AnotherGrowthBegins".Localized());

                OutputText("Scenes.Fera.MountPumpkin.CryOutHoldingStomach".Localized());

                OutputText("Scenes.Fera.MountPumpkin.BeforeConsciousnessThorns".Localized());
            }
            else
            {
                OutputText("Scenes.Fera.MountPumpkin.TriumphantRoarClimaxAgain".Localized());

                OutputText("Scenes.Fera.MountPumpkin.OverOverGrowingGourd".Localized());

                OutputText("Scenes.Fera.MountPumpkin.ThankTheGods".Localized());

                OutputText("Scenes.Fera.MountPumpkin.FaintRumblingIsOnly".Localized());

                if (!Player.Weapon.IsAttached())
                    OutputText("Scenes.Fera.MountPumpkin.PickingUpYourWeapon".Localized());
                else
                    OutputText("Scenes.Fera.MountPumpkin.RaisingYourGuard".Localized());

                OutputText("Scenes.Fera.MountPumpkin.AsWouldProtect".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartII(2));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));
        }

        public void PumpkinScenePartII(int variant = 0)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 1);

            switch (variant)
            {
                case 0:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.TitanicRentShivers".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.TitanicRentShiversAgain".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.PumpkinStemShivers".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartII.ErrorInvalidSceneVariant".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.StandingInTheMiddle".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.AwestruckByDevelopment".Localized((variant > 0 ? "oh-so-fuckable pumpkin" : "giant magical gourd")));

            OutputText("Scenes.Fera.PumpkinScenePartII.GoddessCirclesYou".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.PumpkinScenePartII.BallsWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Fera.PumpkinScenePartII.VaginaWord".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Fera.PumpkinScenePartII.CockWord".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartII.BareGroinWord".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.AsSheExplainsGrip".Localized());

            switch (variant)
            {
                case 0:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.ArmWord".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.PackageWord".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.SnatchWord".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartII.ErrorWord".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.RelaxesAndSheGoes".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.PertBustCatchesYour".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.InterruptsSlidingPalm".Localized());

            switch (variant)
            {
                case 0:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.YourGroinTeasingly".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.SensitiveUndersideCock".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Fera.PumpkinScenePartII.HerFreeHandWorking".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Fera.PumpkinScenePartII.YourOtherWord".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartII.AnotherWord".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartII.DickWithChildlikeGlee".Localized());
                }
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartII.SensitiveFoldsVagina".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Fera.PumpkinScenePartII.FreeHandWorking".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartII.ErrorWordAgain".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartII.NeverTooGoodThing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), () => PumpkinScenePartIIAccept(variant));

            AddButton(1, "Common.Button.Resist".Localized(), () => PumpkinScenePartIIResist(variant));
        }

        public void PumpkinScenePartIIAccept(int variant = 0)
        {
            ClearOutput();

            OutputText("Scenes.Fera.PumpkinScenePartIIAccept.PricklyVinesBoilOut".Localized(((!Player.IsNaked() && variant == 0) ? ", swiftly undressing you as they go" : ""), (variant == 1 ? "the only part left mobile being your poor, depleted dick. It swings freely, controlled as effortlessly by the tainted goddess as your viridian prison." : "no part of you left mobile.")));

            OutputText("Scenes.Fera.PumpkinScenePartIIAccept.CryOutTheySlide".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIIAccept.DimlyFeelRivulets".Localized((Player.SexOrientation > 50 ? "shafts smearing you with pre" : "tits smearing you with breastmilk")));

            if (variant == 1 || (variant == 0 && Player.HasCock() && (!Player.HasVagina() || Player.SexOrientation < 50)))
            {
                OutputText("Scenes.Fera.PumpkinScenePartIIAccept.PleasantNumbness".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIIAccept.FeelAPussySliding".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIIAccept.GigglesAtYourExpression".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIIAccept.HowLongItGoes".Localized());

                HideUpDown();

                Player.Orgasm("Dick");

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartIII(variant, 1));
            }
            else
                if (variant == 2 || (variant == 0 && Player.HasVagina()))
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.PleasantNumbnessAgain".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.FeelAPenisSliding".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.GigglesAtExpressionAgain".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.HowLongGoesAgain".Localized());

                    HideUpDown();

                    Player.Orgasm("Vaginal");

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartIII(variant, 2));
                }
                else
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.PleasantNumbnessThird".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.FeelPenisSlidingAgain".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.GigglesAtExpressionThird".Localized());

                    OutputText("Scenes.Fera.PumpkinScenePartIIAccept.HowLongGoesThird".Localized());

                    HideUpDown();

                    Player.Orgasm("Anal");

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => PumpkinScenePartIII(variant, 0));
                }
        }

        public void PumpkinScenePartIIResist(int variant = 0)
        {
            ClearOutput();

            int perkChoice = 0;

            if (variant == 1 || (variant == 0 && Player.HasCock() && (!Player.HasVagina() || Player.SexOrientation < 50)))
                perkChoice = 0;
            else
                if (variant == 2 || (variant == 0 && Player.HasVagina()))
                    perkChoice = 1;
                else
                    perkChoice = Player.SexOrientation > 50 ? 1 : 0;

            OutputText("Scenes.Fera.PumpkinScenePartIIResist.NeuronsFireLikeMad".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIIResist.SheSaysWithHint".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIIResist.MomentLaterYouFeel".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIIResist.SultryMaidenSaunters".Localized());

            switch (variant)
            {
                case 0:
                {
                if (Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIIResist.MayNotHaveProper".Localized());

                    if (perkChoice == 1)
                        OutputText("Scenes.Fera.PumpkinScenePartIIResist.WombInTheFuture".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartIIResist.CockInTheFuture".Localized());
                }
                else
                    if (perkChoice == 1)
                        OutputText("Scenes.Fera.PumpkinScenePartIIResist.SeemedFittingToEnhance".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartIIResist.SeemedFittingGiveYou".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIIResist.SeemedFittingGiveAgain".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIIResist.SeemedFittingEnhanceAgain".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartIIResist.ErrorWord".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIIResist.WalksOffIntoDarkness".Localized());

            if (perkChoice == 1)
            {
                OutputText("Scenes.Fera.PumpkinScenePartIIResist.PerkGainedFerasBoon".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonBreedingBitch, 0, 0, 0, 0);
            }
            else
            {
                OutputText("Scenes.Fera.PumpkinScenePartIIResist.PerkGainedFerasAgain".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonAlpha, 0, 0, 0, 0);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1)
                DoNext(() => PostFeraGhost());
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void PumpkinScenePartIII(int variant = 0, int sexType = 0)
        {
            ClearOutput();

            OutputText("Scenes.Fera.PumpkinScenePartIII.SlapCrackOfPalm".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIII.SultryMaidenWord".Localized());

            switch (sexType)
            {
                case 0:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIII.PatsSloshingMiddleAgain".Localized());
                }

                break;

                case 1:
                {
                if (Player.Balls > 0)
                    OutputText("Scenes.Fera.PumpkinScenePartIII.PatsYourSloshingSack".Localized());
                else
                    OutputText("Scenes.Fera.PumpkinScenePartIII.CaressesSensitiveSpot".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIII.PatsSloshingMiddleAgain".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartIII.ErrorWord".Localized());

            OutputText("Scenes.Fera.PumpkinScenePartIII.AsVinesUnwind".Localized());

            switch (variant)
            {
                case 0:
                {
                int perkChoice = 0;

                if (sexType == 0)
                    perkChoice = Player.SexOrientation > 50 ? 1 : 0;
                else
                    perkChoice = sexType - 1;

                if (sexType == 0)
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIII.WentAheadGaveYou".Localized());

                    if (perkChoice == 1)
                        OutputText("Scenes.Fera.PumpkinScenePartIII.WombInTheFuture".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartIII.CockInTheFuture".Localized());
                }
                else
                    if (perkChoice == 1)
                        OutputText("Scenes.Fera.PumpkinScenePartIII.ExplainsPlantingKiss".Localized());
                    else
                        OutputText("Scenes.Fera.PumpkinScenePartIII.ExplainsPlantingKissAgain".Localized());

                if (perkChoice == 1)
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIII.PerkGainedFerasBoon".Localized());

                    Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonBreedingBitch, 0, 0, 0, 0);
                }
                else
                {
                    OutputText("Scenes.Fera.PumpkinScenePartIII.PerkGainedFerasAgain".Localized());

                    Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonAlpha, 0, 0, 0, 0);
                }
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIII.ExplainsBeforeGivingYou".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIII.DisappearsAlongWithAll".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIII.PerkGainedFerasThird".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonAlpha, 0, 0, 0, 0);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.PumpkinScenePartIII.ExplainsBeforeGivingAgain".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIII.DisappearsAlongAllAgain".Localized());

                OutputText("Scenes.Fera.PumpkinScenePartIII.PerkGainedFerasFourth".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonBreedingBitch, 0, 0, 0, 0);
                }

                break;
            }

            if (true )
                OutputText("Scenes.Fera.PumpkinScenePartIII.ErrorWordAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 30f));

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1)
                DoNext(() => PostFeraGhost());
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void PostFeraGhost()
        {
            ClearOutput();

            OutputText("Scenes.Fera.PostFeraGhost.BonafidePredatorsChamp".Localized());

            OutputText("Scenes.Fera.PostFeraGhost.StopInYourTracks".Localized());

            OutputText("Scenes.Fera.PostFeraGhost.ShouldraExplains".Localized());

            OutputText("Scenes.Fera.PostFeraGhost.WellYesDidHappen".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Hunt4poon".Localized(), () => AnswerShouldra(0));

            AddButton(1, "Common.Button.Hunt4demons".Localized(), () => AnswerShouldra(1));

            AddButton(2, "Common.Button.Hunt4fera".Localized(), () => AnswerShouldra(2));

            AddButton(3, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), () => AnswerShouldra(3));
        }

        public void AnswerShouldra(int answer = 0)
        {

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Fera.AnswerShouldra.HuntYouDeclare".Localized());

                OutputText("Scenes.Fera.AnswerShouldra.YellsTheWraith".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Fera.AnswerShouldra.OtherConcernsForThe".Localized());

                OutputText("Scenes.Fera.AnswerShouldra.SighsTheWraithWhining".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.AnswerShouldra.YouAreNoPawn".Localized());

                OutputText("Scenes.Fera.AnswerShouldra.SeemsTakenAback".Localized((CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) < 2 ? "Don't blame me if I have to ditch ya before Fera targets me. Gods can easily destroy me with or without a body to anchor my spirit." : "I just... I don't want to lose you, [name].")));
                }

                break;

                case 3:
                {
                OutputText("Scenes.Fera.AnswerShouldra.NeverConsentedToBeing".Localized());
                }

                break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FeraSceneTwoIntroduction()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PumpkinSeedsEaten, 0);

            CoC.Instance.Flags.Set(GameFlagIds.FeraEncounterDate, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 2);

            OutputText("Scenes.Fera.FeraSceneTwoIntroduction.WhileWalkingThrough".Localized());

            OutputText("Scenes.Fera.FeraSceneTwoIntroduction.FlopBackIntoSoft".Localized());

            OutputText("Scenes.Fera.FeraSceneTwoIntroduction.HereSpotProof".Localized());

            OutputText("Scenes.Fera.FeraSceneTwoIntroduction.SingleNormalLookingBee".Localized());

            OutputText("Scenes.Fera.FeraSceneTwoIntroduction.CreepingShiverCrawls".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Explore".Localized(), ExploreFerasWonderland);

            if (Player.Level >= 15 || View.Time.Days >= 90)
                AddButton(14, "Common.Button.Leave".Localized(), () => FeraAkbalQuestIntro());
            else
                AddButton(14, "Common.Button.Leave".Localized(), LeaveFerasWonderland);
        }

        public void LeaveFerasWonderland()
        {
            ClearOutput();

            OutputText("Scenes.Fera.LeaveFerasWonderland.NahThatWholePlace".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ExploreFerasWonderland()
        {
            ClearOutput();

            AwardAchievement(AchievementIds.HolidayHalloweenII);

            OutputText("Scenes.Fera.ExploreFerasWonderland.StraighteningToFullHeight".Localized(
                Player.Mf("[hips] swaggering excitedly", "[hips] sashaying excitedly")
            ));

            OutputText("Scenes.Fera.ExploreFerasWonderland.IdlyWonderWhat".Localized());

            OutputText("Scenes.Fera.ExploreFerasWonderland.WondrousAromaGuides".Localized());

            OutputText("Scenes.Fera.ExploreFerasWonderland.StumbleAFewSteps".Localized());

            OutputText("Scenes.Fera.ExploreFerasWonderland.FlowerWouldStandNearly".Localized());

            OutputText("Scenes.Fera.ExploreFerasWonderland.PetalsScarletRed".Localized());

            OutputText("Scenes.Fera.ExploreFerasWonderland.GatherSomeOfNectar".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Fera.ExploreFerasWonderland.YourHeartIsHammering".Localized());

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Fera.ExploreFerasWonderland.TelltaleTightness".Localized());
            else
                if (Player.HasVagina() && !Player.HasCock())
                    OutputText("Scenes.Fera.ExploreFerasWonderland.TelltaleHeat".Localized());
                else
                    if (Player.HasVagina() && Player.HasCock())
                        OutputText("Scenes.Fera.ExploreFerasWonderland.TelltaleSticky".Localized());
                    else
                    {
                        OutputText("Scenes.Fera.ExploreFerasWonderland.WellThatWasNice".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 1f));

                        if (Player.Level >= 15 || View.Time.Days >= 90)
                            DoNext(() => FeraAkbalQuestIntro());
                        else
                            DoNext(() => LeaveAfterFlowerHuffing());

                        return ;
                    }

            OutputText("Scenes.Fera.ExploreFerasWonderland.BiteLipAndSearch".Localized());

            ClearButtons();

            if (Player.Intelligence >= 40)
            {
                OutputText("Scenes.Fera.ExploreFerasWonderland.WithThatAccomplished".Localized(((Player.Level >= 15 || View.Time.Days >= 90) ? ", but that may draw out whatever nefarious entity had tended this unholy garden." : ".")));

                if (Player.HasCock())
                    AddButton(0, "Common.Button.Continue".Localized(), FuckDemCorruptFlowerLips);
                else
                    AddButton(0, "Common.Button.Continue".Localized(), StickAStamenInIt);

                if (Player.Level >= 15 || View.Time.Days >= 90)
                    AddButton(14, "Common.Button.Leave".Localized(), () => FeraAkbalQuestIntro());
                else
                    AddButton(14, "Common.Button.Leave".Localized(), LeaveAfterFlowerHuffing);
            }
            else
            {
                OutputText("Scenes.Fera.ExploreFerasWonderland.ThatThoughtSlipsOut".Localized());

                if (Player.HasCock())
                    AddButton(0, "Common.Button.Next".Localized(), FuckDemCorruptFlowerLips);
                else
                    AddButton(0, "Common.Button.Next".Localized(), StickAStamenInIt);
            }

            if (Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Fera.ExploreFerasWonderland.TheOnlyWord".Localized());

                if (Player.Intelligence >= 40)
                    OutputText("Scenes.Fera.ExploreFerasWonderland.OtherWord".Localized());

                OutputText("Scenes.Fera.ExploreFerasWonderland.QuestionLeftIsAre".Localized());

                AddButton(0, "Common.Button.Lips".Localized(), FuckDemCorruptFlowerLips);

                AddButton(1, "Common.Button.Stamen".Localized(), StickAStamenInIt);
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 1f));
        }

        public void LeaveAfterFlowerHuffing()
        {
            ClearOutput();

            OutputText("Scenes.Fera.LeaveAfterFlowerHuffing.TheFartherYouGet".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckDemCorruptFlowerLips()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FerasTrapSprung, 1);

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.DazedlyStaggerForward".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.SlightStumbleWork".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.DragYourselfUp".Localized());

            if (Player.BiggestCockLength() >= 20)
            {
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.EasilyRammingEndowment".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.PluralS".Localized());

                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.StraightIntoMouth".Localized());
            }
            else
                if (Player.BiggestCockLength() >= 8)
                {
                    OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.SlippingLargeEndowments".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.PluralSAgain".Localized());

                    OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.RightIntoMouth".Localized());
                }
                else
                    if (Player.BiggestCockLength() >= 4)
                    {
                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.EasingTheTopHal".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.VesSuffix".Localized());
                        else
                            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.FLetter".Localized());

                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.OfYourPhallus".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.EsSuffix".Localized());

                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.IntoMouth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.BarelyManagingTip".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.PluralSThird".Localized());

                        OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.IntoMouthAgain".Localized());
                    }

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.ContactIsEverything".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.PluralSFourth".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.TwitchInExcitement".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.BindingsAroundArms".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.CushyLowerLip".Localized());
            else
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.LoinsAgainstCushy".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.FastDiminishingLight".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.BindingsArentTight".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.AtThisPointThe".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.MotionsSlowAmbrosial".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.DelugeOfDrugs".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.AreWord".Localized());
            else
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.IsWord".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.BuriedInHotWet".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.PerfectlySmoothHand".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.ItsFeraGoddess".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.MindHasAllAbility".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.FeraRubsYour".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.InnerThighs".Localized());
            else
                OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.BallsWord".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.SqueezingEncouragingly".Localized());

            OutputText("Scenes.Fera.FuckDemCorruptFlowerLips.NotEnoughGuidance".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DickBoostingFuntimes);
        }

        public void DickBoostingFuntimes()
        {
            ClearOutput();

            OutputText("Scenes.Fera.DickBoostingFuntimes.HangThereSpreadEagled".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.GoddessCommands".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.PlumpFlowerLips".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Fera.DickBoostingFuntimes.PluralS".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.JustTimeToTake".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Fera.DickBoostingFuntimes.HeadsWord".Localized());
            else
                OutputText("Scenes.Fera.DickBoostingFuntimes.CockHeadWord".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.SwellUpALittle".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Fera.DickBoostingFuntimes.PluralSAgain".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.BiggerHarderFatter".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Fera.DickBoostingFuntimes.ItWord".Localized());
            else
                OutputText("Scenes.Fera.DickBoostingFuntimes.TheyWord".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.ShouldBe".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.UnseenButFelt".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.DickBoostingFuntimes.EmptyWord".Localized());
            else
                OutputText("Scenes.Fera.DickBoostingFuntimes.EmptiesWord".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.SoYouCanStart".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.DickBoostingFuntimes.CupsYourSack".Localized());
            else
                OutputText("Scenes.Fera.DickBoostingFuntimes.TicklesYourTaint".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.SpurYouToThrust".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.TameThatFuckHole".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.OnCommandPoundAway".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.PinchesYourAndInstructs".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.SqueezesHarder".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.CooesUnloadYour".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.SputterAndSpurt".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Fera.DickBoostingFuntimes.BallsWord".Localized());
            else
                OutputText("Scenes.Fera.DickBoostingFuntimes.TaintWord".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.AsYouDoWhispering".Localized());

            OutputText("Scenes.Fera.DickBoostingFuntimes.ShudderAsClimaxStarts".Localized());

            var x = 0;

            while (x < Player.Cocks.Count)
            {
                Player.Cocks[x].Length += 1;

                if (Player.Cocks[x].Thickness < Player.Cocks[x].Length / 5)
                    Player.Cocks[x].Thickness = Player.Cocks[x].Length / 5;

                x += 1;
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 10f), new DynStat(GameStats.Corruption, 5f));

            Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonSeeder, 0, 0, 0, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PostSeederUpgrade);
        }

        public void PostSeederUpgrade()
        {
            ClearOutput();

            OutputText("Scenes.Fera.PostSeederUpgrade.AwakenFullyDressed".Localized());

            OutputText("Scenes.Fera.PostSeederUpgrade.PerkGainedFerasBoon".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(4));
        }

        public void StickAStamenInIt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FerasTrapSprung, 1);

            OutputText("Scenes.Fera.StickAStamenInIt.DazedlyStaggerOneAfter".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.StumblingBeginToWriggle".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Fera.StickAStamenInIt.TailWord".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Fera.StickAStamenInIt.GooWord".Localized());
                else
                    OutputText("Scenes.Fera.StickAStamenInIt.KneesWord".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.DragYourselfUpUntil".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.StamenReactToYou".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.AtSameTimePetals".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.StrugglesCease".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Fera.StickAStamenInIt.BobbingWord".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.ArentHopingTheyll".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.DelugeOfDrugs".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.PerfectlySmoothHand".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.ItsFeraGoddess".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.MindHasAllAbility".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.RubsYourTweaking".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.NotEnoughGuidance".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.AddsSheBrushesClit".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Fera.StickAStamenInIt.MusesWithWideGrin".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.PullsHerFingerOut".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Fera.StickAStamenInIt.OrWord".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.TightDickMilkingMother".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Fera.StickAStamenInIt.LibidinousDickGirl".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.QuestionSayEnd".Localized());

            OutputText("Scenes.Fera.StickAStamenInIt.QuestionAskedSheAwaits".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)), new DynStat(GameStats.Corruption, 2f));

            ClearButtons();

            AddButton(0, "Common.Button.Gape".Localized(), GapeMePleaseFera);

            AddButton(1, "Common.Button.Tight".Localized(), TightLikeAToigar);

            if (!Player.HasCock())
                AddButton(2, "Common.Button.Dick".Localized(), GrowADickForFera);
            else
                AddButton(2, "Common.Button.Dick".Localized(), null);
        }

        public void GapeMePleaseFera()
        {
            ClearOutput();

            OutputText("Scenes.Fera.GapeMePleaseFera.SmilesAsYouAnnounce".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.HearSquelchingSlithering".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.MassOfTendrils".Localized());

            if (Player.Vaginas[0].Looseness >= 5)
                OutputText("Scenes.Fera.GapeMePleaseFera.SpreadingWellWidened".Localized());
            else
                if (Player.Vaginas[0].Looseness >= 3)
                    OutputText("Scenes.Fera.GapeMePleaseFera.SpreadingYourLips".Localized());
                else
                    if (Player.Vaginas[0].Looseness >= 2)
                        OutputText("Scenes.Fera.GapeMePleaseFera.TokenResistance".Localized());
                    else
                        OutputText("Scenes.Fera.GapeMePleaseFera.MoreLittleResistant".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.DoNotFeelOunce".Localized());

            Player.CuntChange(90000, true, true, false);

            OutputText("Scenes.Fera.GapeMePleaseFera.VoiceWhispersWithA".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.ExhalingYourAnswer".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.SapOozingTendrils".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Human)
                OutputText("Scenes.Fera.GapeMePleaseFera.ToeWord".Localized());
            else
                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                    OutputText("Scenes.Fera.GapeMePleaseFera.TailWord".Localized());
                else
                    OutputText("Scenes.Fera.GapeMePleaseFera.FootWord".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.ClimaxArchingYourBack".Localized());

            if (Player.Wetness() < 4)
                OutputText("Scenes.Fera.GapeMePleaseFera.EvenThoughNotSquirter".Localized());

            OutputText("Scenes.Fera.GapeMePleaseFera.PumpOnUnrelenting".Localized());

            Player.CuntChange(90000, true, true, false);

            OutputText("Scenes.Fera.GapeMePleaseFera.ComeDownFeraResumes".Localized());

            Player.CuntChange(90000, true, true, false);

            OutputText("Scenes.Fera.GapeMePleaseFera.PantThoughYouDont".Localized());

            Player.CuntChange(90000, true, true, false);

            Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonWideOpen, 0, 0, 0, 0);

            Player.Fertility += 20;

            if (Player.Fertility > 50)
                Player.Fertility = 50;

            if (Player.Vaginas[0].Wetness < 3)
                Player.Vaginas[0].Wetness = 3;

            if (Player.Vaginas[0].Looseness < 4)
                Player.Vaginas[0].Looseness = 4;

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GapeByFeraEpilogue);
        }

        public void GapeByFeraEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Fera.GapeByFeraEpilogue.AwakenFullyDressed".Localized());

            if (Player.Pregnancy.Incubation == 0)
                OutputText("Scenes.Fera.GapeByFeraEpilogue.ButYoudRatherFuck".Localized());
            else
                OutputText("Scenes.Fera.GapeByFeraEpilogue.SentencePeriod".Localized());

            OutputText("Scenes.Fera.GapeByFeraEpilogue.IdlyReachIntoYour".Localized());

            OutputText("Scenes.Fera.GapeByFeraEpilogue.HeadBackToCamp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(4));
        }

        public void TightLikeAToigar()
        {
            ClearOutput();

            OutputText("Scenes.Fera.TightLikeAToigar.SmilesShePetsForehead".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.OneOfTheTentacles".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.InkySpotsExpand".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.DontMindTheTone".Localized());

            if (Player.Body.Get<SkinPart>().Adj == "latex" || Player.Body.Get<SkinPart>().Adj == "rubber")
                OutputText("Scenes.Fera.TightLikeAToigar.NotThatAHoney".Localized(
                    Player.Body.Get<SkinPart>().Adj
                ));

            OutputText("Scenes.Fera.TightLikeAToigar.ChangeHowQuickly".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.BreakingAwaySheSmiles".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.SableShaftDisappears".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.CoosDropsDown".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.GroanInUnrestrainedLust".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.TouchingPliantVulva".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.SmilesTentacleFinally".Localized());

            if (Player.Vaginas[0].Looseness >= 5)
                OutputText("Scenes.Fera.TightLikeAToigar.SlippingPastEntrance".Localized());
            else
                if (Player.Vaginas[0].Looseness >= 3)
                    OutputText("Scenes.Fera.TightLikeAToigar.SlidingIntoEntrance".Localized());
                else
                    if (Player.Vaginas[0].Looseness >= 2)
                        OutputText("Scenes.Fera.TightLikeAToigar.SlowlyWorkingItsWay".Localized());
                    else
                        OutputText("Scenes.Fera.TightLikeAToigar.BurrowingPastRestrictive".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.LeavingTrailOfSlick".Localized());

            Player.CuntChange(1, true, true, false);

            OutputText("Scenes.Fera.TightLikeAToigar.FingersReachDown".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.YouCumYouCum".Localized());

            OutputText("Scenes.Fera.TightLikeAToigar.NoChoiceButTo".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 20f), new DynStat(GameStats.Corruption, 3f));

            Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonMilkingTwat, 0, 0, 0, 0);

            if (Player.Vaginas[0].Looseness > 2)
                Player.Vaginas[0].Looseness = 2;

            Player.Fertility += 10;

            if (Player.Fertility >= 50)
                Player.Fertility = 50;

            if (Player.Vaginas[0].Wetness < 3)
                Player.Vaginas[0].Wetness = 3;

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TightnessEpilogue);
        }

        public void TightnessEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Fera.TightnessEpilogue.AwakenFullyDressed".Localized());

            OutputText("Scenes.Fera.TightnessEpilogue.HeadBackToCamp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(4));
        }

        public void GrowADickForFera()
        {
            ClearOutput();

            OutputText("Scenes.Fera.GrowADickForFera.LusciousLipsTwistUp".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.JuicySchluckSounds".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.CrouchesDownNextTo".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.AsFeraTrailsOff".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Fera.GrowADickForFera.PressYourTailTo".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.FeelTheWetness".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.WholeBodyShudders".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.PullingBackTentacleCup".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.StalkAboveCrotch".Localized());

            OutputText("Scenes.Fera.GrowADickForFera.CryingWithSublime".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 10f), new DynStat(GameStats.Sensitivity, 10f), new DynStat(GameStats.Corruption, 3f));

            Player.CreateCock();

            Player.Cocks[0].Length = 10;

            Player.Cocks[0].Thickness = 2.5f;

            if (Random.Shared.Next(2) == 0)
                Player.Cocks[0].Id = new Identifier("coc", "tentacle");

            Player.CreatePerkIfNotHasPerk(PerkIds.FerasBoonSeeder, 0, 0, 0, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), AfterFeraGivesYouACawk);
        }

        public void AfterFeraGivesYouACawk()
        {
            ClearOutput();

            OutputText("Scenes.Fera.AfterFeraGivesYouACawk.AwakenFullyDressed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(4));
        }

        public void FeraAkbalQuestIntro(bool fromGlade = true)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PumpkinSeedsEaten, 0);

            CoC.Instance.Flags.Set(GameFlagIds.FeraEncounterDate, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.FeraReleased, 3);

            if (fromGlade)
                OutputText("Scenes.Fera.FeraAkbalQuestIntro.TurnAroundHeadOut".Localized());
            else
                OutputText("Scenes.Fera.FeraAkbalQuestIntro.WhileWalkingThrough".Localized());

            OutputText("Scenes.Fera.FeraAkbalQuestIntro.ShadowPassesTheEdge".Localized());

            OutputText("Scenes.Fera.FeraAkbalQuestIntro.TurnForwardYetAgain".Localized());

            OutputText("Scenes.Fera.FeraAkbalQuestIntro.WasJustHopingYoud".Localized((CoC.Instance.Flags.Get(GameFlagIds.FerasTrapSprung) > 0 ? " again" : "")));

            OutputText("Scenes.Fera.FeraAkbalQuestIntro.EffortsToStayFocused".Localized((CoC.Instance.Flags.Get(GameFlagIds.MetMaraeCorrupted) > 0 ? "was" : "is")));

            OutputText("Scenes.Fera.FeraAkbalQuestIntro.FamiliarWithA".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => FeraAkbalGetQuest(0));

            AddButton(1, "Common.Button.No".Localized(), () => FeraAkbalGetQuest(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) == 0 && CoC.Instance.Flags.Get(GameFlagIds.PlayerResistedAkbal) > 0)
                AddButton(2, "Common.Button.Mock".Localized(), () => FeraAkbalGetQuest(2));
        }

        public void FeraAkbalGetQuest(int response = 1)
        {
            ClearOutput();

            switch (response)
            {
                case 0:
                {
                OutputText("Scenes.Fera.FeraAkbalGetQuest.IndeedYouDoKnow".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 0)
                    OutputText("Scenes.Fera.FeraAkbalGetQuest.ThatSaidDidntStop".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Fera.FeraAkbalGetQuest.AkbalSheSaysYes".Localized());

                OutputText("Scenes.Fera.FeraAkbalGetQuest.BurstsWithLaughter".Localized());
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Fera.FeraAkbalGetQuest.CantSayYoure".Localized());

                OutputText("Scenes.Fera.FeraAkbalGetQuest.GodOfTerrestrialFire".Localized());
            }

            OutputText("Scenes.Fera.FeraAkbalGetQuest.WhyBringUpAkbal".Localized());

            OutputText("Scenes.Fera.FeraAkbalGetQuest.GoddessShiftsPosition".Localized());

            OutputText("Scenes.Fera.FeraAkbalGetQuest.SoThenThere".Localized());

            OutputText("Scenes.Fera.FeraAkbalGetQuest.ClarifiesHerSmug".Localized());

            OutputText("Scenes.Fera.FeraAkbalGetQuest.GoddessStrutsOn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, 1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}