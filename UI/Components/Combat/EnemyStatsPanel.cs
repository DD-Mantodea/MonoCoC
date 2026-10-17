using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components.Combat
{
    // 原版 coc.view.OneMonsterView 的对应实现。
    // 外观与 CombatStatsView 完全一致（sidebarEnemy 底图 + 红分隔线 + 三条 StatBar + 右对齐数值），
    // 只是可承载任意敌人，从而能在右侧纵向堆叠多个面板。
    public class EnemyStatsPanel : Container
    {
        public const int PanelWidth = 230;

        public const int PanelHeight = 190;

        private const int BarWidth = 214;

        private readonly Label _name;

        private readonly Label _level;

        private readonly Label _hp;

        private readonly Label _lust;

        private readonly Label _fatigue;

        private readonly ColorRect _hpFill;

        private readonly ColorRect _lustFill;

        private readonly ColorRect _fatigueFill;

        public EnemyStatsPanel()
        {
            LayoutData.Size.X = PanelWidth;

            LayoutData.Size.Y = PanelHeight;

            new UIImage(ManagerInstance<TextureManager>.Instance["sidebarEnemy"])
                .Size(PanelWidth, PanelHeight)
                .Position(0, 0)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            new Label(UIFonts.Get("sidebarTitle"))
                .Text("ENEMY")
                .Position(8, 10)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _name = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(10, 32)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            AddDivider(54);

            _level = AddRow("LEVEL", 60);

            _hp = AddRow("HP", 86);

            _lust = AddRow("LUST", 118);

            _fatigue = AddRow("FATIGUE", 150);

            _hpFill = FillAt(106, new Color(177, 125, 94));

            _lustFill = FillAt(138, new Color(136, 1, 1));

            _fatigueFill = FillAt(170, new Color(96, 0, 0));

        }

        private void AddDivider(int y) =>

            new ColorRect(new Color(96, 0, 0)).Size(214, 1).Position(8, y).LayoutType(LayoutType.Relative).Of(this);

        private Label AddRow(string text, int y)
        {
            new Label(UIFonts.Get("sidebar")).Text(text).Position(10, y).LayoutType(LayoutType.Relative).Of(this);

            return new Label(UIFonts.Get("sidebar")).Text("").Position(8, y).LayoutType(LayoutType.Relative).Of(this);
        }

        private ColorRect FillAt(int y, Color color) =>

            new ColorRect(color).Size(0, 8).Position(8, y).LayoutType(LayoutType.Relative).Of(this);

        public void Show(Enemy enemy)
        {
            Visible = true;

            CanHit = true;

            _name.Text(enemy.ShortName);

            _level.Text($"{enemy.Level:0}");

            _hp.Text($"{enemy.Health:0}/{enemy.MaxHealth:0}");

            _lust.Text($"{enemy.Lust:0}/{enemy.MaxLust:0}");

            _fatigue.Text($"{enemy.Fatigue:0}/100");

            SetRight(_level, PanelWidth - 8);

            SetRight(_hp, PanelWidth - 8);

            SetRight(_lust, PanelWidth - 8);

            SetRight(_fatigue, PanelWidth - 8);

            _hpFill.LayoutData.Size.X = Fill(enemy.Health, enemy.MaxHealth);

            _lustFill.LayoutData.Size.X = Fill(enemy.Lust, enemy.MaxLust);

            _fatigueFill.LayoutData.Size.X = Fill(enemy.Fatigue, 100);

        }

        public void Hide() { Visible = false; CanHit = false; }

        private static int Fill(float value, float max) =>

            max <= 0 ? 0 : (int)Math.Clamp(value / max * BarWidth, 0, BarWidth);

        private static void SetRight(Label label, int right)
        {
            var size = label.RichTextLayout.Measure(1000);

            label.LayoutData.Position.X = right - size.X;
        }
    }
}
