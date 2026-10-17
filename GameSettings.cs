using MonoCoC.Configuration;
using MonoCoC.Contents.Settings;

namespace MonoCoC
{
    /// <summary>
    /// Read-only game rule facade. Settings themselves live in ModeSettings /
    /// OptionStore; CoC should not be a settings container.
    /// </summary>
    public static class GameSettings
    {
        public static int Difficulty => ModeSettings.Instance.Difficulty;

        public static bool OldAscension => ModeSettings.Instance.OldAscension;

        public static bool CreepingTaint => ModeSettings.Instance.Taint;

        public static bool Hyper => OptionStore.Instance.Get("hyper", 0) > 0;

        public static bool FilthEnabled => OptionStore.Instance.Get("filth", 1) > 0;

        public static bool AllowChild => OptionStore.Instance.Get("underage", 1) >= 1;

        public static bool AllowBaby => OptionStore.Instance.Get("underage", 1) >= 2;

        public static bool ShouldraChild => OptionStore.Instance.Get("shouldraChild", 0) > 0;

        public static bool GenericLoliShota => OptionStore.Instance.Get("genericLoliShota", 0) > 0;

        public static bool GargoyleChild => OptionStore.Instance.Get("gargoyleChild", 0) > 0;

        public static bool LowStandards => OptionStore.Instance.Get("lowStandards", 0) > 0;

        public static bool UrtaDisabled => OptionStore.Instance.Get("urtaDisabled", 0) > 0;

        public static bool Metric
        {
            get => OptionStore.Instance.Get("metric", 0) > 0;

            set => OptionStore.Instance.Set("metric", value ? 1 : 0);
        }
    }
}
