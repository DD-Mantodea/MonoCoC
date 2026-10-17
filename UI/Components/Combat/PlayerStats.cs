using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Combat
{
    public class PlayerStats : Container
    {

        // 主菜单的非游戏界面（成就/致谢/说明/读档/设置）隐藏属性栏
        public static bool HiddenByMenu;

        private const int BarWidth = 187;

        private readonly Dictionary<string, Label> _values = [];

        private readonly Dictionary<string, ColorRect> _fills = [];

        private readonly Dictionary<string, ColorRect> _minFills = [];

        private readonly Color _hpColor = new(177, 125, 94);

        private readonly Color _barColor = new(96, 0, 0);

        private Label _name = null!;

        public PlayerStats()
        {
            new UIImage(ManagerInstance<TextureManager>.Instance["sidebar1"])
                .Size(205, 602)
                .Position(0, 0)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            AddTitle("PLAYER", 8, 12);

            _name = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(10, 42)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            AddDivider(68);

            AddSection("CORE STATS", 70);

            var y = 100;

            AddStatRow("Strength", ref y);

            AddStatRow("Toughness", ref y);

            AddStatRow("Speed", ref y);

            AddStatRow("Intelligence", ref y);

            AddStatRow("Libido", ref y);

            AddStatRow("Sensitivity", ref y);

            AddStatRow("Corruption", ref y);

            AddSection("COMBAT", 288);

            y = 318;

            AddBarRow("HP", ref y, _hpColor);

            AddBarRow("Lust", ref y, _barColor);

            AddBarRow("Fatigue", ref y, _barColor);

            AddSection("ADVANCEMENT", 428);

            y = 458;

            AddStatRow("Level", ref y);

            AddStatRow("XP", ref y);

            AddStatRow("Gems", ref y);

            AddLabel("Day", 10, 548);

            AddLabel("Time", 105, 548);
        }

        private void AddTitle(string text, int x, int y) =>

            new Label(UIFonts.Get("sidebarTitle"))
                .Text(text)
                .Position(x, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

        private void AddSection(string text, int y)
        {
            AddTitle(text, 8, y);

            AddDivider(y + 26);
        }

        private void AddDivider(int y) =>

            new ColorRect(new Color(96, 0, 0))
                .Size(187, 1)
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

        private Label AddLabel(string key, int x, int y)
        {
            var label = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(x, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _values[key] = label;

            return label;
        }

        private void AddStatRow(string key, ref int y)
        {
            new Label(UIFonts.Get("sidebar"))
                .Text(key)
                .Position(10, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            var value = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _values[key] = value;

            y += 26;
        }

        private void AddBarRow(string key, ref int y, Color color)
        {
            new Label(UIFonts.Get("sidebar"))
                .Text(key)
                .Position(10, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            var value = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _values[key] = value;

            _fills[key] = new ColorRect(color)
                .Size(0, 7)
                .Position(8, y + 24)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            if (key == "Lust")
                _minFills[key] = new ColorRect(new Color(136, 1, 1))
                    .Size(0, 7)
                    .Position(8, y + 24)
                    .LayoutType(LayoutType.Relative)
                    .Of(this);

            y += 36;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var scene = Scene.Current;

            Visible = !HiddenByMenu && scene != null && scene.GetType().Name != "CharCreation";

            var player = CoC.Instance?.Player;

            if (player == null || !Visible)
                return;

            _name.Text(player.ShortName);

            Set("Strength", player.Strength);

            Set("Toughness", player.Toughness);

            Set("Speed", player.Speed);

            Set("Intelligence", player.Intelligence);

            Set("Libido", player.Libido);

            Set("Sensitivity", player.Sensitivity);

            Set("Corruption", player.Corruption);

            Set("HP", player.Health, player.MaxHealth);

            Set("Lust", player.Lust, player.MaxLust);

            Set("Fatigue", player.Fatigue, player.MaxFatigue());

            Set("Level", player.Level);

            Set("XP", player.Experience, player.RequiredXP());

            Set("Gems", player.Gems);

            _values["Day"].Text($"Day {GameView.Instance?.Time.Days ?? 0}");

            _values["Time"].Text($"Time {GameView.Instance?.Time.Hours ?? 0:00}:{GameView.Instance?.Time.Minutes ?? 0:00}");
        }

        private void Set(string key, float value, float max = 0)
        {
            if (_values.TryGetValue(key, out var label))
            {
                label.Text(max > 0

                    ? $"{value:0}/{max:0}"

                    : $"{value:0}");

                SetRight(label, 195);
            }

            if (_fills.TryGetValue(key, out var fill))
            {
                var width = max <= 0 ? 0 : (int)Math.Clamp(value / max * BarWidth, 0, BarWidth);

                fill.LayoutData.Size.X = width;
            }

            if (key == "Lust" && _minFills.TryGetValue(key, out var minFill) && max > 0)
            {
                var min = CoC.Instance.Player?.MinLust() ?? 0;

                minFill.LayoutData.Size.X =

                    (int)Math.Clamp(min / max * BarWidth, 0, BarWidth);
            }
        }

        private static void SetRight(Label label, int right)
        {
            var size = label.RichTextLayout.Measure(1000);

            label.LayoutData.Position.X = right - size.X;
        }
    }
}
