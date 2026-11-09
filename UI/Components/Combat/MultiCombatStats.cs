using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Combat
{
    // 原版 coc.view.MonsterStatsView 的对应实现：
    // 按 monsterArray 顺序纵向排列多个敌人面板，点击面板切换当前攻击目标；
    // 当前目标在面板上方显示一个向下的三角指示（程序生成的抗锯齿贴图）。
    public class MultiCombatStats : Container, IUIStatefulPart
    {
        private sealed record Snapshot(List<Enemy> Enemies, int Selected, Action<int>? Select);

        public const int Gap = 8;

        private const int PointerWidth = 20;

        private const int PointerHeight = 19;

        private readonly List<EnemyStatsPanel> _panels = [];

        private Action<int>? _select;

        private readonly List<Enemy> _enemies = [];

        private int _selected;

        public object? CaptureState() => new Snapshot([.. _enemies], _selected, _select);

        public void RestoreState(object value)
        {
            if (value is not Snapshot state)
                return;

            if (state.Enemies.Count == 0)
            {
                HideAll();

                return;
            }

            Refresh(state.Enemies, state.Selected, state.Select ?? (_ => { }));
        }

        public MultiCombatStats()
        {
            for (var i = 0; i < 3; i++)
            {
                var index = i;

                var panel = new EnemyStatsPanel()
                    .Position(0, i * (EnemyStatsPanel.PanelHeight + Gap))
                    .LayoutType(LayoutType.Relative)
                    .Of(this);

                panel.OnClick = () => _select?.Invoke(index);

                panel.Hide();

                _panels.Add(panel);
            }

            Visible = false;
        }

        // 照原版 refreshStats：前 N 个面板显示，其余隐藏
        public void Refresh(IReadOnlyList<Enemy> enemies, int selected, Action<int> select)
        {
            _select = select;

            _selected = selected;

            _enemies.Clear();

            _enemies.AddRange(enemies);

            for (var i = 0; i < _panels.Count; i++)
            {
                _panels[i].LayoutData.Position.Y = i * (EnemyStatsPanel.PanelHeight + Gap);

                if (i < enemies.Count)
                    _panels[i].Show(enemies[i]);
                else
                    _panels[i].Hide();
            }

            Visible = enemies.Count > 0;
        }

        public void HideAll()
        {
            foreach (var panel in _panels)
                panel.Hide();

            Visible = false;
        }

    }
}
