namespace MonoCoC.Contents.Scenes.Vanilla
{
    public static class CampSleep
    {
        public static string HomeDesc() => "bedroll";

        public static string BedDesc() => "bed";
        // 原版 Camp.hx:141 isForceSleepTime()：24 点后或凌晨 4 点前强制入睡
        public static bool IsForceSleepTime()
        {
            var hours = Scene.View.Time.Hours;

            return hours >= 24 || hours < 4;
        }

        public static void SleepWrapper() => CampScene.Sleep();
    }
}