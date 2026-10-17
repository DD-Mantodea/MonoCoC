using MonoCoC.Configuration;

namespace MonoCoC.Contents.Settings
{

    /// <summary>
    /// 原版 GameSettingsPlayer.hx:112 @:structInit class SettingsModes —— 玩法模式设置。
    /// 全部 14 个字段原样对应；C# 侧用 OptionStore 持久化（与 survival / silly 等既有设定共用同一存储）。
    /// </summary>
    public class ModeSettings
    {
        private static ModeSettings? _instance;

        public static ModeSettings Instance => _instance ??= new ModeSettings();

        /// <summary>原版 :113 difficulty —— -2 简单 / 0 普通 / 1 困难 / 2 噩梦 / 3 极限。</summary>
        public virtual int Difficulty => OptionStore.Instance.Get("difficulty", 0);

        /// <summary>原版 :114 survival</summary>
        public virtual bool Survival => OptionStore.Instance.Get("survival", 0) > 0;

        /// <summary>原版 :115 realistic</summary>
        public virtual bool Realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        /// <summary>原版 :116 hardcore</summary>
        public virtual bool Hardcore => OptionStore.Instance.Get("hardcore", 0) > 0;

        /// <summary>
        /// 原版 :117 hardcoreSlot（原版为槽位字符串，C# 侧统一用 OptionStore 的 int 槽位号，见 Scene.cs:166）。
        /// </summary>
        public virtual int HardcoreSlot => OptionStore.Instance.Get("hardcoreSlot", 0);

        /// <summary>原版 :118 silly</summary>
        public virtual bool Silly => OptionStore.Instance.Get("silly", 0) > 0;

        /// <summary>原版 :119 hyper</summary>
        public virtual bool Hyper => OptionStore.Instance.Get("hyper", 0) > 0;

        /// <summary>原版 :120 temptation —— 高淫欲时强制显示诱惑按钮</summary>
        public virtual bool Temptation => OptionStore.Instance.Get("temptation", 0) > 0;

        /// <summary>原版 :121 taint —— 开启后随时间缓慢累积腐蚀（CoC.hx:897 creepingTaint）</summary>
        public virtual bool Taint => OptionStore.Instance.Get("taint", 0) > 0;

        /// <summary>原版 :122 cooldowns —— 关闭后战斗招式无冷却</summary>
        public virtual bool Cooldowns => OptionStore.Instance.Get("cooldowns", 0) > 0;

        /// <summary>原版 :123 scaling —— 敌人按玩家等级缩放</summary>
        public virtual bool Scaling => OptionStore.Instance.Get("scaling", 0) > 0;

        /// <summary>原版 :124 longHaul —— Player.hx:3873 isLongHaul()</summary>
        public virtual bool LongHaul => OptionStore.Instance.Get("longHaul", 0) > 0;

        /// <summary>原版 :125 oldAscension —— CoC.hx:884</summary>
        public virtual bool OldAscension => OptionStore.Instance.Get("oldAscension", 0) > 0;

        /// <summary>原版 :126 prison</summary>
        public virtual bool Prison => OptionStore.Instance.Get("prison", 0) > 0;
    }
}