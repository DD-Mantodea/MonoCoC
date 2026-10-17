using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.World
{

    public class PlayerEvents : ITimeAware
    {
        // 原版 PlayerEvents 的 checkedTurkey / checkedDream 计数器：每次长 tick 只检查一次
        private static int _checkedTurkey;

        private static int _checkedDream;

        public bool TimeChange()
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var survival = OptionStore.Instance.Get("survival", 0) > 0;

            var needNext = false;

            if (player.CumMultiplier > 19999)
                player.CumMultiplier = 19999;

            if (player.BallSize > 400)
                player.BallSize = 400;

            if (flags.Get(GameFlagIds.SockCounter) > 0)
            {
                flags.Set(GameFlagIds.SockCounter, flags.Get(GameFlagIds.SockCounter) - 1);

                if (flags.Get(GameFlagIds.SockCounter) < 0)
                    flags.Set(GameFlagIds.SockCounter, 0);

                if (flags.Get(GameFlagIds.SockCounter) > 24)
                    flags.Set(GameFlagIds.SockCounter, 24);
            }

            if (!survival || player.Hunger >= 10)
            {
                player.HoursSinceCum += 1;

                if (player.HasPerk(PerkIds.MaraesGiftProfractory))
                    player.HoursSinceCum += 2;

                if (player.HasPerk(PerkIds.FerasBoonAlpha))
                    player.HoursSinceCum += 2;

                if (player.HasPerk(PerkIds.ParasiteMusk))
                    player.HoursSinceCum += 2;
            }

            // 原版 PlayerEvents.hx:63 creepingTaint：开启「腐蚀蔓延」时随时间缓慢累积腐蚀
            if (GameSettings.CreepingTaint && !player.HasPerk(PerkIds.PurityBlessing))
            {
                // 原版 :65：Zetaz 药水未治愈时额外多涨 0.02
                if (flags.Get(GameFlagIds.ZetazPotionCured) < 1)
                    player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Corruption, 0.02f));

                player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Corruption, 0.02f));
            }

            if (!player.HasPerk(PerkIds.WellAdjusted))
            {
                player.ApplyStats(new DynStatsOptions(Scale: false),

                    new DynStat(GameStats.Lust, player.Libido * 0.04f));

                if (player.HasPerk(PerkIds.Lusty))
                    player.ApplyStats(new DynStatsOptions(Scale: false),

                        new DynStat(GameStats.Lust, player.Libido * 0.02f));
            }
            else
            {
                player.ApplyStats(new DynStatsOptions(Scale: false),

                    new DynStat(GameStats.Lust, player.Libido * 0.02f));

                if (player.HasPerk(PerkIds.Lusty))
                    player.ApplyStats(new DynStatsOptions(Scale: false),

                        new DynStat(GameStats.Lust, player.Libido * 0.01f));
            }

            // 原版 PlayerEvents.hx:89：佩戴羽毛发夹时可能改变蜥蜴类发型（Mutations.hx:465）
            if (player.FeatheryHairPinEquipped()
                && Mutations.Mutations.Instance.LizardHairChange("PlayerEvents-benoitHairPin") != 0)

                needNext = true;

            if ((int)player.JewelryEffectId == 9 && player.Corruption < 80)
                player.ApplyStats(new DynStat(GameStats.Corruption, player.JewelryEffectMagnitude / 100f));

            if ((int)player.JewelryEffectId == 8)
                player.ApplyStats(new DynStat(GameStats.Corruption, -(player.JewelryEffectMagnitude / 100f)));

            if (player.Armor?.Id == ArmorIds.DivineArmor)
                player.ApplyStats(new DynStat(GameStats.Corruption, -0.1f));

            TickHunger(player, flags, survival);

            var schizophrenia = flags.Get(GameFlagIds.AchievementProgressSchizophrenia);

            if (player.Corruption >= 80 && (schizophrenia == 0 || schizophrenia == 2))
                flags.Set(GameFlagIds.AchievementProgressSchizophrenia, schizophrenia + 1);

            if (player.Corruption <= 20)
            {
                schizophrenia = flags.Get(GameFlagIds.AchievementProgressSchizophrenia);

                if (schizophrenia == 1 || schizophrenia == 3)
                    flags.Set(GameFlagIds.AchievementProgressSchizophrenia, schizophrenia + 1);
            }

            if (player.Corruption >= 100 && flags.Get(GameFlagIds.AchievementProgressCleanSlate) == 0)
                flags.Set(GameFlagIds.AchievementProgressCleanSlate, 1);

            if (player.Corruption <= 0 && flags.Get(GameFlagIds.AchievementProgressCleanSlate) == 1)
                flags.Set(GameFlagIds.AchievementProgressCleanSlate, 2);

            if (flags.Get(GameFlagIds.BasiliskResistanceTracker) >= 100 && !player.HasPerk(PerkIds.BasiliskResistance))
            {
                Scene.OutputText("PlayerEvents.Perk.BasiliskResistanceGained".Localized());

                Scene.OutputText("PlayerEvents.Perk.BasiliskResistanceDetail".Localized());

                player.CreatePerk(PerkIds.BasiliskResistance, 0, 0, 0, 0);

                needNext = true;
            }

            if (flags.Get(GameFlagIds.TimesTransformed) >= 100 && !player.HasPerk(PerkIds.TransformationResistance))
            {
                Scene.OutputText("PlayerEvents.Perk.TransformationResistanceGained".Localized());

                Scene.OutputText("PlayerEvents.Perk.TransformationResistanceDetail".Localized());

                player.CreatePerk(PerkIds.TransformationResistance, 0, 0, 0, 0);

                needNext = true;
            }

            if (player.HasPerk(PerkIds.HistoryDEUSVULT) && !player.IsPureEnough(25)
                && player.PerkValue(PerkIds.HistoryDEUSVULT, 2) != 1)
            {
                Scene.OutputText("PlayerEvents.History.DeusVultFailed".Localized());

                player.SetPerkValue(PerkIds.HistoryDEUSVULT, 1, -25);

                player.SetPerkValue(PerkIds.HistoryDEUSVULT, 2, 1);

                return true;
            }

            if (player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Harpy
                && player.Body.Get<TailPart>().Id == BodyPartTypes.Harpy

                && player.HasPerk(PerkIds.HarpyWomb)

                && player.HasStatusEffect(StatusEffectIds.Eggs)

                && player.StatusEffectValue(StatusEffectIds.Eggs, 1) == 0)
            {
                player.ChangeStatusValue(StatusEffectIds.Eggs, 1, 1);

                Scene.OutputText("PlayerEvents.HarpyWomb.EggsGrowLarge".Localized());

                needNext = true;
            }

            if (player.Vaginas.Count == 0 && player.HasPerk(PerkIds.Diapause))
            {
                Scene.OutputText("PlayerEvents.Diapause.Lost".Localized());

                player.RemovePerk(PerkIds.Diapause);

                needNext = true;
            }

            var tailPart = player.Body.Get<TailPart>();

            if (player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Naga
                && tailPart.Id != BodyPartTypes.None)
            {
                Scene.OutputText("PlayerEvents.Naga.TailAbsorbed".Localized());

                Scene.OutputText("PlayerEvents.Naga.FormSmooth".Localized());

                tailPart.Id = BodyPartTypes.None;

                needNext = true;
            }

            if (player.HasPerk(PerkIds.WetPussy) && player.Vaginas.Count > 0 && player.Vaginas[0].Wetness < 2)
            {
                Scene.OutputText("PlayerEvents.WetPussy.Restored".Localized(player.VaginaDescript(0)));

                player.Vaginas[0].Wetness = 2;

                needNext = true;
            }

            if (player.HasPerk(PerkIds.MaraesGiftButtslut) && player.Anal.Wetness < 2)
            {
                Scene.OutputText("PlayerEvents.Buttslut.Remoistened".Localized());

                player.Anal.Wetness = 2;

                needNext = true;
            }

            if (player.Pregnancy.Incubation <= 0 && player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
                player.ClearPregnancy();

            if (flags.Get(GameFlagIds.MinotaurSonsCumRepeatCooldown) > 0)
                flags.Set(GameFlagIds.MinotaurSonsCumRepeatCooldown, flags.Get(GameFlagIds.MinotaurSonsCumRepeatCooldown) - 1);

            // 原版 PlayerEvents.hx:199-207：蜂腹 / 蛛腹 / 蝎尾每回合补充毒液
            var tail = player.Body.Get<TailPart>();

            if (tail.Id == BodyPartTypes.BeeAbdomen
                || tail.Id == BodyPartTypes.SpiderAbdomen

                || tail.Id == BodyPartTypes.Scorpion)
            {
                if (tail.Recharge < 5)
                    tail.Recharge = 5;

                tail.Venom += tail.Recharge;

                if (tail.Venom > 100)
                    tail.Venom = 100;
            }

            // 原版 PlayerEvents.hx:939-1090 的每日块（hours == 0 时执行）
            if (Scene.View.Time.Hours == 0)
                needNext = DailyUpdate() || needNext;

            // 原版 EventParser.hx:255-260：每小时检查一次冰制武器是否融化
            if (Items.Behaviors.Vanilla.IceWeaponBehavior.PlayerHasIceWeapon() != ""
                && !Items.Behaviors.Vanilla.IceWeaponBehavior.Melt(player, false,

                    text => Scene.OutputText(text)))

                needNext = true;

            // 原版 PlayerEvents.hx:377-381：媚唇膏倒计时
            if (player.HasStatusEffect(StatusEffectIds.LustStickApplied))
            {
                player.ChangeStatusValue(StatusEffectIds.LustStickApplied, 0, -1);

                if (player.StatusEffectValue(StatusEffectIds.LustStickApplied, 0) <= 0)
                {
                    player.RemoveStatusEffect(StatusEffectIds.LustStickApplied);

                    Scene.OutputText("PlayerEvents.LustStickApplied.Fades".Localized());

                    needNext = true;
                }
            }

            // 原版 PlayerEvents.hx:382-405：哈比口红倒计时
            if (player.HasStatusEffect(StatusEffectIds.Luststick))
            {
                player.ChangeStatusValue(StatusEffectIds.Luststick, 0, -1);

                if (Random.Shared.Next(2) == 0 && player.HasCock())
                {
                    if (flags.Get(GameFlagIds.PcCurrentlyLuststickAffected) == 0)
                    {
                        Scene.OutputText("PlayerEvents.LustStick.Spike".Localized());

                        flags.Set(GameFlagIds.PcCurrentlyLuststickAffected, flags.Get(GameFlagIds.PcCurrentlyLuststickAffected) + 1);

                        needNext = true;
                    }

                    player.ApplyStats(new DynStat(GameStats.Lust, 20));

                    if (player.Lust > player.MaxLust)
                        player.Lust = player.MaxLust;
                }

                if (player.StatusEffectValue(StatusEffectIds.Luststick, 0) <= 0)
                {
                    player.RemoveStatusEffect(StatusEffectIds.Luststick);

                    player.Libido = flags.Get(GameFlagIds.LuststickLibidoInitial);

                    Scene.OutputText("PlayerEvents.LustStick.WearsOff".Localized());

                    needNext = true;
                }
            }

            // 原版 PlayerEvents.hx:370：没有「雌雄莫辨」perk 时每小时修正面部女性化数值
            if (!player.HasPerk(PerkIds.Androgyny))
            {
                var femText = player.FixFemininity();

                if (femText != "")
                {
                    Scene.OutputText(femText);

                    needNext = true;
                }
            }

            return needNext;
        }

        public bool TimeChangeLarge()
        {
            // 原版 PlayerEvents.hx:1096 timeChangeLarge()：夜间长事件（节日、梦境、天气）
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var time = Scene.View.Time;

            var year = DateTime.Now.Year;

            // 原版 :1097 土星节圣诞鸡
            if (Random.Shared.Next(4) == 0 && Holidays.IsSaturnalia()
                && (int)player.Gender > 0 && time.Hours == 6

                && flags.Get(GameFlagIds.XmasChickenYear) < year)
            {
                SceneInstance<Scenes.Vanilla.Areas.Plains.XmasMiscScene>.Get().GetAChristmasChicken();

                return true;
            }

            // 原版 :1101 圣诞精灵
            if (time.Hours == 1 && Holidays.IsSaturnalia()
                && year > flags.Get(GameFlagIds.PcEncounteredChristmasElfBefore))
            {
                SceneInstance<Scenes.Vanilla.Seasonal.XmasElfScene>.Get().XmasBitchEncounter();

                return true;
            }

            // 原版 :1105 感恩节火鸡
            if (_checkedTurkey++ == 0 && Random.Shared.Next(5) == 0 && (time.Hours == 18 || time.Hours == 19)
                && (year > flags.Get(GameFlagIds.TurkeyFuckYearDone) || flags.Get(GameFlagIds.MoreTurkey) > 0)

                && Holidays.IsThanksgiving() && (int)player.Gender > 0)
            {
                SceneInstance<Scenes.Vanilla.Seasonal.ThanksgivingScene>.Get().DatTurkeyRumpMeeting();

                return true;
            }

            // 原版 :1110 每晚只能做一个梦
            if (_checkedDream++ == 0 && time.Hours == 3)
            {
                if ((int)player.Gender > 0 && time.Days == 10)
                {
                    SceneInstance<Scenes.Vanilla.DreamsScene>.Get().DayTenDreams();

                    return true;
                }

                if (SceneInstance<Scenes.Vanilla.Npcs.IzmaScene>.Get().NightmareCheck())
                {
                    SceneInstance<Scenes.Vanilla.Npcs.IzmaScene>.Get().DaughterNightmare();

                    return true;
                }

                // 原版 :1119 蜂类产卵者的梦
                if (player.HasCock() && player.HasPerk(PerkIds.BeeOvipositor)
                    && player.Eggs() >= 20 && Random.Shared.Next(6) == 0)
                {
                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T01".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T02".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T03".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T04".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T05".Localized());

                    if (player.CumQ() >= 1000)
                        Scene.OutputText("Scenes.Playerevents.OvipositorDream.T06".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T07".Localized());

                    player.FertilizeEggs();

                    player.Orgasm("Dick");

                    player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Lust, 20f));

                    Scene.DoNext(Scene.PlayerMenu);

                    return true;
                }

                // 原版 :1139 蛛类产卵者的梦
                if (player.HasCock() && player.HasPerk(PerkIds.SpiderOvipositor)
                    && player.Eggs() >= 20 && Random.Shared.Next(6) == 0)
                {
                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T08".Localized());

                    if (player.HasVagina())
                        Scene.OutputText(player.VaginaDescript(0) + "Common.Text.And".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T09".Localized());

                    if (player.HasVagina())
                        Scene.OutputText("Scenes.Playerevents.OvipositorDream.T10".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T11".Localized());

                    if (player.HasVagina())
                        Scene.OutputText("Scenes.Playerevents.OvipositorDream.T12".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T13".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T14".Localized());

                    if (player.CumQ() > 1000)
                        Scene.OutputText("Scenes.Playerevents.OvipositorDream.T15".Localized());

                    Scene.OutputText("Scenes.Playerevents.OvipositorDream.T16".Localized());

                    player.FertilizeEggs();

                    player.Orgasm("Dick");

                    player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Lust, 20f));

                    Scene.DoNext(Scene.PlayerMenu);

                    return true;
                }

                // 原版 :1167 Ceraph 的身体部件梦，频率随已赠予部件数升高
                var ceraphParts = flags.Get(GameFlagIds.CeraphDicksOwned)

                    + flags.Get(GameFlagIds.CeraphPussiesOwned)

                    + flags.Get(GameFlagIds.CeraphTitsOwned);

                var ceraphPeriod = ceraphParts switch { 0 => 0, 1 => 10, 2 => 7, 3 => 5, 4 => 4, _ => 3 };

                if (ceraphPeriod > 0 && time.Days % ceraphPeriod == 0)
                {
                    SceneInstance<Scenes.Vanilla.Npcs.CeraphScene>.Get().CeraphBodyPartDreams();

                    return true;
                }

                // 原版 :1181 Dominika 后续梦境
                if (flags.Get(GameFlagIds.DominikaSpecialFollowup) > 0 && flags.Get(GameFlagIds.DominikaSpecialFollowup) < 4)
                {
                    SceneInstance<Scenes.Vanilla.Places.TelAdre.Dominika>.Get().FellatrixDream();

                    return true;
                }

                // 原版 :1186 海葵孩子的梦
                if (SceneInstance<Scenes.Vanilla.Npcs.AnemoneScene>.Get().KidAXP() >= 40 && flags.Get(GameFlagIds.HadKidADream) == 0
                    && player.Gender != 0)
                {
                    SceneInstance<Scenes.Vanilla.Npcs.AnemoneScene>.Get().KidADreams();

                    flags.Set(GameFlagIds.HadKidADream, 1);

                    return true;
                }

                // 原版 :1191：穿紫罗兰袜的阴茎会引出「异变之梦」
                if (player.ViridianChange())
                {
                    SceneInstance<Scenes.Vanilla.DreamsScene>.Get().FuckedUpCockDreamChange();

                    return true;
                }

                // 原版 :1195 Helspawn 的梦
                if (player.Gender != 0 && flags.Get(GameFlagIds.SleepWith) == ""
                    && SceneInstance<Scenes.Vanilla.Npcs.HelSpawnScene>.Get().HelspawnLover())
                {
                    SceneInstance<Scenes.Vanilla.Npcs.HelSpawnScene>.Get().SlutspawnDreaming();

                    return true;
                }

                // 原版 :1199 与 Marble 同睡时的噩梦
                if (Random.Shared.Next(40) == 0 && flags.Get(GameFlagIds.SleepWith) == "Marble")
                {
                    if (flags.Get(GameFlagIds.MarblePurified) == 1)
                        SceneInstance<Scenes.Vanilla.DreamsScene>.Get().MarblePureNightmare1();
                    else
                        SceneInstance<Scenes.Vanilla.DreamsScene>.Get().MarbleCorruptNightmare();

                    return true;
                }

                // 原版 :1207 随机梦境
                if ((player.LibidoPercent > 50 || player.LustPercent > 40) && (SceneInstance<Scenes.Vanilla.DreamsScene>.Get().DreamSelect()))
                        return true;
            }

            // 原版 :1213 史莱姆坏结局
            if (player.StatusEffectValue(StatusEffectIds.SlimeCraving, 0) >= 18 && player.Strength <= 1)
            {
                SceneInstance<Scenes.Vanilla.Areas.Lake.GooGirlScene>.Get().SlimeBadEnd();

                return true;
            }

            // 原版 :1218 蜂屌满 lust
            if (player.HasCock() && player.Cocks[0].Id == BodyPartTypes.Bee
                && player.Lust >= player.MaxLust)
            {
                if (player.HasItem(ConsumableIds.BeeHoney) || player.HasItem(ConsumableIds.PureHoney)
                    || player.HasItem(ConsumableIds.SpecialHoney))
                {
                    SceneInstance<Scenes.Vanilla.MasturbationScene>.Get().MasturbateGo();

                    return true;
                }

                SceneInstance<Scenes.Vanilla.Areas.Forest.BeeGirlScene>.Get().BeeSexForCocks(false);

                return true;
            }

            // 原版 :1229 强制封顶屌的尺寸，防止存档数值溢出
            if (player.HasCock())
                foreach (var cock in player.Cocks)
                {
                    if (cock.Length > 9999.9f)
                        cock.Length = 9999.9f;

                    if (cock.Thickness > 999.9f)
                        cock.Thickness = 999.9f;
                }

            // 原版 :1243 天气轮换
            if (flags.Get(GameFlagIds.GameEnd) > 0 && flags.Get(GameFlagIds.WeatherChangeCooldown) <= 0)
            {
                var roll = Random.Shared.Next(100);

                var weather = roll < 40 ? 0 : roll < 60 ? 1 : roll < 80 ? 2 : roll < 96 ? 3 : 4;

                flags.Set(GameFlagIds.CurrentWeather, weather);

                var cooldown = Random.Shared.Next(6, 54);

                if (weather >= 3)
                    cooldown /= weather == 4 ? 3 : 2;

                flags.Set(GameFlagIds.WeatherChangeCooldown, cooldown);
            }

            return false;
        }

        public static void TickHunger(Player player, GameFlags flags, bool survival)
        {
            if (!survival)
                return;

            var multiplier = 1.0f;

            if (player.HasPerk(PerkIds.Survivalist))
                multiplier -= 0.2f;

            if (player.HasPerk(PerkIds.Survivalist2))
                multiplier -= 0.2f;

            player.Hunger -= MathF.Ceiling(player.Hunger / 25f) * (0.5f * multiplier);

            if (player.ButtPregnancy.Type == PregnancyIds.GooStuffed)
                player.Hunger = 100;

            if (player.Hunger <= 0)
            {
                if (player.Thickness < 25)
                {
                    player.TakeDamage(player.MaxHealth / 25f);

                    player.ChangeFatigue(2);

                    player.ApplyStats(
                        new DynStat(GameStats.Strength, -0.5f),

                        new DynStat(GameStats.Toughness, -0.5f));
                }
                else if ((Hour + 2) % 4 == 0)
                {
                    player.ModThickness(1, 1);

                    player.ModTone(1, 1);
                }

                player.Hunger = 0;
            }

            if (player.Hunger < 10 && Hour % 4 == 0)
            {
                player.ModThickness(1, 1);

                player.ModTone(1, 1);
            }

            if (player.Hunger < 25)
                flags.Set(GameFlagIds.AchievementProgressFasting, player.Hunger > 0

                    ? flags.Get(GameFlagIds.AchievementProgressFasting) + 1

                    : 0);
            else
                flags.Set(GameFlagIds.AchievementProgressFasting, 0);

            if (player.Armor?.Id == ArmorIds.GooArmor)
            {
                if (player.Hunger < 15)
                {
                    Scene.OutputText("PlayerEvents.Hunger.GooArmorFeeds".Localized());

                    player.Hunger = 20;
                }

                if (player.Hunger < 20)
                    player.Hunger = 20;
            }
        }

        public static int Hour => Scene.View.Time.Hours;

        /// <summary>
        /// 原版 PlayerEvents.hx:939 起的每日结算块（在 hours == 0 时由 TimeChange 调用）。
        /// 说明：原版该块共 ~150 行，本方法先覆盖 L939-1035 的计数器递减与农场腐化推进，
        /// 其余段落（Benois 库存、Rubi 收入等）见下一轮。
        /// </summary>
        public static bool DailyUpdate()
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            var time = Scene.View.Time;

            var needNext = false;

            // 原版 :940 比基尼护甲加成逐日递减并夹在 0-8
            flags.Set(GameFlagIds.BikiniArmorBonus, flags.Get(GameFlagIds.BikiniArmorBonus) - 1);

            if (flags.Get(GameFlagIds.BikiniArmorBonus) < 0)
                flags.Set(GameFlagIds.BikiniArmorBonus, 0);

            if (flags.Get(GameFlagIds.BikiniArmorBonus) > 8)
                flags.Set(GameFlagIds.BikiniArmorBonus, 8);

            // 原版 :945 距 Vala 强暴尝试的计时
            if (flags.Get(GameFlagIds.TimeSinceValaAttemptedRapePc) > 0)
                flags.Set(GameFlagIds.TimeSinceValaAttemptedRapePc, flags.Get(GameFlagIds.TimeSinceValaAttemptedRapePc) - 1);

            // 原版 :948 寻找钥匙的计时
            if (flags.Get(GameFlagIds.GatsAngelTimeToFindKey) > 0)
                flags.Set(GameFlagIds.GatsAngelTimeToFindKey, flags.Get(GameFlagIds.GatsAngelTimeToFindKey) + 1);

            // 原版 :951 每天重置的「今日已……」旗标
            flags.Set(GameFlagIds.BrookeMetToday, 0);

            // 原版 :953 每两天递减的 Kaiju 坏结局计数
            if (time.Days % 2 == 0 && flags.Get(GameFlagIds.KaijuBadEndCounter) > 0)
            {
                flags.Set(GameFlagIds.KaijuBadEndCounter, flags.Get(GameFlagIds.KaijuBadEndCounter) - 1);

                if (flags.Get(GameFlagIds.KaijuBadEndCounter) < 0)
                    flags.Set(GameFlagIds.KaijuBadEndCounter, 0);
            }

            flags.Set(GameFlagIds.GildedJerked, 0);

            flags.Set(GameFlagIds.FedScyllaToday, 0);

            flags.Set(GameFlagIds.NotHelpedArianToday, 0);

            flags.Set(GameFlagIds.BenoitTalkedToday, 0);

            flags.Set(GameFlagIds.BenoitHairpinTalkedToday, 0);

            flags.Set(GameFlagIds.RogarFuckedToday, 0);

            // 原版 :963 淫棒抗性累积递减
            if (flags.Get(GameFlagIds.LuststickResistanceProgress) > 0)
                flags.Set(GameFlagIds.LuststickResistanceProgress, flags.Get(GameFlagIds.LuststickResistanceProgress) - 1);

            // 原版 :967 Dominika 学习冷却
            if (flags.Get(GameFlagIds.DominikaLearningCooldown) > 0)
            {
                flags.Set(GameFlagIds.DominikaLearningCooldown, flags.Get(GameFlagIds.DominikaLearningCooldown) - 1);

                if (flags.Get(GameFlagIds.DominikaLearningCooldown) < 0)
                    flags.Set(GameFlagIds.DominikaLearningCooldown, 0);
            }

            // 原版 :972 Loppe 拒绝计数
            if (flags.Get(GameFlagIds.LoppeDenialCounter) > 0)
            {
                flags.Set(GameFlagIds.LoppeDenialCounter, flags.Get(GameFlagIds.LoppeDenialCounter) - 1);

                if (flags.Get(GameFlagIds.LoppeDenialCounter) < 0)
                    flags.Set(GameFlagIds.LoppeDenialCounter, 0);
            }

            // 原版 :977 仙灵群交倒计时
            if (flags.Get(GameFlagIds.WeeklyFairyOrgyCountdown) > 0)
            {
                flags.Set(GameFlagIds.WeeklyFairyOrgyCountdown, flags.Get(GameFlagIds.WeeklyFairyOrgyCountdown) - 1);

                if (flags.Get(GameFlagIds.WeeklyFairyOrgyCountdown) < 0)
                    flags.Set(GameFlagIds.WeeklyFairyOrgyCountdown, 0);
            }

            // 原版 :985 今日挤奶器使用标记
            if (flags.Get(GameFlagIds.UsedMilkerToday) > 0)
                flags.Set(GameFlagIds.UsedMilkerToday, 0);

            // 原版 :987 史莱姆娘（乳胶女孩）的每日产出与情绪变化
            if (SceneInstance<Scenes.Vanilla.Npcs.LatexGirlScene>.Get().LatexGooFollower())
            {
                var goo = SceneInstance<Scenes.Vanilla.Npcs.LatexGirlScene>.Get();

                goo.GooFluid(-2, false);

                if (goo.GooFluid() < 50)
                    goo.GooHappiness(-1, false);

                if (goo.GooFluid() < 25)
                    goo.GooHappiness(-1, false);

                if (goo.GooHappiness() < 75)
                    goo.GooObedience(-1, false);

                if (goo.GooHappiness() >= 90)
                    goo.GooObedience(1, false);
            }

            // 原版 :1021 农场腐化每日推进（原版此处丢弃返回值）
            Scenes.Vanilla.Places.FarmCorruption.UpdateFarmCorruption();

            // 原版 :1022 草药避孕药倒计时（生效中时每日子计数 -1，归零则移除）
            if ((player.HasStatusEffect(StatusEffectIds.Contraceptives)) && ((int)player.StatusEffectValue(StatusEffectIds.Contraceptives, 1) == 1))
                {
                    player.AddStatusValue(StatusEffectIds.Contraceptives, 1, -1);

                    if ((int)player.StatusEffectValue(StatusEffectIds.Contraceptives, 1) < 0)
                        player.RemoveStatusEffect(StatusEffectIds.Contraceptives);
                }

            // 原版 :1030 鲨鱼娘计数递减
            if ((int)player.StatusEffectValue(StatusEffectIds.SharkGirl, 0) > 0)
                player.AddStatusValue(StatusEffectIds.SharkGirl, 0, -1);

            // 原版 :1033 生发血清：按已用次数决定每日增长量
            if (flags.Get(GameFlagIds.IncreasedHairGrowthTimeRemaining) > 0)
            {
                switch (flags.Get(GameFlagIds.IncreasedHairGrowthSerumTimesApplied))
                {
                    case 1:
                        player.GrowHair(0.2f);

                        break;

                    case 2:
                        player.GrowHair(0.5f);

                        break;

                    case 3:
                        player.GrowHair(1.1f);

                        break;
                }

                flags.Set(GameFlagIds.IncreasedHairGrowthTimeRemaining,

                    flags.Get(GameFlagIds.IncreasedHairGrowthTimeRemaining) - 1);

                // 原版 :1058 血清失效时播报，并恢复被蛇怪化中断的生长
                if (flags.Get(GameFlagIds.IncreasedHairGrowthTimeRemaining) <= 0)
                {
                    flags.Set(GameFlagIds.IncreasedHairGrowthTimeRemaining, 0);

                    flags.Set(GameFlagIds.IncreasedHairGrowthSerumTimesApplied, 0);

                    Scene.OutputText("PlayerEvents.Hair.SerumWearsOff".Localized());

                    if (flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) > 0)
                    {
                        flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                        Scene.OutputText("PlayerEvents.Hair.GrowthResumed".Localized());
                    }

                    Scene.OutputText("Common.Text.Pg".Localized());

                    needNext = true;
                }
            }

            // 原版 :1072 未因蛇怪化停摆时，头发每日 +0.1、胡须 +0.02
            if (flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) == 0)
            {
                player.GrowHair(0.1f);

                var beard = player.Body.Get<BeardPart>();

                if (beard.Length > 0 && beard.Length < 12)
                    player.GrowBeard(0.02f);
            }

            // 原版 :1082 龙息冷却每日清除
            if (player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

            return needNext;
        }
    }
}