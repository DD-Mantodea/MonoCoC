using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using System;

namespace MonoCoC.Contents.World
{
    /// <summary>
    /// 原版 scenes/seasonal/Seasons.hx：按现实日期判定节日/季节（北半球），
    /// 以及「每日都是节日」调试开关。
    /// </summary>
    public static class Holidays
    {
        public const int Winter = 0;

        public const int Spring = 1;

        public const int Summer = 2;

        public const int Autumn = 3;

        public const int Witch = -1;

        private static DateTime Now => DateTime.Now;

        // 原版 isItSaturnalia()：12/18–1/1
        public static bool IsSaturnalia(bool strict = false) =>

            (!strict && EveryDay())

            || (!strict && CoC.Instance.Player.HasPerk(PerkIds.AChristmasCarol))

            || (Now.Month == 12 && Now.Day >= 18)

            || (Now.Month == 1 && Now.Day <= 1);

        public static bool IsAprilFools() => Now.Month == 4 && Now.Day == 1;

        // 原版 isItValentine()：2/7–2/21
        public static bool IsValentine(bool strict = false) =>

            (!strict && EveryDay())

            || (Now.Month == 2 && Now.Day >= 7 && Now.Day <= 21);

        // 原版 isItHalloween()：10/24–11/7，或吃掉 4 个以上南瓜籽
        public static bool IsHalloween(bool strict = false) =>

            (!strict && EveryDay())

            || (Now.Month == 10 && Now.Day >= 24)

            || (Now.Month == 11 && Now.Day <= 7)

            || (!strict && CoC.Instance.Flags.Get(GameFlagIds.PumpkinSeedsEaten) >= 4);

        // 原版 isItEaster()：4/1–4/15（以 4/8 为中心）
        public static bool IsEaster(bool strict = false) =>

            (!strict && EveryDay())

            || (Now.Month == 4 && Now.Day <= 15);

        // 原版 isItThanksgiving()：11/18–12/2，或带够 15 份食物
        public static bool IsThanksgiving(bool strict = false) =>

            (!strict && EveryDay())

            || (Now.Month == 11 && Now.Day >= 18)

            || (Now.Month == 12 && Now.Day <= 2)

            || (!strict && CoC.Instance.Player.CountTotalFoodItems() >= 15);

        public static int Season => Now.Month switch
        {
            12 or 1 or 2 => Winter,

            3 or 4 or 5 => Spring,

            6 or 7 or 8 => Summer,

            9 or 10 or 11 => Autumn,

            _ => Witch,
        };

        public static bool IsWinter(bool strict = false) => (!strict && EveryDay()) || Season == Winter;

        public static bool IsSpring(bool strict = false) => (!strict && EveryDay()) || Season == Spring;

        public static bool IsSummer(bool strict = false) => (!strict && EveryDay()) || Season == Summer;

        public static bool IsAutumn(bool strict = false) => (!strict && EveryDay()) || Season == Autumn;

        // 旧调用点用的别名（原版圣诞季即 Saturnalia）
        public static bool IsXmas() => IsSaturnalia();

        public static bool EveryDay()
        {
            var flags = CoC.Instance?.Flags;

            return flags != null && flags.Get(GameFlagIds.ItsEveryDay) > 0;
        }
    }
}