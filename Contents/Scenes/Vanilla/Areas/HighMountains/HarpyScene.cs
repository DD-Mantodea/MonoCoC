using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<HarpyScene>("coc:harpy")]
    public class HarpyScene : Scene
    {

        public override void Entry()
        {

        }

        public void HarpyVictoryuuuuu()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Harpyscene.HarpyVictoryuuuuu.ScreamsOutInOne".Localized());
            else
                OutputText("Scenes.Harpyscene.HarpyVictoryuuuuu.CantContainHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Anal".Localized(), null);

            AddButton(1, "Common.Button.Pussy".Localized(), null);

            AddButton(2, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), null);

            AddButton(3, "Common.Button.Scissor".Localized(), null);

            AddButton(4, "Common.Button.ClitFuck".Localized(), null);

            AddButton(5, "Common.Button.LayEggs".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Harpyscene.HarpyVictoryuuuuu.WhatDoToHer".Localized());

                if (Player.HasCock())
                {
                    if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                        AddButton(0, "Common.Button.Anal".Localized(), WinAndRapeHarpyAnally);

                    if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                        AddButton(1, "Common.Button.Pussy".Localized(), VictoryHarpyGetsHerPussyRaped);
                }

                AddButton(2, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), WinOnHarpyAndOralRape);

                if (Player.HasVagina())
                {
                    if (!Player.HasTailInsteadOfLegs())
                        AddButton(3, "Common.Button.Scissor".Localized(), HarpyScissorSurprise);

                    if (Player.GetClitLength() >= 3.5f)
                        AddButton(4, "Common.Button.ClitFuck".Localized(), ClitFuckAHarpy);
                }
            }

            if (Player.CanOvipositSpider() && (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs || Player.Body.Get<FacePart>().Id == BodyPartTypes.SpiderFangs))
                AddButton(5, "Common.Button.LayEggs".Localized(), SpoidahsLegEggsInHarpeis);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void HarpyLossU()
        {

            float x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (Player.Gender == Gender.Genderless && Player.IsGoo())
            {
                HarpyGooGenderlessLoss();

                CombatScene.CleanupAfterCombat();

                return ;
            }

            if (x >= 0 || Player.HasVagina())
            {
                if (Player.Lust >= Player.MaxLust)
                    HarpyLossLust();
                else
                    HarpyDamageLoss();
            }
            else
            {
                OutputText("Scenes.Harpyscene.HarpyLossU.ThoughDefeatedBy".Localized());

                CombatScene.CleanupAfterCombat();
            }
        }

        public void HarpyGooGenderlessLoss()
        {
            ClearOutput();

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.TriumphantLooksDown".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.AsksPenis".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.TellHerNo".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.AsksPussy".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.NopeWord".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.OhDisappointed".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.GrabsHandfulOfYou".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.TwoMoreHarpiesFlap".Localized());

            OutputText("Scenes.Harpyscene.HarpyGooGenderlessLoss.SmallestWalks".Localized());
        }

        public void HarpyLossLust()
        {
            ClearOutput();

            int x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            OutputText("Scenes.Harpyscene.HarpyLossLust.CollapseInBrokenHeap".Localized());

            OutputText("Scenes.Harpyscene.HarpyLossLust.PsychoticCry".Localized());

            OutputText("Scenes.Harpyscene.HarpyLossLust.NotJustHerClaws".Localized(
                 Appearance.AllBreastsDescript(Player)
            ));

            if (Player.Body.Get<HairPart>().Length > 12)
                OutputText("Scenes.Harpyscene.HarpyLossLust.MovesOutOfYour".Localized());

            OutputText("Scenes.Harpyscene.HarpyLossLust.WhenSheSmirks".Localized());

            if (Player.HasVagina() && x < 0)
            {
                OutputText("Scenes.Harpyscene.HarpyLossLust.GrabbingByAnkles".Localized());

                if (Player.HasPlainSkin())
                    OutputText("Scenes.Harpyscene.HarpyLossLust.ShortlyAfterRepeats".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.WithinMomentsThree".Localized());

                if (Player.HasPlainSkin())
                    OutputText("Scenes.Harpyscene.HarpyLossLust.AlleviatingPain".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.InstantlyStopStruggling".Localized());

                if (Player.HasPlainSkin())
                    OutputText("Scenes.Harpyscene.HarpyLossLust.AlwaysFollowedSoothing".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.LineTheCurve".Localized());

                ShowImage("harpy-loss-female");

                OutputText("Scenes.Harpyscene.HarpyLossLust.DifferentFeeling".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.HasPlainSkin())
                    OutputText("Scenes.Harpyscene.HarpyLossLust.AcrossReddeningSkin".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.LittleBlackTongue".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.BeingLiftedUp".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.DoesntTakeLong".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.RecoverFewHours".Localized());
            }
            else
            {
                OutputText("Scenes.Harpyscene.HarpyLossLust.FlapTheirWings".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.DropYouOnPurpose".Localized(
                     Player.CockDescript(x)
                ));

                ShowImage("harpy-loss-male");

                OutputText("Scenes.Harpyscene.HarpyLossLust.LittleSlitBit".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Harpyscene.HarpyLossLust.FeelHerLittleTongue".Localized());
                else
                    OutputText("Scenes.Harpyscene.HarpyLossLust.FeelLittleTongueAgain".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.WheneverOneOnTop".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Harpyscene.HarpyLossLust.JustAboutToClimax".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.GroundRapidlyApproaches".Localized());

                OutputText("Scenes.Harpyscene.HarpyLossLust.LastConsciousMoment".Localized());
            }

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));
        }

        public void HarpyDamageLoss()
        {
            ClearOutput();

            int x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            var y = x + 1;

            if (y >= Player.Cocks.Count)
                y -= 2;

            if (y == x && Player.Cocks.Count > 1)
                if (y == 0)
                    y += 1;
                else
                    y -= 1;

            OutputText("Scenes.Harpyscene.HarpyDamageLoss.CollapseInBrokenHeap".Localized());

            OutputText("Scenes.Harpyscene.HarpyDamageLoss.PsychoticCry".Localized());

            if (Player.HasCock() && x >= 0)
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.CrazedBrutalHands".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.EachOfWord".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.FloppingSideToSide".Localized());

                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.HierarchyEventuallySettled".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.CraningNeckToSlurp".Localized());

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.ThirdHarpyLeaps".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.PetiteBosomGrinding".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.GoldenLipsGlinting".Localized());
                }
                else
                {
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.HierarchySettledBetween".Localized(Player.CockDescript(x), (noFur ? "" : ", feathered"), Player.VaginaDescript(0)));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.HandsMovingUpGrope".Localized());

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.TinyTinglesLeft".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.BounceAndSway".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.SlowlyLeansDown".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.TonedChestPertBreasts".Localized());

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.PeriodBreak".Localized());
                }

                Npcs.SophieLuststick.Apply(8);
            }
            else
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.HierarchyDeterminedTwo".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.BiggestBreastSize() >= 3)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.JiggleSlapping".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.LeansSlowlyOver".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.PetiteChestSquashing".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.PeriodBreakAgain".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.LipsPress".Localized());

                ShowImage("harpy-loss-female");

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.SuccumbPowerful".Localized());
            }

            if (x >= 0 && Player.CockTotal() == 1)
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.AnotherPowerfulSensation".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.HugeBottomRest".Localized((noFur ? "" : " feathery")));

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.ComeToBear".Localized((noFur ? "" : " feathery")));

                if (Player.Balls >= 0)
                {
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.BounceWithSuchForce".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.SwingingBackSpanking".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.SentencePeriod".Localized());
                }
                else
                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.ShakingAndJiggling".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.SentencePeriodAgain".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.AnyonesGuessHow".Localized());
            }
            else
                if (x >= 0 && Player.CockTotal() > 1)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.SquabblingHasntGone".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(y),

                         Player.CockDescript(y),

                         Player.CockDescript(x)
                    ));
                else
                {
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.ImmenseAssIs".Localized((noFur ? "" : " feathered")));

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.HungryFeminineMouths".Localized());

                    if (Player.BiggestLactation() >= 1)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.SubtleSlurps".Localized());

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.HandsFindTheirWay".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Harpyscene.HarpyDamageLoss.BellySquishingYours".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.DrugLacedMouth".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));
                }

            if (x >= 0 && Player.HasCock())
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.RideYouBrutally".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.NumberOfTimes".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.EveryPartScreams".Localized());
            }
            else
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.FingersAndFirm".Localized());

                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.BelliesPudgier".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.AtLastAllThree".Localized());
            }

            if (x >= 0 && Player.HasCock())
            {
                OutputText("Scenes.Harpyscene.HarpyDamageLoss.AfterCountlessHours".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Harpyscene.HarpyDamageLoss.EachOfWordAgain".Localized());

                OutputText("Scenes.Harpyscene.HarpyDamageLoss.ShakesAndQuivers".Localized());
            }

            OutputText("Scenes.Harpyscene.HarpyDamageLoss.AfterFewHours".Localized());

            Player.ChangeFatigue(20);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 2f));
        }

        public void VictoryHarpyGetsHerPussyRaped()
        {
            ClearOutput();

            int x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            var y = x + 1;

            if (y >= Player.Cocks.Count)
                y -= 2;

            if (y == x && Player.Cocks.Count > 1)
                if (y == 0)
                    y += 1;
                else
                    y -= 1;

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.StepOverSubmissive".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.WrappingArmsImmense".Localized());

            ShowImage("harpy-win-male");

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.SumptuousEnormous".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.HeadCreepsPast".Localized(
                     Player.CockDescript(y),

                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.SlowlySlipInto".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.ViciouslyClenchingAss".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.PulseThroughHer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.EventuallyBottomOut".Localized());
            else
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.LeaningLowOverBack".Localized(
                     Player.CockDescript(x)
                ));

            if (Player.Gender == Gender.Male)
            {
                if (Player.BiggestBreastSize() < 1)
                {
                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.TakingCareNotWaste".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.PluralS".Localized());

                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.AlongWord".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.TheirWord".Localized());
                    else
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.TheWord".Localized());

                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.WholeLengthClose".Localized());
                }
                else
                {
                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.LaughInGlee".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.PossessiveMassage".Localized());
                    else
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.MassagesWord".Localized());

                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.YourCockWord".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.AndCockWord".Localized(
                             Player.CockDescript(y)
                        ));

                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.WhileKneadOver".Localized(
                         Appearance.AllBreastsDescript(Player),

                         Appearance.AllBreastsDescript(Player)
                    ));
                }
            }
            else
                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.GrinningMassaging".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.ExposedGirlIs".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.DecideGiveHerWhat".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.AndCockWordAgain".Localized(
                     Player.CockDescript(y)
                ));

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.IntoGrindingHole".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.PluralSAgain".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.PeriodSpace".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.SlapsWetlyImmense".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.ScreamsThrustsFat".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.SeemsLikeHours".Localized());

            OutputText("Scenes.Harpyscene.VictoryHarpyGetsHerPussyRaped.OnceFlowSubsides".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void WinAndRapeHarpyAnally()
        {
            ClearOutput();

            int x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            var y = x + 1;

            if (y >= Player.Cocks.Count)
                y -= 2;

            if (y == x && Player.Cocks.Count > 1)
                if (y == 0)
                    y += 1;
                else
                    y -= 1;

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.GrinningStrippingFree".Localized((noFur ? "" : "those marvelous, tickly feathers coating ")));

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.PetiteBustSquishes".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ParagraphBreak".Localized());

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.NotWishingToKeep".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            ShowImage("harpy-win-male");

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.BirdGirlAttempts".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ComeToRestAgainst".Localized());

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.HarpyGirlFreeze".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.BiggestBreastSize() < 1 && !Player.HasVagina())
                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.PupilsConstrictFeathery".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));
            else
            {
                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.PupilsConstrictFeatheryAgain".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.MotionsCausingBounce".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.KeepFirmGrip".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.StrugglesDrivingWild".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.MakesYourSlap".Localized(
                             Player.VaginaDescript(0)
                        ));
                }

                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ParagraphBreakThird".Localized());
            }

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.EarSplittingSquawk".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ChestMashingAgainst".Localized());

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.SubmittingToWill".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.ThrowingHeadBack".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Harpyscene.WinAndRapeHarpyAnally.StillLeakingSeed".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void WinOnHarpyAndOralRape()
        {
            ClearOutput();

            float x = -1;

            if (Player.HasCock())
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            var y = x + 1;

            if ((y == x && Player.Cocks.Count > 1) && (y >= Player.Cocks.Count))
                    y -= 2;

            OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.StandThreateningly".Localized());

            if (Player.HasCock())
                ShowImage("harpy-win-male-oral");

            OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.GrinningLewdlySqueeze".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.FluidsSeepingPlush".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.HasCock())
            {
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.CommaSpace".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.AndWord".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.EachOfWord".Localized());

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.JumpsToAttention".Localized());
            }

            OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.GraspingOntoHead".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.PressesHerToned".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.PressingAgainst".Localized());

            OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.ParagraphBreak".Localized());

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.BreakingTheKiss".Localized(
                     Player.CockDescript(0)
                ));

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.ExpertFingersTease".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.HandsCreepDown".Localized());

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.ParagraphBreakAgain".Localized());
            }
            else
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.BreakTheKiss".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                    {
                        OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.JiggleSlightly".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                        if (Player.BiggestLactation() >= 1)
                            OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.MilkDribblesOut".Localized());
                    }
                    else
                        OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.SentencePeriod".Localized());

                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.SurgingDownBody".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.MovesThatHand".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.OtherPerfectHand".Localized(
                             Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.ParagraphBreakThird".Localized());
                }

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.TearingLipsAway".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.GraspOntoKneading".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.PartsYourThighs".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));
            }

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.IsntFinishedYet".Localized(
                     Player.CockDescript(1)
                ));

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.SlipsOneHand".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.OtherHandDoing".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.GreedilyGobbles".Localized(
                     Player.CockDescript(0),

                     Player.CockDescript(0)
                ));

                if (Player.Balls > 0 && Player.Cocks[0].Length <= 16)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.DeepThroatsYou".Localized());

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.SluttyTrillsSquawks".Localized());
            }

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.ApproachChangesQuickly".Localized((noFur ? "" : " finely-feathered"), Player.VaginaDescript(0), (noFur ? "" : ", feathered")));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.LustyContinuesSuck".Localized(
                     Player.CockDescript(1)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.DribblingAllOver".Localized());

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.CumOverAndOver".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.FingersNeverStop".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Harpyscene.WinOnHarpyAndOralRape.OneLastTime".Localized((noFur ? "" : ", feathered")));

                Npcs.SophieLuststick.Apply(8);
            }

            CombatController.Instance!.Context.Enemy.Lust = 98;

            CombatController.Instance!.Context.Enemy.Health = 2;

            Player.Lust = Player.MaxLust;

            CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));
        }

        public void HarpyScissorSurprise()
        {
            ClearOutput();

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.StareDownDefeated".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.ArmsTrembleEffort".Localized());
            else
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.LeansBackKneeling".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.NoTimeToWaste".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.VulnerableWord".Localized());
            else
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.LustMadWord".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.HarpyWord".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.SlidingOneHand".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.LooksUpEyes".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.WorryWord".Localized());
            else
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.DesireWord".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.BeforeSheCanBegin".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.EyesWideWithFear".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.LickCommand".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.BarelyHoldYour".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.CloseEyesGettingLost".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.FinallyBringsMouth".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Harpyscene.HarpyScissorSurprise.AgainWord".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprise.FingersFirmlyStuffed".Localized());

            DoNext(() => HarpyScissorSurprisePtII());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
        }

        public void HarpyScissorSurprisePtII()
        {
            ClearOutput();

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.PlantingSquarelyChest".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.GrinningShift".Localized());

            ShowImage("harpy-win-female");

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.SensitiveSnatchQuivering".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.FullyAwareThreat".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.ClitsGrindTogether".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.AccommodatingLittleSlut".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.EyesShootOpen".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.FewMinutesGrinding".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.ThrashingSendsOver".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.BodyTooDrained".Localized());

            OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.SmileToYourself".Localized());

            Player.Orgasm("Vaginal");

            if (Player.CanDepositEel())
            {
                OutputText("Scenes.Harpyscene.HarpyScissorSurprisePtII.EelParasiteSlipped".Localized());

                Player.DepositEel();
            }

            CombatScene.CleanupAfterCombat();
        }

        public void SpoidahsLegEggsInHarpeis()
        {
            ClearOutput();

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.PerchedAboveDowned".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.WatchGoldenLips".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.EggsWord".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.BrainStringsAll".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.DrawingWebbingInto".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.FeatherCladQuiets".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.ManagesToStandUp".Localized());

            if (Player.Tallness > 78)
                OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.UpWord".Localized());
            else
                if (Player.Tallness == 78)
                    OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.ForwardWord".Localized());
                else
                    OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.DownWord".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.EyesClosedLipsPuckered".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.EyesSnapOpenPanic".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.StifledMoans".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.ReleasingAsPullFree".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.LeaningBackSpiderHalf".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.EachCockWord".Localized());

                if (Player.HasVagina() && Player.HasCock())
                    OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.AndWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.YourVaginaWord".Localized());

                OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.EnjoyingTheView".Localized());
            }

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.FeatheredSlutTakes".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.HaveToAdmitSight".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.CasuallyShiftPair".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.CalmlyRemindHer".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.MouthWorksOpen".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.FaceGlowingHeat".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.PleaseFillMe".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.SmilingSlideIn".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.CrossEyedEcstasy".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.BreathCatchesThroat".Localized());

            OutputText("Scenes.Harpyscene.SpoidahsLegEggsInHarpeis.RelievedOfEggs".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void ClitFuckAHarpy()
        {
            ClearOutput();

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.EyeingHaplessBird".Localized(
                 Player.VaginaDescript(0)
            ));

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.MoanMasturbateDipping".Localized());
            else
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.MoanStruggleRise".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.WrenchThickThighs".Localized((noFur ? "" : " Her lips are shrouded in a downy, feathery fuzz, but you can easily make out the heavy, lust-glossed labia.")));

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.StopsStrugglesWhen".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.DropTalonedToes".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.DruggedMakeUp".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.SmilingMischievouslyDares".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.RollCheekyChick".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.SinkingFingersFleshy".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.PullOutRelease".Localized());

            ShowImage("harpy-win-female");

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.LungingForwardGrab".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.DeepInBirdPussy".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.RunnelsOfLubricant".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Harpyscene.ClitFuckAHarpy.StreamsOfLubricant".Localized());
                else
                    OutputText("Scenes.Harpyscene.ClitFuckAHarpy.SquirtsOfLubricant".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.EvidenceJustHowHungry".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.PrimarilyFocusedOn".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.FreshGushFluid".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Harpyscene.ClitFuckAHarpy.FreshSpoutGirlspunk".Localized());
                else
                    OutputText("Scenes.Harpyscene.ClitFuckAHarpy.TorrentialSplashGirlspunk".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.TimedFullBodySeizure".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.SmilingDrunkenlyManages".Localized((noFur ? "Her feminine juices coat" : "The sodden feathers that ring her snatch stick to")));

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.AsksRollingBack".Localized());

            OutputText("Scenes.Harpyscene.ClitFuckAHarpy.BirdBrainedBimbo".Localized());

            if (Player.Toughness / 20 + Random.Shared.Next(20) < 6)
            {
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.EnduranceGivesOut".Localized());

                CombatController.Instance!.Context.Enemy.Lust = 98;

                CombatController.Instance!.Context.Enemy.Health = 2;

                Player.Lust = Player.MaxLust;

                CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

                CombatScene.CleanupAfterCombat();

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -1f));
            }
            else
            {
                OutputText("Scenes.Harpyscene.ClitFuckAHarpy.EyesClose".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                CombatScene.CleanupAfterCombat();
            }
        }

    }
}