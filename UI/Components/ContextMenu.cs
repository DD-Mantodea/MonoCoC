using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    // 二级菜单通用条目：文案走 i18n key，Invoke 返回给玩家的提示（null 表示无提示）。
    public sealed class ContextMenuEntry
    {
        public required string LabelKey;

        public Func<string?>? Invoke;

        public int Order;
    }

    // 通用二级菜单（右键菜单 / 下拉 / 长按菜单都复用它）。
    // 外观全部来自 HJSON：容器背景/描边来自节点的 states，条目来自 itemTemplate（MenuItem 模板）；
    // 组件只负责"条目内容、位置、点击、关闭"这些交互。
    public class ContextMenu : Container
    {
        private readonly List<Button> _buttons = [];

        private readonly List<ContextMenuEntry> _entries = [];

        // 由 UIScreenBuilder 按节点的 itemTemplate 提供（模板声明外观，代码只负责实例化）。
        public Func<Button>? ItemFactory;

        public int Padding = 6;

        public int ItemHeight = 36;

        public int Spacing = 2;

        public Action<ContextMenuEntry>? Chosen;

        public Action? Dismissed;

        public bool IsOpen { get; private set; }

        private int _scroll;

        private int _contentHeight;

        private int _viewHeight;

        public ContextMenu()
        {
            CanHit = true;

            Visible = false;

            LayoutData.LayoutType = LayoutType.Relative;

            Scissor = true;
        }

        public void Open(Vector2 position, IReadOnlyList<ContextMenuEntry> entries)
        {
            if (entries.Count == 0)
                return;

            _entries.Clear();

            _entries.AddRange(entries);

            _entries.Sort((a, b) => a.Order.CompareTo(b.Order));

            Rebuild();

            var width = Math.Min(Width, LayoutConstants.ScreenWidth - 16);

            _contentHeight = Padding * 2 + _entries.Count * ItemHeight + Math.Max(0, _entries.Count - 1) * Spacing;

            // 超过屏幕高度时滚动查看，菜单本身限制在屏幕内。
            _viewHeight = Math.Min(_contentHeight, LayoutConstants.ScreenHeight - 16);

            _scroll = 0;

            var height = _viewHeight;

            var x = Math.Clamp(position.X, 8, Math.Max(8, LayoutConstants.ScreenWidth - width - 8));

            var y = position.Y + height + 8 > LayoutConstants.ScreenHeight
                ? Math.Max(8, position.Y - height)
                : Math.Max(8, position.Y);

            if (width != Width)
            {
                LayoutData.Size = new Vector2(width, LayoutData.Size.Y);

                foreach (var button in _buttons)
                    button.LayoutData.Size = new Vector2(width - Padding * 2, ItemHeight);
            }

            LayoutData.Position = new Vector2(x, y);

            LayoutData.Size = new Vector2(width, height);

            ApplyScroll();

            Visible = true;

            IsOpen = true;

            UIManager.Instance?.InputBlock = this;
        }

        private void ApplyScroll()
        {
            for (var index = 0; index < _buttons.Count; index++)
                _buttons[index].LayoutData.Position = new Vector2(Padding, Padding + index * (ItemHeight + Spacing) - _scroll);
        }
        public void Close(bool notify = true)
        {
            if (!IsOpen)
                return;

            Visible = false;

            IsOpen = false;

            if (UIManager.Instance is not null && ReferenceEquals(UIManager.Instance.InputBlock, this))
                UIManager.Instance.InputBlock = null;

            if (notify)
                Dismissed?.Invoke();
        }

        private void Rebuild()
        {
            foreach (var button in _buttons)
                LayoutData.Children.Remove(button);

            _buttons.Clear();

            var width = Width;

            var items = new List<Button>();

            for (var index = 0; index < _entries.Count; index++)
            {
                var entry = _entries[index];

                var button = ItemFactory?.Invoke() ?? new Button();

                button.SetLabel(entry.LabelKey.Localized());

                items.Add(button);

                button.Click(() =>
                {
                    Chosen?.Invoke(entry);

                    Close();
                });

                AddChild(button);

                button.LayoutData.LayoutType = LayoutType.Relative;

                button.LayoutData.Position = new Vector2(Padding, Padding + index * (ItemHeight + Spacing));

                button.LayoutData.Size = new Vector2(width - Padding * 2, ItemHeight);

                _buttons.Add(button);
            }

            // 菜单宽度取最长条目 + 左右内边距，避免长文案贴边/被裁。
            var needed = width;

            foreach (var button in items)
            {
                var textWidth = button.Label?.RichTextLayout?.Measure(100000).X ?? 0f;

                needed = Math.Max(needed, (int)MathF.Ceiling(textWidth) + Padding * 2 + 16);
            }

            if (needed != width)
            {
                LayoutData.Size = new Vector2(needed, LayoutData.Size.Y);

                foreach (var button in _buttons)
                    button.LayoutData.Size = new Vector2(needed - Padding * 2, ItemHeight);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsOpen)
            {
                if (Rectangle.Contains(UserInput.GetMousePos()))
                {
                    var wheel = UserInput.GetDeltaWheelValue();

                    if (wheel != 0 && _contentHeight > _viewHeight)
                    {
                        _scroll = Math.Clamp(_scroll - Math.Sign(wheel) * 40, 0, _contentHeight - _viewHeight);

                        ApplyScroll();
                    }
                }

                var outside = !Rectangle.Contains(UserInput.GetMousePos());

                if ((UserInput.MouseLeftPressed || UserInput.MouseRightPressed) && outside)
                    Close();
                else if (UserInput.IsJustPress(Keys.Escape))
                    Close();
            }

            base.Update(gameTime);
        }
    }
}
