using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 PlayerInfo.hx:636 doubleAttackOptions
    public static class DoubleAttackOptions
    {
        public static void Show()
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            Scene.OutputText("Scenes.Perks.DoubleAttackIntro".Localized());

            var current = CoC.Instance.Flags.Get(GameFlagIds.DoubleAttackStyle);

            for (var style = 0; style < 3; style++)
            {
                var text = ("Scenes.Perks.DoubleAttackStyle" + style).Localized();

                Scene.OutputText(style == current ? "[pg-][b:" + text + "]" : "[pg-]" + text);
            }

            Scene.AddButton(0, "Scenes.Perks.AllDouble".Localized(), () => Set(0))
                .DisableIf(current == 0, "Scenes.Perks.CurrentSetting".Localized());

            Scene.AddButton(1, "Scenes.Perks.Dynamic".Localized(), () => Set(1))
                .DisableIf(current == 1, "Scenes.Perks.CurrentSetting".Localized());

            Scene.AddButton(2, "Scenes.Perks.Single".Localized(), () => Set(2))
                .DisableIf(current == 2, "Scenes.Perks.CurrentSetting".Localized());

            Scene.AddButton(14, "Common.Back".Localized(), PerksScene.BackToPerks);
        }

        private static void Set(int value)
        {
            CoC.Instance.Flags.Set(GameFlagIds.DoubleAttackStyle, value);

            Show();
        }
    }

    // 原版 PlayerInfo.hx:664 ascToleranceOption / mightOption / chargeOption / tfResistOption
    [Register<PerkOptionsScene>("coc:perk_options")]
    public class PerkOptionsScene : Scene
    {
        private static string _text = "";

        private static bool _enabled;

        private static Action<bool> _set = _ => { };

        private static Action _back = () => { };

        public static void Show(string text, bool enabled, Action<bool> set, Action back)
        {
            _text = text;

            _enabled = enabled;

            _set = set;

            _back = back;

            Go<PerkOptionsScene>();
        }

        public static void ShowTolerance(Action back) => Show(
            "Scenes.Perks.ToleranceText".Localized(),

            Player.PerkValue(PerkIds.AscensionTolerance, 2) == 0,

            value => Player.SetPerkValue(PerkIds.AscensionTolerance, 2, value ? 0 : 1), back);

        public static void ShowMight(Action back) => Show(
            "Scenes.Perks.MightText".Localized(),

            Player.PerkValue(PerkIds.Battlemage, 1) == 0,

            value => Player.SetPerkValue(PerkIds.Battlemage, 1, value ? 0 : 1), back);

        public static void ShowCharge(Action back) => Show(
            "Scenes.Perks.ChargeText".Localized(),

            Player.PerkValue(PerkIds.Spellsword, 1) == 0,

            value => Player.SetPerkValue(PerkIds.Spellsword, 1, value ? 0 : 1), back);

        public static void ShowTfResist(Action back) => Show(
            "Scenes.Perks.TfResistText".Localized(),

            Player.PerkValue(PerkIds.TransformationResistance, 2) == 0,

            value => Player.SetPerkValue(PerkIds.TransformationResistance, 2, value ? 0 : 1), back);

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            OutputText(_text);

            OutputText(_enabled

                ? "[pg][b:Enabled]"

                : "[pg][b:Disabled]");

            AddButton(0, "Scenes.Perks.Enable".Localized(), () =>
            {
                _set(true);

                Show(_text, true, _set, _back);
            }).DisableIf(_enabled, "Scenes.Perks.AlreadyEnabled".Localized());

            AddButton(1, "Scenes.Perks.Disable".Localized(), () =>
            {
                _set(false);

                Show(_text, false, _set, _back);
            }).DisableIf(!_enabled, "Scenes.Perks.AlreadyDisabled".Localized());

            AddButton(14, "Common.Back".Localized(), _back);
        }
    }
}
