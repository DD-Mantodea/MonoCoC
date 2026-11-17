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

        public static bool Hyper => OptionStore.Instance.Get("coc:hyper", 0) > 0;

        public static bool Silly => OptionStore.Instance.Get("coc:silly", 0) > 0;

        public static bool Realistic => OptionStore.Instance.Get("coc:realistic", 0) > 0;

        public static bool Survival => OptionStore.Instance.Get("coc:survival", 0) > 0;

        public static bool FilthEnabled => OptionStore.Instance.Get("coc:filth", 1) > 0;

        public static bool AllowChild => OptionStore.Instance.Get("coc:underage", 1) >= 1;

        public static bool AllowBaby => OptionStore.Instance.Get("coc:underage", 1) >= 2;

        public static bool ShouldraChild => OptionStore.Instance.Get("coc:shouldraChild", 0) > 0;

        public static bool GenericLoliShota => OptionStore.Instance.Get("coc:genericLoliShota", 0) > 0;

        public static bool GargoyleChild => OptionStore.Instance.Get("coc:gargoyleChild", 0) > 0;

        public static bool LowStandards => OptionStore.Instance.Get("coc:lowStandards", 0) > 0;

        public static bool UrtaDisabled => OptionStore.Instance.Get("coc:urtaDisabled", 0) > 0;

        public static bool Metric
        {
            get => OptionStore.Instance.Get("coc:metric", 0) > 0;

            set => OptionStore.Instance.Set("coc:metric", value ? 1 : 0);
        }
    }
}
