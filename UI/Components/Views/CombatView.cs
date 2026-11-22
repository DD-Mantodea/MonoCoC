using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using Microsoft.Xna.Framework;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class CombatView : GameView<CombatView>
    {
        public GameView? ReturnView;

        public new static CombatView? Current => GameView.Current as CombatView;

        public EnemyStats? EnemyStats;

        public Container? EnemiesHost;

        public UIImage? TargetPointer;

        private readonly List<EnemyStats> _enemyCards = [];

        private Action<int>? _selectEnemy;

        private int _enemyScroll;

        private const int EnemyCardWidth = 300;

        private const int EnemyCardHeight = 220;

        private const int EnemyCardGap = 8;

        private CombatContext? _context;

        public CombatView() => Initialize();

        protected override string ScreenFile => "Views\\CombatView.hjson";

        [UIRequired("game.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("game.input")]
        private void BindInput(TextBox part) => Input = part;

        [UIRequired("game.output")]
        private void BindOutput(TextArea part) => Output = part;

        [UIRequired("game.playerStats")]
        private void BindPlayerStats(PlayerStats part) => PlayerStats = part;

        [UIRequired("game.buttons")]
        private void BindButtons(Grid part) => ButtonGrid = part;

        [UIRequired("game.enemyStats")]
        private void BindEnemyStats(EnemyStats part) => EnemyStats = part;

        [UIRequired("game.enemies")]
        private void BindEnemiesHost(Container part) => EnemiesHost = part;

        [UIRequired("game.targetPointer")]
        private void BindTargetPointer(UIImage part) => TargetPointer = part;

        protected override void OnPartsBound()
        {
            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                    Buttons[index] = button;

            Scene.ReapplyActions();

            if (EnemiesHost is not null)
            {
                EnemiesHost.Scissor = true;

                EnemiesHost.CanHit = true;

                EnemiesHost.Visible = false;
            }
        }

        public void ShowCombatStats(Enemy enemy)
        {
            if (TargetPointer is not null)
                TargetPointer.Visible = false;

            if (EnemiesHost is not null)
                EnemiesHost.Visible = false;

            EnemyStats?.Show(enemy);
        }

        public void ShowCombatStats(IReadOnlyList<Enemy> enemies, int selected, System.Action<int> select)
        {
            EnemyStats?.Hide();

            _selectEnemy = select;

            _enemyScroll = 0;

            BuildEnemyCards(enemies);

            if (EnemiesHost is not null)
                EnemiesHost.Visible = true;

            ApplyEnemyScroll();

            if (TargetPointer is not null)
            {
                TargetPointer.LayoutData.Position.X = (EnemiesHost?.Position.X ?? CombatStatsX) +
                    (EnemyCardWidth - 20) / 2;

                TargetPointer.LayoutData.Position.Y = EnemyCardTop(selected) - 4;

                TargetPointer.Visible = enemies.Count > 1 && selected >= 0 && selected < enemies.Count;
            }
        }

        public void HideCombatStats()
        {
            EnemyStats?.Hide();

            if (EnemiesHost is not null)
                EnemiesHost.Visible = false;

            if (TargetPointer is not null)
                TargetPointer.Visible = false;
        }

        private void BuildEnemyCards(IReadOnlyList<Enemy> enemies)
        {
            if (EnemiesHost is null)
                return;

            while (_enemyCards.Count < enemies.Count)
            {
                var card = new EnemyStats();

                card.LayoutData.Size = new Vector2(EnemyCardWidth, EnemyCardHeight);

                card.LayoutData.LayoutType = LayoutType.Relative;

                card.Of(EnemiesHost);

                _enemyCards.Add(card);
            }

            while (_enemyCards.Count > enemies.Count)
            {
                var card = _enemyCards[^1];

                _enemyCards.RemoveAt(_enemyCards.Count - 1);

                EnemiesHost.RemoveChild(card);
            }

            for (var i = 0; i < enemies.Count; i++)
            {
                _enemyCards[i].Show(enemies[i]);

                _enemyCards[i].Visible = true;
            }
        }

        private int EnemyCardTop(int index) =>
            (int)(EnemiesHost?.Position.Y ?? 0) + index * (EnemyCardHeight + EnemyCardGap) - _enemyScroll;

        private void ApplyEnemyScroll()
        {
            var maxScroll = Math.Max(0,
                _enemyCards.Count * (EnemyCardHeight + EnemyCardGap) - EnemyCardGap -
                (int)(EnemiesHost?.LayoutData.Size.Y ?? 0));

            _enemyScroll = Math.Clamp(_enemyScroll, 0, maxScroll);

            for (var i = 0; i < _enemyCards.Count; i++)
                _enemyCards[i].LayoutData.Position = new Vector2(0, i * (EnemyCardHeight + EnemyCardGap) - _enemyScroll);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (EnemiesHost is not { Visible: true } || !EnemiesHost.IsHovering)
                return;

            var wheel = UserInput.GetDeltaWheelValue();

            if (wheel == 0)
                return;

            _enemyScroll -= Math.Sign(wheel) * 48;

            ApplyEnemyScroll();
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (EnemiesHost is { Visible: true } && args.Type == InputType.MouseLeftClick &&
                EnemiesHost.Rectangle.Contains(args.MousePosition))
            {
                var index = (int)((args.MousePosition.Y - EnemiesHost.Position.Y + _enemyScroll) /
                                  (EnemyCardHeight + EnemyCardGap));

                if (index >= 0 && index < _enemyCards.Count)
                    _selectEnemy?.Invoke(index);
            }

            return base.OnInput(args);
        }

        public override void ApplyContext(Context<CombatView> context)
        {
            if (context is not CombatContext combat)
                return;

            _context = combat;

            ReturnView = combat.ReturnView;

            StartWith(combat);
        }

        public void StartWith(CombatContext context)
        {
            if (context.Enemies.Count <= 1)
            {
                ShowCombatStats(context.Enemy);

                return;
            }

            ShowCombatStats(context.Enemies, context.CurrTarget, SwitchTarget);
        }

        public void SwitchTarget(int index)
        {
            if (_context is null)
                return;

            _context.SetTarget(index);

            ShowCombatStats(_context.Enemies, _context.CurrTarget, SwitchTarget);
        }

        public void UpdateEnemies(IReadOnlyList<Enemy> enemies)
        {
            if (_context is null)
                return;

            ShowCombatStats(enemies, _context.CurrTarget, SwitchTarget);
        }

        public void Return()
        {
            if (ReturnView is not null)
                SwitchTo(ReturnView);
        }
    }
}
