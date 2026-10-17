using MonoCoC.Attributes;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<AnemoneScene>("coc:anemone")]
    public class AnemoneScene : Scene, ITimeAware
    {

        public bool TimeChange()
        {
            var needNext = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) == 0
                    && CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) >= 5

                    && View.Time.Hours > 10

                    && View.Time.Hours < 18

                    && Random.Shared.Next(4) == 0)
                {
                    KidABabysitsCows();

                    needNext = true;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) == 1
                    && View.Time.Hours > 10

                    && View.Time.Hours < 18

                    && Random.Shared.Next(4) == 0)

                    CoC.Instance.Flags.Set(GameFlagIds.KidSitter, 2);
                else if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) == 2)
                    CoC.Instance.Flags.Set(GameFlagIds.KidSitter, 1);
            }

            if (Player.HasStatusEffect(StatusEffectIds.AnemoneArousal))
            {
                if (Player.Pregnancy.Incubation > 1)
                {
                    Player.RemoveStatusEffect(StatusEffectIds.AnemoneArousal);

                    OutputText("Scenes.Anemonescene.TimeChange.ArousalFadesPregnant".Localized());
                }
                else if (!Player.HasVagina())
                {
                    Player.RemoveStatusEffect(StatusEffectIds.AnemoneArousal);

                    OutputText("Scenes.Anemonescene.TimeChange.ArousalFadesNoVagina".Localized());
                }

                needNext = true;
            }

            return needNext;
        }

        public bool TimeChangeLarge() => false;

        public void ApplyVenom(float str = 1)
        {
            Player.TakeLustDamage(2 * str);

            // 原版 AnemoneVenomDebuff.hx:11：Str / Spe 各减 str（同一战斗增益源上叠加）
            var debuff = Player.CreateOrFindStatusEffect(StatusEffectIds.AnemoneVenom);

            var stacks = debuff.Values[0] + str;

            debuff.Values[0] = stacks;

            CombatCalculator.AddCombatBuff(Player, GameStats.Strength, -stacks, "AnemoneVenom", StatusEffectIds.AnemoneVenom);

            CombatCalculator.AddCombatBuff(Player, GameStats.Speed, -stacks, "AnemoneVenom", StatusEffectIds.AnemoneVenom);
        }

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Anemone);

        public override void Entry()
        {

        }

        public string WeaponName()
        {

            if (!CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                return "<b>ERROR: No weapon</b>";

            var item = Identifier.TryParse(CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId), out var anemoneWeaponId) ? ItemRegistry.GetDefinition(anemoneWeaponId) : null;

            if (item != null && (ItemRegistry.Create(item) is Weapon || ItemRegistry.Create(item) is Shield))
                return ItemRegistry.Create(item).Name;

            if (item == ConsumableIds.WhiteBook) return "white book";
            if (item == ConsumableIds.BlackBook) return "black book";
            if (item == WeaponIds.Wingstick) return "wingstick";
            if (item == UseableIds.TeddyBear) return "teddy bear";
            return "<b>ERROR: Unknown item, update AnemoneScene.WeaponName()</b>";
        }

        public string WeaponNameLong()
        {

            if (!CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                return "<b>ERROR: No weapon</b>";

            var item = Identifier.TryParse(CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId), out var anemoneWeaponId) ? ItemRegistry.GetDefinition(anemoneWeaponId) : null;

            if (item != null && (ItemRegistry.Create(item) is Weapon || ItemRegistry.Create(item) is Shield))
                return ItemRegistry.Create(item).LongName;

            return item == null ? "<b>ERROR: Unknown item, update AnemoneScene.WeaponNameLong()</b>" : ItemRegistry.Create(item).Name;
        }

        public bool AnemoneFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0;
        }

        public void AnemonePreg()
        {

            Player.KnockUp(PregnancyIds.Anemone, PregnancyIds.AnemoneIncubation, 101);
        }

        public int KidAXP(int diff = 0)
        {

            if (diff == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.KidAXp);

            CoC.Instance.Flags.Set(GameFlagIds.KidAXp, CoC.Instance.Flags.Get(GameFlagIds.KidAXp) + diff);

            if (CoC.Instance.Flags.Get(GameFlagIds.KidAXp) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.KidAXp, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.KidAXp) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.KidAXp, 100);

            return CoC.Instance.Flags.Get(GameFlagIds.KidAXp);
        }

        public void MortalAnemoneeeeee()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetAnemone) == 0 || Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)))
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesMetAnemone, CoC.Instance.Flags.Get(GameFlagIds.TimesMetAnemone) + 1);

                if (Player.Location == "swim")
                {
                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.DriftingLoamyFloor".Localized());

                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.LookUnderFloraButt".Localized());

                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.SmilesStandsHardeningCock".Localized());
                }
                else
                {
                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.StepIntoBoatDistracted".Localized());

                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.SmilesBobsToSurface".Localized());

                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)))
                    {
                        MinoCumForAnemonieeeeez();

                        return;
                    }

                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.SurpriseSubsidesWoolyArousal".Localized());
                }
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesMetAnemone, CoC.Instance.Flags.Get(GameFlagIds.TimesMetAnemone) + 1);

                if (Player.Location == "swim")
                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.LongStrokeEffortlessDrift".Localized());
                else
                    OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.UnmoorBoatBubblingSound".Localized());
            }

            OutputText("Scenes.Anemonescene.MortalAnemoneeeeee.FightingAnemonePrompt".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryAnemones);

            var anemone = EnemyRegistry.Create(new Identifier("coc", "anemone"));

            CombatScene.Start(anemone, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            Player.ApplyStats(new DynStat(GameStats.Lust, 4f));

            ApplyVenom(1);
        }

        public void DefeatAnemone()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Anemonescene.DefeatAnemone.KneesBuckleCollapsesSplash".Localized());
            else
                OutputText("Scenes.Anemonescene.DefeatAnemone.SlumpsDownMasturbating".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.YourAss".Localized(), null);

            AddButton(1, "Common.Button.YourCock".Localized(), null);

            AddButton(2, "Common.Button.YourVagina".Localized(), null);

            AddButton(3, "Common.Button.HerButt".Localized(), null);

            AddButton(4, "Common.Button.LayEgg".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Anemonescene.DefeatAnemone.HaveYourWayParts".Localized());

                AddButton(0, "Common.Button.YourAss".Localized(), VictoryButtholeRape);

                if (Player.HasCock())
                    AddButton(1, "Common.Button.YourCock".Localized(), RapeAnemoneWithDick);

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.YourVagina".Localized(), RapeAnemoneWithPussy);

                if (Player.HasVagina() && Player.GetClitLength() >= 4 || Player.HasCockThatFits(48))
                    AddButton(3, "Common.Button.HerButt".Localized(), AnemoneButtPlugginz);

                if (Player.CanOviposit())
                    AddButton(4, "Common.Button.LayEgg".Localized(), AnemoneGetsLayedByBeePositor);

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                    AddButton(12, "Common.Button.UnbirthIt".Localized(), AnemoneFunWithUnbirthing);

                if (Player.HasVagina() && Player.BiggestBreastSize() >= 4 && false)
                    AddButton(5, "Common.Button.Titfuck".Localized(), null);
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void AnemoneFunWithUnbirthing()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Anemonescene.AnemoneFunWithUnbirthing.ParasitesRollOntoBack".Localized());

            OutputText("Scenes.Anemonescene.AnemoneFunWithUnbirthing.AwkwardWaitingGruntDispleasure".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Anemonescene.AnemoneFunWithUnbirthing.HandsOnBellySwell".Localized());

            OutputText("Scenes.Anemonescene.AnemoneFunWithUnbirthing.TendrilsPushEntrancePurr".Localized());

            OutputText("Scenes.Anemonescene.AnemoneFunWithUnbirthing.WaggingFingerNaughtyNaughty".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -2);

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            AnemonePreg();

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryButtholeRape()
        {
            ClearOutput();

            ShowImage("anemone-getanal");

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.LookOverBlueShaft".Localized());

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.HardlyStandArousal".Localized());

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.StiffeningTrembling".Localized());

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.AssholeHeatedAphrodisiac".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Anemonescene.VictoryButtholeRape.HandMovesToCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Anemonescene.VictoryButtholeRape.LiftHandJillingOff".Localized(
                        Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.WorkAssholeOnTool".Localized());

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.BlueLoverRestless".Localized(
                Player.AssholeDescript()
            ));

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Anemonescene.VictoryButtholeRape.EachCockTwitchesEjaculates".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Anemonescene.VictoryButtholeRape.Squirt".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Anemonescene.VictoryButtholeRape.Spray".Localized());
                    else
                        OutputText("Scenes.Anemonescene.VictoryButtholeRape.Torrent".Localized());

                OutputText("Scenes.Anemonescene.VictoryButtholeRape.OfSemen".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Anemonescene.VictoryButtholeRape.PussySpasms".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Anemonescene.VictoryButtholeRape.SpasmsAroundTentacles".Localized(
                        Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.AnemoneRightOnEdge".Localized());

            if (Player.Cocks.Count > 0 || Player.Gender == Gender.Genderless)
                OutputText("Scenes.Anemonescene.VictoryButtholeRape.AWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Anemonescene.VictoryButtholeRape.AnotherWord".Localized());

            OutputText("Scenes.Anemonescene.VictoryButtholeRape.GiggleGatherGearKiss".Localized());

            Player.Orgasm("Anal");

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void RapeAnemoneWithDick()
        {
            ClearOutput();

            ShowImage("anemone-male-fuck");

            if (Player.CockThatFits(36) >= 0)
            {
                int x = Player.CockThatFits(36);

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.RubbingVigorouslyLookOver".Localized());

                if (CurrentMonster.LustPercent >= 100)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.DiddlingWithFreeHand".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.UnfasteningGarmentsStroke".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SmileEagerShakeHead".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.LipsOfVagina".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.PullApartFeatheryLabia".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SavoringPushAllWay".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.PushAllWayDick".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SLetter".Localized());

                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.RubWord".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SLetterAgain".Localized());

                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.FeelersLiningPussy".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.AndThemWord".Localized());

                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.AsYouStartPumping".Localized());
                }

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.FuckBeginsInEarnest".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.InTimeWithYour".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.BouncesBrushGills".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ThrustsFasterSackSlap".Localized());
                else
                    if (Player.GetClitLength() >= 6)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ThrustsIntoBlueGirl".Localized(
                            Player.ClitDescript(),

                            Player.VaginaDescript(0)
                        ));

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ParagraphBreak".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.EyesRollBackClimax".Localized());

                if (Player.HasPerk(PerkIds.Flexibility))
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.LeanDownTakeCock".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.OwnOrgasmTakesLonger".Localized());

                if (Player.CumQ() > 500)
                {
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ContinuePourIntoHer".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.EnormousDimensionsWaist".Localized());

                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SheLooksWord".Localized());

                    if (Player.CumQ() < 2000)
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.Thoroughly".Localized());
                    else
                        OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.Monstrously".Localized());

                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.PregnantFinishedBlueDick".Localized());
                }

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.AfterLastCum".Localized(
                    Player.CockDescript(x)
                ));

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.RubbingVigorouslyAgain".Localized());

                if (Player.LustPercent >= 100)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.DiddlingFreeHandAgain".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.UnfasteningGarmentsStrokeAgain".Localized());

                if (Player.Tallness > 48)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.Over".Localized());
                else
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.Before".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.EyesWidenSheerAmount".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SmileStunnedWaggleCocks".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.LipsOfVaginaAgain".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.FeatheryLabiaAgain".Localized());

                if (Player.Tallness > 84)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.FinallyKneelDownBegin".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.FirstInchesSlowGoing".Localized(
                    Appearance.CockNoun(BodyPartTypes.Human),

                    Player.CockDescript(0),

                    Appearance.CockNoun(BodyPartTypes.Human),

                    Appearance.CockNoun(BodyPartTypes.Human)
                ));

                if (Player.Cocks[0].Length > 60)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ImageryStimulationIncrease".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ParagraphBreakThird".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.InordinatelyPleasedThrusting".Localized());

                if (Player.Cocks[0].Length > 60)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.UnableToLookAnywhere".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.EnjoyingTreatmentTwitching".Localized(
                    Appearance.CockNoun(BodyPartTypes.Human),

                    Player.CockDescript(0)
                ));

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.UntilPuffsOutCheeks".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.SentencePeriod".Localized());

                if (Player.CumQ() > 1500)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.CannotHoldVolume".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.TakesOpportunitySqueeze".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.BothSpentAndLimp".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.HandsDazedlyPlaying".Localized());
                else
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithDick.ContinuesSputterCough".Localized());

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            }
        }

        public void RapeAnemoneWithPussy()
        {
            ClearOutput();

            ShowImage("anemone-female-fuck");

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.HandiworkFeminineStirrings".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.LooksVacantlyApproach".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.AfterBrushingHersAside".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.FondleCrownTentacles".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.FeelItHardenAs".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.CaressThenSqueeze".Localized());

            if (!Player.IsNaked())
            {
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.TakeTimeStripArmor".Localized());

                if (Player.BiggestBreastSize() > 2)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.SpecialAttentionToBreasts".Localized());

                if (Player.Body.Hips > 6)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.GoodUseOfHips".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ByTheTimeFinished".Localized());
            }

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.LeanIntoDeepKiss".Localized());

            if (Player.BiggestBreastSize() > 2)
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.LetPlaceholderRubHers".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ThenPullApart".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.TurnAwayDisplayPussy".Localized());
            else
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ReachDownTwoFingers".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ParagraphBreak".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.WastesNoTimeAssessing".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.TakeByUpperArms".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.HairOverYouLines".Localized(
                Player.VaginaDescript(0)
            ));

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.AphrodisiacHungryPussy".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ThrustsHardFast".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.BreastsAnd".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.NipplesComma".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.PushesDeeplyIntoHer".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.BreastsAndAgain".Localized());

                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.NipplesCommaAgain".Localized());
            }

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.SpreadingHeatChestMatch".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.OverwhelmingSensationsContract".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.Vaginas[0].Wetness >= 4)
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.SoaksCrotchWithJuice".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.WringsPenisBlueShaft".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.OnYourBack".Localized());
            else
                if (Player.BiggestBreastSize() < 1)
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.OnYourChest".Localized());
                else
                    OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.BetweenYourBreasts".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.GivesUpStreamsWomb".Localized());

            OutputText("Scenes.Anemonescene.RapeAnemoneWithPussy.ScratchedItchGiveKiss".Localized());

            AnemonePreg();

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToAnemone()
        {
            ClearOutput();

            int x = Player.CockThatFits(36);

            if (Player.Health < 1)
            {
                OutputText("Scenes.Anemonescene.LoseToAnemone.CollapseFaceFirstLake".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.AwakeWashedOntoSand".Localized());

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Anemonescene.LoseToAnemone.ShiveringSlumpRemoveArmor".Localized());

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Anemonescene.LoseToAnemone.ExaminationStopsMilkDribble".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.BackAtFrontMotions".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.HurriedlyComposeExplanation".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Anemonescene.LoseToAnemone.ReachOutInstinctively".Localized());
                else
                    OutputText("Scenes.Anemonescene.LoseToAnemone.SmirkAmusedTurnabout".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.LashesOutSlap".Localized());

                Player.HealthChange(-10);

                Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (Player.CockTotal() > 0)
            {
                ShowImage("anemone-male-fuck");

                if (Player.HasCockThatFits(36))
                {
                    OutputText("Scenes.Anemonescene.LoseToAnemone.SlumpRemoveArmorAgain".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() < 50)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.ADroplet".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.ASquirt".Localized());
                        else
                            OutputText("Scenes.Anemonescene.LoseToAnemone.ASteadyFlow".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.PreCumFollowsStroke".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.PushesBackOnHaunches".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.IntoYourLap".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.DownwardsOntoHams".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.DeliveringLancesVenom".Localized(
                        Player.CockDescript(x),

                        Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                    if (Player.Tallness > 48)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Up".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Down".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.SmileAroundEyes".Localized(
                        Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                    if (Player.CockTotal() > 1)
                    {
                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.YourOtherDickRubs".Localized());
                        else
                            OutputText("Scenes.Anemonescene.LoseToAnemone.RestOfCocksRub".Localized());

                        OutputText("Scenes.Anemonescene.LoseToAnemone.AgainstCheekWetness".Localized());
                    }

                    OutputText("Scenes.Anemonescene.LoseToAnemone.ParagraphBreak".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.HandsFromWaterHair".Localized(
                        Player.CockDescript(x),

                        Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Anemonescene.LoseToAnemone.HerOtherHandSlips".Localized());

                        if (Player.VaginalCapacity() < 15)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.AFewFingers".Localized());
                        else
                            if (Player.VaginalCapacity() < 30)
                                OutputText("Scenes.Anemonescene.LoseToAnemone.Partway".Localized());
                            else
                                OutputText("Scenes.Anemonescene.LoseToAnemone.AllTheWay".Localized());

                        OutputText("Scenes.Anemonescene.LoseToAnemone.IntoPlaceholderTingle".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.GetClitLength() > 5)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.HavingAchievedPullsOut".Localized(
                                Appearance.CockNoun(Player.Cocks[x].Id)
                            ));
                    }

                    OutputText("Scenes.Anemonescene.LoseToAnemone.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.HeatRubbingOnCock".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.AndClit".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.QuicklyGetsToYou".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() > 500)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.BowingOutCheeks".Localized());

                    if (Player.CumQ() > 2000)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.SquirtingOutNose".Localized());

                    if (Player.Cocks.Count > 1)
                    {
                        OutputText("Scenes.Anemonescene.LoseToAnemone.AsWord".Localized());

                        if (Player.Cocks.Count == 2)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.YourCocksShoot".Localized());
                        else
                            OutputText("Scenes.Anemonescene.LoseToAnemone.RestOfCocksShoot".Localized());

                        OutputText("Scenes.Anemonescene.LoseToAnemone.FaceAndHair".Localized());

                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.DrawingPatternScore".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Anemonescene.LoseToAnemone.PaintingSkinWhite".Localized());
                            else
                                OutputText("Scenes.Anemonescene.LoseToAnemone.WhitewashingUpperBody".Localized());
                    }

                    OutputText("Scenes.Anemonescene.LoseToAnemone.SwallowsGreedilyStrokes".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.TautBlueBellyDistending".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.SentencePeriod".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.GratefulRestHandsPumping".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.CockTotal() > 1)
                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.AndYourOtherFollows".Localized(
                                Player.CockDescript(1)
                            ));
                        else
                            OutputText("Scenes.Anemonescene.LoseToAnemone.AndOtherPricksFollow".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.BlueGirlContinuesStroking".Localized());

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();
                }
                else
                {
                    OutputText("Scenes.Anemonescene.LoseToAnemone.SlumpRemoveArmorThird".Localized());

                    if (Player.CumQ() > 500)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.InASteadyStream".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.SentencePeriodAgain".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.AlmostReverentlyCaresses".Localized(
                        Player.CockDescript(0),

                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Waist".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Flank".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.PushesShouldersDown".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.DrawsYouBackwards".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Hands".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.ForelegsKneeHyphen".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.DownFrontToKeep".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.HeadDipsIntoLakewater".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.BehindYouTakenPenis".Localized());

                    Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                    OutputText("Scenes.Anemonescene.LoseToAnemone.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.TakingMomentTransferHip".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.YourSackTightens".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.DoesntEscapeBlueLover".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.SLetter".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.Prevent".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.SLetterAgain".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.AnySemenEscaping".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.NearlyAnHourPerformance".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.BallsAnd".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.EventuallyCannotTakeMore".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.ClawingTormentorTentacles".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.BuckingStampingGround".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.LookingIrritatedBringEnd".Localized());

                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.Strings".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Anemonescene.LoseToAnemone.Streams".Localized());
                        else
                            OutputText("Scenes.Anemonescene.LoseToAnemone.Gouts".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.OfJism".Localized());

                    if (Player.Tallness < 48 && !Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.FlyOverHeadTurning".Localized());
                    else
                        if (Player.Tallness < 84 && !Player.IsTaur())
                            OutputText("Scenes.Anemonescene.LoseToAnemone.CatchAirRainDown".Localized());
                        else
                        {
                            OutputText("Scenes.Anemonescene.LoseToAnemone.LandRightOnYour".Localized());

                            if (Player.BiggestBreastSize() >= 1)
                                OutputText("Scenes.Anemonescene.LoseToAnemone.BreastsAnd".Localized());

                            OutputText("Scenes.Anemonescene.LoseToAnemone.FaceHearGiggling".Localized());
                        }

                    OutputText("Scenes.Anemonescene.LoseToAnemone.SeveralMinutesClimax".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Anemonescene.LoseToAnemone.SemenColored".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.WaterAroundHerPush".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.PushingInertFormOff".Localized());

                    if (Player.Tallness < 48 && !Player.IsTaur())
                        OutputText("Scenes.Anemonescene.LoseToAnemone.LooksYouOverDrinks".Localized());
                    else
                        OutputText("Scenes.Anemonescene.LoseToAnemone.LeansOverLickingSemen".Localized());

                    OutputText("Scenes.Anemonescene.LoseToAnemone.HavingFedGrinsGrabs".Localized());

                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                    CombatScene.CleanupAfterCombat();
                }
            }
            else
            {
                ShowImage("anemone-female-fuck");

                OutputText("Scenes.Anemonescene.LoseToAnemone.ShiveringFallToKnees".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Anemonescene.LoseToAnemone.MomentBrightensAgain".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.MusesNoFood".Localized());

                if (Player.Tallness < 48)
                    OutputText("Scenes.Anemonescene.LoseToAnemone.PullingYouUpright".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.SentencePeriodThird".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.SittingShallowWater".Localized());

                if (Player.BiggestBreastSize() > 1)
                    OutputText("Scenes.Anemonescene.LoseToAnemone.BreastsAndAgain".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.DriftsDownPastNavel".Localized(
                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0)
                ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                OutputText("Scenes.Anemonescene.LoseToAnemone.ExclaimsIntensifying".Localized());

                OutputText("Scenes.Anemonescene.LoseToAnemone.ReleasesHandTransfersHip".Localized(
                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Anemonescene.LoseToAnemone.SeveralMinutesFinalOrgasm".Localized(
                    Player.VaginaDescript(0)
                ));

                AnemonePreg();

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 10f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void MinoCumForAnemonieeeeez()
        {

            OutputText("Scenes.Anemonescene.MinoCumForAnemonieeeeez.SurpriseSubsidesWoolyArousal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 0)
                OutputText("Scenes.Anemonescene.MinoCumForAnemonieeeeez.MakePresentOfBottle".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 1)
                    OutputText("Scenes.Anemonescene.MinoCumForAnemonieeeeez.RidingHighShareBuzz".Localized());
                else
                {
                    OutputText("Scenes.Anemonescene.MinoCumForAnemonieeeeez.HellNoNotGive".Localized());

                    var anemone = EnemyRegistry.Create(new Identifier("coc", "anemone"));

                    CombatScene.Start(anemone, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

                    Player.ApplyStats(new DynStat(GameStats.Lust, 4f));

                    ApplyVenom(1);

                    return;
                }

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoRequestsBallAfterLose.ButtonGive".Localized(), GiveMino);

            AddButton(1, "Scenes.Aiko.AikoRequestsBallAfterLose.ButtonDontGive".Localized(), DontGiveMino);
        }

        public void DontGiveMino()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.DontGiveMino.LookSternlyHoldHand".Localized());

            var anemone = EnemyRegistry.Create(new Identifier("coc", "anemone"));

            CombatScene.Start(anemone, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            Player.ApplyStats(new DynStat(GameStats.Lust, 4f));

            ApplyVenom(1);
        }

        public void GiveMino()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.MinotaurCum));

            OutputText("Scenes.Anemonescene.GiveMino.NodGirlQuietYay".Localized());

            if (Player.Lust < 30)
            {
                OutputText("Scenes.Anemonescene.GiveMino.WatchingFondlingDevolves".Localized());

                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Anemonescene.GiveMino.CockBecomesLittleErect".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Anemonescene.GiveMino.PlaceholderAchesWithNeed".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Anemonescene.GiveMino.AssholeBeginsTingle".Localized());

                OutputText("Scenes.Anemonescene.GiveMino.ShakeOffHeadBack".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.GiveMino.FondlingDevolvesGenuine".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.YourAss".Localized(), VictoryButtholeRape);

                if (Player.HasCock())
                    AddButton(1, "Common.Button.YourCock".Localized(), RapeAnemoneWithDick);
                else
                    AddButton(1, "Common.Button.YourCock".Localized(), null);

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.YourVagina".Localized(), RapeAnemoneWithPussy);
                else
                    AddButton(2, "Common.Button.YourVagina".Localized(), null);

                SetExitButton("Common.Button.Back".Localized());

                return;
            }

            ReturnToCamp(1);
        }

        public void AnemoneButtPlugginz()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.ImaginingClimaxLookOver".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.PlaysWithHerself".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.NiceNotLookingFor".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.OpeningArmorStroke".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.OneCockPlaceholder".Localized());
                else
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.YourPlaceholder".Localized(
                        Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.AsY".Localized());
            }
            else
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.YLetter".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.CircleAroundBehind".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.RoundedBlueCheeks".Localized());

            if (Player.HasCockThatFits(48))
            {
                if (!Player.IsTaur())
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.FishCockOutGarments".Localized());
                else
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.RubYourCockFit".Localized());

                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.BetweenSmoothCurves".Localized());
            }
            else
            {
                if (!Player.IsTaur())
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.FingerYourPussy".Localized());
                else
                    OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.ImagineTightnessOfHole".Localized());

                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.UntilClitFills".Localized());
            }

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.PromptingGiggle".Localized());

            if (Player.HasCock() && Player.HasCockThatFits(48))
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.SmearPreCumCrack".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.PuttingHandsDown".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.FindNothingNoAsshole".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.AlarmedNoiseStillness".Localized());

            if (Player.HasCockThatFits(48))
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.CockWord".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.ClitWord".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.DroopingTowardWater".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.DemandWheresTheHole".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.LightsUpUnderstands".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.RetortHaveToHave".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.ThoughtfulPointsMouth".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.WhereDoesComeOut".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.BlushesBluePointsMouth".Localized());

            OutputText("Scenes.Anemonescene.AnemoneButtPlugginz.AnusWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckIt".Localized(), AnemoneQuoteUnquoteAnal);

            if (!Player.IsTaur())
                AddButton(1, "Common.Button.Hotdog".Localized(), HotdogTheAnemone);
            else
                AddButton(1, "Common.Button.Hotdog".Localized(), null);

            AddButton(2, "Common.Button.FuckOff".Localized(), FuckingAssholelessAnemoneeeez);
        }

        public void AnemoneQuoteUnquoteAnal()
        {
            ClearOutput();

            var dick = (Player.HasCockThatFits(48));

            var x = 0;

            ShowImage("anemone-bj");

            if (dick)
                x = Player.CockThatFits(48);

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.InMoodForAnal".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EverAPuristStroke".Localized());

                if (dick)
                    OutputText(Player.CockDescript(x));
                else
                    OutputText(Player.ClitDescript());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.UntilProtrudesFromHole".Localized());
            }

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.RegardsIntentlyAsApproach".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.SeizeHerHead".Localized());
            else
            {
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.MountHerHead".Localized());

                if (!dick)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.AnglingClitForward".Localized());
            }

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.HairDeliversVenom".Localized());

            if (!dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ChickHyphen".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.PrickIntoEagerFace".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.MoistMouthWelcomes".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.DickWord".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ClitWord".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.FirstSurpriseThenRelish".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.CockHeadPlaceholder".Localized(
                    x + 1
                ));
            else
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.Tip".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EmbracedByRipplingWalls".Localized());

            if (!dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ThatWontCome".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.TightnessAdjusts".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.GirthOfYourPrick".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.GirlThOfButton".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.LooksUpAtYou".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.Unseen".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EyesATwinkle".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.HairReachesForward".Localized());

                if (!Player.HasCock())
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.BallsSwellWithBlood".Localized());
            }

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.HipsTakeOver".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.DoesntTakeLongClimax".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.DickWordAgain".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ClitWordAgain".Localized());

            OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.StrokingVigorouslySmears".Localized());

            if (dick)
            {
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.CockBeginsEjaculate".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.PushSoMuchCum".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EvenWhenStomachStretches".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.SentencePeriod".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.SatisfiedPullOut".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.LeaveHerThere".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.BobbingRoundly".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.HeadBackToCamp".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ClitPackedWithNerves".Localized());

                if (Player.Wetness() < 4)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.DroolsItsLube".Localized());
                else
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.SquirtsObscenely".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.BodyShiversPainful".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.LoverHopefulWatchesWaits".Localized());

                if (Player.CockTotal() == 0)
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EvenTriesPushThem".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.SentencePeriodAgain".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.ClitRemainsUnproductive".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.EventuallyBodyCalms".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.WhinesPoutingNegligent".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.PointingToSeedSquiggles".Localized());

                OutputText("Scenes.Anemonescene.AnemoneQuoteUnquoteAnal.AndLeaveHer".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -.5f), new DynStat(GameStats.Sensitivity, -.5f));

            CombatScene.CleanupAfterCombat();
        }

        public void HotdogTheAnemone()
        {
            ClearOutput();

            ShowImage("anemone-doggy");

            var dick = (Player.HasCockThatFits(48));

            var x = 0;

            if (dick)
                x = Player.CockThatFits(48);

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.SpiritOfTheThing".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.CockFitPlaceholder".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.ClitPlaceholder".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.BetweenRoundCheeks".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.PreCum".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.YourJuice".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.DroolsOntoHer".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.CallHeyCouldHelp".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.TheAnemone".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.ExcitedBySight".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.LookingAlmostBored".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.ObligesSnakesTowardYou".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.YourCockHeadPlaceholder".Localized(
                    x + 1
                ));
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.NervePackedClit".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.RespondingAutonomously".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.NowThrobbingPrick".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.SwollenRedClit".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.WicksMoreVenom".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.BeforeRubSkinOff".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.CockWord".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.ClitWord".Localized());

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.YouComeBodyShakes".Localized());

            if (dick)
            {
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.SwollenPrickFountains".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Anemonescene.HotdogTheAnemone.WhenFinishedNotBlue".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.PussyClampsDown".Localized());

                if (Player.Wetness() < 4)
                    OutputText("Scenes.Anemonescene.HotdogTheAnemone.DroolingDownThighs".Localized());
                else
                    OutputText("Scenes.Anemonescene.HotdogTheAnemone.SquirtingCumBehind".Localized());

                OutputText("Scenes.Anemonescene.HotdogTheAnemone.SighsImpatiently".Localized());
            }

            OutputText("Scenes.Anemonescene.HotdogTheAnemone.SlipFromBetween".Localized());

            if (dick)
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.DaresNotMove".Localized());
            else
                OutputText("Scenes.Anemonescene.HotdogTheAnemone.LooksIndolently".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -.5f), new DynStat(GameStats.Sensitivity, -.5f));

            CombatScene.CleanupAfterCombat();
        }

        public void FuckingAssholelessAnemoneeeez()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.FuckingAssholelessAnemoneeeez.DammitFunCompletelyRuined".Localized());

            OutputText("Scenes.Anemonescene.FuckingAssholelessAnemoneeeez.SaysNoFoodTurning".Localized());

            OutputText("Scenes.Anemonescene.FuckingAssholelessAnemoneeeez.PlaceHandBehindBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

            CombatScene.CleanupAfterCombat();
        }

        public void AnemoneGetsLayedByBeePositor()
        {
            ClearOutput();

            if (Player.CanOvipositSpider())
            {
                SpiderOvipositAnAnemone();

                return;
            }

            ShowImage("anemone-egg");

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.TentaclesSingSirenSong".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.Reclining".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.VigorouslyMasturbating".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.PussyCallToYou".Localized());

            if (Player.Body.Get<AntennaePart>().Id.Path != "none")
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.Antennae".Localized());
            else
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.Nose".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.TingleEyesLightUp".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.WatchMountingArousal".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.StopMakingStupid".Localized());

            if (silly)
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.Duck".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.FaceYouReply".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.LooksQuizzicalWraps".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.WordlessGroanDrops".Localized());

            OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.IntendedReceptaclePullsAway".Localized());

            if (Player.Sensitivity >= 50 || Player.IsTaur())
            {
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.SaysWeirdJerkingOvipositor".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.HurriedlyOpenArmor".Localized());

                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.MalePartErupts".Localized());
                    else
                        OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.MalePartsErupt".Localized());

                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.InSympathyLancingSemen".Localized());
                }

                if (Player.Eggs() > 40)
                {
                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.EventuallySoManyEggs".Localized());

                    if (Player.HasCock() && Player.CumQ() > 1000)
                        OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.AndSoMuchCum".Localized());

                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.CollectAroundClumping".Localized());
                }

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.LastRollsFromYou".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.RepeatsTuggingOrgan".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.RespondWeakInLegs".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.SaysChunksWastedEggs".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.NoSuchVacillations".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.BlueGirlStruggles".Localized());

                if (Player.Eggs() < 20)
                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.DoesntTakeLongFinish".Localized());
                else
                {
                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.SoManyCome".Localized());

                    if (Player.Eggs() >= 40)
                        OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.BumpsMultiplyUpward".Localized());

                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.SentencePeriod".Localized());
                }

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.RelievedPullShrinkingTube".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.MoansPressingDown".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.ThatsTooBad".Localized());

                if (silly && Player.FertilizedEggs() > 1)
                    OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.BrieflyAmuseImagining".Localized());

                OutputText("Scenes.Anemonescene.AnemoneGetsLayedByBeePositor.GatheringThingsDepart".Localized(
                    Player.Mf("laugh", "giggle")
                ));
            }

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void SpiderOvipositAnAnemone()
        {
            ClearOutput();

            ShowImage("anemone-egg");

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.AsResistanceEbbs".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.EyesUnfocusedFatigue".Localized());
            else
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.TwoFingersPlunging".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.AdvanceAbdomenBends".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.CoolWetFingersConnect".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.NotReallyAnswerWanted".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.GraspingNettle".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.ArmorTightensEachCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.InsideArmorDampens".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.PushYourselfFinishWorking".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.WhimpersThrashingWater".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.EvenArmsLegsFree".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.AnswerSoonFirstOrder".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.AdorablePuppyEyes".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.LetMeFinishFirst".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.FrowningShakesHead".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.GoForYour".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.CockWord".Localized());
                else
                    OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.CuntWord".Localized());

                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.Leaving".Localized());
            }
            else
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.Leave".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.HoldingBagOfEggs".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.GruntSearchingPussy".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.StartsInSurprise".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.SighsRelaxingUnder".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.CriesAsItEnters".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.MurmurDontWorry".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.UnfasteningWithOneHand".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.NextTwoEggsSlip".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.CoverHairWithStrings".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.SLetter".Localized());
            }

            if (Player.Eggs() >= 40)
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.SoManyEggsPump".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.FinallyLetHerGo".Localized());

            OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.PantsTurnBack".Localized());

            if (Player.Corruption < 75)
            {
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.DidSayWould".Localized());

                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.PantsClosesEyes".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.OhThatILied".Localized());

                OutputText("Scenes.Anemonescene.SpiderOvipositAnAnemone.GracesRetreatingBack".Localized());
            }

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void AnemoneKidBirthPtII()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.AwakeLookSkyward".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.VentureTentativeGreeting".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.EyesRaiseSmileBeneath".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.CrossStepsToBarrel".Localized());

            if (Player.Tallness > 48)
                OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.FaceAnglesUp".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.PeeringIntoBarrel".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.AskWillingToExit".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.StillStaysPut".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.NowExposedAboveCalf".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.PlungingDipperAnkles".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.DoesSheExpectDrink".Localized());

            OutputText("Scenes.Anemonescene.AnemoneKidBirthPtII.ChangingTackHesitantBrain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.KeepIt".Localized(), KeepAnemoneKid);

            AddButton(1, "Common.Button.Eject".Localized(), GetRidOfAnemone);
        }

        public void GetRidOfAnemone()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.GetRidOfAnemone.EnoughSummoningBackbone".Localized());

            OutputText("Scenes.Anemonescene.GetRidOfAnemone.UponReachingDumpContents".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, -1);

            Go<CampScene>();
        }

        public void KeepAnemoneKid()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.FrownStareOpaqueEyes".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.AskWhatCallHer".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.StartsKidAttemptingDuplicate".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.WavingHandBreakIn".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.RejoinsTiltingHead".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.ReallyKidA".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.SarahsYoureBorn".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.YeGodsLittleFishes".Localized());

            OutputText("Scenes.Anemonescene.KeepAnemoneKid.KidAAddedFollowers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, 1);

            Save.SetBool("didSoakInABarrel", false);

            Go<CampScene>();
        }

        public void AnemoneBarrelDescription()
        {

            OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.ParagraphBreak".Localized());

            if (CampGuards.IsGuard("KidA"))
                OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.DaughterStandsByBarrel".Localized(
                    WeaponName()
                ));
            else
                if (CampSleep.IsForceSleepTime())
                    OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidASleepingBarrel".Localized());
                else
                    if (View.Time.Hours <= 10)
                        OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidAStandsBarrel".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) > 1)
                            OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidAAbsentBarrel".Localized());
                        else
                            if (View.Time.Hours < 16)
                                OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidADeepBarrel".Localized());
                            else
                                if (View.Time.Hours < 22)
                                    OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidAPeekingOut".Localized());
                                else
                                    OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.KidASeatedRim".Localized());

            OutputText("Scenes.Anemonescene.AnemoneBarrelDescription.ParagraphBreakAgain".Localized());
        }

        public void ApproachAnemoneBarrel()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.WalkOverToBarrel".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KidItemFindHours) != View.Time.Days)
                OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.ItemSitsNext".Localized());

            if (CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.SheHasPlaceholderSitting".Localized(
                    WeaponNameLong()
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) > 1)
                OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.KidAAwayNow".Localized());
            else
                if (KidAXP() < 25)
                    OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.KidASinksRim".Localized());
                else
                    if (KidAXP() < 75)
                        OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.KidAPokesHead".Localized());
                    else
                        OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.KidALeanHead".Localized());

            OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.WhatDoYouNeed".Localized());

            if (CampGuards.IsGuard("KidA"))
                OutputText("Scenes.Anemonescene.ApproachAnemoneBarrel.KidAWatchingEnemies".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                AddButton(5, "Common.Button.TakeWeapon".Localized(), TakeOutOfAnemone);
            else
                AddButton(5, "Common.Button.GiveWeapon".Localized(), GiveAnemoneWeapon);

            if (CoC.Instance.Flags.Get(GameFlagIds.KidItemFindHours) != View.Time.Days)
                AddButton(6, "Common.Button.Item".Localized(), GetAnemoneItem);
            else
                AddButton(6, "Common.Button.Item".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.KidSitter) <= 1)
            {
                AddButton(0, "Common.Button.Appearance".Localized(), KidAAppearance);

                if (CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId) && (Player.MaxFatigue() - Player.Fatigue) >= 10)
                    AddButton(1, "Common.Button.Tutor".Localized(), TutorAnemoneKid);
                else
                    if (!CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                        AddButton(1, "Common.Button.Tutor".Localized(), null);
                    else
                        if ((Player.MaxFatigue() - Player.Fatigue) < 10)
                            AddButton(1, "Common.Button.Tutor".Localized(), null);

                if (CampGuards.IsGuard("KidA"))
                    AddButton(4, "Common.Button.StopGuarding".Localized(), AnemoneWatchToggle);
                else
                    if (!CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                        AddButton(4, "Common.Button.GuardCamp".Localized(), null);
                    else
                        AddButton(4, "Common.Button.GuardCamp".Localized(), AnemoneWatchToggle);

                if (CoC.Instance.Flags.Get(GameFlagIds.HadKidADream) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) >= 2)
                    AddButton(2, "Common.Button.Sex".Localized(), KidASexMenu);
                else
                    AddButton(2, "Common.Button.Masturbation".Localized(), KidAMasturbation);

                AddButton(10, "Common.Button.Evict".Localized(), EvictANemone);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void KidAAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidAAppearance.HardToInspectBarrel".Localized());

            OutputText("Scenes.Anemonescene.KidAAppearance.WantingToSeeMore".Localized());

            OutputText("Scenes.Anemonescene.KidAAppearance.WatchHerMoods".Localized());

            ApproachAnemoneBarrel();
        }

        public void GetAnemoneItem()
        {
            ClearOutput();

            Identifier itype = default;

            OutputText("Scenes.Anemonescene.GetAnemoneItem.ReachDownPickPresent".Localized());

            if (KidAXP() == 0)
                itype = ConsumableIds.DriedTentacle;
            else
                if (KidAXP() < 50)
                    itype = RandomUtils.Choice([ConsumableIds.IncubiDraft, ConsumableIds.SuccubiMilk, ConsumableIds.ImpFood, ConsumableIds.GoblinAle, ConsumableIds.SlimyClothoth, ConsumableIds.LustDraft, ConsumableIds.W_FRUIT, ItemIds.Equinum, ItemIds.GreenGel]);
                else
                    if (KidAXP() < 75)
                    {
                        itype = RandomUtils.Choice([ConsumableIds.WhiteBook, ConsumableIds.BeeHoney, ConsumableIds.OviElixir, ConsumableIds.SharkTooth, ArmorIds.S_SWMWR, ConsumableIds.LustDraft, UseableIds.BeeChitin]);

                        if (Random.Shared.Next(100) == 0)
                            itype = ItemIds.BimboLiqueur;
                    }
                    else
                        if (KidAXP() < 100)
                        {
                            itype = RandomUtils.Choice([ConsumableIds.MinotaurBlood, WeaponIds.LargeAxe, ArmorIds.ComfortableClothes, ConsumableIds.LustDraft, WeaponIds.LustDagger]);

                            if (Random.Shared.Next(100) == 0)
                                itype = ConsumableIds.BroBrew;
                        }
                        else
                        {
                            float choice = Random.Shared.Next(100);

                            if (choice == 0)
                                itype = ConsumableIds.BroBrew;
                            else
                                if (choice == 1)
                                    itype = ItemIds.BimboLiqueur;
                                else
                                    itype = RandomUtils.Choice([ConsumableIds.TigersharkTooth, ConsumableIds.SweetGossamer, ConsumableIds.BlackGossamer, ConsumableIds.ToughSilk, ConsumableIds.Reptlum]);
                        }

            OutputText("Scenes.Anemonescene.GetAnemoneItem.PlaceholderPeriod".Localized(
                ItemRegistry.Create(itype).LongName
            ));

            if (itype == WeaponIds.LargeAxe)
                OutputText("Scenes.Anemonescene.GetAnemoneItem.HolyHowDragThis".Localized());

            OutputText("Scenes.Anemonescene.GetAnemoneItem.ParagraphBreak".Localized());

            Player.Inventory.Add(ItemRegistry.Create(itype), 1);

            ApproachAnemoneBarrel();

            CoC.Instance.Flags.Set(GameFlagIds.KidItemFindHours, View.Time.Days);
        }

        public void GiveAnemoneWeapon()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.GiveAnemoneWeapon.WhatWantToGive".Localized());

            var buttonSlot = 0;

            var foundItem = false;

            for (var x = 0; x < 10; x++)
                if (Player.Inventory.Slots[x].Quantity > 0 && GiveableToAnemone(Player.Inventory.Slots[x].Item.Id))
                {
                    var capturedSlot = x;

                    var item = Player.Inventory.Slots[x].Item;

                    AddButton(buttonSlot++, item.ShortNameKey.Localized(), () => PlaceInAnemone(capturedSlot));

                    foundItem = true;
                }

            if (!foundItem)
                OutputText("Scenes.Anemonescene.GiveAnemoneWeapon.NoAppropriateItemsBold".Localized());

            AddButton(buttonSlot, "Common.Button.Barrel".Localized(), ApproachAnemoneBarrel);
        }

        public bool GiveableToAnemone(Identifier item)
        {

            return item == ConsumableIds.WhiteBook || item == ConsumableIds.BlackBook || item == WeaponIds.Wingstick || (item == UseableIds.TeddyBear && silly) || false || item == ItemIds.DragonShield;
        }

        public void PlaceInAnemone(int slot)
        {
            ClearOutput();

            if (Player.Inventory.Slots[slot].Item.Id.ToString() == UseableIds.TeddyBear.ToString())
                OutputText("Scenes.Anemonescene.PlaceInAnemone.LeaveStuffedToy".Localized());
            else
                OutputText("Scenes.Anemonescene.PlaceInAnemone.LeaveItemByBarrel".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AnemoneWeaponId, Player.Inventory.Slots[slot].Item.Id.ToString());

            Player.Inventory.Slots[slot].RemoveOneItem();

            ApproachAnemoneBarrel();
        }

        public void TakeOutOfAnemone()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.TakeOutOfAnemone.TakeItemBack".Localized());

            var itype = Identifier.TryParse(CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId), out var anemoneWeaponId2) ? ItemRegistry.GetDefinition(anemoneWeaponId2) : null;

            if (CampGuards.IsGuard("KidA"))
            {
                OutputText("Scenes.Anemonescene.TakeOutOfAnemone.DaughterCannotGuard".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AnemoneWatch, 0);
            }

            Player.Inventory.Add(ItemRegistry.Create(itype), 1);

            ApproachAnemoneBarrel();

            Flags.Remove(GameFlagIds.AnemoneWeaponId);
        }

        public void AnemoneWatchToggle()
        {
            ClearOutput();

            if (CampGuards.IsGuard("KidA"))
            {
                OutputText("Scenes.Anemonescene.AnemoneWatchToggle.TellKidNotWorry".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AnemoneWatch, 0);
            }
            else
                if (!CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                    OutputText("Scenes.Anemonescene.AnemoneWatchToggle.FeatherweightBarehanded".Localized());
                else
                {
                    OutputText("Scenes.Anemonescene.AnemoneWatchToggle.KidASmilesCheers".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AnemoneWatch, 1);
                }

            ApproachAnemoneBarrel();
        }

        public void TutorAnemoneKid()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.TooHornyToFocus".Localized());

                ApproachAnemoneBarrel();

                return;
            }

            OutputText("Scenes.Anemonescene.TutorAnemoneKid.ObedientlyClimbsOut".Localized());

            if (KidAXP() < 33)
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.HoldingPlaceholderChest".Localized(
                    WeaponNameLong()
                ));
            else
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.AttentiveStanceWithPlaceholder".Localized(
                    WeaponNameLong()
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == UseableIds.TeddyBear.ToString())
            {
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.HandleHerselfIndependently".Localized());

                OutputText("Scenes.Anemonescene.TutorAnemoneKid.InstructionCuddlesBear".Localized());
            }
            else
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.SpendTimeInstructing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Pipe.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Mace.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.WizardStaff.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.EldritchStaff.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.LethiciteStaff.ToString())
            {
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.ThoughActsNotSerious".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Pipe.ToString())
                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.Pipe".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Mace.ToString())
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.Mace".Localized());
                    else
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.Stick".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) != ItemIds.Mace.ToString())
                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.StillEnoughToBruise".Localized());
                else
                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.ManagesToBruise".Localized());

                Player.HealthChange(-5);

                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Mace.ToString())
                    Player.HealthChange(-15);

                KidAXP(6);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.RidingCrop.ToString())
                {
                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.EnjoysSmackingRidingCrop".Localized());

                    if (Player.Libido < 50)
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.Is".Localized());
                    else
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.Isnt".Localized());

                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.EnoughStopThinking".Localized());

                    Player.HealthChange(-5);

                    if (Player.Libido >= 50)
                        Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                    KidAXP(6);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.LustDagger.ToString())
                    {
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.EnchantedDaggerLight".Localized());

                        Player.HealthChange(-5);

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        KidAXP(5);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Dagger.ToString())
                        {
                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.DaggerLightOneHanded".Localized());

                            Player.HealthChange(-5);

                            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                            KidAXP(5);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.BeautifulSword.ToString())
                            {
                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.SwordDancesPerfectWeight".Localized());

                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.PerhapsTooCorruptSword".Localized());

                                Player.HealthChange(-1);

                                KidAXP(-2);
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.RaphaelsRapier.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.JeweledRapier.ToString())
                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.RapierLightMultitudeReminders".Localized());
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.LargeAxe.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.MarblesHammer.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Warhammer.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.LargeAxe.ToString())
                                    {
                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.CanBarelyLiftWeapon".Localized());

                                        KidAXP(-4);

                                        Player.HealthChange(-5);
                                    }
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Katana.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Spellblade.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Scimitar.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.BrokenScarredBlade.ToString())
                                        {
                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.LightSwordGoodMatch".Localized());

                                            KidAXP(4);

                                            Player.HealthChange(-20);
                                        }
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Spear.ToString())
                                            {
                                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.SpearCloseCombat".Localized());

                                                KidAXP(-1);

                                                Player.HealthChange(-5);
                                            }
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == SceneIds.Core.Weapon.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.SuccubusWhip.ToString())
                                                {
                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.WhipExtensionOfHand".Localized());

                                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.SuccubusWhip.ToString())
                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.DemonicEnchantmentActivates".Localized());

                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.PointDiggingIntoGroin".Localized());

                                                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.SuccubusWhip.ToString())
                                                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                                                    KidAXP(6);
                                                }
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.SpikedGauntlets.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.HookedGauntlets.ToString())
                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.WearsGauntletsEasily".Localized());
                                                    else
                                                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Wingstick.ToString())
                                                        {
                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.StaresAtStick".Localized());

                                                            Player.HealthChange(-10);

                                                            Flags.Remove(GameFlagIds.AnemoneWeaponId);

                                                            KidAXP(5);
                                                        }
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.DragonShield)
                                                            {
                                                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.ProtegeTakesToShield".Localized());

                                                                KidAXP(5);

                                                                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                                                            }
                                                            else
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ConsumableIds.WhiteBook.ToString())
                                                                {
                                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.PartLiteracyPartMagic".Localized());

                                                                    KidAXP(-5);
                                                                }
                                                                else
                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ConsumableIds.BlackBook.ToString())
                                                                    {
                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.SitsAttentively".Localized());

                                                                        if (Player.HasCock())
                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.Tighten".Localized());
                                                                        else
                                                                            if (Player.HasVagina())
                                                                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.Dampen".Localized());
                                                                            else
                                                                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.BecomeAHindrance".Localized());

                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.PartBetweenLegsPointed".Localized());

                                                                        Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                                                                        KidAXP(4);
                                                                    }
                                                                    else
                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Spellblade.ToString())
                                                                        {
                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.AttemptsDrawSaber".Localized());

                                                                            Flags.Remove(GameFlagIds.AnemoneWeaponId);

                                                                            KidAXP(-5);

                                                                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.ScarredBlade), 1);

                                                                            ReturnToCamp(1);

                                                                            KidAXP(-5);

                                                                            return;
                                                                        }
                                                                        else
                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.FlintlockPistol.ToString())
                                                                            {
                                                                                OutputText("Scenes.Anemonescene.TutorAnemoneKid.AlreadyKnowsGun".Localized());

                                                                                if (silly)
                                                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.PewPewPew".Localized());

                                                                                if (Player.Speed >= 70)
                                                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.EasilyDodgeBullets".Localized());
                                                                                else
                                                                                    if (Player.Speed >= 40 && Player.Speed < 70)
                                                                                    {
                                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.TryToDodgeBullets".Localized());

                                                                                        Player.HealthChange(-10);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.TryBestToAvoid".Localized());

                                                                                        Player.HealthChange(-40);
                                                                                    }

                                                                                KidAXP(5);
                                                                            }
                                                                            else
                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == WeaponIds.Crossbow.ToString())
                                                                                {
                                                                                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.AlreadyKnowsCrossbow".Localized());

                                                                                    if (Player.Speed >= 60)
                                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.EasilyDodgeBolts".Localized());
                                                                                    else
                                                                                        if (Player.Speed >= 30 && Player.Speed < 60)
                                                                                        {
                                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.TryToDodgeBolts".Localized());

                                                                                            Player.HealthChange(-10);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.TryBestAvoidAgain".Localized());

                                                                                            Player.HealthChange(-40);
                                                                                        }

                                                                                    KidAXP(5);
                                                                                }
                                                                                else
                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ItemIds.Flail.ToString())
                                                                                    {
                                                                                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.HoldsUpFlail".Localized());

                                                                                        Player.HealthChange(-10);

                                                                                        KidAXP(-2);

                                                                                        return;
                                                                                    }
                                                                                    else
                                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == UseableIds.TeddyBear.ToString())
                                                                                        {
                                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.TurnsAroundSetsBear".Localized());

                                                                                            KidAXP(5);
                                                                                        }
                                                                                        else
                                                                                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.ForLifeOfHer".Localized(
                                                                                                WeaponName()
                                                                                            ));

            if (Player.Health < 1)
            {
                OutputText("Scenes.Anemonescene.TutorAnemoneKid.GroanFallFlat".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) != WeaponIds.SpikedGauntlets.ToString() && CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) != WeaponIds.HookedGauntlets.ToString())
                    OutputText("Scenes.Anemonescene.TutorAnemoneKid.ThumpHittingGround".Localized());

                OutputText("Scenes.Anemonescene.TutorAnemoneKid.AnemoneGaspWorldSlips".Localized());

                OutputText("Scenes.Anemonescene.TutorAnemoneKid.EightHoursLaterBold".Localized());

                OutputText("Scenes.Anemonescene.TutorAnemoneKid.BlearyEyesFace".Localized());

                OutputText("Scenes.Anemonescene.TutorAnemoneKid.UmHi".Localized());

                Player.Sleeping = true;

                ReturnToCamp(1);

                Player.CreateStatusEffect(StatusEffectIds.PostAnemoneBeatdown, 0, 0, 0, 0);

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.HadKidADream) > 0 && KidAXP() >= 40 && Player.Lust >= Player.MaxLust)
            {
                if (KidASex())
                    return;
                else
                    if (KidAXP() >= 40 && Player.Lust >= Player.MaxLust)
                    {
                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.CollapseOntoBackPanting".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.SomethingTooEnormous".Localized());
                        else
                            OutputText("Scenes.Anemonescene.TutorAnemoneKid.NothingUseful".Localized());

                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.RelieveBurdenDesperation".Localized());

                        OutputText("Scenes.Anemonescene.TutorAnemoneKid.AppearingDecisionPats".Localized());

                        ReturnToCamp(1);
                    }
            }
            else
            {
                if (Player.Level < 10)
                    Player.Experience += 30;

                Player.ChangeFatigue(10);

                ReturnToCamp(1);
            }
        }

        public void KidASexMenu()
        {

            ClearButtons();

            if (KidAXP() < 40)
                AddButton(0, "Common.Button.Sex".Localized(), null);
            else
            {
                AddNextButton("Common.Button.Sex".Localized(), () => KidASex(false));

                AddNextButton("Common.Button.Lovemaking".Localized(), KidALovemaking);
            }

            AddNextButton("Common.Button.Masturbation".Localized(), KidAMasturbation);

            AddButton(14, "Common.Button.Back".Localized(), () => ApproachAnemoneBarrel());
        }

        public bool KidASex(bool cont = true)
        {
            ClearOutput();

            if (!cont)
                OutputText("Scenes.Anemonescene.KidASex.SmilingLustily".Localized());

            int x = 0;

            int y = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) == 2 && Player.HasCockThatFits(36))
            {
                x = Player.CockThatFits(36);

                OutputText("Scenes.Anemonescene.KidASex.ParagraphBreak".Localized());

                ShowImage("anemone-kid-male-vagfuck");

                OutputText("Scenes.Anemonescene.KidASex.CollapseOntoBackPanting".Localized(
                    Appearance.CockNoun(Player.Cocks[x].Id),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Anemonescene.KidASex.FlushesWinces".Localized());

                OutputText("Scenes.Anemonescene.KidASex.BeginningToMoveHips".Localized(
                    Appearance.CockNoun(Player.Cocks[x].Id)
                ));

                OutputText("Scenes.Anemonescene.KidASex.GirlDoesntSeemBetter".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Anemonescene.KidASex.PourIntoSecondOrgasm".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Anemonescene.KidASex.EvenSecondNotEnough".Localized(
                        Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Anemonescene.KidASex.SentencePeriod".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Anemonescene.KidASex.LoadsFromOtherShaft".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Anemonescene.KidASex.SLetter".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.OozeDownLegs".Localized());
                }

                OutputText("Scenes.Anemonescene.KidASex.SpentBlueGirlSlumps".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) < 3)
                    CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, 3);

                ReturnToCamp(2);

                return true;
            }
            else
                if (Player.HasCockThatFits(60))
                {
                    x = Player.CockThatFits(60);

                    y = -1;

                    var temp = 0;

                    while (temp < Player.CockTotal())
                    {
                        if (temp != x)
                        {
                            y = temp;

                            break;
                        }

                        temp += 1;
                    }

                    OutputText("Scenes.Anemonescene.KidASex.ParagraphBreakAgain".Localized());

                    ShowImage("anemone-kid-male-bj");

                    OutputText("Scenes.Anemonescene.KidASex.CollapsePantingAgain".Localized(
                        Appearance.CockNoun(Player.Cocks[x].Id)
                    ));

                    OutputText("Scenes.Anemonescene.KidASex.EmbarrassmentCannotBrake".Localized(
                        Player.CockDescript(x)
                    ));

                    if (y >= 0 || Player.Balls > 0 || Player.HasVagina())
                    {
                        OutputText("Scenes.Anemonescene.KidASex.FeelBashfulFingers".Localized());

                        if (y >= 0)
                            OutputText(Player.CockDescript(y));
                        else
                            if (Player.Balls > 0)
                                OutputText(Player.SackDescript());
                            else
                                if (Player.HasVagina())
                                    OutputText(Player.VaginaDescript(0));

                        if (y >= 0 || Player.Balls > 0)
                            OutputText("Scenes.Anemonescene.KidASex.PullingItFree".Localized());

                        OutputText("Scenes.Anemonescene.KidASex.DexterousDigitsEmboldened".Localized());

                        if (y >= 0)
                        {
                            OutputText("Scenes.Anemonescene.KidASex.AnotherHandJoins".Localized());

                            if (Player.Cocks[y].Length >= 36)
                                OutputText("Scenes.Anemonescene.KidASex.TowersOverHead".Localized());
                            else
                                OutputText("Scenes.Anemonescene.KidASex.QuiversIntoVenomousLocks".Localized());

                            OutputText("Scenes.Anemonescene.KidASex.EveryTimeBrushesHair".Localized());
                        }
                        else
                            if (Player.Balls > 0)
                                OutputText("Scenes.Anemonescene.KidASex.OneHandCupsBelow".Localized());
                            else
                                if (Player.HasVagina())
                                {
                                    OutputText("Scenes.Anemonescene.KidASex.FingersPartLabia".Localized());

                                    if (Player.GetClitLength() >= 6)
                                        OutputText("Scenes.Anemonescene.KidASex.WorkingItFree".Localized());
                                    else
                                        OutputText("Scenes.Anemonescene.KidASex.GiveButtonAttention".Localized());

                                    OutputText("Scenes.Anemonescene.KidASex.OtherHandSlipsIn".Localized());
                                }
                    }

                    OutputText("Scenes.Anemonescene.KidASex.AttentionsReachPeak".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Anemonescene.KidASex.YourBallsTightenUp".Localized());
                    else
                        OutputText("Scenes.Anemonescene.KidASex.YourBodyTenses".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.DivesDownOntoShaft".Localized());

                    if (Player.Balls > 0 || y >= 0)
                    {
                        OutputText("Scenes.Anemonescene.KidASex.EyesTwinkleHands".Localized());

                        if (y >= 0)
                            OutputText(Player.CockDescript(y));
                        else
                            OutputText(Player.SackDescript());

                        OutputText("Scenes.Anemonescene.KidASex.RenewEffortsSquirts".Localized());
                    }
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Anemonescene.KidASex.EyesTwinkleHandPlunges".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.SwallowsGreedily".Localized());

                    if (y >= 0)
                        OutputText("Scenes.Anemonescene.KidASex.AndOntoHerHair".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.OralContractionsSqueeze".Localized());

                    if (Player.CumQ() >= 500)
                    {
                        OutputText("Scenes.Anemonescene.KidASex.SquirtAfterSquirt".Localized());

                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Anemonescene.KidASex.YourClimaxIsSpent".Localized());
                        else
                            OutputText("Scenes.Anemonescene.KidASex.CannotHoldMore".Localized());
                    }

                    OutputText("Scenes.Anemonescene.KidASex.FinallySpendLastLoad".Localized());

                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Anemonescene.KidASex.GetsToFeetReturns".Localized());
                    else
                        OutputText("Scenes.Anemonescene.KidASex.BarelyMoveDistension".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.WearilyRaiseHead".Localized());

                    OutputText("Scenes.Anemonescene.KidASex.LayBackSpentSlip".Localized());

                    Player.Orgasm("Dick");

                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) == 1)
                        CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, 2);

                    ReturnToCamp(2);

                    return true;
                }
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Anemonescene.KidASex.ParagraphBreakThird".Localized());

                        ShowImage("anemone-kid-female-sex");

                        OutputText("Scenes.Anemonescene.KidASex.CollapsePantingThird".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Anemonescene.KidASex.LaughableReactionVenom".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.Vaginas[0].Looseness >= 3)
                            OutputText("Scenes.Anemonescene.KidASex.BarelyFeelLittleShaft".Localized());

                        OutputText("Scenes.Anemonescene.KidASex.EmboldenedPicksUpLegs".Localized());

                        OutputText("Scenes.Anemonescene.KidASex.GraceOfFirstTimer".Localized());

                        if (Player.GetClitLength() < 6)
                            OutputText("Scenes.Anemonescene.KidASex.LittleButtonRubbed".Localized());
                        else
                            if (Player.GetClitLength() < 36)
                                OutputText("Scenes.Anemonescene.KidASex.MonstrousChickStick".Localized());
                            else
                                OutputText("Scenes.Anemonescene.KidASex.IncredibleClitorisLength".Localized());

                        OutputText("Scenes.Anemonescene.KidASex.ThoughTechniqueNothing".Localized());

                        OutputText("Scenes.Anemonescene.KidASex.PullAwayStagger".Localized());

                        AnemonePreg();

                        Player.SlimeFeed();

                        Player.Orgasm("Vaginal");

                        if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) == 1)
                            CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, 2);

                        ReturnToCamp(1);

                        return true;
                    }

            return false;
        }

        public void EvictANemone()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.EvictANemone.ReallyEvictTheAnemone".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ReallyEvictDaAnemone);

            AddButton(1, "Common.Button.No".Localized(), ApproachAnemoneBarrel);
        }

        public void ReallyEvictDaAnemone()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.TimeToReclaimBarrel".Localized());

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.ReachingShorePush".Localized());

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.TurnsHeadToStranger".Localized());

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.OtherParrotsGrin".Localized());

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.AnemoneReturnsEnthusiastically".Localized());

            OutputText("Scenes.Anemonescene.ReallyEvictDaAnemone.TwoGirlsContinueGreeting".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AnemoneKid, -1);

            ReturnToCamp(1);
        }

        public void KidADreams()
        {

            OutputText("Scenes.Anemonescene.KidADreams.MiddleOfNightBold".Localized());

            if (Player.HasCock() && (!Player.HasVagina() || Player.Femininity < 50))
            {
                ShowImage("anemone-kid-male-masti");

                OutputText("Scenes.Anemonescene.KidADreams.ChurchBellChimes".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.PastorCoughsPolitely".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.NodAbsentlyStaring".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.PronounceManAndWife".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.RaiseHandsLiftVeil".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.SapphireFaceOpaqueEyes".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Anemonescene.KidADreams.Thigh".Localized());
                else
                    OutputText("Scenes.Anemonescene.KidADreams.Cheek".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.LeansInToKiss".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.EyesSnapRedMoon".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Anemonescene.KidADreams.SoftHandStrokesFace".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.SlightWeightOnCrotch".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.SlowlyBacksAway".Localized());
            }
            else
            {
                ShowImage("anemone-kid-female-masti");

                OutputText("Scenes.Anemonescene.KidADreams.EldersSonBehindSchoolhouse".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.DrawUpToCorner".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.PealsSuddenly".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.BizarreSapphireSight".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Anemonescene.KidADreams.WithSighGrabHead".Localized());
                else
                    OutputText("Scenes.Anemonescene.KidADreams.NeitherSaysWord".Localized());

                OutputText("Scenes.Anemonescene.KidADreams.SighingTurnOver".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(50 + Player.Sensitivity / 2)));

            Go<CampScene>();
        }

        public void KidABabysitsCows()
        {

            OutputText("Scenes.Anemonescene.KidABabysitsCows.ComeOnGetOut".Localized());

            OutputText("Scenes.Anemonescene.KidABabysitsCows.SoundOfVoiceRaised".Localized());

            OutputText("Scenes.Anemonescene.KidABabysitsCows.CowGirlContinues".Localized());

            OutputText("Scenes.Anemonescene.KidABabysitsCows.MarbleHorseStance".Localized());

            if (KidAXP() >= 66)
            {
                OutputText("Scenes.Anemonescene.KidABabysitsCows.OnlyFewSteps".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.DoYouEvenUnderstand".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.WhirlOfTendrils".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.MarbleTurnsToChildren".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.ChildrenGrumbleRetreats".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.EmbraceShortLived".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KidSitter, -1);
            }
            else
            {
                OutputText("Scenes.Anemonescene.KidABabysitsCows.InitiallyPlodsAlong".Localized(
                    WeaponName()
                ));

                OutputText("Scenes.Anemonescene.KidABabysitsCows.MarbleSaysDrawsUp".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.HoldsArmamentCloser".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.MarbleExclaimsDifferent".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.HuffingSeatsOnStone".Localized());

                OutputText("Scenes.Anemonescene.KidABabysitsCows.NoSoonerAdmonitionOut".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KidSitter, 1);
            }
        }

        public void RepeatCowSitting()
        {

            OutputText("Scenes.Anemonescene.RepeatCowSitting.MarbleLooksUpWarmly".Localized());

            OutputText("Scenes.Anemonescene.RepeatCowSitting.TurningThatWay".Localized());

            OutputText("Scenes.Anemonescene.RepeatCowSitting.PictureBookOldFaded".Localized());

            OutputText("Scenes.Anemonescene.RepeatCowSitting.NotLongBeforeMarble".Localized());

            OutputText("Scenes.Anemonescene.RepeatCowSitting.DaughterEatRightUp".Localized());

            OutputText("Scenes.Anemonescene.RepeatCowSitting.Sweetie".Localized());
        }

        public void KidAWatchesSharks()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidAWatchesSharks.LookOverSharkFamily".Localized());

            if (KidAXP() < 40 || !CoC.Instance.Flags.Has(GameFlagIds.AnemoneWeaponId))
                OutputText("Scenes.Anemonescene.KidAWatchesSharks.SharksNoticeStandUp".Localized());
            else
                if (KidAXP() < 40 && (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ConsumableIds.BlackBook.ToString() || CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ConsumableIds.WhiteBook.ToString()))
                {
                    OutputText("Scenes.Anemonescene.KidAWatchesSharks.WatchesCarefully".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == ConsumableIds.WhiteBook.ToString())
                        OutputText("Scenes.Anemonescene.KidAWatchesSharks.BlindsWithFlash".Localized());
                    else
                        OutputText("Scenes.Anemonescene.KidAWatchesSharks.TurnsDeepBlue".Localized());

                    OutputText("Scenes.Anemonescene.KidAWatchesSharks.BeforeAnyReact".Localized());
                }
                else
                    if (KidAXP() < 75)
                        OutputText("Scenes.Anemonescene.KidAWatchesSharks.SharksLookFromPlay".Localized(
                            WeaponName()
                        ));
                    else
                        OutputText("Scenes.Anemonescene.KidAWatchesSharks.DoesntHesitateBursts".Localized());

            ReturnToCamp(1);
        }

        public void GoblinNightAnemone()
        {

            OutputText("Scenes.Anemonescene.GoblinNightAnemone.ThatNightBold".Localized());

            OutputText("Scenes.Anemonescene.GoblinNightAnemone.NoisyClumpGabbling".Localized(
                WeaponName()
            ));

            OutputText("Scenes.Anemonescene.GoblinNightAnemone.GoblinsSlowUpStop".Localized());

            OutputText("Scenes.Anemonescene.GoblinNightAnemone.DemandsMoveIt".Localized());

            if (Player.Tallness > 48)
                OutputText("Scenes.Anemonescene.GoblinNightAnemone.Tall".Localized());
            else
                OutputText("Scenes.Anemonescene.GoblinNightAnemone.Short".Localized());

            OutputText("Scenes.Anemonescene.GoblinNightAnemone.ToldUsKnockUp".Localized(
                Player.Mf("studly", "gorgeous")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) == UseableIds.TeddyBear.ToString())
            {
                OutputText("Scenes.Anemonescene.GoblinNightAnemone.NuzzlingStuffedToy".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.ThrownOffStammers".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.AnotherGoblinChimes".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.FirstGoblinCoughs".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.ShakesHeadDenying".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.EmphasizesPoint".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.SaysDefeatedGoblins".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.AfterGoblinsLeft".Localized());
            }
            else
            {
                OutputText("Scenes.Anemonescene.GoblinNightAnemone.LooksLittleSurprised".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.ThoughCantSeeFace".Localized(
                    WeaponNameLong()
                ));

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.SaysBlueBadass".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.SpokeswomanTaken".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.LoneGoblinShaken".Localized());

                OutputText("Scenes.Anemonescene.GoblinNightAnemone.ShoutsDontThinkOver".Localized());
            }
        }

        public void KidAMasturbation()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidAMasturbation.CallAnemoneOut".Localized());

            OutputText("Scenes.Anemonescene.KidAMasturbation.BlushesHoldingCrotch".Localized());

            if (KidAXP() < 33)
            {
                OutputText("Scenes.Anemonescene.KidAMasturbation.SlideHandPressLabia".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                KidAXP(1);

                ReturnToCamp(1);

                return;
            }

            OutputText("Scenes.Anemonescene.KidAMasturbation.ExplainMasturbationHealthy".Localized());

            OutputText("Scenes.Anemonescene.KidAMasturbation.TremblingMuttersAh".Localized());

            OutputText("Scenes.Anemonescene.KidAMasturbation.DoesntReactImmediately".Localized());

            OutputText("Scenes.Anemonescene.KidAMasturbation.WincingGrindsFingers".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            KidAXP(5);

            ReturnToCamp(1);
        }

        public void KidALovemaking()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidALovemaking.CuriousSmileForms".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.EyesFollowLazyDrift".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.StartOffLight".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.LendingElegance".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.PlaceholderPoundsInPlaceholder".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.HowCouldYouNot".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.PlaceholderSeemsToGrow".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.SeemsLittleStunned".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.PullBackEyesClouded".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.LittleHeatAmplified".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking.InsteadScoopHerBack".Localized());

            KidALovemaking2();
        }

        public void KidALovemaking2()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidALovemaking2.FingersEntwining".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.HandsSlipWrap".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.EvenLacksExperience".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.EachSqueezeSinkLower".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.FirmPressOfKnee".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.DoubtPlaceholderContain".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.SingleMindedPurpose".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking2.DespitePleasurePulsing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            KidALovemaking3();
        }

        public void KidALovemaking3()
        {
            ClearOutput();

            OutputText("Scenes.Anemonescene.KidALovemaking3.BlueBlurSettles".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.ThoughTriesToPress".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.FreshDoseVenom".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.LikeThisRawScent".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.WantsMorePushingBack".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.MuchLikeHotSting".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.DelightfulPause".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.NotAMomentLater".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.MustNeedThisMore".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.ConsideringOftenQuiet".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.OnlyOnceGrowsCalm".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.EspeciallySpreadsWetness".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.EncouragedByReaction".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.HandNeverStops".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.VaguelyAwareSlowing".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.WhenFinallyStirs".Localized());

            OutputText("Scenes.Anemonescene.KidALovemaking3.TinyWaveAboveRim".Localized());

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

    }
}