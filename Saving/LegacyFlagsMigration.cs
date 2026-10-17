using MonoCoC.Contents.Flags;

namespace MonoCoC.Saving
{
    // 旧存档兼容：把历史上「挤在一个 int 里」的 flag 拆成独立字段。
    // 只在读到 legacy 值、且新字段尚未写入时执行一次，执行后清掉 legacy。
    public static class LegacyFlagsMigration
    {
        public static void Apply()
        {
            MigrateBeeGirlStatus();
        }

        private static void MigrateBeeGirlStatus()
        {
            var packed = CoC.Instance.Flags.Get(GameFlagIds.BeeGirlStatus);

            if (packed == 0)
                return;

            // 原版打包布局：bit31 = badEndWarning，bit16-30 = conversation，bit0-15 = attitude
            var attitude = packed & 0x0000FFFF;

            var conversation = (packed & 0x7FFF0000) >> 16;

            var badEndWarning = (packed & unchecked((int)0x80000000)) != 0;

            // 新字段已有值时以新值为准（避免覆盖玩家在拆分版本后的进度）
            if (!CoC.Instance.Flags.Has(GameFlagIds.BeeGirlAttitude) && attitude != 0)
                CoC.Instance.Flags.Set(GameFlagIds.BeeGirlAttitude, attitude);

            if (!CoC.Instance.Flags.Has(GameFlagIds.BeeGirlConversation) && conversation != 0)
                CoC.Instance.Flags.Set(GameFlagIds.BeeGirlConversation, conversation);

            if (!CoC.Instance.Flags.Has(GameFlagIds.BeeGirlBadEndWarning) && badEndWarning)
                CoC.Instance.Flags.Set(GameFlagIds.BeeGirlBadEndWarning, 1);

            CoC.Instance.Flags.Set(GameFlagIds.BeeGirlStatus, 0);
        }
    }
}