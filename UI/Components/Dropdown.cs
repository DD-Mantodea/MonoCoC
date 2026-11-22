using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MonoCoC.UI.Components
{
    public class Dropdown : Component, IFromHjson<Dropdown>
    {
        private readonly Button _header;

        private readonly Component _list;

        private readonly List<Button> _items = [];

        private Component? _elevatedRow;

        private int _elevatedIndex = -1;

        private IReadOnlyList<string> _options = [];

        public Dropdown()
        {
            _header = new Button("", "", "coc:menu")
            {
                LayoutData = { LayoutType = LayoutType.Relative },
                TextColor = UIParseUtils.ColorOf("#35291FFF"),
                StateStyle = HeaderStyle()
            };

            _header.Click(Toggle);

            AddChild(_header);

            _list = new Component
            {
                CanHit = true,
                BackgroundColor = UIParseUtils.ColorOf("#F7EFE0FF"),
                Visible = false,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            AddChild(_list);
        }

        public Action<int>? ValueChanged;

        public int SelectedIndex { get; private set; }

        public void SetOptions(IReadOnlyList<string> options, int selected)
        {
            _options = options;

            foreach (var item in _items)
                _list.RemoveChild(item);

            _items.Clear();

            for (var index = 0; index < options.Count; index++)
            {
                var optionIndex = index;

                var item = new Button("", options[index], "coc:menu")
                {
                    LayoutData = { LayoutType = LayoutType.Relative },
                    TextColor = UIParseUtils.ColorOf("#35291FFF")
                };

                item.Click(() => Select(optionIndex));

                _list.AddChild(item);

                _items.Add(item);
            }

            SelectedIndex = Math.Clamp(selected, 0, Math.Max(0, options.Count - 1));

            if (_options.Count > 0)
                _header.SetLabel(_options[SelectedIndex] + "  ▾");
        }

        private void Toggle()
        {
            _list.Visible = !_list.Visible;

            if (!_list.Visible)
            {
                RestoreRow();

                return;
            }

            ElevateRow();
        }

        // The option list overflows the row that owns it, so the row has to be drawn and
        // hit-tested above the rows that follow it. Rows are positioned by SettingPane.Relayout
        // from its own row list, so moving the box inside its container changes z-order only.
        private void ElevateRow()
        {
            var row = LayoutData.Parent;

            var container = row?.LayoutData.Parent;

            if (row is null || container is null || _elevatedRow is not null)
                return;

            var children = container.LayoutData.Children;

            var index = children.IndexOf(row);

            if (index < 0 || index == children.Count - 1)
                return;

            children.RemoveAt(index);

            children.Add(row);

            _elevatedRow = row;

            _elevatedIndex = index;
        }

        private void RestoreRow()
        {
            var row = _elevatedRow;

            _elevatedRow = null;

            if (row is null)
                return;

            var children = row.LayoutData.Parent?.LayoutData.Children;

            if (children is null)
                return;

            children.Remove(row);

            children.Insert(Math.Clamp(_elevatedIndex, 0, children.Count), row);

            _elevatedIndex = -1;
        }

        private void Select(int index)
        {
            SelectedIndex = index;

            if (_options.Count > 0)
                _header.SetLabel(_options[index] + "  ▾");

            _list.Visible = false;

            RestoreRow();

            ValueChanged?.Invoke(index);
        }

        public override void Update(GameTime gameTime)
        {
            _header.LayoutData.Size = new Vector2(Width, 40);

            _list.LayoutData.Size = new Vector2(Width, Math.Max(1, _items.Count * 36));

            _list.LayoutData.Position = new Vector2(0, 40);

            for (var index = 0; index < _items.Count; index++)
            {
                _items[index].LayoutData.Size = new Vector2(Width, 36);

                _items[index].LayoutData.Position = new Vector2(0, index * 36);
            }

            base.Update(gameTime);
        }
        private static UIStateStyle HeaderStyle() => new()
        {
            NormalBackground = UIParseUtils.ColorOf("#F7EFE0FF"),
            NormalBorder = UIParseUtils.ColorOf("#B79A70FF"),
            NormalBorderThickness = 1,
            HoverBackground = UIParseUtils.ColorOf("#EFE3CCFF"),
            HoverBorder = UIParseUtils.ColorOf("#9F8156FF"),
            HoverBorderThickness = 1,
            PressedBackground = UIParseUtils.ColorOf("#E0C9AAFF"),
            PressedBorder = UIParseUtils.ColorOf("#9F8156FF"),
            PressedBorderThickness = 1
        };

        public static string TypeId => "coc:dropdown";

        public static Component FromHjson(JsonObject node, UIBuildContext context)
        {
            var dropdown = new Dropdown();

            if (node.TryGetValue("options", out var options) && options is JsonArray array)
            {
                var values = new List<string>();

                foreach (var entry in array)
                    values.Add(entry.ToString().Trim('"'));

                dropdown.SetOptions(values, node.Int("selected", 0));
            }

            return dropdown;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<Dropdown>();
    }
}
