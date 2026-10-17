using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components.Combat
{
    public class CombatStats : Container
    {
        private const int BarWidth = 214;

        private readonly Label _name;

        private readonly Label _level;

        private readonly Label _hp;

        private readonly Label _lust;

        private readonly Label _fatigue;

        private readonly ColorRect _hpFill;

        private readonly ColorRect _lustFill;

        private readonly ColorRect _fatigueFill;

        private Enemy? _enemy;

        private float _hpValue;

        private float _lustValue;

        private float _fatigueValue;

        public CombatStats()
        {
            new UIImage(ManagerInstance<TextureManager>.Instance["sidebarEnemy"])
                .Size(230, 190)
                .Position(0, 0)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            new Label(UIFonts.Get("sidebarTitle"))
                .Text("ENEMY")
                .Position(8, 4)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _name = new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(10, 26)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            AddDivider(48);

            _level = AddRow("LEVEL", 54);

            _hp = AddRow("HP", 80);

            _lust = AddRow("LUST", 112);

            _fatigue = AddRow("FATIGUE", 144);

            _hpFill = FillAt(100, new Color(177, 125, 94));

            _lustFill = FillAt(132, new Color(136, 1, 1));

            _fatigueFill = FillAt(164, new Color(96, 0, 0));
        }

        private void AddDivider(int y) =>

            new ColorRect(new Color(96, 0, 0))
                .Size(214, 1)
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

        private Label AddRow(string text, int y)
        {
            new Label(UIFonts.Get("sidebar"))
                .Text(text)
                .Position(10, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            return new Label(UIFonts.Get("sidebar"))
                .Text("")
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);
        }

        private ColorRect FillAt(int y, Color color) =>

            new ColorRect(color)
                .Size(0, 8)
                .Position(8, y)
                .LayoutType(LayoutType.Relative)
                .Of(this);

        public void Show(Enemy enemy)
        {
            _enemy = enemy;

            _hpValue = enemy.Health;

            _lustValue = enemy.Lust;

            _fatigueValue = enemy.Fatigue;

            Visible = true;
        }

        public void Hide()
        {
            Visible = false;

            _enemy = null;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible || _enemy == null)
                return;

            _hpValue = Approach(_hpValue, _enemy.Health);

            _lustValue = Approach(_lustValue, _enemy.Lust);

            _fatigueValue = Approach(_fatigueValue, _enemy.Fatigue);

            _name.Text(_enemy.ShortName);

            _level.Text(_enemy.Level.ToString("0"));

            _hp.Text($"{_enemy.Health:0}/{_enemy.MaxHealth:0}");

            _lust.Text($"{_enemy.Lust:0}/{_enemy.MaxLust:0}");

            _fatigue.Text($"{_enemy.Fatigue:0}/100");

            SetRight(_level, 222);

            SetRight(_hp, 222);

            SetRight(_lust, 222);

            SetRight(_fatigue, 222);

            _hpFill.LayoutData.Size.X = FillWidth(_hpValue, _enemy.MaxHealth);

            _lustFill.LayoutData.Size.X = FillWidth(_lustValue, _enemy.MaxLust);

            _fatigueFill.LayoutData.Size.X = FillWidth(_fatigueValue, 100);
        }

        private static float Approach(float current, float target)
        {
            if (Math.Abs(target - current) < 0.5f)
                return target;

            return current + (target - current) * 0.12f;
        }

        private static int FillWidth(float value, float max) =>

            max <= 0 ? 0 : (int)Math.Clamp(value / max * BarWidth, 0, BarWidth);

        private static void SetRight(Label label, int right)
        {
            var size = label.RichTextLayout.Measure(1000);

            label.LayoutData.Position.X = right - size.X;
        }
    }
}
