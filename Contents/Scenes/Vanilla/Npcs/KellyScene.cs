using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Places;
using MonoCoC.Contents.Scenes.Vanilla.Places.Farm;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    [Register<KellyScene>("coc:kelly_scene")]
    public class KellyScene : Scene, ITimeAware
    {

        public static bool KellyTattooFlagSet(Flag<string> flag) => !string.IsNullOrEmpty(CoC.Instance.Flags.Get(flag));

        public static int NumKellyTattoos()
        {
            var count = 0;

            if (KellyTattooFlagSet(GameFlagIds.KellyTattooCollarbone)) count++;
            if (KellyTattooFlagSet(GameFlagIds.KellyTattooShoulders)) count++;
            if (KellyTattooFlagSet(GameFlagIds.KellyTattooLowerback)) count++;
            if (KellyTattooFlagSet(GameFlagIds.KellyTattooButt)) count++;

            return count;
        }

        public static int NumKellyTribalTats()
        {
            static bool Tribal(string text) => text.Contains("A tribal tattoo", StringComparison.Ordinal);

            var count = 0;

            if (Tribal(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooCollarbone))) count++;
            if (Tribal(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooShoulders))) count++;
            if (Tribal(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooLowerback))) count++;
            if (Tribal(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooButt))) count++;

            return count;
        }

        public static bool WhitneyCorrupt() => CoC.Instance.Flags.Get(GameFlagIds.WhitneyCorruptionComplete) > 0;

        public static bool TelAdreAllowedInto() => Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1;

        public static bool AmilyFollowerCorrupt()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) <= 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta);

            if (visitingUrta == 1 || visitingUrta == 2)
                return false;

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2;
        }

        public static void KeltEncounter() => SceneInstance<KeltScene>.Get().KeltEncounter();

        public const int KellyPregnancyPlayerFather = 25; 

        public static readonly int[] KellyPregnancyEvents = [280, 200, 100];

        public static int KellyPregnancyType => CoC.Instance.Flags.Get(GameFlagIds.KellyPregnancyType) & 0xFFFF;

        public static bool KellyIsPregnant => KellyPregnancyType != 0;

        public static int KellyPregnancyIncubation => CoC.Instance.Flags.Get(GameFlagIds.KellyIncubation);

        public static void KellyPregnancyAdvance()
        {
            var next = KellyPregnancyIncubation - 1;

            if (next >= 0)
                CoC.Instance.Flags.Set(GameFlagIds.KellyIncubation, next);
        }

        public static int KellyPregnancyEvent
        {
            get
            {
                if (KellyPregnancyType == 0)
                    return 0;

                if (KellyPregnancyType != KellyPregnancyPlayerFather)
                    return 1;

                for (var i = 0; i < KellyPregnancyEvents.Length; i++)
                    if (KellyPregnancyIncubation > KellyPregnancyEvents[i])
                        return i + 1;

                return KellyPregnancyEvents.Length + 1;
            }
        }

        public static void KellyKnockUpForce()
        {
            CoC.Instance.Flags.Set(GameFlagIds.KellyPregnancyType, 0);

            CoC.Instance.Flags.Set(GameFlagIds.KellyIncubation, 0);
        }

        public static void KellyKnockUp(int type, int incubation)
        {
            if (KellyIsPregnant)
                return;

            CoC.Instance.Flags.Set(GameFlagIds.KellyPregnancyType, type);

            CoC.Instance.Flags.Set(GameFlagIds.KellyIncubation, type == 0 ? 0 : incubation);
        }

        public static bool KellyKnockUpChance(int floor, int ceiling)
        {
            if (KellyIsPregnant)
                return false;

            var chance = Math.Clamp(floor + Player.VirilityQ(), floor, ceiling);

            return Random.Shared.NextDouble() * 100 < chance;
        }

        public bool TimeChange()
        {
            KellyPregnancyAdvance();

            if (View.Time.Hours == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.KellyRewardCooldown) > 0 && View.Time.Days % 3 == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.KellyRewardCooldown, 0);

                if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) >= 4)
                    CoC.Instance.Flags.Set(GameFlagIds.KellyDisobeyingCounter,

                        CoC.Instance.Flags.Get(GameFlagIds.KellyDisobeyingCounter) + 1);
            }

            if (KellyIsPregnant && KellyPregnancyIncubation == 0)
            {
                KellyPopsOutARunt();

                KellyKnockUpForce();

                return true;
            }

            return false;
        }

        public bool TimeChangeLarge() => false;

        public override void Entry()
        {
            KellySprite();
        }

        public void KellySprite(bool nude = false)
        {
            var pregnant = KellyIsPregnant;

            var bigBoobs = CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) != 0;

            SpriteSelect(pregnant

                ? (bigBoobs ? "kelly_brst_preg" : "kelly_preg")

                : (bigBoobs ? "kelly_brst" : "kelly"));

            _ = nude; 
        }

        public bool HasPinkEgg()
        {

            return Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePink));
        }

        public void BreakingKeltOptions()
        {
            ClearOutput();

            if ((!Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 0) || CoC.Instance.Flags.Get(GameFlagIds.NeverResistKelt) == 1 || Player.StatusEffectValue(StatusEffectIds.Kelt, 1) >= 40 || !Player.HasStatusEffect(StatusEffectIds.Kelt))
            {

                KeltEncounter();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) > 0)
            {
                if (!Player.HasCock())
                {
                    OutputText("Scenes.Kelly.BreakingKeltOptions.BreakKeltNeedsTool".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());

                    return ;
                }

                ResistKeltsBSBreakHimIntro();

                return ;
            }

            OutputText("Scenes.Kelly.BreakingKeltOptions.KeltAbuseForTraining".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Endure".Localized(), KeltEncounter);

            AddButton(1, "Common.Button.ResistKelt".Localized(), ResistKeltsBSBreakHimIntro);

            AddButton(2, "Common.Button.NeverResist".Localized(), NeverBreakKeltIntoKelly);

            AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), FightToBeatKelt);

            AddButton(10, "Common.Button.Appearance".Localized(), KeltAppearance);
        }

        public void KeltAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltAppearance.KeltCentaurDescription".Localized());

            OutputText("Scenes.Kelly.KeltAppearance.SquareChiseledFace".Localized());

            OutputText("Scenes.Kelly.KeltAppearance.HairyDirtyChest".Localized());

            OutputText("Scenes.Kelly.KeltAppearance.ArmedWithBow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BreakingKeltOptions);
        }

        public void FightToBeatKelt()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.FightToBeatKelt.ReadyWeaponAgainstKelt".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KeltKillPlan, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void ResistKeltsBSBreakHimIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 0)
            {
                OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.AnnoyedByKeltRudeness".Localized());

                if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 15) || (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10) && HasPinkEgg()) || (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 10) && HasPinkEgg()) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 15)))
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.MissingTamingItems".Localized());

                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.FifteenMilkHint".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                }
                else
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.TeachHimLessonPrompt".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Yes".Localized(), BreakKeltGo);

                    AddButton(1, "Common.Button.NotYet".Localized(), () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());

                    AddButton(2, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), NeverBreakKeltIntoKelly);
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 1)
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.EagerToFindKelt".Localized());

                    if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 10) || (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5) && HasPinkEgg()) || (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 5) && HasPinkEgg())))
                    {
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.KeepAnotherLesson".Localized());

                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.TenMilkHint".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());

                        return ;
                    }

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), SecondKeltBreaking);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 2)
                    {
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.BackOfFarmMeeting".Localized());

                        if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 5)))
                        {
                            OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.AcquireSuccubiMilk".Localized());

                            OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.FiveMilkHint".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());

                            return ;
                        }

                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.ShakingMilkVial".Localized());

                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.KeltDrawsBow".Localized());

                        CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 3)
                        {
                            KellySprite();

                            FinalKeltBreaking();
                        }
                        else
                        {
                            KellySprite();

                            ApproachKelly();
                        }
        }

        public void NeverBreakKeltIntoKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.NeverResistKelt, 1);

            OutputText("Scenes.Kelly.NeverBreakKeltIntoKelly.NeverBreakKeltDecision".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoToKelt".Localized(), KeltEncounter);

            AddButton(1, "Common.Button.GoHome".Localized(), () => ReturnToCamp(1));
        }

        public void BreakKeltGo()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.BreakKeltGo.ApproachUppityCentaur".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltHostileGreeting".Localized(
                Player.Mf("sissy", "bitch")
            ));

            OutputText("Scenes.Kelly.BreakKeltGo.ColdCalmRefusal".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltFuckOffSpeech".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.WhamImpact".Localized());

            if (Player.Tallness < 70)
                OutputText("Scenes.Kelly.BreakKeltGo.JumpUpPrefix".Localized());
            else
                OutputText("Scenes.Kelly.BreakKeltGo.YLetter".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.StrikesKeltForehead".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.DragUnconsciousKelt".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.RopesOnBarnFloor".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltWakingThreat".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltTauntsAboutFucking".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.CruelSmilePullsMilk".Localized());

            if (HasPinkEgg())
                OutputText("Scenes.Kelly.BreakKeltGo.EggMentionFragment".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.PourMilkIntoKeg".Localized());

            if (HasPinkEgg())
                OutputText("Scenes.Kelly.BreakKeltGo.ThrowEggIntoKeg".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltPleadingRefusal".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeepStirringFluid".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.PullOutAndStroke".Localized());
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.SlapHardeningDick".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kelly.BreakKeltGo.SLetter".Localized());

                OutputText("Scenes.Kelly.BreakKeltGo.AgainstYourBelly".Localized());
            }

            OutputText("Scenes.Kelly.BreakKeltGo.AddFlavorToMeal".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltProtestsBinding".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.MasturbateHalfHorse".Localized());
            else
                OutputText("Scenes.Kelly.BreakKeltGo.TouchYourselfLead".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.PrecumDroplets".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltWarnsAboutJob".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.StopAfterEnoughPre".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Kelly.BreakKeltGo.PrecumIt".Localized());
            else
                OutputText("Scenes.Kelly.BreakKeltGo.PrecumThey".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.LiftKegToKelt".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.TauntAboutWives".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltShockedDenial".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.ShoveFunnelInMouth".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.SwiftFeminization".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.CheckGenitalsChange".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.DelightfulSobs".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.SoothingForOwnGood".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.DisarmingSmirkLead".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.WhipOutCock".Localized());
            else
                OutputText("Scenes.Kelly.BreakKeltGo.GrindBetweenTits".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.SentencePeriod".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.GirlishVoiceLine".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.BreaksEyeContact".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.BackAdverb".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.DownAdverb".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.UnderYouPhrase".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.AtYourCockPressed".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.WrapsTongueCocksucker".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.HindlegsDancePrefix".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.CockslapHer".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.KeltScoldsTechnique".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.WhiningApology".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.SuggestBreastsInstead".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.NodsWrapsBreasts".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakKeltGo.ThrustThroughCleavage".Localized());
            else
                OutputText("Scenes.Kelly.BreakKeltGo.GrabNipplesThrust".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.OccasionalTipKiss".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.ClimaxRagingGrunt".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.MediocreDismissal".Localized());

            OutputText("Scenes.Kelly.BreakKeltGo.WalkAwayAfterUse".Localized());

            if (HasPinkEgg())
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10);
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 10);
            }
            else
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 15))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 15);
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 15);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SecondKeltBreaking()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.SecondKeltBreaking.StrollUpToKelt".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.PointAndLaugh".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.MockLittleSlut".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.BastardAccusation".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.RemindOfWhorePast".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.SneakyPrickThreat".Localized());

            OutputText("Scenes.Kelly.SecondKeltBreaking.FightBegins".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DefeatKellyNDBREAKHIM()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltMoansMantitsLust".Localized());
            else
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltGroansSlumpingDamage".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.CloseInEasyNow".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.RetreatThenTurnBack".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.WontLetHimSlip".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SpeedNoMatchSuffices".Localized());

                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltRearsAndFidgets".Localized());

                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.UnmountAndDragBarn".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeepUpAsCentaur".Localized());

                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KickThenRopeLoop".Localized());
            }

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltWalksMeeklyHauled".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FunnelInMouth".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ChokingInsteadOfReply".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.GirlBeautyPraise".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SobbingSlapWithCock".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.DemandAnswerKelly".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TearsAndCockslap".Localized());

            if (silly && AmilyFollowerCorrupt())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.AmilyHearsCumbuckets".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ShakesHeadSlappingStops".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.NameKellyAccepted".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SmileGrabHerHead".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PushHerUnderneath".Localized());
            else
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PullHerClose".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.YourErectCocks".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TasteLessonThenWork".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FasterThanLastTime".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FondlesBallsWithCare".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.LipsSuckBetter".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PullHeadToGroin".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.YouFragmentWord".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.CockHyphenSuffix".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.HalfHeartedSlap".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.MinistrationsArouse".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.GenitalsEquineBack".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TitsInsteadOfThroat".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ArticleAWord".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.QuickStepAnd".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.HeavyThrust".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.LaterFragmentWord".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.BreastValleyGrind".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TitfuckFasterQuiver".Localized());

            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SilentWalkAway".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            if (HasPinkEgg())
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5);
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 5);
            }
            else
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 10);
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 10);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel, 2);

            CombatScene.CleanupAfterCombat();
        }

        public void BreakingKeltNumeroThree()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SlumpingDown".Localized());
            else
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.MoaningAloud".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SubmitsWithoutFight".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ShameRuinedGirl".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ForceMouthBottlePour".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GoSomeplacePrivate".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.FollowsMeekly".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.TwoMoreVials".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.AsksHerName".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.RemainsSilentStill".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SometimesWithCock".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.YellNameQuestion".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GivesInAfterSlaps".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.StammersKellyName".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.WhatAreYouQuestion".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.CentauressAdmission".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.AsksHerPurpose".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SwearToSuckWhole".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LethiceImpatience".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ThinHandsFacefuck".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.TestsKellyOnceMore".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PushFromBehind".Localized());
            else
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PullWord".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PinHeadThroatShove".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BetterStillNotEnough".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.QuietThenBegs".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.EffortNotGenuine".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.MoveCockHerFace".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BriskReach".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.DownPushingHer".Localized());
            else
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GrabOneTit".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LustRisesConvulsively".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SecondBreastNeeded".Localized());
            else
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BucksAndWhinnies".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.KellyMoansErratic".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.FamiliarWarmthBuilds".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BallsChurn".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ClimaxApproaching".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SqueezeBoobsScream".Localized());
            else
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LetterY".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LoudGroanCumJets".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ApostropheBreasts".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.HeedlessOfHerPain".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.OrderToDrink".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SlowObeysKegForced".Localized());

            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BreastsExpandArousal".Localized());

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5);
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 5);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel, 3);

            CombatScene.CleanupAfterCombat();
        }

        public void FinalKeltBreaking()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.FinalKeltBreaking.ApproachFinalLesson".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.ReunionGreeting".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.CoyGrinQuestion".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.BegsForCum".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.TriumphantLaughter".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.TauntingBlowjobDemand".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.CriesInExcitement".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.FinalKeltBreaking.KneelsDown".Localized());
            else
                OutputText("Scenes.Kelly.FinalKeltBreaking.BendsOver".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.EagerCockSucking".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.FinalKeltBreaking.HandsCaressBalls".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.InsatiableHunger".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.SavoringPleasure".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.FinalKeltBreaking.HindlegHaunches".Localized());
            else
                OutputText("Scenes.Kelly.FinalKeltBreaking.ButtWord".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.RelentlessThroatfuck".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.FinalKeltBreaking.BackAway".Localized());
            else
                OutputText("Scenes.Kelly.FinalKeltBreaking.PushHerAway".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.PullOutOfMouth".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.TitfuckBegins".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.FinalKeltBreaking.GropeBountifulTits".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.BreastValleyTightness".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.CumslutBaptism".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresHerName".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.IdentityQuestion".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresCumslutRole".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.PurposeQuestion".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresPurpose".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.PraiseTakeItAll".Localized());

            if (silly)
                OutputText("Scenes.Kelly.FinalKeltBreaking.CumCannonShout".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.FamiliarPowerGathers".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.SpunkFountainErupts".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.CumflowEbbing".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.AppearanceUnchanged".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.OffersToLickClean".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.AwkwardAgreement".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.InstantObedience".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.FinalLessonSpeech".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.DeniesFormerSelf".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.AffirmsHerPlace".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.NeverReply".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.MasterPraise".Localized());

            OutputText("Scenes.Kelly.FinalKeltBreaking.DepartingButtPat".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 8f));

            CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel, 4);

            DoNext(() => ReturnToCamp(1));
        }

        public void KeltFucksShitUp()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltFucksShitUp.DefeatedKeltTaunts".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), KeltFucksShitUpII);
        }

        public void KeltFucksShitUpII()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltFucksShitUpII.WakingAtFarmEdge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KeltKillPlan) != 1)
            {
                OutputText("Scenes.Kelly.KeltFucksShitUpII.SuccubiMilkLoss".Localized());

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 5);

                CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel,

                    CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) - 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.KeltBreakLevel, 1);
            }

            CombatScene.CleanupAfterCombat();
        }

        public void KellyAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KellyAppearance.KellyBodyOverview".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor)
            ));

            OutputText("Scenes.Kelly.KellyAppearance.BraidLeverage".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor)
            ));

            OutputText("Scenes.Kelly.KellyAppearance.SoftBreastPair".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                OutputText("Scenes.Kelly.KellyAppearance.SecondTitRow".Localized());

            if (KellyIsPregnant)
                OutputText("Scenes.Kelly.KellyAppearance.LeakingMilkDrops".Localized());

            OutputText("Scenes.Kelly.KellyAppearance.GenitalLeadIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) == 0)
                OutputText("Scenes.Kelly.KellyAppearance.VirginDescriptor".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 0)
                OutputText("Scenes.Kelly.KellyAppearance.PinkPussy".Localized());
            else
                OutputText("Scenes.Kelly.KellyAppearance.BlackMareCunt".Localized());

            OutputText("Scenes.Kelly.KellyAppearance.DrippingMoisture".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyHeatTime) > 0 && !KellyIsPregnant)
                OutputText("Scenes.Kelly.KellyAppearance.SignsOfHeat".Localized());

            OutputText("Scenes.Kelly.KellyAppearance.HumanlikeAsshole".Localized());

            switch (KellyPregnancyEvent)
            {
                case 1:
                {
                OutputText("Scenes.Kelly.KellyAppearance.SlightBellyBulge".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Kelly.KellyAppearance.ObviousPregnancy".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Kelly.KellyAppearance.OffspringStirring".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Kelly.KellyAppearance.BloatedWithLife".Localized());
                }

                break;
            }

            if (NumKellyTattoos() > 0)
            {
                OutputText("Scenes.Kelly.KellyAppearance.ParagraphBreak".Localized());

                if (NumKellyTribalTats() == 4)
                    OutputText("Scenes.Kelly.KellyAppearance.TattooCoverage".Localized());
                else
                {
                    if (NumKellyTattoos() > 1)
                        OutputText("Scenes.Kelly.KellyAppearance.TattooListHeading".Localized());
                    else
                        OutputText("Scenes.Kelly.KellyAppearance.TattooListLead".Localized());

                    if (KellyTattooFlagSet(GameFlagIds.KellyTattooCollarbone))
                        OutputText(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooCollarbone));

                    if (KellyTattooFlagSet(GameFlagIds.KellyTattooShoulders))
                        OutputText(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooShoulders));

                    if (KellyTattooFlagSet(GameFlagIds.KellyTattooLowerback))
                        OutputText(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooLowerback));

                    if (KellyTattooFlagSet(GameFlagIds.KellyTattooButt))
                        OutputText(CoC.Instance.Flags.Get(GameFlagIds.KellyTattooButt));
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 0)
                if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) == 1)
                {
                    OutputText("Scenes.Kelly.KellyAppearance.OffspringPossessive".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1)
                        OutputText("Scenes.Kelly.KellyAppearance.MaleChild".Localized());
                    else
                        OutputText("Scenes.Kelly.KellyAppearance.FemaleChild".Localized());

                    OutputText("Scenes.Kelly.KellyAppearance.DistantChildField".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1)
                        OutputText("Scenes.Kelly.KellyAppearance.SonPlayingDistant".Localized());
                    else
                        OutputText("Scenes.Kelly.KellyAppearance.DaughterPlayingDistant".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) == 2)
                        OutputText("Scenes.Kelly.KellyAppearance.ChildrenPlaySounds".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.KellyKids))
                        ));
                    else
                        OutputText("Scenes.Kelly.KellyAppearance.ChildrenPlayEchoes".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.KellyKids))
                        ));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachKelly);
        }

        public void ApproachKelly()
        {
            ClearOutput();

            KellySprite();

            if (!KellyTattooFlagSet(GameFlagIds.KellyHairColor))
                CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "chestnut brown");

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesPunishedKelly) == 0 && Random.Shared.Next(3) == 0 && Player.HasCock())
            {
                CoC.Instance.Flags.Set(GameFlagIds.KellyDisobeyingCounter, 0);

                PunishKelly();

                return ;
            }

            if (View.Time.Hours >= 9 && View.Time.Hours <= 11 && CoC.Instance.Flags.Get(GameFlagIds.KellyKids) >= 2)
            {
                OutputText("Scenes.Kelly.ApproachKelly.BarnShadowNursing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 2)
                    OutputText("Scenes.Kelly.ApproachKelly.YoungestChildrenWord".Localized());

                OutputText("Scenes.Kelly.ApproachKelly.ChildrenWithWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                    OutputText("Scenes.Kelly.ApproachKelly.TopRowWord".Localized());

                OutputText("Scenes.Kelly.ApproachKelly.MilkLadenTits".Localized());

                OutputText("Scenes.Kelly.ApproachKelly.GrumpyChildComplaints".Localized());
            }
            else
                if (View.Time.Hours >= 15 && View.Time.Hours <= 16 && CoC.Instance.Flags.Get(GameFlagIds.KellyKids) >= 4)
                {
                    OutputText("Scenes.Kelly.ApproachKelly.KellyInFieldCenter".Localized());

                    OutputText("Scenes.Kelly.ApproachKelly.ComeBackLater".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
                else
                    OutputText("Scenes.Kelly.ApproachKelly.CanteringGreeting".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), KellyAppearance);

            if (Player.Lust >= 33)
                AddButton(1, "Common.Button.Sex".Localized(), KellySexMenu);
            else
            {
                OutputText("Scenes.Kelly.ApproachKelly.NotArousedEnough".Localized());

                AddButton(1, "Common.Button.Sex".Localized(), null);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 0)
            {
                if (Player.HasItem(ItemIds.Equinum))
                {
                    OutputText("Scenes.Kelly.ApproachKelly.EquinumGiftOption".Localized());

                    AddButton(5, "Common.Button.GiveEquinum".Localized(), GiveKellyEquinum);
                }
                else
                {
                    OutputText("Scenes.Kelly.ApproachKelly.EquinumLockedOption".Localized());

                    AddButton(5, "Common.Button.GiveEquinum".Localized(), null);
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                    {
                        OutputText("Scenes.Kelly.ApproachKelly.SuccubiMilkGift".Localized());

                        AddButton(5, "Common.Button.GiveSucmilk".Localized(), GiveKellySuccubiMilk);
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.ApproachKelly.SuccubiMilkLocked".Localized());

                        AddButton(5, "Common.Button.GiveSucmilk".Localized(), null);
                    }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.CaninePepper)))
            {
                OutputText("Scenes.Kelly.ApproachKelly.CaninePepperGift".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) == 0)
                    OutputText("Scenes.Kelly.ApproachKelly.UncertainTransformation".Localized());

                OutputText("Scenes.Kelly.ApproachKelly.SentencePeriod".Localized());

                AddButton(6, "Common.Button.GiveCaninep".Localized(), GiveKellyAPepper);
            }
            else
                AddButton(6, "Common.Button.GiveCaninep".Localized(), null);

            var beingGood = true;

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KellyDisobeyingCounter) >= 3 && Player.HasCock())
            {
                OutputText("Scenes.Kelly.ApproachKelly.PleasuringInYourAbsence".Localized());

                AddButton(7, "Common.Button.Punish".Localized(), PunishKelly);

                beingGood = false;
            }
            else
                AddButton(7, "Common.Button.Punish".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPunishedKelly) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KellyRewardCooldown) == 0 && Random.Shared.Next(3) == 0 && beingGood)
            {
                OutputText("Scenes.Kelly.ApproachKelly.GoodSpiritsReward".Localized());

                AddButton(8, "Common.Button.Reward".Localized(), RewardKelly);
            }
            else
                AddButton(8, "Common.Button.Reward".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 0)
                AddButton(10, "Common.Button.Race".Localized(), KellyRace);

            CoC.Instance.Flags.Set(GameFlagIds.KellyDisobeyingCounter, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 0)
                SetExitButton("Common.Button.Back".Localized());
            else
                AddButton(14, "Common.Button.Back".Localized(), FarmCorruption.RootScene);
        }

        public void KellySexMenu()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Kelly.KellySexMenu.SexMenuGreeting".Localized());

            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.HasCockThatFits(300) || CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                {
                    if (KellyIsPregnant)
                        AddButton(0, "Common.Button.PregFuck".Localized(), KellyPregSex);
                    else
                        if (!Player.IsTaur())
                            AddButton(0, "Common.Button.FuckCunt".Localized(), FuckKellysCunt);
                        else
                            AddButton(0, "Common.Button.FuckCunt".Localized(), TaurOnTaurSexKelly);

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) == 0)
                        AddButton(0, "Common.Button.Virginfuck".Localized(), TakeKellysVirginity);

                    if (Player.CountCocksOfType("TENTACLE") >= 2)
                        AddButton(1, "Common.Button.Tentafuck".Localized(), TentaFuckKelly);
                    else
                        AddButton(1, "Common.Button.Tentafuck".Localized(), null);
                }
                else
                    OutputText("Scenes.Kelly.KellySexMenu.TooBigForVagina".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) == 0 && Player.CockThatFits(18, "length") < 0 && !Player.IsTaur())
                    OutputText("Scenes.Kelly.KellySexMenu.TooBigForTits".Localized());
                else
                    if (!Player.IsTaur())
                        AddButton(2, "Common.Button.Titfuck".Localized(), KellyTitJob);
                    else
                        AddButton(2, "Common.Button.Titfuck".Localized(), null);

                AddButton(3, "Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), KellyBJsAhoy);

                AddButton(4, "Common.Button.TalkAndHj".Localized(), TalkNHandToKelly);
            }

            AddButton(14, "Common.Button.Back".Localized(), ApproachKelly);
        }

        public void FuckKellysCunt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) + 1);

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Kelly.FuckKellysCunt.StripOutOfArmor".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.KellyAsksEagerly".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.GestureTurnAround".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.SmackHindquarters".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.MoanSwishTail".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.PinchNippleWarning".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.DeclareSatisfactory".Localized());

            if (Player.Tallness < 72)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.TurnForStool".Localized());

                if (Player.Tallness < 52)
                    OutputText("Scenes.Kelly.FuckKellysCunt.DiminutiveWord".Localized());

                OutputText("Scenes.Kelly.FuckKellysCunt.HeightHindersReach".Localized());
            }

            OutputText("Scenes.Kelly.FuckKellysCunt.PressAgainstEntrance".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.OtherErection".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Kelly.FuckKellysCunt.HangSuffix".Localized());
                else
                    OutputText("Scenes.Kelly.FuckKellysCunt.HangsWord".Localized());

                OutputText("Scenes.Kelly.FuckKellysCunt.DrippingsAcrossBelly".Localized());
            }

            OutputText("Scenes.Kelly.FuckKellysCunt.SlideInsideTunnel".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.FuckKellysCunt.MoanUnrepentant".Localized());

            if (Player.CockArea(x) >= 300)
                OutputText("Scenes.Kelly.FuckKellysCunt.BottomOut".Localized());
            else
                OutputText("Scenes.Kelly.FuckKellysCunt.SlideDeepInside".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.SmackAssWarmth".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.ImpaledMoan".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.FuckKellysCunt.LouderMoans".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.FamiliarThrob".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.FuckKellysCunt.RoilingBalls".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.ClimaxApproaching".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) > 0)
                OutputText("Scenes.Kelly.FuckKellysCunt.FragrantWord".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.CumSoaksLegs".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.FuckKellysCunt.JismSprays".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Kelly.FuckKellysCunt.SoggySexToy".Localized());

            if (Player.CumQ() >= 1200)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.SpunkRoundsBelly".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Kelly.FuckKellysCunt.PregnantWithSpooge".Localized());

                OutputText("Scenes.Kelly.FuckKellysCunt.SentencePeriod".Localized());
            }

            OutputText("Scenes.Kelly.FuckKellysCunt.LayPantingOnBack".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.LustSpentPraise".Localized());

            if (!KellyIsPregnant)
                OutputText("Scenes.Kelly.FuckKellysCunt.BroodmareTransformation".Localized());
            else
                OutputText("Scenes.Kelly.FuckKellysCunt.KnitForKids".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.SentencePeriodAgain".Localized());

            OutputText("Scenes.Kelly.FuckKellysCunt.DismountWithSpring".Localized());

            Player.Orgasm("Dick");

            KellyPreggers();

            DoNext(() => ReturnToCamp(1));
        }

        public void TaurOnTaurSexKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) + 1);

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.RemoveLowerArmor".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TeaseFromFront".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.CommandLieDown".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SackToken".Localized());
            else
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TaintWord".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PressIntoFace".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BallsTingle".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.LewdPopAway".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SLetter".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.GestureRiseAndTurn".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TooTallToStand".Localized());
            else
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.LiftTail".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.MountWithLurch".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PlungeDeepInside".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 0)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PinkPussy".Localized());
            else
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BlackHorseCunt".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SheathInCanal".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.CantSeeHerFace".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SackTokenAgain".Localized());
            else
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.HindlegsWord".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SplatterFreshWaves".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) > 0)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.MuskyWord".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.AssertControl".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BegForFoal".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            if (Player.CumQ() >= 1000)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BellyGrowsRound".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Kelly.TaurOnTaurSexKelly.OverfilledSpillage".Localized());

                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PeriodSpace".Localized());
            }

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.DismountCumDump".Localized());

            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SlipInMud".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.HelpHerUp".Localized());
            else
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.WalkAwaySnickering".Localized());

            KellyPreggers();

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void TentaFuckKelly()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.TentaFuckKelly.RevealTentacleCocks".Localized());

            var one = -1;

            var two = -2;

            var three = -3;

            var four = -4;

            var five = -5;

            var temp = 0;

            while (temp < Player.Cocks.Count)
            {
                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle"))
                    if (one < 0)
                        one = temp;
                    else
                        if (two < 0)
                            two = temp;
                        else
                            if (three < 0)
                                three = temp;
                            else
                                if (four < 0)
                                    four = temp;
                                else
                                    if (five < 0)
                                        five = temp;

                temp += 1;
            }

            OutputText("Scenes.Kelly.TentaFuckKelly.ShoveIntoMouth".Localized(
                Player.CockDescript(one),

                Player.CockDescript(one)
            ));

            OutputText("Scenes.Kelly.TentaFuckKelly.SkilledMouthWork".Localized(
                Player.CockDescript(two),

                Player.CockDescript(two)
            ));

            OutputText("Scenes.Kelly.TentaFuckKelly.ReamBothSides".Localized(
                Player.CockDescript(one)
            ));

            if (three >= 0)
                OutputText("Scenes.Kelly.TentaFuckKelly.TremblingDisplay".Localized(
                    Player.CockDescript(three),

                    Player.CockDescript(three)
                ));

            if (four >= 0)
                OutputText("Scenes.Kelly.TentaFuckKelly.TentacleTrembles".Localized(
                    Player.CockDescript(four),

                    Player.CockDescript(three),

                    Player.CockDescript(four)
                ));

            if (five >= 0)
                OutputText("Scenes.Kelly.TentaFuckKelly.TentacleFindsHand".Localized(
                    Player.CockDescript(five)
                ));

            if (Player.CountCocksOfType("TENTACLE") > 5)
                OutputText("Scenes.Kelly.TentaFuckKelly.WrapAroundTentacle".Localized());

            OutputText("Scenes.Kelly.TentaFuckKelly.PressureBuilds".Localized(
                Player.CockDescript(one),

                Player.CockDescript(two)
            ));

            if (three >= 0)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.RemainingTentacle".Localized());

                if (four >= 0)
                    OutputText("Scenes.Kelly.TentaFuckKelly.EruptSuffix".Localized());
                else
                    OutputText("Scenes.Kelly.TentaFuckKelly.EruptWord".Localized());

                OutputText("Scenes.Kelly.TentaFuckKelly.UnholyFountain".Localized());
            }

            OutputText("Scenes.Kelly.TentaFuckKelly.CumShowerEnds".Localized());

            OutputText("Scenes.Kelly.TentaFuckKelly.CumBathPraise".Localized());

            OutputText("Scenes.Kelly.TentaFuckKelly.WallowInPuddle".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Kelly.TentaFuckKelly.DisgustedWatching".Localized());

            OutputText("Scenes.Kelly.TentaFuckKelly.LeaveHerEnjoying".Localized());

            KellyPreggers();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveKellyEquinum()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.GiveKellyEquinum.WickedVialIdea".Localized());

            OutputText("Scenes.Kelly.GiveKellyEquinum.NervousQuestion".Localized());

            OutputText("Scenes.Kelly.GiveKellyEquinum.HushPetReassurance".Localized());

            OutputText("Scenes.Kelly.GiveKellyEquinum.DemureReply".Localized());

            OutputText("Scenes.Kelly.GiveKellyEquinum.SmackBodaciousRump".Localized());

            OutputText("Scenes.Kelly.GiveKellyEquinum.DisappointedMoan".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KellyCuntType, 1);

            OutputText("Scenes.Kelly.GiveKellyEquinum.SoakingWetHorsecunt".Localized());

            Player.ConsumeItem(ItemIds.Equinum);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachKelly);
        }

        public void GiveKellySuccubiMilk()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.DangleSuccubiMilk".Localized());

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.UncorkAndHand".Localized());

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.ShrinkToPetite".Localized());

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.ThankForTreat".Localized());

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.WithholdPraise".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KellyCuntType, 0);

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.HumanlikePussy".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachKelly);
        }

        public void PunishKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesPunishedKelly, CoC.Instance.Flags.Get(GameFlagIds.TimesPunishedKelly) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPunishedKelly) == 1)
            {
                OutputText("Scenes.Kelly.PunishKelly.SilentAtHalt".Localized());

                OutputText("Scenes.Kelly.PunishKelly.TraceAndSpeak".Localized());

                OutputText("Scenes.Kelly.PunishKelly.MoanAsYouCircle".Localized());

                OutputText("Scenes.Kelly.PunishKelly.LaughWantonBitch".Localized());

                OutputText("Scenes.Kelly.PunishKelly.GaspingPlea".Localized());

                OutputText("Scenes.Kelly.PunishKelly.GentleGoOn".Localized());

                OutputText("Scenes.Kelly.PunishKelly.HesitantConfession".Localized());

                OutputText("Scenes.Kelly.PunishKelly.ReliefAtTruth".Localized());

                OutputText("Scenes.Kelly.PunishKelly.HumbleRequest".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.PunishKelly.GuiltOnHerFace".Localized());

                OutputText("Scenes.Kelly.PunishKelly.AskBadCumslut".Localized());

                OutputText("Scenes.Kelly.PunishKelly.WhisperedYes".Localized());

                OutputText("Scenes.Kelly.PunishKelly.AskNeedPunishing".Localized());

                OutputText("Scenes.Kelly.PunishKelly.LouderYes".Localized());
            }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenKellyForPunishment) > 0 && !TelAdreAllowedInto())
                OutputText("Scenes.Kelly.PunishKelly.LackingRideIdea".Localized());
            else
                AddButton(1, "Common.Button.Ride".Localized(), RideKellyForPunishment);

            AddButton(0, "Common.Button.Rimjob".Localized(), GetARimjobFromKelly);

            AddButton(14, "Common.Button.Back".Localized(), ApproachKelly);
        }

        public void GetARimjobFromKelly()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesRimJobbedByKelly, CoC.Instance.Flags.Get(GameFlagIds.TimesRimJobbedByKelly) + 1);

            OutputText("Scenes.Kelly.GetARimjobFromKelly.PeelOffArmor".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.SlitherWriggleUp".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.GestureGroundLieDown".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.GestureLieCarefully".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Kelly.GetARimjobFromKelly.FlareAndPresent".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.UnpracticedTeeth".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.LeanBackSlowly".Localized());
            else
                if (!Player.IsTaur())
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.PressHerHead".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.LaughAtTongue".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.SmirkGripHair".Localized());
            else
                OutputText("Scenes.Kelly.GetARimjobFromKelly.GyrateButt".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.ThinFingersBrush".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.TongueProbesDeeper".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.GetARimjobFromKelly.BreatheGoodGirl".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kelly.GetARimjobFromKelly.HardenSuffix".Localized());
            else
                OutputText("Scenes.Kelly.GetARimjobFromKelly.HardenWord".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.BlissfulBulge".Localized());

            if (Player.IsNaga())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.BoneCrushingClench".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.SighPraise".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kelly.GetARimjobFromKelly.AndBallsToken".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.AndPussyToken".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.DeservedShamelessSlut".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.HarshWordsSpur".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.FingersFrigYou".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.GruntingPeak".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Kelly.GetARimjobFromKelly.TandemOrgasm".Localized());

            OutputText("Scenes.Kelly.GetARimjobFromKelly.FinalThrusts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesRimJobbedByKelly) == 1)
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.ShakeAtIgnorance".Localized());

                OutputText("Scenes.Kelly.GetARimjobFromKelly.WaveAtRivulets".Localized());

                if (Player.CumQ() >= 700)
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.OozingLake".Localized());

                OutputText("Scenes.Kelly.GetARimjobFromKelly.KellyPauses".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.SheStaysAwhile".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.KnowsTheDrill".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.SheStaysAwhileAgain".Localized());
            }

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RideKellyForPunishment()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesRiddenKellyForPunishment, CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenKellyForPunishment) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenKellyForPunishment) == 1)
            {
                OutputText("Scenes.Kelly.RideKellyForPunishment.StrokeKellySide".Localized());

                if (Player.Tallness < 78 && !Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.SwingOntoBack".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Kelly.RideKellyForPunishment.IdealRidingHalf".Localized());
                    else
                        if (Player.IsDrider())
                            OutputText("Scenes.Kelly.RideKellyForPunishment.SpiderHalfCompromise".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.LeanInClose".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusTitRows) > 0)
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TopRowOfPrefix".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.BreastsWord".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.OrderRideToFarm".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyLactating) > 0)
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ThinMilkArcs".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.RaggedBreathGasp".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShortTripSore".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ConsiderBeingRidden".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.UselessSteedWhisper".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyLactating) > 0)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusTitRows) > 0)
                            OutputText("Scenes.Kelly.RideKellyForPunishment.AllFourOfPrefix".Localized());
                        else
                            OutputText("Scenes.Kelly.RideKellyForPunishment.HLetter".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.SwollenTeatsMilk".Localized());
                    }

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ServeAnotherWay".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Kelly.RideKellyForPunishment.MarchGait".Localized());
                    else
                        OutputText("Scenes.Kelly.RideKellyForPunishment.CanterGait".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.FollowsAfterPause".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveBehindBarn".Localized());
                }

                OutputText("Scenes.Kelly.RideKellyForPunishment.FindSmallerBarn".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.WideEyedPensiveness".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.AskRiddenTaboo".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.TabooAnswerSigh".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.SuggestOwnedHarness".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.AssInsultLoud".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.PlaceBitTeeth".Localized());

                if (Player.Tallness < 78 && !Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdmireBoundSlut".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.GiddyUpOnceAround".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.SetsOffCanter".Localized());

                    if (Player.IsBiped())
                        OutputText("Scenes.Kelly.RideKellyForPunishment.FeetIntoStirrups".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBreezeRide".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCamp".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Kelly.RideKellyForPunishment.FeveredFucking".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdmireBoundSlutAgain".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShowPonyOrder".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.LovelyDayBreeze".Localized());

                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCampAgain".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.FeveredFuckingAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCampThird".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.WishLiveForever".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                }

                KellyPreggers();

                OutputText("Scenes.Kelly.RideKellyForPunishment.WithdrawCatchBreath".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.GoodLessonPant".Localized());

                OutputText("Scenes.Kelly.RideKellyForPunishment.LuckyToHaveYou".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenKellyForPunishment) == 2)
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.StrideToStones".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.PleaseNoLesson".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShushAssCalm".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.FixOnMantle".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusTitRows) > 0)
                        OutputText("Scenes.Kelly.RideKellyForPunishment.JustAsWellFour".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.StepBackAdmire".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdorableButHidden".Localized());

                    if (Player.Tallness < 78 && !Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.HeadTowardsTown".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrival".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LearningLessonGuard".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreets".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AskBeforeOwned".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.CosmeticsRibbonsBox".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLook".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.GoodGirlRibbon".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveTelAdreSaddled".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.GrabReignsStride".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LearningLessonGuardAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AskBeforeOwnedAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.CosmeticsRibbonsBoxAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookAgain".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.GoodGirlRibbonAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveTelAdreWalking".Localized());
                    }

                    OutputText("Scenes.Kelly.RideKellyForPunishment.HumiliationWetGag".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBobbingHead".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyEncapsulatedMouth".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Kelly.RideKellyForPunishment.PromptForThanks".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.DevotedThankYou".Localized());

                    KellyPreggers();
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.StrideToStonesAgain".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.PleaseNoLessonAgain".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShushAssCalmAgain".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.DifficultAboutThis".Localized());

                    if (Player.Tallness < 78 && !Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.KnowWhereGoing".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalThird".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsThird".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.WipeTearsBlinkers".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.SexShopUnknownBox".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookThird".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LoosenForFuck".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtBoxRibbon".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveSaddledAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.JerkReignsOrder".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalFourth".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsFourth".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.WipeTearsBlinkersAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtUnknownBoxAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookFourth".Localized(
                            Player.CockDescript(x)
                        ));

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LoosenForFuckAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtBoxRibbonAgain".Localized());

                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveLeadingKelly".Localized());
                    }

                    OutputText("Scenes.Kelly.RideKellyForPunishment.HumiliationWetGagAgain".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBobbingHeadAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyEncapsulatedMouthAgain".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Kelly.RideKellyForPunishment.PromptForThanksAgain".Localized());

                    OutputText("Scenes.Kelly.RideKellyForPunishment.DevotedThankYouAgain".Localized());
                }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, .5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeKellysVirginity()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Kelly.TakeKellysVirginity.ClaimPrizePromise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                OutputText("Scenes.Kelly.TakeKellysVirginity.HorsePrefix".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.CuntWord".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.CentaurFidgetsNervously".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.MareSexToySpeech".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.LieDownCommand".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyLowersMeekly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                OutputText("Scenes.Kelly.TakeKellysVirginity.LustSwollenFuckHole".Localized());
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.BloomingPinkPussy".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.GoodQuestion".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.KneelAndTest".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                OutputText("Scenes.Kelly.TakeKellysVirginity.EquineWord".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.HymenVisibleStroke".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                OutputText("Scenes.Kelly.TakeKellysVirginity.SqueezeOnyxVulva".Localized());
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.FondleSlickLabia".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.MolestingEquestrianToy".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyMoansSensitive".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.FourLeggedSlutCries".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.AnswerWithSmack".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.ClaimMaidenhead".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.TakeKellysVirginity.LineUpBehindHer".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.SlideHindlegsBehind".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.FirstContactThrill".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.SqueezeKelly".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kelly.TakeKellysVirginity.SnuggleBehindBreast".Localized());
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.AssWord".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.LaughThenSpear".Localized());

            if (Player.CockArea(x) <= 300)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.NestleSheathAgainst".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                    OutputText("Scenes.Kelly.TakeKellysVirginity.PlumpEquineLips".Localized());
                else
                    OutputText("Scenes.Kelly.TakeKellysVirginity.SlipperyPinkLips".Localized());
            }
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.PressCervixBarrier".Localized(
                    Player.CockHead(x)
                ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.PinkishFrothOozes".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.PullTailAsk".Localized(
                Player.Mf("man", "breeder")
            ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyEyesCross".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.DefloweringMemoryFades".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.KellySquirmsShuddering".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.TakeKellysVirginity.GloryingRelease".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.JismIntoWomb".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Kelly.TakeKellysVirginity.BellyRoundsWithSpunk".Localized());

                if (Player.CumQ() >= 3500)
                    OutputText("Scenes.Kelly.TakeKellysVirginity.SeedFurther".Localized());
            }

            OutputText("Scenes.Kelly.TakeKellysVirginity.PullOutSigh".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 0)
                OutputText("Scenes.Kelly.TakeKellysVirginity.WellUsedRedness".Localized());
            else
                OutputText("Scenes.Kelly.TakeKellysVirginity.ShiningJuicesDrip".Localized());

            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyPassedOut".Localized());

            KellyPreggers();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, .5f));

            CoC.Instance.Flags.Set(GameFlagIds.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(GameFlagIds.KellyVaginallyFuckedCount) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveKellyAPepper()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTimesPeppered) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAPepper.OpenWideInstruction".Localized());

                OutputText("Scenes.Kelly.GiveKellyAPepper.SpicyTasteMoan".Localized());

                OutputText("Scenes.Kelly.GiveKellyAPepper.FurtherPepperChanges".Localized());

                OutputText("Scenes.Kelly.GiveKellyAPepper.MasterPleaseMoan".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KellyBonusBoobRows, 1);

                CoC.Instance.Flags.Set(GameFlagIds.KellyHeatTime, 48);

                KellySprite();
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAPepper.GrinDanglePepper".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) == 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAPepper.FleshBurgeonsMoan".Localized());

                    OutputText("Scenes.Kelly.GiveKellyAPepper.FurtherPepperChangesAgain".Localized());

                    OutputText("Scenes.Kelly.GiveKellyAPepper.MasterPleaseMoanAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAPepper.NoOutwardEffects".Localized());

                    OutputText("Scenes.Kelly.GiveKellyAPepper.FuckMeWhimper".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.KellyHeatTime, CoC.Instance.Flags.Get(GameFlagIds.KellyHeatTime) + 48);
            }

            CoC.Instance.Flags.Set(GameFlagIds.KellyTimesPeppered, CoC.Instance.Flags.Get(GameFlagIds.KellyTimesPeppered) + 1);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.CaninePepper));

            DoNext(() => ReturnToCamp(1));
        }

        public void KellyTitJob()
        {
            ClearOutput();

            int x = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) == 0)
                x = Player.CockThatFits(18, "length");
            else
                x = Player.BiggestCockIndex();

            OutputText("Scenes.Kelly.KellyTitJob.BreastsBounceApproach".Localized());

            if (KellyIsPregnant)
                OutputText("Scenes.Kelly.KellyTitJob.MilkBeadsFlicking".Localized());

            OutputText("Scenes.Kelly.KellyTitJob.WeightReminderBody".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kelly.KellyTitJob.MasterSeesSomething".Localized());

            OutputText("Scenes.Kelly.KellyTitJob.KneelGruntCommand".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length <= 18)
            {
                OutputText("Scenes.Kelly.KellyTitJob.HoldShouldersMove".Localized(
                    Player.CockDescript(x)
                ));

                if (KellyIsPregnant)
                    OutputText("Scenes.Kelly.KellyTitJob.MilkDrizzleTaste".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Kelly.KellyTitJob.DeepConcentration".Localized());

                OutputText("Scenes.Kelly.KellyTitJob.RollsTongueMoan".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                    OutputText("Scenes.Kelly.KellyTitJob.HipsPressLowerTits".Localized());

                OutputText("Scenes.Kelly.KellyTitJob.TongueFlicksOverhead".Localized());

                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Kelly.KellyTitJob.AdmireHandiwork".Localized());
                else
                    OutputText("Scenes.Kelly.KellyTitJob.CoatWithPotency".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyTitJob.PrickDownTorso".Localized());

                OutputText("Scenes.Kelly.KellyTitJob.SaySlavesMust".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kelly.KellyTitJob.SighClaspTopBreasts".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kelly.KellyTitJob.ExpertWristRoll".Localized());

                if (KellyIsPregnant)
                    OutputText("Scenes.Kelly.KellyTitJob.MilkDrizzleTasteAgain".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Kelly.KellyTitJob.DeepConcentrationAgain".Localized());

                OutputText("Scenes.Kelly.KellyTitJob.RollsTongueMoanAgain".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Kelly.KellyTitJob.AdmireHandiworkAgain".Localized());
                else
                    OutputText("Scenes.Kelly.KellyTitJob.CoatWithPotencyAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KellyPreggers()
        {

            if (KellyKnockUpChance(CoC.Instance.Flags.Get(GameFlagIds.KellyHeatTime) > 0 ? 15 : 1, 80))
                KellyKnockUp(KellyPregnancyPlayerFather, PregnancyIds.CentaurIncubation - 84);
        }

        public void KellyPregSex()
        {
            ClearOutput();

            int x = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyCuntType) == 1)
                x = Player.BiggestCockIndex();
            else
                x = Player.CockThatFits(300);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Kelly.KellyPregSex.PregnancyScentPussy".Localized());

            OutputText("Scenes.Kelly.KellyPregSex.RemoveArmorCocksGrow".Localized());

            OutputText("Scenes.Kelly.KellyPregSex.CentauressScreamsArches".Localized());

            if (Player.Cocks[x].Id != BodyPartTypes.Horse)
                OutputText("Scenes.Kelly.KellyPregSex.GetsYoursInstead".Localized());

            OutputText("Scenes.Kelly.KellyPregSex.PoundScentedSlut".Localized());

            OutputText("Scenes.Kelly.KellyPregSex.BloatedBellySways".Localized());

            OutputText("Scenes.Kelly.KellyPregSex.GraspHipsContractions".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -4f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KellyPopsOutARunt()
        {

            OutputText("Scenes.Kelly.KellyPopsOutARunt.BarnVisitSurprise".Localized());

            OutputText("Scenes.Kelly.KellyPopsOutARunt.KellyHaystackNewborn".Localized());

            OutputText("Scenes.Kelly.KellyPopsOutARunt.SayHiToKid".Localized());

            var gender = Random.Shared.Next(2) + 1;

            OutputText("Scenes.Kelly.KellyPopsOutARunt.ApproachFoalView".Localized());

            if (gender == (int)Gender.Female)
                OutputText("Scenes.Kelly.KellyPopsOutARunt.HerWord".Localized());
            else
                OutputText("Scenes.Kelly.KellyPopsOutARunt.HisWord".Localized());

            OutputText("Scenes.Kelly.KellyPopsOutARunt.MotherMilk".Localized());

            if (gender == (int)Gender.Female)
                OutputText("Scenes.Kelly.KellyPopsOutARunt.CuteProtrusionsDevelop".Localized());
            else
                OutputText("Scenes.Kelly.KellyPopsOutARunt.NoticeFoalNub".Localized());

            OutputText("Scenes.Kelly.KellyPopsOutARunt.OffspringGrowStrong".Localized());

            if (Player.Corruption > 80)
                OutputText("Scenes.Kelly.KellyPopsOutARunt.PicturingGrownKids".Localized());

            OutputText("Scenes.Kelly.KellyPopsOutARunt.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.KellyFirstKidGender, gender);

            CoC.Instance.Flags.Set(GameFlagIds.KellyKids, CoC.Instance.Flags.Get(GameFlagIds.KellyKids) + 1);

            if (gender == (int)Gender.Male)
                CoC.Instance.Flags.Set(GameFlagIds.KellyKidsMale, CoC.Instance.Flags.Get(GameFlagIds.KellyKidsMale) + 1);
        }

        public void TalkNHandToKelly()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTalkNHandTimes) == 0)
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.PurrsLeastInterestingThing".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.HelpsRemoveLowerArmor".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.GentlyTugsQuicklyHardens".Localized());
                    else
                        OutputText("Scenes.Kelly.TalkNHandToKelly.TugsBothCocksHard".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.ClopsRearTowardKelly".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.WarmHandsAround".Localized());
                    else
                        OutputText("Scenes.Kelly.TalkNHandToKelly.HandWrapsBoth".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.TellsHerRatherTalk".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.GrinningCentaurSlave".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.SlidesOffLowerArmor".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.LetterS".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.SighsAsShePumps".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.CocksThey".Localized());
                    else
                        OutputText("Scenes.Kelly.TalkNHandToKelly.CockIt".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.RapidStiffening".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.TrotsBehindYou".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.WarmHandsAroundAgain".Localized());
                    else
                        OutputText("Scenes.Kelly.TalkNHandToKelly.HandWrapsBothAgain".Localized());
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.KellyTalkNHandTimes, CoC.Instance.Flags.Get(GameFlagIds.KellyTalkNHandTimes) + 1);

            var temp = Random.Shared.Next(5);

            if (temp == 0)
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutWives".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.KellyStopsPumping".Localized());

                if (!Player.HasVagina())
                    OutputText("Scenes.Kelly.TalkNHandToKelly.NowWorksForMan".Localized());
                else
                    OutputText("Scenes.Kelly.TalkNHandToKelly.NowWorksForWoman".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.LaughsMusically".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kelly.TalkNHandToKelly.ImaginesEveryCockHole".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Kelly.TalkNHandToKelly.ImaginesTongueFucking".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.SymphonyOfSqueals".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.ClearerBloodSettled".Localized());

                if (Player.Corruption < 70)
                    OutputText("Scenes.Kelly.TalkNHandToKelly.KeltVictimsUnsettle".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.ThanksKellyButLeaves".Localized());

                OutputText("Scenes.Kelly.TalkNHandToKelly.ThanksMasterSoMuch".Localized());
            }
            else
                if (temp == 1)
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.AsksToMoveIn".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.MoveInOnceRepaid".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.AsksWhatPaidMeans".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.TroubleBringingTrueSelf".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.WhispersHandsSliding".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kelly.TalkNHandToKelly.SecondCockWord".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.CannotRepayThoughtless".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.EyesClosedEnjoyingWaves".Localized());

                    OutputText("Scenes.Kelly.TalkNHandToKelly.AdoringGreenEyes".Localized());
                }
                else
                    if (temp == 2)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.AsksCentaursSubDom".Localized());

                        OutputText("Scenes.Kelly.TalkNHandToKelly.NotLikeTheDemons".Localized());

                        if (!Player.IsTaur())
                            OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAir".Localized());
                        else
                            OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGround".Localized());

                        OutputText("Scenes.Kelly.TalkNHandToKelly.NuzzlesCocksWhenDone".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Kelly.TalkNHandToKelly.LetterSAgain".Localized());

                        OutputText("Scenes.Kelly.TalkNHandToKelly.SlapsAssAndLeaves".Localized());
                    }
                    else
                        if (temp == 3)
                        {
                            OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutAura".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.LongStoryMoveHands".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.TracesBulgingVein".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.PausesBeforeBending".Localized());

                            if (Player.Balls == 0)
                                OutputText("Scenes.Kelly.TalkNHandToKelly.SucklesBaseOfCock".Localized());
                            else
                                OutputText("Scenes.Kelly.TalkNHandToKelly.SucklesAtBalls".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.BeautifulCoolingTactic".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.ContinuesLongStory".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.StrugglesToFocus".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.SmackingLipsFedTwo".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 0)
                                OutputText("Scenes.Kelly.TalkNHandToKelly.BearingRaisingChildren".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.HundredTimesHappier".Localized());

                            OutputText("Scenes.Kelly.TalkNHandToKelly.GlowOfGratitude".Localized());
                        }
                        else
                            if (!WhitneyCorrupt())
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutWhitney".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.HardNoteInVoice".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.ArchesNeckSensation".Localized());

                                if (!Player.IsTaur())
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAirAgain".Localized());
                                else
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGroundAgain".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.SentencePeriod".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.WantsHerGroveling".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.ExcusesHerselfBendsIn".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.TrillsDrawingSyllable".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.RepliesStifflyDenial".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.CallsHimMeanImagines".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.KnowsWhatSheDoing".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.TensesAndCums".Localized());

                                if (!Player.IsTaur())
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAirThird".Localized());
                                else
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGroundThird".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.WoggleCoilsThroughOrgasm".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.WhitneyIsRightHand".Localized());

                                OutputText("Scenes.Kelly.TalkNHandToKelly.InnocentlyAgreesRightHand".Localized());
                            }

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void RewardKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KellyRewardCooldown, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTimesRewarded) == 0)
            {
                OutputText("Scenes.Kelly.RewardKelly.AddedBounceToCanter".Localized());

                OutputText("Scenes.Kelly.RewardKelly.ColorHighInCheeks".Localized());

                OutputText("Scenes.Kelly.RewardKelly.DifficultNotToLaugh".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.RewardKelly.HoldsHeadUpProudly".Localized());

                OutputText("Scenes.Kelly.RewardKelly.CheerfulHurray".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.KellyTimesRewarded, CoC.Instance.Flags.Get(GameFlagIds.KellyTimesRewarded) + 1);

            ClearButtons();

            if (Player.HasCockThatFits(300))
                AddButton(0, "Common.Button.Applesauce".Localized(), GiveKellyAppleSauce);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.HairDyeBlack)) && CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor) != "sable black")
                AddButton(1, "Items.BlackDye.Short".Localized(), () => DyeKellysBitchAssHair(ConsumableIds.HairDyeBlack));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.HairDyeBlond)) && CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor) != "slutty blonde")
                AddButton(2, "Items.BlondDye.Short".Localized(), () => DyeKellysBitchAssHair(ConsumableIds.HairDyeBlond));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.HairDyePurple)) && CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor) != "garish purple")
                AddButton(3, "Items.PurpleDye.Short".Localized(), () => DyeKellysBitchAssHair(ConsumableIds.HairDyePurple));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.HairDyePink)) && CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor) != "bright pink")
                AddButton(4, "Items.PinkDye.Short".Localized(), () => DyeKellysBitchAssHair(ConsumableIds.HairDyePink));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.HairDyeBrown)) && CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor) != "chestnut brown")
                AddButton(5, "Items.BrownDye.Short".Localized(), () => DyeKellysBitchAssHair(ConsumableIds.HairDyeBrown));

            AddButton(14, "Common.Button.Back".Localized(), ApproachKelly);
        }

        public void DyeKellysBitchAssHair(Identifier color)
        {
            ClearOutput();

            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.BringsHairDyeGift".Localized());

            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.AgesSinceDidHair".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTimesDiedHair) == 0)
                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.NeverDoneHairBefore".Localized());

            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.FunWalkToBarn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KellyTimesDiedHair, CoC.Instance.Flags.Get(GameFlagIds.KellyTimesDiedHair) + 1);

            if (color == ConsumableIds.HairDyeBlack)
            {
                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsJetBlackHair".Localized());

                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SaysLooksSevere".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "sable black");

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBlack));
            }
            else
                if (color == ConsumableIds.HairDyeBlond)
                {
                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsFlaxenHair".Localized());

                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.FeelsVeryRight".Localized());

                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.PurrsRetyingBraid".Localized());

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBlond));

                    CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "slutty blonde");
                }
                else
                    if (color == ConsumableIds.HairDyePurple)
                    {
                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsPurpleHair".Localized());

                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.ServesAsReminder".Localized());

                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.HateToForgetReward".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "garish purple");

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyePurple));
                    }
                    else
                        if (color == ConsumableIds.HairDyePink)
                        {
                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SmilesAtPinkHair".Localized());

                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.AgreesFeelsGirly".Localized());

                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.ThanksForReward".Localized());

                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyePink));

                            CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "bright pink");
                        }
                        else
                            if (color == ConsumableIds.HairDyeBrown)
                            {
                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SighsTwistingLock".Localized());

                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.OfCourseThankYou".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.KellyHairColor, "chestnut brown");

                                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.HairDyeBrown));
                            }
                            else
                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.BugReportPlaceholder".Localized(color));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachKelly);
        }

        public void GiveKellyAppleSauce()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTimesApplesauced) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TapChinAskLikes".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.FurrowsBrowRecalling".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.JerkOffWellSpent".Localized());

                if (!WhitneyCorrupt())
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WantsCheckOrchard".Localized());
                else
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WantsCheckOrchardFretfully".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.DismissWhitneyAirily".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.CarefulPromisePlea".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WildAppleTrees".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.StoneMashApples".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.StripArmorStroke".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasm".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.BowlBrimsOver".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.PositionOverBowl".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmAgain".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.CockTorrentsOut".Localized());
                }

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.EnjoyAfterglowGlade".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.LaughAtHerWorry".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BlowThousandTimes".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.FetchMetalBowl".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.GroveTransformed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ClearedOblongPatch".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PickBestApples".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GraspCockReverie".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmOnce".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.BowlBrimsOverAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.PositionOverBowlReverie".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmTwice".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.CockTorrentsOutAgain".Localized());
                }

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.EnjoyAfterglowGladeAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.AsksFairGift".Localized());

                if (silly)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ParsleySageRosemary".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SlapAssPassing".Localized());
            }

            var x = Player.CockThatFits(300);

            var y = Player.CockThatFits2(300);

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SmellGetsHerWet".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SetHandsOnBowl".Localized(
                    Player.CockDescript(x),

                    CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor)
                ));

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.RadiatingHeatInside".Localized(
                    Player.CockHead(x),

                    Player.CockDescript(x)
                ));

                if (Player.Cocks[x].Thickness >= 4)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GirthSpreadsTunnel".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KellyGroansArching".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsToken".Localized());
                else
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThighsWord".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WondersIfSheCame".Localized());

                if (y >= 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.EyesOnHerAsshole".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));

                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WaitHeadThenThrust".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));
                }

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MeasuredDriving".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BraidLeverage".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsSlappingThighs".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PushTowardsReward".Localized(
                    Player.CockDescript(x)
                ));

                if (y >= 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.AnusClenchesAround".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WordlessShoutOrgasm".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SmellWetsHerAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TutAndWithhold".Localized(
                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.HoldBackAtEntrance".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Cocks[x].Thickness >= 4)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GirthSpreadsTunnelAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KellyGroansArchingAgain".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsTokenAgain".Localized());
                else
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThighsWordAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WondersIfCameAgain".Localized());

                if (y >= 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThinkOfHerAsshole".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y),

                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MeasuredDrivingAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BraidForceBowl".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.KellyHairColor)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsSlappingThighsAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PushTowardsRewardAgain".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PantingKellyThrustsBack".Localized(
                    Player.CockDescript(x)
                ));

                if (y >= 0)
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.AnusClenchesAroundAgain".Localized(
                        Player.CockDescript(y)
                    ));

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WordlessShoutOrgasmAgain".Localized());
            }

            OutputText("Scenes.Kelly.GiveKellyAppleSauce.PoolOntoFloor".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KneesGiving".Localized());

            OutputText("Scenes.Kelly.GiveKellyAppleSauce.AchingCockClenching".Localized());

            OutputText("Scenes.Kelly.GiveKellyAppleSauce.BendHeadKiss".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.CumGlazedBreath".Localized());
            else
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TastesPrettyGood".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyTimesApplesauced) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MurmursBestTreat".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WatchAndDress".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PurrsTastesBetter".Localized());

                OutputText("Scenes.Kelly.GiveKellyAppleSauce.DressAndAmbleBack".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.KellyTimesApplesauced, CoC.Instance.Flags.Get(GameFlagIds.KellyTimesApplesauced) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void KellyBJsAhoy()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KellyBJsAhoy.StepInKissAnswer".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.AlreadyRagingHard".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Kelly.KellyBJsAhoy.ItPronoun".Localized());
            else
                OutputText("Scenes.Kelly.KellyBJsAhoy.ThemPronoun".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.PullAwayTeaseLips".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrainOutOfArmor".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.TeaseHerFace".Localized());

                if (Player.Cocks[Player.BiggestCockIndex()].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.HorseCockSheath".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.TenderTortureGroan".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.BegToSwallow".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedToneGoOn".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SlowRollBackward".Localized());

                if (Player.Balls > 0 && Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BrushBallsAside".Localized());

                    OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrust".Localized());

                    if (Player.BallSize <= 8)
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelop".Localized());

                        if (Player.Balls > 2)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourth".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirst".Localized());

                        if (KellyIsPregnant)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenToken".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitch".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttempt".Localized());

                        if (Player.Balls > 2)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthAgain".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackFirst".Localized());

                        if (KellyIsPregnant)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLaden".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitchSpaced".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGrip".Localized());
                    }
                }
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressVagina".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustAgain".Localized());
                    }
                    else
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressBalls".Localized());

                            if (Player.BallSize <= 8)
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelopBare".Localized());

                                if (Player.Balls >= 4)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthThird".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirstAgain".Localized());

                                if (KellyIsPregnant)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenTokenAgain".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitchComma".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttemptBare".Localized());

                                if (Player.Balls >= 4)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthFourth".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackAgain".Localized());

                                if (KellyIsPregnant)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenAgain".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsSpaceBitch".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGripComma".Localized());
                            }
                        }

                OutputText("Scenes.Kelly.KellyBJsAhoy.HeavyBreathLips".Localized());

                if (Player.BiggestCockLength() > 15)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.LongWayUpLift".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.ReachCockHeadCrown".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.LookDownBemused".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.GrinAtCumslut".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusementSoakPraise".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.BitByBitDeeper".Localized());

                if (Player.BiggestCockLength() > 12)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.CantTakeAllDick".Localized());
                else
                    OutputText("Scenes.Kelly.KellyBJsAhoy.MouthFitsThroat".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueUnderbellyStroke".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.BoobsHeldOut".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallsClenchComma".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.PullOutGripBase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.OntoAllFourTits".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.DeepReleaseEcstasy".Localized());

                if (Player.CumQ() >= 750)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BarelyControlCum".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.FemaleSexOrgasms".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.FinallySpent".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrainDownwardsFree".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.AdmireCentaurCock".Localized());

                if (Player.Cocks[Player.BiggestCockIndex()].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.WonderCentaurHorseCock".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.TenderTortureHear".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.BegSwallowInline".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.LetSecondPass".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedToneGoAgain".Localized());

                if (Player.CockTotal() > 1 || Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SlowRollForward".Localized());

                if (Player.HasVagina() && Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BrushBallsAsideAgain".Localized());

                    OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustThird".Localized());

                    if (Player.BallSize <= 8)
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelopAgain".Localized());

                        if (Player.Balls >= 4)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthFifth".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirstThird".Localized());

                        if (KellyIsPregnant)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenHyphen".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishCentaurBitchSpaced".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttemptAgain".Localized());

                        if (Player.Balls >= 4)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthSixth".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackThird".Localized());

                        if (KellyIsPregnant)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenThird".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishCentaurBitch".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0)
                            OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGripThird".Localized());
                    }
                }
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressVaginaAgain".Localized());

                        OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustFourth".Localized());
                    }
                    else
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.KellyHotTonguePresses".Localized());

                            if (Player.BallSize <= 8)
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.AfterCurlingTongueOrbs".Localized());

                                if (Player.Balls >= 4)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdThenFourth".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.BackToFirstOrb".Localized());

                                if (KellyIsPregnant)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenWord".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.BreastsPolishingContinues".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.AfterCurlingTongueAttempts".Localized());

                                if (Player.Balls >= 4)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThirdThenFourthAgain".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirPunctuated".Localized());

                                if (KellyIsPregnant)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenWordAgain".Localized());

                                OutputText("Scenes.Kelly.KellyBJsAhoy.BreastsPolishingConcludes".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0)
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuThrilling".Localized());
                            }
                        }

                OutputText("Scenes.Kelly.KellyBJsAhoy.BreathingHeavilyLipsBase".Localized());

                if (Player.BiggestCockLength() > 15)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.LongWayDownDescent".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.ReachesCockHeadCrown".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedWorshipRhythm".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.EnoughKellySipsStream".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedTeaseLovelyBitch".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.MilkingRednessDeeper".Localized());

                if (Player.BiggestCockLength() > 12)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.CantTakeAllAgain".Localized());
                else
                    OutputText("Scenes.Kelly.KellyBJsAhoy.MouthGoodFitThroat".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueForwardUnderbelly".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.HoldingBoobsOffered".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallsClenchWord".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.PullOutPointDown".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.GritCumAnnouncement".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.PussyLickingWord".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallSuckingWord".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.CockAddictCatchLoad".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.CockSlitDilatesRopes".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.OntoFourTitsAgain".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.DeepReleaseEcstatic".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.FemaleSexClenchesSympathy".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.SpentClopAdmire".Localized());

                if (Player.CumQ() < 750)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SterlingJobFrostedBoobs".Localized());
                else
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.VirilityEcstaticTrance".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                        OutputText("Scenes.Kelly.KellyBJsAhoy.AllFourOfWord".Localized());

                    OutputText("Scenes.Kelly.KellyBJsAhoy.AlabasterOozeReservoir".Localized());
                }
            }

            OutputText("Scenes.Kelly.KellyBJsAhoy.ThanksForUsingMe".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.LaughSettleFacing".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kelly.KellyBJsAhoy.ClutchingAchingPrick".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.KnowsButPleasingFix".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.WatchKellyLicksClean".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KellyBonusBoobRows) > 0)
                OutputText("Scenes.Kelly.KellyBJsAhoy.CantReachLowerTits".Localized());

            OutputText("Scenes.Kelly.KellyBJsAhoy.TrailOfCumNipple".Localized());

            if (Player.Libido < 50)
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrokeLanguidHardAgain".Localized());
            else
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrokeLanguidStraining".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SensualLickVaginaGleam".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kelly.KellyBJsAhoy.NeedilySuckleBalls".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.EngulfCockPlumpLips".Localized());

                OutputText("Scenes.Kelly.KellyBJsAhoy.NeverDoubtGoodGirl".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KellyRace()
        {
            ClearOutput();

            RegisterMacro("kids", CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1);

            RegisterMacro("kidMale", CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1);

            RegisterMacro("children", CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1 ? "children" : (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1 ? "son" : "daughter"));

            RegisterMacro("kid ey", CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1 ? "they" : (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1 ? "he" : "she"));

            RegisterMacro("kid em", CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1 ? "them" : (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1 ? "him" : "her"));

            RegisterMacro("kid eir", CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KellyFirstKidGender) == 1 ? "his" : "her"));

            OutputText("Scenes.Kelly.KellyRace.WalkOverToField".Localized((CoC.Instance.Flags.Get(GameFlagIds.KellyKids) > 2 ? "The rest of them" : "[if (kidMale) {His|Her}] sibling"), (CoC.Instance.Flags.Get(GameFlagIds.KellyKids) == 2 ? "both" : "all " + NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.KellyKids)))));

            OutputText("Scenes.Kelly.KellyRace.ChildrenCrowdGreeting".Localized());

            OutputText("Scenes.Kelly.KellyRace.SuggestARace".Localized());

            if (Player.Body.Get<LowerBodyPart>().LegCount == 1)
            {
                OutputText("Scenes.Kelly.KellyRace.LittleCentaurGiggles".Localized());

                OutputText("Scenes.Kelly.KellyRace.NeverUnderestimateSomeone".Localized());
            }
            else
                if (Player.IsDrider())
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenNervouslyLook".Localized());

                    OutputText("Scenes.Kelly.KellyRace.DoYourBestRegardless".Localized());

                    OutputText("Scenes.Kelly.KellyRace.ChildrenNotConvinced".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenJumpExcitement".Localized());

                    OutputText("Scenes.Kelly.KellyRace.DragYouOffAnxious".Localized());
                }

            OutputText("Scenes.Kelly.KellyRace.ExplainRulesWalking".Localized());

            OutputText("Scenes.Kelly.KellyRace.KidsClaimWinner".Localized());

            OutputText("Scenes.Kelly.KellyRace.KellyStartsRace".Localized());

            OutputText("Scenes.Kelly.KellyRace.CountdownGo".Localized());

            if (Player.Speed < 60)
            {
                OutputText("Scenes.Kelly.KellyRace.EarlyLeadPursuit".Localized());

                OutputText("Scenes.Kelly.KellyRace.SecondCornerAccept".Localized());

                OutputText("Scenes.Kelly.KellyRace.DaddySoSlowTaunt".Localized());

                OutputText("Scenes.Kelly.KellyRace.SufferingCeasesMerciful".Localized());
            }
            else
                if (Player.Speed < 80)
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenBoundAhead".Localized());

                    OutputText("Scenes.Kelly.KellyRace.FocusOnGrassRhythm".Localized());

                    OutputText("Scenes.Kelly.KellyRace.AchingPropelsAcrossGrass".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenBoundBehind".Localized());

                    OutputText("Scenes.Kelly.KellyRace.FocusGrassInstead".Localized());

                    OutputText("Scenes.Kelly.KellyRace.EnjoyAirChase".Localized());
                }

            OutputText("Scenes.Kelly.KellyRace.ChildrenPantingExhausted".Localized());

            OutputText("Scenes.Kelly.KellyRace.SayGoodbyesReturn".Localized());

            Player.ChangeFatigue(20);

            DoNext(() => ReturnToCamp(1));
        }

    }
}