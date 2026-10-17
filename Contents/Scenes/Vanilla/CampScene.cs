using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Stats;
using MonoCoC.Registry;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Cabin;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places;
using MonoCoC.Contents.Followers;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<CampScene>("coc:camp")]
    public class CampScene : Scene
    {
        // 原版 scenes/Camp.hx:138 justReturnedToCamp：刚回营地时才触发贵妇衬衫的营地事件
        public static bool JustReturnedToCamp;

        // 原版 Camp.hx:3455 setLevelButton()：按可升级/可加属性/可买专长显示不同按钮
        public static void SetLevelButton()
        {
            var player = CoC.Instance.Player;

            if (player.CanLevelUp())
                AddButton(10, "Scenes.Camp.LevelUp.LevelUp".Localized(), () => Go<LevelUpScene>(),

                    "Scenes.Camp.LevelUp.LevelUpHint".Localized());
            else if (player.CanBuyStats())
                AddButton(10, "Scenes.Camp.LevelUp.StatUp".Localized(), () => Go<LevelUpScene>(),

                    "Scenes.Camp.LevelUp.StatUpHint".Localized((int)player.StatPoints));
            else if (player.CanBuyPerks())
                AddButton(10, "Scenes.Camp.LevelUp.PerkUp".Localized(), () => Go<LevelUpScene>(),

                    "Scenes.Camp.LevelUp.PerkUpHint".Localized((int)player.PerkPoints));
        }

        // 原版 Camp.hx:2976 buildFatigue()：建墙/建门的疲劳，帮手越多、属性越高越省
        public static SaveContent Save => SaveContent(new Identifier("coc", "camp"));

        // 原版 Camp.hx:4241 enjoyANiceBarrel()
        private static void EnjoyANiceBarrel()
        {
            ClearOutput();

            var anemone = SceneInstance<AnemoneScene>.Get().AnemoneFollower();

            OutputText("Scenes.Camp.EnjoyANiceBarrel.ShuffleAlongCamp".Localized());

            OutputText(anemone || GetCampPopulation() > 1

                ? "Scenes.Camp.EnjoyANiceBarrel.AroundIfNeeded".Localized(GetCampPopulation() == 1 ? "someone" : "anyone")

                : "Scenes.Camp.EnjoyANiceBarrel.KeepEyeOnThings".Localized());

            OutputText("Scenes.Camp.EnjoyANiceBarrel.RememberTheBarrel".Localized());

            OutputText(anemone

                ? "Scenes.Camp.EnjoyANiceBarrel.LittleOccupiedIzma".Localized()

                : "Scenes.Camp.EnjoyANiceBarrel.NoOneElseUsing".Localized());

            if (anemone)
                OutputText("Scenes.Camp.EnjoyANiceBarrel.MaybeIzmaClose".Localized());

            OutputText("Scenes.Camp.EnjoyANiceBarrel.DecidedThenStrip".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Camp.EnjoyANiceBarrel.AdaptedForAir".Localized());
            else
            {
                OutputText("Scenes.Camp.EnjoyANiceBarrel.ManageToHeft".Localized());

                if (Player.Strength > 50)
                    OutputText("Scenes.Camp.EnjoyANiceBarrel.ConsiderableStrength".Localized());
                else
                {
                    OutputText("Scenes.Camp.EnjoyANiceBarrel.ThrashingTailMomentum".Localized());

                    if (Player.Strength > 20)
                        OutputText("Scenes.Camp.EnjoyANiceBarrel.WorkingSurprisingly".Localized());
                    else
                    {
                        OutputText("Scenes.Camp.EnjoyANiceBarrel.BreathQuickensMuscles".Localized());

                        if (anemone)
                        {
                            OutputText("Scenes.Camp.EnjoyANiceBarrel.UmHelp".Localized());

                            OutputText("Scenes.Camp.EnjoyANiceBarrel.BlinkBreathingSlows".Localized());

                            OutputText("Scenes.Camp.EnjoyANiceBarrel.DumpsYouOverRim".Localized());
                        }
                        else
                            OutputText("Scenes.Camp.EnjoyANiceBarrel.NotBeDeterred".Localized());
                    }
                }
            }

            OutputText("Scenes.Camp.EnjoyANiceBarrel.FirstCaressOfWater".Localized());

            OutputText(anemone

                ? "Scenes.Camp.EnjoyANiceBarrel.KidASettlesBeside".Localized()

                : "Scenes.Camp.EnjoyANiceBarrel.DiveBeneathRim".Localized());

            OutputText("Scenes.Camp.EnjoyANiceBarrel.CantStayForever".Localized());

            if (anemone)
                OutputText("Scenes.Camp.EnjoyANiceBarrel.KidAWavesGoodbye".Localized());

            Player.HasGottenWashed();

            DoNext(() => ReturnToCamp(2));
        }

        public static int BuildFatigue(int amount = 100)
        {
            var player = CoC.Instance.Player;

            var helpers = 0;

            if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                helpers += 1;

            if (SceneInstance<HelScene>.Get().FollowerHel())
                helpers += 1;

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                helpers += 1;

            var fatigueAmount = amount;

            fatigueAmount -= (int)(player.Strength / 5);

            fatigueAmount -= (int)(player.Toughness / 10);

            fatigueAmount -= (int)(player.Speed / 10);

            if (player.HasPerk(PerkIds.IronMan))
                fatigueAmount -= 20;

            fatigueAmount /= helpers + 1;

            return Math.Max(15, fatigueAmount);
        }

        public override void Entry()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            MonoCoC.UI.Components.Views.GameView.SetPlayerStatsVisible(true);

            ClearOutput();

            ClearButtons();

            Player.FixStorage();

            // 原版 :3751 fixFlags()：进营地时补一次旧存档的塔通关标记
            FixFlags();

            // 原版 Camp.hx:333：存档版本低于当前版本时先走升级向导
            var saveVersion = Flags.Get(GameFlagIds.ModSaveVersion);

            if (saveVersion >= 3 && saveVersion < CurrentSaveVersion)
            {
                PromptSaveUpdate();

                return;
            }

            // 原版 Camp.hx:3455 setLevelButton()
            // 原版 setLevelButton() 操作的是顶部 MENU_LEVEL 按钮，不是底部第 10 槽
            // 底部 10 槽保留给 Pets（原版 Camp.hx:1049）

            // 原版 Camp.hx:596/599：Exgartuan 状态检查
            if (ExgartuanCampUpdate())
                return;

            // 原版 Camp.hx:1076-1088：营地坏结局触发
            if (Configuration.OptionStore.Instance.Get("realistic", 0) > 0
                && Player.BallSize > 18 + (Player.Strength / 2) + (Player.Tallness / 4))
            {
                BadEndGiantBalls();

                return;
            }

            if (Configuration.OptionStore.Instance.Get("survival", 0) > 0
                && Player.Hunger <= 0 && Player.Health <= 0

                && Player.Strength + Player.Toughness < 30)
            {
                BadEndHunger();

                return;
            }

            if (Player.MinLust() >= Player.MaxLust
                && Flags.Get(GameFlagIds.ShouldraSleepTimer) > -168

                && Player.Eggs() < 20

                && !Player.HasStatusEffect(StatusEffectIds.BimboChampagne)

                && !Player.HasStatusEffect(StatusEffectIds.LustStickApplied)

                && Player.JewelryEffectId != 1)
            {
                BadEndMinLust();

                return;
            }

            if (Flags.Get(GameFlagIds.CampCabinProgress) == 0 && View.Time.Days >= 14)
                Flags.Set(GameFlagIds.CampCabinProgress, 1);

            // 原版 :323 刚回营地 → 先检查大事件，有事件就只显示事件，点 Next 再回营地
            if (JustReturnedToCamp)
            {
                // 原版 Camp.hx:730：美人鱼泡桶事件
                if (!Save.GetBool("didSoakInABarrel")
                    && Places.Lake.SwimScene.IsFullMermaid

                    && HasChest()

                    && Flags.Get(GameFlagIds.CurrentWeather) < 3

                    && MonoCoC.Utils.Utils.Chance((int)(0.15f * Player.FatiguePercent + 1)))
                {
                    JustReturnedToCamp = false;

                    Save.SetBool("didSoakInABarrel", true);

                    EnjoyANiceBarrel();

                    return;
                }

                JustReturnedToCamp = false;

                if (Contents.Items.Behaviors.Vanilla.NobleShirtBehavior.CampEventsMajor())
                {
                    AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                    return;
                }

                // 原版 :770 小事件插在营地描述之前
                Contents.Items.Behaviors.Vanilla.NobleShirtBehavior.CampEventsMinor();
            }

            BuildCampDescription();

            AddButton(0, "Scenes.Camp.Explore".Localized(), () => Go<ExploreScene>(),

                "Scenes.Camp.Hint.Explore".Localized());

            // 原版 scenes/Camp.hx:1026：发现过地点才启用 Places
            if (PlacesCount() > 0)
                AddButton(1, "Scenes.Camp.Places".Localized(), () => Go<PlacesScene>(), "Scenes.Camp.PlacesHint".Localized());
            else
                AddButtonDisabled(1, "Scenes.Camp.Places".Localized(), "Scenes.Camp.NoPlacesYet".Localized());

            AddButton(2, "Scenes.Camp.Inventory".Localized(), () => Go<InventoryScene>(),

                "Scenes.Camp.Hint.Inventory".Localized());

            AddButton(3, "Scenes.Camp.Storage".Localized(), () => Go<StorageScene>(),

                "Scenes.Camp.Hint.Storage".Localized());

            AddButton(4, "Scenes.Camp.CampActions".Localized(), CampActions,

                "Scenes.Camp.Hint.CampActions".Localized());

            if (FollowersCount() > 0)
                AddButton(5, "Common.Button.Followers".Localized(), () => FollowersScene.Open(FollowerIds.Roles.Follower),

                    "Scenes.Camp.Hint.Followers".Localized());

            if (LoversCount() > 0)
                AddButton(6, "Common.Button.Lovers".Localized(), () => FollowersScene.Open(FollowerIds.Roles.Lover),

                    "Scenes.Camp.Hint.Lovers".Localized());

            if (SlavesCount() > 0)
                AddButton(7, "Common.Button.Slaves".Localized(), () => FollowersScene.Open(FollowerIds.Roles.Slave),

                    "Scenes.Camp.Hint.Slaves".Localized());

            if (PetsCount() > 0)
                AddButton(10, "Common.Button.Pets".Localized(), CampPetsMenu,

                    "Scenes.Camp.Hint.Pets".Localized());

            SceneInstance<MasturbationScene>.Get().SetMasturbateButton(index: 8);

            if (Player.Fatigue > 30f || Player.Health / Player.MaxHealth <= 0.9f)
                AddButton(9, "Scenes.Camp.Rest".Localized(), Rest, "Scenes.Camp.RestHint".Localized());
            else
                AddButton(9, "Scenes.Camp.Wait".Localized(), Wait, "Scenes.Camp.WaitHint".Localized());

            if (IsCanSleepTime())
                AddButton(9, "Scenes.Camp.Sleep".Localized(), Sleep, "Scenes.Camp.SleepHint".Localized());

            // 原版 :1054 太黑不能出门（覆盖 Explore / Places 两个按钮）
            if (View.Time.Hours < 6)
            {
                AddButtonDisabled(0, "Scenes.Camp.Explore".Localized(), "Scenes.Camp.TooDarkOutside".Localized());

                AddButtonDisabled(1, "Scenes.Camp.Places".Localized(), "Scenes.Camp.TooDarkOutside".Localized());
            }
        }

        private void BadEndGiantBalls()
        {
            ClearOutput();

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.FallOverLargeBalls".Localized());

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.CantMoveStarve".Localized());

            ClearButtons();

            if (Player.HasItem(ItemIds.Reducto))
            {
                OutputText("Scenes.Camp.BadEndGIANTBALLZ.HaveReducto".Localized());

                AddButton(1, ItemRegistry.Create(ItemIds.Reducto).Name, ApplyReductoAndEscapeBadEnd);
            }

            if (SceneInstance<RathazulScene>.Get().FollowerRathazul())
            {
                OutputText("Scenes.Camp.BadEndGIANTBALLZ.CallRathazul".Localized());

                AddButton(2, "Rathazul", CallRathazulAndEscapeBadEnd);
            }

            if (FollowerRegistry.HasFollower(FollowerIds.ShouldraFollower))
            {
                OutputText("Scenes.Camp.BadEndGIANTBALLZ.CallShouldra".Localized());

                AddButton(3, "Scenes.Camp.BadEndGIANTBALLZ.ButtonShouldra".Localized(), () =>

                    SceneInstance<ShouldraFollowerScene>.Get()
                        .ShouldraReductosYourBallsUpInsideYa(true));
            }

            if (Player.HasItem(ItemIds.Reducto) == false
                && !FollowerRegistry.HasFollower(FollowerIds.ShouldraFollower))

                GameOver();
        }

        // 原版 Camp.hx:2621 callRathazulAndEscapeBadEnd()
        private void CallRathazulAndEscapeBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.RathazulArrives".Localized());

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.RathazulHelps".Localized());

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.RathazulRubsPaste".Localized());

            Player.BallSize -= 4 + Random.Shared.Next(6);

            if (Player.BallSize < 1)
                Player.BallSize = 1;

            var cap = 16 + Player.Strength / 2 + Player.Tallness / 4;

            if (Player.BallSize > cap)
                Player.BallSize = cap;

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.ScrotumShift".Localized());

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.RathazulAdvice".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        private void ApplyReductoAndEscapeBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.ReductoPaste".Localized());

            Player.BallSize -= 4 + Random.Shared.Next(6);

            if (Player.BallSize < 1)
                Player.BallSize = 1;

            if (Player.BallSize > 18 + (Player.Strength / 2) + (Player.Tallness / 4))
                Player.BallSize = 17 + (Player.Strength / 2) + (Player.Tallness / 4);

            OutputText("Scenes.Camp.BadEndGIANTBALLZ.ScrotumShift".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Lust, -10f));

            Player.ConsumeItem(ItemIds.Reducto);

            DoNext(() => ReturnToCamp(1));
        }

        private void BadEndHunger()
        {
            ClearOutput();

            Player.Hunger = 0.1f;

            OutputText("Scenes.Camp.BadEndHunger.TooWeakToStand".Localized());

            var companions = CompanionsCount();

            if (companions > 1)
                OutputText("Scenes.Camp.BadEndHunger.CompanionsMourn".Localized());
            else if (companions == 1)
                OutputText("Scenes.Camp.BadEndHunger.FellowCompanionMourns".Localized());

            Player.Health = 0;

            GameOver();
        }

        private void BadEndMinLust()
        {
            ClearOutput();

            OutputText("Scenes.Camp.BadEndMinLust.ThoughtOfReleaseOverwhelms".Localized());

            OutputText("Scenes.Camp.BadEndMinLust.SpendRestOfLife".Localized());

            Player.Orgasm("Generic");

            GameOver();
        }

        private static bool BuiltWall => Flags.Get(GameFlagIds.CampWallProgress) >= 100;

        private static void ShootTheSky()
        {
            ClearOutput();

            var damage = (int)MathF.Round(Player.Strength + Player.WeaponAttack);

            Flags.Set(GameFlagIds.SkyDamage, Flags.Get(GameFlagIds.SkyDamage) + damage);

            if (Flags.Get(GameFlagIds.SkyDamage) < 1776)
            {
                OutputText("Scenes.Camp.Sky.Shoot".Localized());

                OutputText("Scenes.Camp.Sky.Damage".Localized(damage));

                OutputText("Scenes.Camp.Sky.Yeehaw".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Camp.Sky.ShootAgain".Localized());

            OutputText("Scenes.Camp.Sky.Damage".Localized(damage));

            OutputText("Scenes.Camp.Sky.Falls".Localized());

            OutputText("Scenes.Camp.Sky.AskRape".Localized());

            Flags.Set(GameFlagIds.SkyDamage, 0);

            ClearButtons();

            AddButton(0, "Common.Yes".Localized(), SkyRape);

            AddButton(1, "Common.No".Localized(), () => ReturnToCamp(1));
        }

        private static void SkyRape()
        {
            ClearOutput();

            if (Player.HasCock())
                OutputText("Scenes.Camp.Sky.RapeStartCock".Localized());
            else if (Player.HasVagina())
                OutputText("Scenes.Camp.Sky.RapeStartVagina".Localized());
            else
                OutputText("Scenes.Camp.Sky.RapeStartNone".Localized());

            OutputText("Scenes.Camp.Sky.RapeDemand".Localized());

            var body = Player.HasCock() ? "cock" : Player.HasVagina() ? "cunt" : "butthole";

            OutputText((CompanionsCount() > 0

                ? "Scenes.Camp.Sky.CompliesWithCamp"

                : "Scenes.Camp.Sky.Complies").Localized(body));

            if (Player.HasCock())
                OutputText("Scenes.Camp.Sky.FinalCock".Localized());
            else if (Player.HasVagina())
                OutputText("Scenes.Camp.Sky.FinalVagina".Localized());
            else
                OutputText("Scenes.Camp.Sky.FinalNone".Localized());

            OutputText("Scenes.Camp.Sky.Bless".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Toughness, 7f),

                new DynStat(GameStats.Speed, 7f), new DynStat(GameStats.Intelligence, 6f));

            DoNext(() => ReturnToCamp(1));
        }

        private static void PromptHangImpSkull()
        {
            ClearOutput();

            var skulls = Flags.Get(GameFlagIds.CampWallSkulls);

            if (skulls >= 100)
            {
                OutputText("Scenes.Camp.Skulls.Full".Localized());

                DoNext(() => Go<CampScene>());

                return;
            }

            OutputText("Scenes.Camp.Skulls.Prompt".Localized());

            if (skulls > 0)
                OutputText("Scenes.Camp.Skulls.Current".Localized(skulls == 1 ? "is" : "are",

                    skulls == 1 ? "one imp skull" : skulls + " imp skulls"));

            ClearButtons();

            AddButton(0, "Common.Yes".Localized(), HangImpSkull);

            AddButton(1, "Common.No".Localized(), () => Go<CampScene>());
        }

        private static void HangImpSkull()
        {
            ClearOutput();

            OutputText("Scenes.Camp.Skulls.Hang".Localized());

            Player.ConsumeItem(UseableIds.ImpSkull);

            Flags.Set(GameFlagIds.CampWallSkulls, Flags.Get(GameFlagIds.CampWallSkulls) + 1);

            var skulls = Flags.Get(GameFlagIds.CampWallSkulls);

            OutputText("Scenes.Camp.Skulls.HangCount".Localized(skulls == 1 ? "is" : "are",

                skulls == 1 ? "one imp skull" : skulls + " imp skulls"));

            DoNext(() => Go<CampScene>());
        }

        private static void InspectSkulls()
        {
            ClearOutput();

            OutputText("Scenes.Camp.Skulls.Inspect1".Localized());

            var skulls = Flags.Get(GameFlagIds.CampWallSkulls);

            if (skulls == 1)
                OutputText("Scenes.Camp.Skulls.InspectOne".Localized());
            else if (skulls < 15)
                OutputText("Scenes.Camp.Skulls.InspectFew".Localized());
            else if (skulls < 100)
                OutputText("Scenes.Camp.Skulls.InspectMany".Localized());
            else
                OutputText("Scenes.Camp.Skulls.InspectWall".Localized());

            OutputText("Scenes.Camp.Skulls.InspectEnd".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        private static bool ExgartuanCampUpdate()
        {
            if (!Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                return false;

            var value = (int)Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0);

            if (value == 1 && (Player.CockArea(0) < 100 || Player.Cocks.Count == 0))
            {
                ClearOutput();

                OutputText("Scenes.Camp.ExgartuanCampUpdate.UrgeToUrinate".Localized());

                OutputText((Player.HasCock()

                    ? "Scenes.Camp.ExgartuanCampUpdate.TooSmallForExgartuan"

                    : "Scenes.Camp.ExgartuanCampUpdate.DemonDidntStay").Localized());

                Player.RemoveStatusEffect(StatusEffectIds.Exgartuan);

                AwardAchievement(AchievementIds.UrineTrouble, display: true);

                DoNext(() => Go<CampScene>());

                return true;
            }

            if (value == 2 && Player.BiggestBreastSize() < 12)
            {
                ClearOutput();

                OutputText("Scenes.Camp.ExgartuanCampUpdate.BlackMilkDribbles".Localized());

                OutputText("Scenes.Camp.ExgartuanCampUpdate.ParagraphBreakCloseBold".Localized());

                Player.RemoveStatusEffect(StatusEffectIds.Exgartuan);

                DoNext(() => Go<CampScene>());

                return true;
            }

            return false;
        }

        private static bool BuiltGate => Flags.Get(GameFlagIds.CampWallGate) > 0;

        private static List<string> BuildHelpers()
        {
            var helpers = new List<string>();

            if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                helpers.Add("Marble");

            if (SceneInstance<HelScene>.Get().FollowerHel())
                helpers.Add("Helia");

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                helpers.Add("Kiha");

            return helpers;
        }

        private static string FormatHelpers(List<string> helpers)
        {
            return helpers.Count switch
            {
                0 => "",

                1 => helpers[0],

                2 => helpers[0] + " and " + helpers[1],

                _ => string.Join(", ", helpers.GetRange(0, helpers.Count - 1)) + " and " + helpers[^1],
            };
        }

        private static void BuildCampWallPrompt()
        {
            ClearOutput();

            if (!Player.HasFatigue(BuildFatigue()))
            {
                OutputText("Scenes.Camp.Wall.TooExhausted".Localized());

                DoNext(() => Go<CampScene>());

                return;
            }

            if (Flags.Get(GameFlagIds.CampWallProgress) == 0)
            {
                OutputText("Scenes.Camp.Wall.FirstPrompt".Localized());

                Flags.Set(GameFlagIds.CampWallProgress, 1);
            }
            else
            {
                if (Flags.Get(GameFlagIds.CampWallProgress) <= 20)
                    OutputText("Scenes.Camp.Wall.Continue".Localized());

                OutputText("Scenes.Camp.Wall.Segments".Localized(Flags.Get(GameFlagIds.CampWallProgress) / 20));
            }

            OutputText("Scenes.Camp.Wall.Cost".Localized());

            if (Flags.Get(GameFlagIds.CampCabinWoodResources) >= 100
                && Player.KeyItemV1(KeyItemIds.CarpentersToolbox) >= 50

                && Flags.Get(GameFlagIds.CampCabinStoneResources) >= 50)
            {
                ClearButtons();

                AddButton(0, "Common.Yes".Localized(), BuildCampWall);

                AddButton(1, "Common.No".Localized(), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Camp.Wall.NotEnough".Localized());

                DoNext(() => Go<CampScene>());
            }
        }

        private static void BuildCampWall()
        {
            var helpers = BuildHelpers();

            Flags.Set(GameFlagIds.CampCabinStoneResources, Flags.Get(GameFlagIds.CampCabinStoneResources) - 50);

            Flags.Set(GameFlagIds.CampCabinWoodResources, Flags.Get(GameFlagIds.CampCabinWoodResources) - 100);

            Player.AddKeyValue(KeyItemIds.CarpentersToolbox, 0, -50);

            ClearOutput();

            if (Flags.Get(GameFlagIds.CampWallProgress) == 1)
            {
                OutputText("Scenes.Camp.Wall.Learn".Localized());

                Flags.Set(GameFlagIds.CampWallProgress, 20);
            }
            else
            {
                OutputText("Scenes.Camp.Wall.Remember".Localized());

                Flags.Set(GameFlagIds.CampWallProgress, Flags.Get(GameFlagIds.CampWallProgress) + 20);
            }

            OutputText("Scenes.Camp.Wall.BuildText".Localized());

            if (helpers.Count > 0)
            {
                OutputText("[pg]" + FormatHelpers(helpers));

                OutputText((helpers.Count == 1 ? "Scenes.Camp.Wall.HelperAssist" : "Scenes.Camp.Wall.HelpersAssist")
                    .Localized(FormatHelpers(helpers)));
            }

            Player.ChangeFatigue(BuildFatigue());

            if (helpers.Count >= 2)
            {
                OutputText("Scenes.Camp.Wall.OneHour".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else if (helpers.Count == 1)
            {
                OutputText("Scenes.Camp.Wall.TwoHours".Localized());

                DoNext(() => ReturnToCamp(2));
            }
            else
            {
                OutputText((BuildFatigue() >= 75 ? "Scenes.Camp.Wall.Daunting" : "Scenes.Camp.Wall.Easy").Localized());

                DoNext(() => ReturnToCamp(4));
            }

            if (Flags.Get(GameFlagIds.CampWallProgress) >= 100)
                OutputText("Scenes.Camp.Wall.Done".Localized());
        }

        private static void BuildCampGatePrompt()
        {
            ClearOutput();

            if (!Player.HasFatigue(BuildFatigue()))
            {
                OutputText("Scenes.Camp.Wall.TooExhausted".Localized());

                DoNext(() => Go<CampScene>());

                return;
            }

            OutputText("Scenes.Camp.Wall.GatePrompt".Localized());

            OutputText("Scenes.Camp.Wall.GateCost".Localized());

            if (Flags.Get(GameFlagIds.CampCabinWoodResources) >= 100
                && Player.KeyItemV1(KeyItemIds.CarpentersToolbox) >= 100

                && Flags.Get(GameFlagIds.CampCabinStoneResources) >= 100)
            {
                ClearButtons();

                AddButton(0, "Common.Yes".Localized(), BuildCampGate);

                AddButton(1, "Common.No".Localized(), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Camp.Wall.NotEnough".Localized());

                DoNext(() => Go<CampScene>());
            }
        }

        private static void BuildCampGate()
        {
            var helpers = BuildHelpers();

            Flags.Set(GameFlagIds.CampCabinStoneResources, Flags.Get(GameFlagIds.CampCabinStoneResources) - 100);

            Flags.Set(GameFlagIds.CampCabinWoodResources, Flags.Get(GameFlagIds.CampCabinWoodResources) - 100);

            Player.AddKeyValue(KeyItemIds.CarpentersToolbox, 0, -100);

            ClearOutput();

            OutputText("Scenes.Camp.Wall.GateLearn".Localized());

            Flags.Set(GameFlagIds.CampWallGate, 1);

            OutputText("Scenes.Camp.Wall.GateText".Localized());

            if (helpers.Count > 0)
            {
                OutputText("[pg]" + FormatHelpers(helpers));

                OutputText((helpers.Count == 1 ? "Scenes.Camp.Wall.HelperAssist" : "Scenes.Camp.Wall.HelpersAssist")
                    .Localized(FormatHelpers(helpers)));
            }

            OutputText("Scenes.Camp.Wall.GateDone".Localized());

            Player.ChangeFatigue(BuildFatigue());

            DoNext(() => ReturnToCamp(1));
        }

        // 原版 Camp.hx:2004 acceptChallenge()
        public static void AcceptChallenge()
        {
            ClearOutput();

            OutputText("Scenes.Camp.Actions.ChallengeTeleport1".Localized());

            OutputText("Scenes.Camp.Actions.ChallengeTeleport2".Localized());

            OutputText("Scenes.Camp.Actions.ChallengeTeleport3".Localized());

            var definition = MonoCoC.Assets.ContentManagers.DungeonManager.Get(
                new MonoCoC.Registry.Identifier("coc", "wizard_tower"));

            if (definition == null)
            {
                OutputText("Scenes.Camp.Actions.ChallengeMissing".Localized());

                AddButton(14, "Common.Back".Localized(), CampActions);

                return;
            }

            var scene = new Contents.Dungeons.DungeonScene(definition, 0, 2, 0);

            Contents.Dungeons.DungeonSession.Start(scene.DungeonContext);

            Go(scene);

            Contents.Dungeons.WizardTowerRooms.MeetLaurentius3(scene.DungeonContext);
        }

        public static void CampActions()
        {
            // 先挂 Back，避免子菜单逻辑异常时整页没有退出按钮
            ClearOutput();

            ClearButtons();

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());

            try
            {
                BuildCampActions();
            }
            catch (Exception ex)
            {
                OutputText("[pg]" + ex.Message);
            }
        }

        // 原版 Camp.hx:1970 magicChangeMenu()
        public static void MagicChangeMenu()
        {
            ClearOutput();

            ClearButtons();

            AddButton(0, "Scenes.Camp.Actions.MagicBW".Localized(), () =>
            {
                Flags.Set(GameFlagIds.MagicSwitch, 0);

                CampActions();
            }, "The most common style of magic in this world.");

            AddButton(1, "Scenes.Camp.Actions.MagicTF".Localized(), () =>
            {
                Flags.Set(GameFlagIds.MagicSwitch, 1);

                CampActions();
            }, "A magic of Earth and Fire created by the demon Akbal.");

            AddButton(14, "Common.Back".Localized(), CampActions);
        }

        // 原版 Camp.hx buildMenu()
        public static void BuildMenu()
        {
            ClearOutput();

            ClearButtons();

            AddButton(14, "Common.Back".Localized(), CampActions);

            var progress = Flags.Get(GameFlagIds.CampCabinProgress);

            if (progress > 0 && progress < 10)
                AddNextButton("Scenes.Camp.BuildCabin".Localized(), () => Go<CabinProgressScene>());

            if (Player.HasKeyItem(KeyItemIds.CarpentersToolbox))
            {
                if (!BuiltWall && GetCampPopulation() >= 4)
                    AddNextButton("Scenes.Camp.Actions.BuildWall".Localized(), BuildCampWallPrompt);

                if (BuiltWall && !BuiltGate)
                    AddNextButton("Scenes.Camp.Actions.BuildGate".Localized(), BuildCampGatePrompt);

                if (!Save.GetBool("dummyBuilt"))
                    AddNextButton("Common.Button.BuildDummy".Localized(),

                        TrainingDummyCamp.BuildDummyPrompt);
            }

            if (BuiltWall && Player.HasItem(UseableIds.ImpSkull))
                AddNextButton("Scenes.Camp.Actions.AddImpSkull".Localized(), PromptHangImpSkull,

                    "Add an imp skull to decorate the wall and to serve as deterrent for imps.");
        }

        // 原版 Camp.hx:773-898 的营地描述
        private static void BuildCampDescription()
        {
            var days = View.Time.Days;

            if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                OutputText("Scenes.Camp.Desc.IsabellaCampsite".Localized());
            else
            {
                if (days < 10)
                    OutputText("Scenes.Camp.Desc.SimpleCampsite".Localized());

                if (days >= 10 && days < 20)
                    OutputText("Scenes.Camp.Desc.LivedInCampsite".Localized());

                if (days >= 20)
                {
                    OutputText("Scenes.Camp.Desc.ComfyCampsite".Localized());

                    if (Flags.Get(GameFlagIds.CampBuiltCabin) > 0
                        && Flags.Get(GameFlagIds.CampCabinFurnitureBed) > 0)

                        OutputText("Scenes.Camp.Desc.FirePitIs".Localized());
                    else
                        OutputText("Scenes.Camp.Desc.FirePitAndTentAre".Localized());

                    OutputText("Scenes.Camp.Desc.SetUpPerfectly".Localized());
                }
            }

            if (days >= 20)
                OutputText("Scenes.Camp.Desc.Artwork".Localized());

            var cabin = Flags.Get(GameFlagIds.CampCabinProgress);

            if (cabin == 7) OutputText("Scenes.Camp.Desc.CabinFrames".Localized());
            else if (cabin == 8) OutputText("Scenes.Camp.Desc.CabinMissingDoor".Localized());
            else if (cabin == 9) OutputText("Scenes.Camp.Desc.CabinMissingFloor".Localized());
            else if (cabin >= 10) OutputText("Scenes.Camp.Desc.CabinNearEdge".Localized());

            if (Flags.Get(GameFlagIds.ClaraImprisoned) > 0)
                SceneInstance<MarblePurification>.Get().ClaraCampAddition();

            if (Flags.Get(GameFlagIds.MarbleNurseryConstruction) == 100
                && SceneInstance<MarbleScene>.Get().MarbleFollower())
            {
                var kids = Flags.Get(GameFlagIds.MarbleKids);

                OutputText(kids == 0

                    ? "Scenes.Camp.Desc.NurseryFuture".Localized()

                    : "Scenes.Camp.Desc.NurseryKids".Localized(
                        MonoCoC.Utils.Utils.NumberOfThings(kids, "child", "children")));
            }

            var harpies = Flags.Get(GameFlagIds.SophieAdultKidCount);

            if (harpies > 0)
            {
                var key = harpies switch
                {
                    1 => "One",

                    <= 3 => "Small",

                    <= 4 => "Medium",

                    <= 10 => "Large",

                    <= 20 => "Giant",

                    <= 50 => "Massive",

                    _ => "Immense",
                };

                OutputText(("Scenes.Camp.Desc.Rookery." + key).Localized());
            }

            if (Player.HasStatusEffect(StatusEffectIds.DefenseCanopy))
                OutputText("Scenes.Camp.Desc.DefenseCanopy".Localized());

            var wall = Flags.Get(GameFlagIds.CampWallProgress);

            if (wall >= 20 && wall < 100)
                OutputText(wall == 20

                    ? "Scenes.Camp.Desc.WallSmall".Localized()

                    : "Scenes.Camp.Desc.WallSome".Localized());
            else if (wall >= 100)
            {
                OutputText("Scenes.Camp.Desc.WallFull".Localized());

                if (Flags.Get(GameFlagIds.CampWallGate) > 0)
                    OutputText("Scenes.Camp.Desc.WallGate".Localized());

                var skulls = Flags.Get(GameFlagIds.CampWallSkulls);

                if (skulls > 0)
                {
                    var key = skulls == 1 ? "One" : skulls < 5 ? "Few" : skulls < 15 ? "Several" : "Many";

                    OutputText(("Scenes.Camp.Desc.Skulls." + key).Localized());

                    if (skulls == 1)
                        OutputText("Scenes.Camp.Desc.SkullsCountOne".Localized());
                    else
                    {
                        var skullText = MonoCoC.Utils.Utils.NumberOfThings(skulls, "skull");

                        // 原版句子里是小写 forty skulls
                        if (skullText.Length > 0)
                            skullText = char.ToLowerInvariant(skullText[0]) + skullText[1..];

                        OutputText("Scenes.Camp.Desc.SkullsCountMany".Localized(skullText));
                    }
                }

                var statues = Flags.Get(GameFlagIds.CampWallStatues);

                if (statues == 1)
                    OutputText("Scenes.Camp.Statues.One".Localized());
                else if (statues > 1)
                    OutputText("Scenes.Camp.Statues.Many".Localized(
                        MonoCoC.Utils.Utils.NumberOfThings(statues, "marble imp statue", "marble imp statues")));
            }
            else
                OutputText("Scenes.Camp.Desc.SimpleTraps".Localized());

            OutputText("Scenes.Camp.Desc.Portal".Localized());

            if (Flags.Get(GameFlagIds.AntKids) > 1000)
                OutputText("Scenes.Camp.Desc.AntKids".Localized());

            if (Flags.Get(GameFlagIds.EmberCurrentlyFreakingAboutMinocum) == 1)
            {
                var ember = SceneInstance<EmberScene>.Get();

                OutputText("Scenes.Camp.Desc.EmberCrusade".Localized(
                    ember.EmberMF("his", "her"), ember.EmberMF("his", "her")));
            }

            // 原版 Camp.hx:907 campFollowers(true)
            CampFollowerDescriptions.AppendAll();

            // 原版 Camp.hx:916-917 campLoversMenu(true) / campSlavesMenu(true)
            CampFollowerDescriptions.AppendRole(FollowerIds.Roles.Lover);

            CampFollowerDescriptions.AppendRole(FollowerIds.Roles.Slave);

            // 原版 Camp.hx:918 akky.locationDesc("Camp")
            var akky = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.AkkyScene>.Get();

            akky.BuildDescs();

            akky.LocationDesc("Camp");

            if (Seasonal.NieveScene.Save.GetBool("kidsPlayed") && Holidays.IsSaturnalia())
                OutputText(Seasonal.NieveScene.Save.GetBool("kidsPlayedSingular")

                    ? "Scenes.Camp.Desc.NieveSnowmanOne".Localized()

                    : "Scenes.Camp.Desc.NieveSnowmanMany".Localized());

            if (Configuration.OptionStore.Instance.Get("survival", 0) > 0 && Player.Hunger < 25)
            {
                OutputText(Player.Hunger < 1

                    ? "Scenes.Camp.Desc.HungerPainful".Localized()

                    : "Scenes.Camp.Desc.HungerLoud".Localized());

                if (Player.Hunger < 10)
                    OutputText("Scenes.Camp.Desc.HungerThin".Localized());

                if (Player.Hunger <= 0)
                    OutputText("Scenes.Camp.Desc.HungerStarving".Localized());
            }
        }

        public static void CampPetsMenu()
        {
            ClearOutput();

            OutputText("Scenes.Cabin.CabinPetsMenu.InteractPets".Localized());

            if (SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.AkkyScene>.Get().IsOwned())
            {
                var akky = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Npcs.AkkyScene>.Get();

                akky.name = CoC.Instance.Flags.Get(GameFlagIds.AkkyName);

                akky.MenuButton(() => Go<CampScene>(), 0, "Camp");
            }

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());
        }

        private static void BuildCampActions()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Camp.Actions.Title".Localized() + "\n");

            // 先挂 Back：即使后面某个原版条件抛异常，玩家也能退出
            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());

            if (BuiltWall)
                AddButton(6, "Scenes.Camp.Actions.AdmireSkulls".Localized(), InspectSkulls,

                    "Scenes.Camp.Actions.Hint.AdmireSkulls".Localized());

            if (Configuration.OptionStore.Instance.Get("silly", 0) > 0 && Player.Weapon.IsFirearm())
                AddNextButton("Scenes.Camp.Actions.Shoot".Localized(), ShootTheSky);

            AddButton(0, "Scenes.Camp.Actions.Swim".Localized(), Swim, "Scenes.Camp.Actions.Hint.Swim".Localized());

            AddButton(1, "Scenes.Camp.Actions.ExaminePortal".Localized(), ExaminePortal, "Scenes.Camp.Actions.Hint.ExaminePortal".Localized());

            if (View.Time.Hours == 19)
                AddButton(2, "Scenes.Camp.Actions.WatchSunset".Localized(), WatchSunset, "Scenes.Camp.Actions.Hint.WatchSunset".Localized());
            else if (View.Time.Hours >= 20 && Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                AddButton(2, "Scenes.Camp.Actions.WatchStars".Localized(), WatchStars, "Scenes.Camp.Actions.Hint.WatchStars".Localized());
            else
            {
                var time = Configuration.OptionStore.Instance.Get("time12Hour", 0) == 1 ? "7pm" : "19:00";

                AddButtonDisabled(2, "Scenes.Camp.Actions.WatchSky".Localized(), "Scenes.Camp.Actions.WatchSkyDisabled".Localized(time));
            }

            TrainingDummyCamp.AddCampActions();

            if (Player.HasPerk(PerkIds.TerrestrialFire))
                AddButton(10, "Scenes.Camp.Actions.ChangeMagic".Localized(), MagicChangeMenu,

                    "Scenes.Camp.Actions.Hint.ChangeMagic".Localized());

            AddButton(3, "Scenes.Camp.Actions.Build".Localized(), BuildMenu, "Scenes.Camp.Actions.Hint.Build".Localized());

            if (Flags.Get(GameFlagIds.CampBuiltCabin) > 0)
                AddButton(4, "Scenes.Camp.EnterCabin".Localized(), () => Go<CabinScene>(), "Scenes.Camp.Actions.Hint.EnterCabin".Localized());
            else
                AddButton(4, "Scenes.Camp.Actions.ReadCodex".Localized(), () => Go<CodexScene>(), "Read any codex entries you have unlocked.");

            if (Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                AddButton(11, "CharCreation.Ascension.Header".Localized(),

                    () => SceneInstance<CharCreation>.Get().PromptAscend(),

                    "Scenes.Camp.Actions.Hint.Ascension".Localized());

            if (Player.HasKeyItem("Dragon Egg"))
                AddNextButton("Scenes.Camp.Actions.Egg".Localized(),

                    () => SceneInstance<EmberScene>.Get().EmberEggInteraction());

            var snow = SceneInstance<Seasonal.NieveScene>.Get();

            if (snow.NieveAvailable() && snow.stage > 0 && snow.stage < 5)
            {
                OutputText(snow.stage == 1

                    ? "Scenes.Camp.Actions.SnowOdd".Localized()

                    : "Scenes.Camp.Actions.Snowman".Localized());

                AddNextButton("Scenes.Camp.Actions.Snow".Localized(), snow.NieveBuilding);
            }

            if (Flags.Get(GameFlagIds.FuckFlowerKilled) == 0
                && Flags.Get(GameFlagIds.FuckFlowerLevel) >= 1

                && Flags.Get(GameFlagIds.FuckFlowerLevel) < 4)
            {
                var label = Flags.Get(GameFlagIds.FuckFlowerLevel) >= 3

                    ? "Scenes.Camp.Actions.Tree".Localized()

                    : "Scenes.Camp.Actions.Plant".Localized();

                AddNextButton(label, () => SceneInstance<HolliScene>.Get().TreeMenu());
            }

            if (View.Time.Hours >= 20 && (Flags.Get(GameFlagIds.ManorProgress) & 1024) != 0)
                AddButton(12, "Scenes.Camp.Actions.Void".Localized(),

                    () => SceneInstance<Contents.Dungeons.ManorScene>.Get().EnterVoid(),

                    "Observe the infinite expanse of the Cosmos and accept realities you once ignored.");

            if ((Flags.Get(GameFlagIds.WizardTowerProgress) & 1024) != 0
                && (Flags.Get(GameFlagIds.WizardTowerProgress) & 2048) == 0)

                AddButton(13, "Scenes.Camp.Actions.Challenge".Localized(), AcceptChallenge,

                    "Teleport back to the Tower's summit and challenge the wizard.");

            if (Contents.Dungeons.WizardTowerRooms.CanVisitLaurentius())
                AddButton(3, "Common.Button.Laurentius".Localized(), () => Contents.Dungeons.WizardTowerRooms.CampLaurentius(), "Challenge the wizard once more.");

            AddButton(14, "Common.Back".Localized(), () => Go<CampScene>());
        }

        public static void Swim()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Camp.Actions.SwimIntro".Localized());

            OutputText(Player.Armor.Name == "slutty swimwear" ? "Scenes.Camp.Actions.SwimSwimwear".Localized() : "Scenes.Camp.Actions.SwimStrip".Localized());

            OutputText("Scenes.Camp.Actions.SwimDeepIntro".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Camp.Actions.SwimShort".Localized());
            else if (Player.Tallness < 72)
                OutputText("Scenes.Camp.Actions.SwimMedium".Localized());
            else
                OutputText("Scenes.Camp.Actions.SwimTall".Localized());

            OutputText("Scenes.Camp.Actions.SwimRelax".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.TellyVised);

            var gills = Player.Body.Get<GillsPart>();

            if (gills.Id == new Identifier("coc", "mermaid"))
                Player.ChangeFatigue(-20);

            AddButton(0, "Common.OK".Localized(), SwimFinish);
        }

        public static void SwimFinish()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.FactoryShutdown) == 2 && Player.Corruption < 50)
            {
                OutputText("Scenes.Camp.Actions.SwimTainted".Localized());

                Player.Corruption += 0.5f;

                Player.Lust += 15;
            }

            OutputText("Scenes.Camp.Actions.SwimFinish".Localized());

            AddButton(0, "Common.OK".Localized(), () => ReturnToCamp(1));
        }

        public static void ExaminePortal()
        {
            ClearOutput();

            ClearButtons();

            if (Flags.Get(GameFlagIds.CampPortalProgress) <= 0)
            {
                OutputText("Scenes.Camp.Actions.PortalFirst".Localized());

                Flags.Set(GameFlagIds.CampPortalProgress, 1);

                AddButton(0, "Common.OK".Localized(), () => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Camp.Actions.PortalReturn".Localized());

            AddButton(0, "Common.OK".Localized(), () => Go<CampScene>());
        }

        public static void WatchSunset()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Camp.Actions.SunsetIntro".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                    if (Player.Corruption < 33)
                    {
                        OutputText("Scenes.Camp.Actions.SunsetNostalgiaLow".Localized());

                        Player.Corruption -= 1;

                        Player.Libido -= 1;

                        Player.Lust -= 30;
                    }
                    else if (Player.Corruption < 66)
                    {
                        OutputText("Scenes.Camp.Actions.SunsetNostalgiaMid".Localized());

                        Player.Corruption -= 0.5f;

                        Player.Libido -= 1;

                        Player.Lust -= 20;
                    }
                    else
                    {
                        OutputText("Scenes.Camp.Actions.SunsetNostalgiaHigh".Localized());

                        Player.Libido -= 1;

                        Player.Lust -= 10;
                    }

                    break;

                case 1:
                    if (Player.Corruption < 33)
                    {
                        OutputText("Scenes.Camp.Actions.SunsetAdventureLow".Localized());

                        Player.Lust -= 30;
                    }
                    else if (Player.Corruption < 66)
                    {
                        OutputText("Scenes.Camp.Actions.SunsetAdventureMid".Localized());

                        Player.Lust -= 20;
                    }
                    else
                    {
                        OutputText("Scenes.Camp.Actions.SunsetAdventureHigh".Localized());

                        Player.Lust -= 10;
                    }

                    break;

                default:
                    OutputText("Scenes.Camp.Actions.SunsetFuture".Localized());

                    Player.Lust -= 30;

                    break;
            }

            OutputText("Scenes.Camp.Actions.SunsetOutro".Localized());

            AddButton(0, "Common.OK".Localized(), () => ReturnToCamp(1));
        }

        public static void WatchStars()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Camp.Actions.StarsIntro".Localized());

            OutputText("Scenes.Camp.Actions.StarsFall".Localized());

            OutputText("Scenes.Camp.Actions.StarsRelax".Localized());

            OutputText(Random.Shared.Next(4) switch
            {
                0 => "Scenes.Camp.Actions.StarsPhallic".Localized(),

                1 => "Scenes.Camp.Actions.StarsBreasts".Localized(),

                2 => "Scenes.Camp.Actions.StarsCentaur".Localized(),

                3 => "Scenes.Camp.Actions.StarsDipper".Localized(),

                _ => "Scenes.Camp.Actions.StarsError".Localized()
            });

            OutputText("Scenes.Camp.Actions.StarsOutro".Localized());

            Player.Lust -= 15;

            AddButton(0, "Common.OK".Localized(), () => ReturnToCamp(1));
        }

        public static void SleepRecovery(bool display = false)
        {
            var multiplier = 1.0f;

            var sleepWith = CoC.Instance.Flags.Get(GameFlagIds.SleepWith);

            if (Flags.Get(GameFlagIds.CampBuiltCabin) > 0
                && CabinInterior.HasBed

                && (sleepWith == "" || sleepWith == "Marble"))

                multiplier += 0.5f;

            if (IsSurvival && Player.Hunger < 25)
            {
                OutputText("Common.Text.YouHaveDifficultySleepingAsYour".Localized());

                multiplier *= 0.5f;
            }

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                if (display)
                    OutputText("Common.Text.YourSleepIsVeryTroubledAnd".Localized());

                multiplier *= 0.5f;

                Player.ApplyStats(new DynStat(GameStats.Toughness, -0.1f), new DynStat(GameStats.Intelligence, -0.1f));
            }
            else if (Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3)
            {
                if (display)
                    OutputText("Common.Text.YouSpendMuchOfTheNight".Localized());

                multiplier *= 0.75f;
            }

            if (Player.Armor.Id.Path == "goo_armor" && Flags.Get(GameFlagIds.ValeriaFluids) <= 0)
                OutputText("Common.Text.YouFeelTheFluidStarvedGoo".Localized());

            if (Player.Armor.Id.Path == "tentacle_bark")
            {
                OutputText("Scenes.Camp.SleepRecovery.LivingTentaclesTease".Localized());

                if (Player.Corruption < 33)
                {
                    OutputText("Scenes.Camp.SleepRecovery.RestlessSleepTroublingDreams".Localized());

                    multiplier *= 0.75f;
                }
                else if (Player.Corruption < 66)
                {
                    OutputText("Scenes.Camp.SleepRecovery.RestlessSleep".Localized());

                    multiplier *= 0.9f;
                }
                else
                    OutputText("Scenes.Camp.SleepRecovery.WonderfulRavagedDreams".Localized());

                OutputText("Common.Text.Pg".Localized());
            }

            SleepRecoveryApply(10, multiplier, display);
        }

        public static void SleepRecoveryApply(float hours, float multi = 0, bool display = false, float hpAdd = 0, float fatigueAdd = 0)
        {
            const float fatigueRecovery = 20;

            const float hpRecovery = 20;

            if (multi == 0)
            {
                multi = 1;

                if (IsSurvival && Player.Hunger < 25) multi *= 0.5f;
                if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl)) multi *= 0.5f;
                else if (Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3) multi *= 0.75f;
            }

            Player.HealthChange(hours * hpRecovery * multi + hpAdd);

            Player.ChangeFatigue(-(hours * fatigueRecovery * multi + fatigueAdd));
        }

        public static void CheckCabinMaterials(int highlight = 0)
        {
            OutputText("Common.Text.Pg".Localized());

            OutputText((highlight == 1 ? "<b>" : "") + "Common.Text.Nails".Localized() + Player.KeyItemValue("Carpenter's Toolbox", 1) + "/" + MaxNailSupply() + "Common.Text.N".Localized() + (highlight == 1 ? "</b>" : ""));

            OutputText((highlight == 2 ? "<b>" : "") + "Common.Text.Wood".Localized() + CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) + "/" + MaxWoodSupply() + "Common.Text.N".Localized() + (highlight == 2 ? "</b>" : ""));

            OutputText((highlight == 3 ? "<b>" : "") + "Common.Text.Stones".Localized() + CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) + "/" + MaxStoneSupply() + "Common.Text.N".Localized() + (highlight == 3 ? "</b>" : ""));
        }

        public static int MaxNailSupply() => 200 + (int)Player.KeyItemValue("Carpenter's Nail Box", 1);

        public static int MaxWoodSupply() => 999;

        public static int MaxStoneSupply() => 999;

        public static int CabinProgress
        {
            get => Flags.Get(GameFlagIds.CampCabinProgress);

            set => Flags.Set(GameFlagIds.CampCabinProgress, value);
        }

        public static bool CampGuarded() => FollowerRegistry.CampGuarded;

        public static int LoversCount() => FollowerRegistry.Count(FollowerIds.Roles.Lover);

        public static bool IsGuard(Identifier follower) => CampGuards.IsGuard(follower.Path);

        public static bool HasCompanions() => FollowersCount() + SlavesCount() > 0;

        public static int FollowersCount() => FollowerRegistry.Count(FollowerIds.Roles.Follower);

        public static int SlavesCount() => FollowerRegistry.Count(FollowerIds.Roles.Slave);

        public static int CompanionsCount() => FollowersCount() + SlavesCount() + LoversCount();

        // ── 原版 scenes/Camp.hx:136-151 长途探索 ─────────────────────────
        // 原版把 timesExplored / maxExplorations 放在 Camp 场景上，用于「长途模式」下
        // 连续返回营地时保留下一次探索目标，超过次数才真正回营。
        public static int LongHaulTimesExplored;

        public static int LongHaulMaxExplorations = 2;

        // 原版 :144 isCanSleepTime()
        public static bool IsCanSleepTime() => View.Time.Hours >= 21 || View.Time.Hours < WakeupTime();

        // 原版 :148 longHaulReset()
        public static void LongHaulReset()
        {
            SceneInstance<ExplorationScene>.Get().currArea = null;

            LongHaulTimesExplored = 0;

            LongHaulMaxExplorations = 1 + Random.Shared.Next(2);
        }

        // 原版 :1174 petsCount()
        public static int PetsCount() => SceneInstance<AkkyScene>.Get().IsOwned() ? 1 : 0;

        // 原版 :1966 magicsKnown()：黑白魔法永远算 1 系
        public static int MagicsKnown()
        {
            var count = 1;

            if (Player.HasPerk(PerkIds.TerrestrialFire))
                count += 1;

            return count;
        }

        // 原版 :2740 placesCount()
        public static int PlacesCount()
        {
            var places = 0;

            if (Flags.Get(GameFlagIds.BazaarEntered) > 0)
                places += 1;

            if (Player.HasStatusEffect(StatusEffectIds.BoatDiscovery))
                places += 1;

            if (Flags.Get(GameFlagIds.FoundCathedral) > 0)
                places += 1;

            if (PlacesScene.DungeonFound())
                places += 1;

            if (FarmFound())
                places += 1;

            if (Flags.Get(GameFlagIds.OwcaUnlocked) > 0)
                places += 1;

            if (Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting))
                places += 1;

            if (Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1)
                places += 1;

            if (Flags.Get(GameFlagIds.AmilyVillageAccessible) > 0)
                places += 1;

            if (Flags.Get(GameFlagIds.MetMinerva) >= 4)
                places += 1;

            if (SceneInstance<SylviaScene>.Get().SylviaProg >= 4)
                places += 1;

            return places;
        }

        // 原版 scenes/Camp.hx farmFound()
        public static bool FarmFound()
        {
            if (Player.HasStatusEffect(StatusEffectIds.MetWhitney)
                && Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) > 1)
            {
                if (Flags.Get(GameFlagIds.FarmDisabled) == 0)
                    return true;

                if (Player.IsCorruptEnough(70)
                    && FarmCorruption.softLevelMin(12)

                    && FarmCorruption.corruptFollowers() >= 2

                    && Flags.Get(GameFlagIds.FarmCorruptionDisabled) == 0)

                    return true;
            }

            if (Flags.Get(GameFlagIds.FarmCorruptionStarted) > 0)
                return true;

            return false;
        }

        // 原版 :3485 getCampPopulation()
        public static int GetCampPopulation()
        {
            var pop = 1; // 你自己也算人口

            pop += CompanionsCount();

            pop += GetCampKidBreakdown().total;

            pop += GetCampMiscCount();

            // 原版 :3492 Kid A 同时算 kid 和 follower，去掉重复的一次
            if (SceneInstance<AnemoneScene>.Get().AnemoneFollower())
                pop--;

            // 原版 :3497 实际不住在营地的跟随者
            if (SceneInstance<CeraphFollowerScene>.Get().CeraphIsFollower())
                pop--;

            if (SceneInstance<NephilaCovenFollowerScene>.Get().NephilaCovenIsFollower())
                pop--;

            return pop;
        }

        // 原版 :3505 getCampMiscCount()
        public static int GetCampMiscCount()
        {
            var pop = 0;

            if (SceneInstance<SophieFollowerScene>.Get().SophieFollower())
                pop += Flags.Get(GameFlagIds.SophieAdultKidCount);

            if (IsabellaScene.isabellaFollower())
                pop += SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren();

            // 原版 :3511 穿着 Valeria（goo armor）时她也算一个人
            if (Player.Armor.Id == ArmorIds.GooArmor)
                pop += 1;

            if (Flags.Get(GameFlagIds.ClaraImprisoned) > 0)
                pop += 1;

            return pop;
        }

        // 原版 :3758 getUniqueKills()
        public static int GetUniqueKills()
        {
            var count = 0;

            if (Flags.Get(GameFlagIds.D1OmnibusKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.ZetazDefeatedAndKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.KeltKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.JojoDeadOrGone) == 2)
                count += 1;

            if (Flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.FuckFlowerKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.TamaniBadEnded) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.DullahanRude) == 3)
                count += 1;

            if (Flags.Get(GameFlagIds.CeraphKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 2)
                count += 1;

            if (Flags.Get(GameFlagIds.KihaKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.AikoBossComplete) == 1)
                count += 1;

            if (OwcaScene.Save.GetBool("rebeccKilled"))
                count += 1;

            // 原版 :3782 Akbal 任务完成且没收他当宠物 = 他死了（AKBAL_QUEST_DONE = 1 << 6）
            if ((Flags.Get(GameFlagIds.AkbalQuestStatus) & 64) > 0
                && !SceneInstance<AkkyScene>.Get().IsOwned())

                count += 1;

            // Lethice Keep
            if (Flags.Get(GameFlagIds.D3GardenerDefeated) == 3)
                count += 1;

            if (Flags.Get(GameFlagIds.D3CentaurDefeated) == 1)
                count += 1;

            if (Flags.Get(GameFlagIds.D3MechanicFightResult) == 1)
                count += 1;

            if (Flags.Get(GameFlagIds.DriderincubusKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.MinotaurkingKilled) > 0)
                count += 1;

            if (Flags.Get(GameFlagIds.LethiceKilled) > 0)
                count += 1;

            if (SceneInstance<FrogGirlScene>.Get().taughtLesson > 2)
                count += 1;

            if (LumberjackScene._executed)
                count += 1;

            return count;
        }

        // 原版 :3751 fixFlags()：凤凰塔已经通关的存档补上 HE L 塔标记
        public static void FixFlags()
        {
            if (DungeonClearChecks.PhoenixTowerClear())
                Flags.Set(GameFlagIds.ClearedHelTower, 1);
        }

        // 原版 :4030 getTotalKills()
        public static int GetTotalKills()
        {
            var count = GetUniqueKills();

            count += Flags.Get(GameFlagIds.ImpsKilled);

            count += Flags.Get(GameFlagIds.GoblinsKilled);

            count += Flags.Get(GameFlagIds.TentacleBeastsKilled);

            count += Flags.Get(GameFlagIds.HellhoundsKilled);

            count += Flags.Get(GameFlagIds.MinotaursKilled);

            count += Flags.Get(GameFlagIds.WormsMassKilled);

            count += Flags.Get(GameFlagIds.AlicesKilled);

            count += Flags.Get(GameFlagIds.HellmouthsKilled);

            count += Areas.Swamp.AlrauneScene.Save.GetInt("alrauneKilled", 0);

            count += Areas.Swamp.FemaleSpiderMorphScene.Save.GetInt("spidersKilled", 0);

            count += Areas.Swamp.PlagueRatScene.Save.GetInt("ratsKilled", 0);

            count += IvorySuccubusScene.Save.GetInt("deadHookers", 0);

            return count;
        }

        // 原版 :3583 totalChildrenForAscension()
        public static int TotalChildrenForAscension()
        {
            var amount = 0;

            amount += Flags.Get(GameFlagIds.AmilyBirthTotal) + Flags.Get(GameFlagIds.PcTimesBirthedAmilykids);

            amount += Flags.Get(GameFlagIds.BenoitEggs) + Flags.Get(GameFlagIds.FemoitEggsLaid);

            amount += Flags.Get(GameFlagIds.CottonKidCount);

            amount += Flags.Get(GameFlagIds.EdrynNumberOfKids);

            amount += SceneInstance<EmberScene>.Get().EmberChildren();

            amount += SceneInstance<IsabellaScene>.Get().TotalIsabellaChildren();

            amount += SceneInstance<IzmaScene>.Get().TotalIzmaChildren();

            amount += Flags.Get(GameFlagIds.KellyKids);

            amount += SceneInstance<KihaFollowerScene>.Get().TotalKihaChildren();

            amount += Flags.Get(GameFlagIds.LoppeKids);

            amount += Flags.Get(GameFlagIds.LynnetteBabyCount);

            amount += Flags.Get(GameFlagIds.MarbleKids);

            amount += Flags.Get(GameFlagIds.MinervaChildren);

            // 原版：5000 只蚂蚁孩子只算 30 个其他孩子
            amount += (int)Math.Pow(Flags.Get(GameFlagIds.AntKids) + Flags.Get(GameFlagIds.PhyllaDriderBabiesCount), 0.4);

            amount += Flags.Get(GameFlagIds.SheilaJoeys) + Flags.Get(GameFlagIds.SheilaImps);

            amount += SceneInstance<SophieBimboScene>.Get().SophieChildren();

            amount += Flags.Get(GameFlagIds.TamaniNumberOfDaughters) / 4;

            amount += SceneInstance<UrtaPregsScene>.Get().UrtaKids();

            amount += SceneInstance<HelSpawnScene>.Get().HelspawnFollower() ? 1 : 0;

            amount += SylviaScene.Save.GetInt("doloresProgress", 0) > 0 ? 1 : 0;

            return amount;
        }

        /// <summary>原版 CoC.hx:395 game.modSaveVersion。</summary>
        public const int CurrentSaveVersion = 15;

        // 原版 :3853 updateAchievements()：导入旧存档时按旗标补发成就
        public static void UpdateAchievements()
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var time = View.Time;

            var survival = Configuration.OptionStore.Instance.Get("survival", 0) > 0;

            void Award(string id) => AwardAchievement(new Identifier("coc", id));

            // Story
            Award("story_newcomer");

            if (flags.Get(GameFlagIds.MaraeQuestComplete) > 0)
                Award("story_marae_savior");

            if (player.HasKeyItem("Zetaz's Map"))
                Award("story_zetaz_revenge");

            if (flags.Get(GameFlagIds.LethiceDefeated) > 0)
                Award("story_finalboss");

            // Areas
            if (flags.Get(GameFlagIds.TimesExploredForest) > 0
                && flags.Get(GameFlagIds.TimesExploredLake) > 0

                && flags.Get(GameFlagIds.TimesExploredDesert) > 0

                && flags.Get(GameFlagIds.TimesExploredMountain) > 0

                && flags.Get(GameFlagIds.TimesExploredPlains) > 0

                && flags.Get(GameFlagIds.TimesExploredSwamp) > 0

                && player.HasStatusEffect(StatusEffectIds.ExploredDeepwoods)

                && flags.Get(GameFlagIds.DiscoveredHighMountain) > 0

                && flags.Get(GameFlagIds.BogExplored) > 0

                && flags.Get(GameFlagIds.DiscoveredGlacialRift) > 0

                && flags.Get(GameFlagIds.DiscoveredVolcanoCrag) > 0)

                Award("zone_explorer");

            if (PlacesCount() >= 10)
                Award("zone_sightseer");

            if (flags.Get(GameFlagIds.TimesExplored) >= 1)
                Award("zone_where_am_i");

            if (flags.Get(GameFlagIds.TimesExploredDesert) >= 100)
                Award("zone_dehydrated");

            if (flags.Get(GameFlagIds.TimesExploredForest) >= 100)
                Award("zone_forest_ranger");

            if (flags.Get(GameFlagIds.TimesExploredLake) >= 100)
                Award("zone_vacationer");

            if (flags.Get(GameFlagIds.TimesExploredMountain) >= 100)
                Award("zone_mountaineer");

            if (flags.Get(GameFlagIds.TimesExploredPlains) >= 100)
                Award("zone_rolling_hills");

            if (flags.Get(GameFlagIds.TimesExploredSwamp) >= 100)
                Award("zone_wet_all_over");

            if (player.StatusEffectValue(StatusEffectIds.ExploredDeepwoods, 0) >= 100)
                Award("zone_we_need_to_go_deeper");

            if (flags.Get(GameFlagIds.DiscoveredHighMountain) >= 100)
                Award("zone_light_headed");

            if (flags.Get(GameFlagIds.BogExplored) >= 100)
                Award("zone_all_murky");

            if (flags.Get(GameFlagIds.DiscoveredGlacialRift) >= 100)
                Award("zone_frozen");

            if (flags.Get(GameFlagIds.DiscoveredVolcanoCrag) >= 100)
                Award("zone_roasted");

            // Places
            if (player.StatusEffectValue(StatusEffectIds.BoatDiscovery, 0) >= 15)
                Award("zone_sea_legs");

            if (player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) >= 30)
                Award("zone_farmer");

            if (flags.Get(GameFlagIds.AmilyVillageExplored) >= 15)
                Award("zone_archaeologist");

            // Levels
            if (player.Level >= 2) Award("level_level_up");
            if (player.Level >= 5) Award("level_novice");
            if (player.Level >= 10) Award("level_apprentice");
            if (player.Level >= 15) Award("level_journeyman");
            if (player.Level >= 20) Award("level_expert");
            if (player.Level >= 30) Award("level_master");
            if (player.Level >= 45) Award("level_grandmaster");
            if (player.Level >= 60) Award("level_illustrious");
            if (player.Level >= 90) Award("level_overlord");
            if (player.Level >= 120) Award("level_are_you_a_god");

            // Population
            var population = GetCampPopulation();

            if (population >= 2) Award("population_first");
            if (population >= 5) Award("population_hamlet");
            if (population >= 10) Award("population_village");
            if (population >= 25) Award("population_town");
            if (population >= 100) Award("population_city");
            if (population >= 250) Award("population_metropolis");
            if (population >= 500) Award("population_megalopolis");
            if (population >= 1000) Award("population_city_state");
            if (population >= 2500) Award("population_kingdom");
            if (population >= 5000) Award("population_empire");

            // Time
            if (time.Days >= 30) Award("time_month");
            if (time.Days >= 180) Award("time_half_year");
            if (time.Days >= 365) Award("time_annual");
            if (time.Days >= 730) Award("time_biennial");
            if (time.Days >= 1095) Award("time_triennial");
            if (time.Days >= 1825) Award("time_long_haul");
            if (time.Days >= 3650) Award("time_decade");
            if (time.Days >= 36500) Award("time_century");

            // Dungeons
            var dungeonsCleared = 0;

            if (DungeonClearChecks.FactoryClear())
            {
                Award("dungeon_shut_down_everything");

                dungeonsCleared += 1;
            }

            if (DungeonClearChecks.DeepCaveClear())
            {
                Award("dungeon_youre_in_deep");

                dungeonsCleared += 1;
            }

            if (DungeonClearChecks.SandCaveClear())
            {
                Award("dungeon_sand_witch_friend");

                dungeonsCleared += 1;
            }

            if (DungeonClearChecks.LethiceStrongholdClear())
            {
                Award("dungeon_end_of_reign");

                dungeonsCleared += 1;
            }

            if (DungeonClearChecks.PhoenixTowerClear())
            {
                Award("dungeon_phoenix_fall");

                dungeonsCleared += 1;

                // 原版：只在当前版本存档的无高潮通关才算
                if (flags.Get(GameFlagIds.TimesOrgasmed) <= 0
                    && flags.Get(GameFlagIds.ModSaveVersion) == CurrentSaveVersion)

                    Award("dungeon_extremely_chaste_delver");
            }

            if (DungeonClearChecks.ManorClear())
                dungeonsCleared += 1;

            if (DungeonClearChecks.TowerDeceptionClear())
                dungeonsCleared += 1;

            if (dungeonsCleared >= 1) Award("dungeon_delver");
            if (dungeonsCleared >= 3) Award("dungeon_delver_apprentice");
            if (dungeonsCleared >= 7) Award("dungeon_delver_master");

            // Fashion
            if (player.Armor.Id.Path == "wizard_robes" && player.Weapon.Id.Path == "wizard_staff")
                Award("fashion_wannabe_wizard");

            if (player.PreviouslyWornClothes.Count >= 10)
                Award("fashion_cosplayer");

            if (player.Armor.Id.Path is "fetish_wear" or "bondage_straps" or "nurse_outfit"
                && player.Weapon.Id.Path is "riding_crop" or "whip" or "succubus_whip" or "lethices_whip")

                Award("fashion_dominatrix");

            if (player.Armor.Id.Path != "nothing"
                && player.LowerGarment.Id.Path == "nothing"

                && player.UpperGarment.Id.Path == "nothing")

                Award("fashion_going_commando");

            if (player.Jewelry.Value >= 1000)
                Award("fashion_bling_bling");

            // Wealth
            if (player.Gems >= 1000) Award("wealth_rich");
            if (player.Gems >= 10000) Award("wealth_hoarder");
            if (player.Gems >= 100000) Award("wealth_gem_vault");
            if (player.Gems >= 1000000) Award("wealth_millionaire");

            // Combat
            if (player.HasStatusEffect(StatusEffectIds.KnowsCharge)
                && player.HasStatusEffect(StatusEffectIds.KnowsBlind)

                && player.HasStatusEffect(StatusEffectIds.KnowsWhitefire)

                && player.HasStatusEffect(StatusEffectIds.KnowsArouse)

                && player.HasStatusEffect(StatusEffectIds.KnowsHeal)

                && player.HasStatusEffect(StatusEffectIds.KnowsMight))

                Award("combat_wizard");

            // Realistic
            if (flags.Get(GameFlagIds.AchievementProgressFasting) >= 168 && survival)
                Award("realistic_fasting");

            // Holidays
            if (SceneInstance<Seasonal.NieveScene>.Get().NieveFollower())
                Award("holiday_christmas_iii");

            if (flags.Get(GameFlagIds.AchievementProgressEggHunter) >= 10)
                Award("holiday_egg_hunter");

            // General
            if (flags.Get(GameFlagIds.DemonsDefeated) >= 25 && time.Days >= 10)
                Award("general_portal_defender");

            if (flags.Get(GameFlagIds.LethiceKilled) == 2)
                Award("general_off_with_her_head");

            if (GetUniqueKills() >= 3)
                Award("general_bad_ender");

            if (flags.Get(GameFlagIds.TimesTransformed) >= 1)
                Award("general_whats_happening_to_me");

            if (flags.Get(GameFlagIds.TimesTransformed) >= 10)
                Award("general_transformer");

            if (flags.Get(GameFlagIds.TimesTransformed) >= 25)
                Award("general_shapeshifty");

            if (flags.Get(GameFlagIds.TimesMasturbated) >= 1)
                Award("general_fapfapfap");

            if (flags.Get(GameFlagIds.TimesMasturbated) >= 10)
                Award("general_faptastic");

            if (flags.Get(GameFlagIds.TimesMasturbated) >= 100)
                Award("general_fapster");

            if (player.Armor.Id.Path == "goo_armor")
                Award("goo_armor");

            if (SceneInstance<HelSpawnScene>.Get().HelspawnFollower())
                Award("general_helspawn");

            if (flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0)
                Award("general_godslayer");

            if (FollowersCount() >= 7)
                Award("general_follow_the_leader");

            if (LoversCount() >= 8)
                Award("general_gotta_love_them_all");

            if (SlavesCount() >= 4)
                Award("general_meet_your_master");

            if (FollowersCount() + LoversCount() + SlavesCount() >= 19)
                Award("general_all_ur_pplz_r_blng_2_me");

            if (flags.Get(GameFlagIds.MansionVisited) >= 3)
                Award("general_freeloader");

            if (player.Perks.Count >= 20) Award("general_perky");
            if (player.Perks.Count >= 35) Award("general_super_perky");
            if (player.Perks.Count >= 50) Award("general_ultra_perky");

            if (player.Strength >= 50 && player.Toughness >= 50 && player.Speed >= 50 && player.Intelligence >= 50)
                Award("general_stats_50");

            if (player.Strength >= 100 && player.Toughness >= 100 && player.Speed >= 100 && player.Intelligence >= 100)
                Award("general_stats_100");

            if (flags.Get(GameFlagIds.AchievementProgressSchizophrenia) >= 4)
                Award("general_schizo");

            if (flags.Get(GameFlagIds.AchievementProgressCleanSlate) >= 2)
                Award("general_clean_slate");

            if (flags.Get(GameFlagIds.AchievementProgressImNoLumberjack) >= 100)
                Award("general_im_no_lumberjack");

            if (flags.Get(GameFlagIds.AchievementProgressDeforester) >= 100)
                Award("general_deforester");

            if (flags.Get(GameFlagIds.AchievementProgressHammerTime) >= 300)
                Award("general_hammer_time");

            if (flags.Get(GameFlagIds.AchievementProgressScavenger) >= 200)
                Award("general_nail_scavenger");

            if (flags.Get(GameFlagIds.AchievementProgressYabbaDabbaDoo) >= 100)
                Award("general_yabba_dabba_doo");

            if (flags.Get(GameFlagIds.AchievementProgressAntworks) >= 200)
                Award("general_antworks");

            if (flags.Get(GameFlagIds.CampCabinFurnitureBed) >= 1
                && flags.Get(GameFlagIds.CampCabinFurnitureNightstand) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureDresser) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureTable) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureChair1) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureChair2) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureBookshelf) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureDesk) >= 1

                && flags.Get(GameFlagIds.CampCabinFurnitureDeskchair) >= 1)

                Award("general_home_sweet_home");

            if (flags.Get(GameFlagIds.CampWallGate) > 0)
                Award("general_make_mareth_great_again");

            if (flags.Get(GameFlagIds.CampWallStatues) >= 100)
                Award("general_terracotta_impy");

            if (player.Tallness >= 132)
                Award("general_up_to_11");

            if (SceneInstance<JojoScene>.Get().JojoFollower())
                Award("general_jojos_bizarre_adventure");

            // Dedicked NPCs
            var npcsDedicked = 0;

            if (flags.Get(GameFlagIds.IzmaNoCock) > 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.CeraphHidingDick) > 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.RubiAdmittedGender) > 0 && flags.Get(GameFlagIds.RubiCockSize) <= 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.BenoitStatus) == 1 || flags.Get(GameFlagIds.BenoitStatus) == 2) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.ArianHealth) > 0 && flags.Get(GameFlagIds.ArianCockSize) <= 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.KatherineUnlocked) > 0 && flags.Get(GameFlagIds.KatherineDickCount) <= 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.MetKitsunes) > 0 && flags.Get(GameFlagIds.RedheadIsFuta) == 0) npcsDedicked += 1;
            if (flags.Get(GameFlagIds.KeltBreakLevel) == 4) npcsDedicked += 1;

            if (npcsDedicked >= 3)
                Award("general_dick_banisher");

            if (npcsDedicked >= 7)
                Award("general_you_bastard");
        }

        // 原版 :3579 promptSaveUpdate()：逐版本迁移旧存档
        private void PromptSaveUpdate()
        {
            ClearOutput();

            var version = Flags.Get(GameFlagIds.ModSaveVersion);

            if (version < 2)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 2);

                OutputText("Scenes.Camp.PromptSaveUpdate.Caution1".Localized());

                OutputText("Scenes.Camp.PromptSaveUpdate.Caution2".Localized());

                OutputText("Scenes.Camp.PromptSaveUpdate.Caution3".Localized());

                OutputText("Scenes.Camp.PromptSaveUpdate.Caution4".Localized());

                OutputText("Scenes.Camp.PromptSaveUpdate.Caution5".Localized());

                AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                return;
            }

            if (version == 2)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 3);

                OutputText("Scenes.Camp.PromptSaveUpdate.AchievementsIntro".Localized());

                OutputText("Scenes.Camp.PromptSaveUpdate.AchievementsSome".Localized());

                UpdateAchievements();

                OutputText("Scenes.Camp.PromptSaveUpdate.AchievementsSaved".Localized());

                AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                return;
            }

            if (version == 3)
            {
                // 回收旧标记，留给后续版本复用
                Flags.Set(GameFlagIds.GiacomoMet, 0);

                Flags.Set(GameFlagIds.GiacomoNoticesWorms, 0);

                Flags.Set(GameFlagIds.PhoenixEncountered, 0);

                Flags.Set(GameFlagIds.PhoenixWankedCounter, 0);

                Flags.Set(GameFlagIds.ModSaveVersion, 4);

                Go<CampScene>();

                return;
            }

            if (version == 4)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 5);

                // 原版 bug：杀掉 Kelt 后弓熟练度会被重置；这里按原版补偿直接拉满
                if (Flags.Get(GameFlagIds.KeltKilled) > 0
                    && Player.StatusEffectValue(StatusEffectIds.Kelt, 0) <= 0)
                {
                    ClearOutput();

                    OutputText("Scenes.Camp.PromptSaveUpdate.KeltBow".Localized());

                    Player.AddMastery(new Identifier("coc", "Bow"), 5);

                    AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                    return;
                }

                Go<CampScene>();

                return;
            }

            if (version == 5)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 6);

                // 原版被重分类的三件装备里，C# 数据只剩毛皮缠腰布
                if (Player.Armor.Id.Path == "fur_loincloth")
                {
                    ClearOutput();

                    OutputText("Scenes.Camp.PromptSaveUpdate.Reclassified".Localized());

                    var removed = Player.SetArmor(Armor.Nothing);

                    Player.Inventory.Add(removed, 1);

                    AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                    return;
                }

                Go<CampScene>();

                return;
            }

            if (version == 6)
            {
                Flags.Set(GameFlagIds.D1OmnibusKilled, Flags.Get(GameFlagIds.CorruptedGladesDestroyed));

                Flags.Set(GameFlagIds.CorruptedGladesDestroyed, 0);

                if (Player.Armor.Id.Path == "goo_armor")
                    Flags.Set(GameFlagIds.ValeriaFluids, 100);

                Flags.Set(GameFlagIds.ModSaveVersion, 7);

                Go<CampScene>();

                return;
            }

            if (version == 7)
            {
                Flags.Set(GameFlagIds.LethiciteArmorTaken, Flags.Get(GameFlagIds.JojoAnalCatchCounter));

                Flags.Set(GameFlagIds.JojoAnalCatchCounter, 0);

                Flags.Set(GameFlagIds.ModSaveVersion, 8);

                Go<CampScene>();

                return;
            }

            if (version == 8)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 9);

                if (!Player.HasFur())
                {
                    Go<CampScene>();

                    return;
                }

                ClearOutput();

                OutputText("Scenes.Camp.PromptSaveUpdate.FurColorIntro".Localized());

                FurColorSelection1();

                return;
            }

            if (version == 9)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 10);

                // 原版：把旧的「Marae's Lethicite」换成带数值的新钥匙物品
                var lethicite = Flags.Get(GameFlagIds.MaraeLethicite);

                if (lethicite > 0 && Player.HasKeyItem("Marae's Lethicite"))
                {
                    Player.RemoveKeyItem("Marae's Lethicite");

                    Player.CreateKeyItem("Marae's Lethicite", lethicite);

                    Flags.Set(GameFlagIds.MaraeLethicite, 0);
                }
            }

            if (version == 10)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 11);

                if (Flags.Get(GameFlagIds.EmberSparVictories) > 0)
                {
                    ClearOutput();

                    OutputText("Scenes.Camp.PromptSaveUpdate.LethiceAgain".Localized());

                    Flags.Set(GameFlagIds.EmberSparVictories, 0);

                    AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());

                    return;
                }
            }

            if (version == 11)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 12);

                Flags.Set(GameFlagIds.GrimdarkMode, 0);
            }

            if (version == 12)
            {
                Flags.Set(GameFlagIds.ModSaveVersion, 13);

                // 原版：把 6/7 阶段往回挪 2，让建造进度停在 10 而不是 12
                if (Flags.Get(GameFlagIds.CampCabinProgress) > 5)
                    Flags.Set(GameFlagIds.CampCabinProgress, Flags.Get(GameFlagIds.CampCabinProgress) - 2);
            }

            if (version == 13)

                Flags.Set(GameFlagIds.ModSaveVersion, 14); // 原版把数字键 2295/2296 迁移成命名键；C# 存档一直按命名键存，无需迁移

            if (version == 14)

                Flags.Set(GameFlagIds.ModSaveVersion, 15); // 原版回收数字键 2194/254/255；C# 存档里不存在这些数字键

            Go<CampScene>();
        }

        // 原版 :3727 furColorSelection1()
        private void FurColorSelection1()
        {
            ClearOutput();

            FurColorButton(0, "Scenes.Camp.FurColor.Brown", "brown");

            FurColorButton(1, "Scenes.Camp.FurColor.Chocolate", "chocolate");

            FurColorButton(2, "Scenes.Camp.FurColor.Auburn", "auburn");

            FurColorButton(3, "Scenes.Camp.FurColor.Orange", "orange");

            AddButton(4, "Common.Button.Next".Localized(), FurColorSelection2);

            FurColorButton(5, "Scenes.Camp.FurColor.Caramel", "caramel");

            FurColorButton(6, "Scenes.Camp.FurColor.Peach", "peach");

            FurColorButton(7, "Scenes.Camp.FurColor.SandyBrown", "sandy brown");

            FurColorButton(8, "Scenes.Camp.FurColor.Golden", "golden");
        }

        // 原版 :3741 furColorSelection2()
        private void FurColorSelection2()
        {
            ClearOutput();

            FurColorButton(0, "Scenes.Camp.FurColor.MidnightBlack", "midnight black");

            FurColorButton(1, "Scenes.Camp.FurColor.Black", "black");

            FurColorButton(2, "Scenes.Camp.FurColor.DarkGray", "dark gray");

            FurColorButton(3, "Scenes.Camp.FurColor.Gray", "gray");

            FurColorButton(5, "Scenes.Camp.FurColor.LightGray", "light gray");

            FurColorButton(6, "Scenes.Camp.FurColor.Silver", "silver");

            FurColorButton(7, "Scenes.Camp.FurColor.White", "white");

            AddButton(9, "Common.Button.Previous".Localized(), FurColorSelection1);

            FurColorButton(10, "Scenes.Camp.FurColor.OrangeWhite", "orange and white");

            FurColorButton(11, "Scenes.Camp.FurColor.BrownWhite", "brown and white");

            FurColorButton(12, "Scenes.Camp.FurColor.BlackWhite", "black and white");

            FurColorButton(13, "Scenes.Camp.FurColor.GrayWhite", "gray and white");
        }

        private void FurColorButton(int index, string key, string color) =>

            AddButton(index, key.Localized(), () => ChooseFurColorSaveUpdate(color));

        // 原版 :3777 chooseFurColorSaveUpdate()
        private void ChooseFurColorSaveUpdate(string color)
        {
            ClearOutput();

            OutputText("Scenes.Camp.FurColor.Chosen".Localized(color));

            Player.Body.Get<SkinPart>().FurColor = color;

            AddNextButton("Common.Button.Next".Localized(), () => Go<CampScene>());
        }

        public static List<string> CompanionsList() =>

            FollowerRegistry.All.Where(definition => definition.IsPresent()).Select(definition => definition.Name).ToList();

        public static List<string> MalePartnersAtCampList()
        {
            var companions = CompanionsList();

            var males = new List<string>();

            if (SceneInstance<ArianScene>.Get().ArianMale() && companions.Contains("Arian"))
                males.Add("Arian");

            if (SceneInstance<Seasonal.NieveScene>.Get().IsMale() && companions.Contains("Nieve"))
                males.Add("Nieve");

            if (SceneInstance<EmberScene>.Get().EmberIsMasculine() && companions.Contains("Ember"))
                males.Add("Ember");

            if (SceneInstance<JojoScene>.Get().CampCorruptJojo() && companions.Contains("Jojo"))
                males.Add("Jojo");

            return males;
        }

        public static List<string> FemalePartnersAtCampList()
        {
            var males = MalePartnersAtCampList();

            var companions = CompanionsList();

            var females = FollowerRegistry.Present(FollowerIds.Roles.Lover)
                .Concat(FollowerRegistry.Present(FollowerIds.Roles.Slave))
                .Select(definition => definition.Name)
                .Where(name => !males.Contains(name))
                .ToList();

            if (SceneInstance<EmberScene>.Get().EmberIsFeminine() && companions.Contains("Ember"))
                females.Add("Ember");

            if (companions.Contains("Sophie"))
                females.Add("Sophie");

            return females;
        }

        public static FollowerDefinition CampGuardRandom()
        {
            var guards = CampGuardList().ToArray();

            if (guards.Length == 0)
                throw new InvalidOperationException("campGuardRandom() called when no guards");

            return Internals.RandomUtils.Choice(guards);
        }

        public static bool BuiltBarrel => true;

        public static bool HasBookshelf => CabinInterior.HasBookshelf;

        public static bool HasChest() =>

            Player.HasKeyItem("Camp - Chest") ||

            Player.HasKeyItem("Camp - Murky Chest") ||

            Player.HasKeyItem("Camp - Ornate Chest");

        public class CampKidBreakdown
        {
            public int dragon;

            public int shark;

            public int harpy;

            public int cow;

            public int ant;

            public int drider;

            public int anemone;

            public int salamander;

            public int total;
        }

        public static CampKidBreakdown GetCampKidBreakdown()
        {
            var count = new CampKidBreakdown();

            if (SceneInstance<EmberScene>.Get().FollowerEmber())
                count.dragon += SceneInstance<EmberScene>.Get().EmberChildren();

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                count.dragon += SceneInstance<KihaFollowerScene>.Get().TotalKihaChildren();

            if (SceneInstance<IzmaScene>.Get().IzmaFollower())
                count.shark += SceneInstance<IzmaScene>.Get().TotalIzmaChildren();

            if (SceneInstance<SophieFollowerScene>.Get().SophieFollower() &&
                Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)

                count.harpy += 1;

            if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                count.cow += Flags.Get(GameFlagIds.MarbleKids);

            if (SceneInstance<AnemoneScene>.Get().AnemoneFollower())
                count.anemone += 1;

            if (SceneInstance<HelSpawnScene>.Get().helspawnName != "" &&
                Flags.Get(GameFlagIds.HelspawnAge) < 3)

                count.salamander += 1;

            if (SceneInstance<Areas.Desert.AntsScene>.Get().PhyllaWaifu())
            {
                count.ant += Flags.Get(GameFlagIds.AntKids);

                count.drider += Flags.Get(GameFlagIds.PhyllaDriderBabiesCount);
            }

            count.total = count.dragon + count.shark + count.harpy + count.cow +

                          count.ant + count.drider + count.anemone + count.salamander;

            return count;
        }

        public static IReadOnlyList<FollowerDefinition> CampGuardList() =>

            FollowerRegistry.Guards;

        public static int HoursUntil(int hour)
        {
            var amount = 0;

            var current = View.Time.Hours;

            while (current != hour)
            {
                current++;

                amount++;

                if (current >= 24)
                    current = 0;
            }

            return amount;
        }

        public static bool IsSurvival => Configuration.OptionStore.Instance.Get("survival", 0) > 0;

        public static bool HasCabinBed => Flags.Get(GameFlagIds.CampBuiltCabin) > 0 && CabinInterior.HasBed;

        public static bool HasBeeCock => Player.Cocks.Exists(cock => cock.Id == new Identifier("coc", "bee"));

        public static bool IsGooArmor => Player.Armor.Id == ArmorIds.GooArmor;

        public static bool IsTentacleBarkArmor => Player.Armor.Id == ArmorIds.TentacleBarkArmor;

        public static string HourText(int hours) => hours == 1 ? "1 hour" : hours + "Common.Text.Hours".Localized();

        public static int WakeupTime()
        {
            var sleepWith = Flags.Get(GameFlagIds.SleepWith);

            var supportsAlarm = sleepWith is "" or "Ember" or "Amily" or "Izma" or "Helia" or "salamanders";

            if (Flags.Get(GameFlagIds.BenoitClockBought) > 0 && supportsAlarm)
                return Flags.Get(GameFlagIds.BenoitClockAlarm);

            return 6;
        }

        public static void Wait()
        {
            ClearOutput();

            var fatigueRecovery = 2f;

            if (Player.HasPerk(PerkIds.SpeedyRecovery))
                fatigueRecovery *= 1.5f;

            if (Player.HasPerk(PerkIds.ControlledBreath))
                fatigueRecovery *= 1.1f;

            if (Player.HasPerk(PerkIds.SpiderBedding))
                fatigueRecovery += 12f;

            var hours = UserInput.Shift ? Math.Max(1, 21 - View.Time.Hours) : 4;

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                fatigueRecovery /= 2f;

            var starvedGoo = IsGooArmor && Flags.Get(GameFlagIds.ValeriaFluids) <= 0;

            View.Time.PassTime(hours);

            if (!starvedGoo)
                Player.ChangeFatigue(-fatigueRecovery * hours);

            DisplayHeader("Scenes.Camp.Title".Localized());

            OutputText("Scenes.Camp.WaitText".Localized(HourText(hours)));

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                OutputText("Scenes.Camp.WaitWithdrawal".Localized());

            if (HasBeeCock)
                OutputText("Scenes.Camp.WaitBee".Localized());

            if (starvedGoo)
                OutputText("Scenes.Camp.WaitGoo".Localized());

            AddButton(0, "Common.OK".Localized(), () => Go<CampScene>());
        }

        public static void Rest()
        {
            ClearOutput();

            Player.Sleeping = false;

            var hpRecovery = 10f;

            var fatigueRecovery = 4f;

            var multiplier = 1f;

            if (Player.HasPerk(PerkIds.Medicine))
                hpRecovery *= 1.5f;

            if (HasCabinBed)
                multiplier += 0.5f;

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                multiplier /= 2f;

            if (IsSurvival && Player.Hunger < 25)
                multiplier /= 2f;

            var available = Math.Max(1, 21 - View.Time.Hours);

            var hours = 0;

            if (UserInput.Shift)
            {
                while (hours < available && (Player.Health < Player.MaxHealth || Player.Fatigue > 0))
                {
                    Player.HealthChange(hpRecovery * multiplier);

                    Player.ChangeFatigue(-fatigueRecovery * multiplier);

                    hours++;

                    if (View.Time.Hours + hours >= 24 || (IsSurvival && Player.Hunger < 5))
                        break;
                }

                if (hours == 0)
                    hours = 1;
            }
            else
                hours = Math.Min(4, available);

            hours = Math.Min(hours, available);

            View.Time.PassTime(hours);

            if (hours > 0 && !UserInput.Shift)
            {
                Player.HealthChange(hpRecovery * multiplier * hours);

                Player.ChangeFatigue(-fatigueRecovery * multiplier * hours);
            }

            DisplayHeader("Scenes.Camp.Title".Localized());

            OutputText(HasCabinBed

                ? "Scenes.Camp.RestCabin".Localized(HourText(hours))

                : "Scenes.Camp.RestNormal".Localized(HourText(hours)));

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                OutputText("Scenes.Camp.RestWithdrawal".Localized());

                Player.Toughness -= 0.1f;

                Player.Intelligence -= 0.1f;
            }

            if (HasBeeCock)
                OutputText("Scenes.Camp.RestBee".Localized());

            if (IsGooArmor && Flags.Get(GameFlagIds.ValeriaFluids) <= 0)
                OutputText("Scenes.Camp.RestGoo".Localized());

            if (IsSurvival && Player.Hunger < 25)
                OutputText("Scenes.Camp.RestHungry".Localized());

            AddButton(0, "Common.OK".Localized(), () => Go<CampScene>());
        }

        public static void SaveSleep()
        {
            var hardcore = Configuration.OptionStore.Instance.Get("hardcore", 0) > 0;

            var hardcoreSlot = Configuration.OptionStore.Instance.Get("hardcoreSlot", 0);

            if (!hardcore || hardcoreSlot <= 0)
            {
                if (!SaveData.TryAutoSave(Player))
                    OutputText("Scenes.Camp.AutoSaveFailed".Localized());

                return;
            }

            try
            {
                var slot = "CoC_" + hardcoreSlot;

                SaveManager.Save(slot, SaveData.Capture(Player, slot).ToJson());
            }
            catch
            {
                OutputText("Scenes.Camp.AutoSaveFailed".Localized());
            }
        }

        public static void Sleep()
        {
            ClearOutput();

            Player.Sleeping = true;

            View.Time.Minutes = 0;

            var hours = HoursUntil(WakeupTime());

            var multiplier = 1f;

            if (HasCabinBed)
                multiplier += 0.5f;

            if (IsSurvival && Player.Hunger < 25)
                multiplier *= 0.5f;

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                multiplier *= 0.5f;
            else if (Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3)
                multiplier *= 0.75f;

            if (IsTentacleBarkArmor)
                if (Player.Corruption < 33)
                    multiplier *= 0.75f;
                else if (Player.Corruption < 66)
                    multiplier *= 0.9f;

            SaveSleep();

            View.Time.PassTime(hours);

            Player.HealthChange(20f * multiplier * hours);

            Player.ChangeFatigue(-20f * multiplier * hours);

            DisplayHeader("Scenes.Camp.Title".Localized());

            OutputText(HasCabinBed

                ? "Scenes.Camp.SleepCabin".Localized(HourText(hours))

                : "Scenes.Camp.SleepNormal".Localized(HourText(hours)));

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                OutputText("Scenes.Camp.SleepWithdrawal".Localized());

                Player.Toughness -= 0.1f;

                Player.Intelligence -= 0.1f;
            }
            else if (Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3)
                OutputText("Scenes.Camp.SleepMinotaur".Localized());

            if (HasBeeCock)
                OutputText("Scenes.Camp.SleepBee".Localized());

            if (IsGooArmor && Flags.Get(GameFlagIds.ValeriaFluids) <= 0)
                OutputText("Scenes.Camp.SleepGoo".Localized());

            if (IsTentacleBarkArmor)
                OutputText(Player.Corruption < 33

                    ? "Scenes.Camp.SleepTentacleLow".Localized()

                    : Player.Corruption < 66

                        ? "Scenes.Camp.SleepTentacleMid".Localized()

                        : "Scenes.Camp.SleepTentacleHigh".Localized());

            if (IsSurvival && Player.Hunger < 25)
                OutputText("Scenes.Camp.SleepHungry".Localized());

            OutputText("Scenes.Camp.WakeUp".Localized(HourText(hours)));

            AddButton(0, "Common.OK".Localized(), () =>
            {
                Player.Sleeping = false;

                Go<CampScene>();
            });
        }
    }
}
