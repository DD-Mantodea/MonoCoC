using Microsoft.Xna.Framework;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;

namespace MonoCoC.UI.Components.Combat
{
    public class EnemyStats : HjsonView, IUIStatefulPart, IFromHjson<EnemyStats>
    {
        private record Snapshot(Enemy? Enemy, bool Visible);

        private readonly string _screenFile;

        private Label _name = null!;

        private Label _level = null!;

        private Bar _hpBar = null!;

        private Bar _lustBar = null!;

        private Bar _fatigueBar = null!;

        private Enemy? _enemy;

        private float _hpValue;

        private float _lustValue;

        private float _fatigueValue;

        public EnemyStats()
            : this("Views\\EnemyStats.hjson")
        {
        }

        public EnemyStats(string screenFile)
            : base(screenFile)
        {
            _screenFile = screenFile;

            Initialize();

            if (LayoutData.Size == Vector2.Zero && Content is not null)
                LayoutData.Size = new Vector2(Content.Rectangle.Width, Content.Rectangle.Height);

            if (Content is not null)
            {
                Content.LayoutData.LayoutType = LayoutType.Relative;

                Content.LayoutData.Position = Vector2.Zero;
            }
        }

        protected override string ScreenFile => _screenFile;

        public static string TypeId => "coc:enemy_stats";

        public static Component FromHjson(Hjson.JsonObject node, UIBuildContext services)
        {
            var view = node.Str("view");

            return new EnemyStats(view.Length == 0 ? "Views\\EnemyStats.hjson" : view);
        }

        [UIRequired("enemy.name")]
        private void BindName(Label part) => _name = part;

        [UIRequired("enemy.level")]
        private void BindLevel(Label part) => _level = part;

        [UIRequired("enemy.hpBar")]
        private void BindHpBar(Bar part) => _hpBar = part;

        [UIRequired("enemy.lustBar")]
        private void BindLustBar(Bar part) => _lustBar = part;

        [UIRequired("enemy.fatigueBar")]
        private void BindFatigueBar(Bar part) => _fatigueBar = part;

        protected override void OnPartsBound()
        {
        }

        public object? CaptureState() => new Snapshot(_enemy, Visible);

        public void RestoreState(object value)
        {
            if (value is not Snapshot state)
                return;

            if (state.Enemy is not null && state.Visible)
                Show(state.Enemy);
            else
                Hide();
        }

        public void Show(Enemy enemy)
        {
            _enemy = enemy;

            _hpValue = enemy.Health;

            _lustValue = enemy.Lust;

            _fatigueValue = enemy.Fatigue;

            Visible = true;

            Refresh();
        }

        public void Hide()
        {
            _enemy = null;

            Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible || _enemy is null)
                return;

            _hpValue = Approach(_hpValue, _enemy.Health);

            _lustValue = Approach(_lustValue, _enemy.Lust);

            _fatigueValue = Approach(_fatigueValue, _enemy.Fatigue);

            Refresh();
        }

        private void Refresh()
        {
            if (_enemy is null)
                return;

            _name.Text(_enemy.ShortName);

            _level.Text(_enemy.Level.ToString("0"));

            _hpBar.Max = _enemy.MaxHealth;

            _hpBar.Value = _hpValue;

            _lustBar.Max = _enemy.MaxLust;

            _lustBar.Value = _lustValue;

            _fatigueBar.Max = 100;

            _fatigueBar.Value = _fatigueValue;
        }

        private static float Approach(float current, float target)
        {
            if (System.Math.Abs(target - current) < 0.5f)
                return target;

            return current + (target - current) * 0.12f;
        }

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<EnemyStats>();
    }
}
