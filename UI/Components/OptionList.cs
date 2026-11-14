using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    public class OptionList : Component
    {
        private readonly List<Button> _buttons = [];

        private readonly List<string> _ids = [];

        private string? _selected;

        public int Columns { get; set; } = 4;

        public int Spacing { get; set; } = 8;

        public Action<string>? SelectionChanged;

        public void SetOptions(IEnumerable<(string Id, string Label)> options)
        {
            foreach (var button in _buttons)
                RemoveChild(button);

            _buttons.Clear();

            _ids.Clear();

            foreach (var (id, label) in options)
            {
                var button = new Button("", label, "coc:menu")
                {
                    BackgroundColor = new Color(0xF7, 0xEF, 0xE0, 0xCC),
                    TextColor = new Color(0x35, 0x29, 0x1F, 255),
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                button.Click(() =>
                {
                    SelectedId = id;

                    SelectionChanged?.Invoke(id);
                });

                AddChild(button);

                _buttons.Add(button);

                _ids.Add(id);
            }

            RefreshLayout();
        }

        public string? SelectedId
        {
            get => _selected;

            set
            {
                _selected = value;

                for (var index = 0; index < _buttons.Count; index++)
                {
                    var selected = _ids[index] == _selected;

                    _buttons[index].BackgroundColor = selected
                        ? new Color(0xF2, 0xE1, 0xBE, 255)
                        : new Color(0xF7, 0xEF, 0xE0, 0xCC);
                }
            }
        }

        public void SetOptionVisible(string id, bool visible)
        {
            var index = _ids.IndexOf(id);

            if (index >= 0)
                _buttons[index].Visible = visible;
        }

        public void SetOptionLabel(string id, string label)
        {
            var index = _ids.IndexOf(id);

            if (index >= 0)
                _buttons[index].Label.Text(label);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            RefreshLayout();
        }

        private void RefreshLayout()
        {
            if (_buttons.Count == 0)
                return;

            var columns = Math.Max(1, Columns);

            var rows = (int)Math.Ceiling(_buttons.Count / (float)columns);

            var width = (Width - Spacing * (columns - 1)) / columns;

            var height = (Height - Spacing * (rows - 1)) / rows;

            for (var index = 0; index < _buttons.Count; index++)
            {
                var column = index % columns;

                var row = index / columns;

                _buttons[index].LayoutData.Size = new Vector2(width, height);

                _buttons[index].LayoutData.Position = new Vector2(
                    column * (width + Spacing),
                    row * (height + Spacing));
            }
        }
    }
}
