using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Storage;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Storage
{
    public class StoragePane : Container
    {
        private const int Padding = 10;

        private const int ButtonHeight = 40;

        private const int ButtonGap = 6;

        private readonly Container _content;

        private readonly List<Component> _rows = [];

        private int _scroll;

        private int _contentHeight;

        private bool _draggingBar;

        private IReadOnlyList<StorageArea> _sections = [];

        private string _title = "";

        private string _description = "";

        private Action<StorageArea, int> _withdraw = (_, _) => { };

        // 原版 scenes/Inventory.hx:1012 describe(storage)：每个储物区自己的说明文字
        private Func<StorageArea, string>? _sectionDescription;

        private readonly HashSet<string> _collapsed = [];

        private readonly List<(StorageArea Section, int Index, Button Button)> _slotButtons = [];

        public Action<StorageArea, int>? OnDragStart;

        public Action<Vector2>? OnDragMove;

        public Action<Vector2>? OnDragEnd;

        public StoragePane(int x, int y, int width, int height)
        {
            Scissor = true;

            CanHit = true;

            LayoutData.Size = new Vector2(width, height);

            LayoutData.Position = new Vector2(x, y);

            _content = new Container()
                .Size(width, 0)
                .Position(0, 0)
                .LayoutType(LayoutType.Relative)
                .Of(this);
        }

        private SpriteFontBase Font => UIFonts.Get("body");

        public void Build(IReadOnlyList<StorageArea> sections, string title, string description, Action<StorageArea, int> withdraw, Func<StorageArea, string>? sectionDescription = null)
        {
            _sections = sections;

            _title = title;

            _description = description;

            _withdraw = withdraw;

            _sectionDescription = sectionDescription;

            Rebuild();
        }

        private void Rebuild()
        {
            _content.LayoutData.Children.Clear();

            _rows.Clear();

            _slotButtons.Clear();

            _scroll = 0;

            _content.LayoutData.Position = Vector2.Zero;

            var y = Padding;

            y = AddLabel(_title, y, true);

            y = AddLabel(_description, y, false);

            y += 6;

            var usable = Width - Padding * 2 - ScrollBar.Thickness;

            const int columns = 3;

            var buttonWidth = (usable - ButtonGap * (columns - 1)) / columns;

            foreach (var section in _sections)
            {
                var expanded = !_collapsed.Contains(section.Definition.Id.ToString());

                var header = new StorageSectionHeader(
                    usable,

                    expanded,

                    section.Definition.NameKey.Localized(),

                    "Scenes.Storage.Capacity".Localized(section.OccupiedSlots(), section.Capacity),

                    () =>
                    {
                        if (!_collapsed.Remove(section.Definition.Id.ToString()))
                            _collapsed.Add(section.Definition.Id.ToString());

                        Rebuild();
                    })
                    .Position(Padding, y)
                    .LayoutType(LayoutType.Relative)
                    .Of(_content);

                y += StorageSectionHeader.HeaderHeight + 4;

                if (_sectionDescription != null)
                {
                    var sectionText = _sectionDescription(section);

                    if (!string.IsNullOrEmpty(sectionText))
                        y = AddLabel(sectionText, y, false);
                }

                if (!expanded)
                    continue;

                for (var index = 0; index < section.Slots.Count; index++)
                {
                    var slot = section.Slots[index];

                    var column = index % columns;

                    var line = index / columns;

                    var button = new Button("button0", slot.IsEmpty ? "Scenes.Inventory.Nothing".Localized() : slot.InvLabel)
                        .Size(buttonWidth, ButtonHeight)
                        .Position(Padding + column * (buttonWidth + ButtonGap), y + line * (ButtonHeight + ButtonGap))
                        .LayoutType(LayoutType.Relative)
                        .Of(_content);

                    button.LabelPadding = 8;

                    _slotButtons.Add((section, index, button));

                    if (slot.IsEmpty)
                    {
                        button.Enabled = false;

                        button.RefreshVisual();
                    }
                    else
                    {
                        var storageIndex = index;

                        button.Hint = slot.TooltipText;

                        button.HintHeader = slot.TooltipHeader;

                        button.OnClick = () => _withdraw(section, storageIndex);

                        button.OnMouseDown = () => OnDragStart?.Invoke(section, storageIndex);

                        button.OnMouseMove = position => OnDragMove?.Invoke(position);

                        button.OnMouseUp = position => OnDragEnd?.Invoke(position);
                    }
                }

                y += ((section.Slots.Count + columns - 1) / columns) * (ButtonHeight + ButtonGap) + 6;
            }

            _contentHeight = y;

            _content.LayoutData.Size = new Vector2(Width, _contentHeight);

            _scroll = Math.Clamp(_scroll, 0, Math.Max(0, _contentHeight - Height));

            _content.LayoutData.Position = new Vector2(0, -_scroll);
        }

        private int AddLabel(string text, int y, bool heading)
        {
            if (string.IsNullOrEmpty(text))
                return y;

            var label = new Label(Font)
                .Text(text)
                .Position(Padding, y)
                .WrapWidth(Width - Padding * 2 - ScrollBar.Thickness)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            _rows.Add(label);

            return y + label.RichTextLayout.Measure(Width - Padding * 2 - ScrollBar.Thickness).Y + 6;
        }

        public bool TryHitStorage(Vector2 position, out StorageArea section, out int index)
        {
            section = null!;

            index = -1;

            if (!Rectangle.Contains(position))
                return false;

            foreach (var entry in _slotButtons)
            {
                if (!entry.Button.Visible || !entry.Button.Rectangle.Contains(position))
                    continue;

                section = entry.Section;

                index = entry.Index;

                return true;
            }

            return false;
        }

        public override void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawChildren(spriteBatch, gameTime);

            if (!ScrollBar.Needed(_contentHeight, Height))
                return;

            var track = ScrollBar.Track(Rectangle);

            var thumb = ScrollBar.Thumb(Rectangle, _contentHeight, Height, _scroll);

            spriteBatch.DrawRectangle(track, UIColors.ScrollTrack);

            spriteBatch.DrawBorder(track, 1, UIColors.ScrollTrackBorder);

            spriteBatch.DrawRectangle(thumb, UIColors.ScrollThumb);

            spriteBatch.DrawBorder(thumb, 1, UIColors.ScrollThumbBorder);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            switch (args.Type)
            {
                case InputType.MouseWheel:
                    SetScroll(_scroll + (args.WheelDelta > 0 ? -40 : 40));

                    return InputResult.Consumed;

                case InputType.MouseLeftDown:
                    if (!ScrollBar.Contains(Rectangle, args.MousePosition, _contentHeight, Height))
                        break;

                    _draggingBar = true;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y));

                    return InputResult.Consumed;

                case InputType.MouseMove:
                    if (!_draggingBar)
                        break;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y));

                    return InputResult.Consumed;

                case InputType.MouseLeftUp:
                    if (!_draggingBar)
                        break;

                    _draggingBar = false;

                    return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        private void SetScroll(int value)
        {
            _scroll = Math.Clamp(value, 0, Math.Max(0, _contentHeight - Height));

            _content.LayoutData.Position = new Vector2(0, -_scroll);
        }
    }

    public class StorageSectionHeader : Container
    {
        public const int HeaderHeight = 32;

        private readonly Action _toggle;

        private readonly bool _expanded;

        public StorageSectionHeader(int width, bool expanded, string name, string capacity, Action toggle)
        {
            _toggle = toggle;

            _expanded = expanded;

            LayoutData.Size = new Vector2(width, HeaderHeight);

            CanHit = true;

            MouseCursor = MouseCursor.Hand;

            var font = UIFonts.Get("body");

            var boldFont = UIFonts.GetAt("palab", 20);

            var indicator = new UIImage(ManagerInstance<TextureManager>.Instance["selection"])
                .Size(12, 11)
                .Position(8, (HeaderHeight - 11) / 2 - 6)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            indicator.Color = Color.Black;

            indicator.Rotation = expanded ? 0 : -MathHelper.PiOver2;

            var nameLabel = new Label(boldFont)
                .Text(name)
                .CanHit(false)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            var nameHeight = nameLabel.RichTextLayout.Measure(Width).Y;

            nameLabel.LayoutData.Position = new Vector2(30, (HeaderHeight - nameHeight) / 2);

            var nameWidth = (int)boldFont.MeasureString(name).X;

            var capacityLabel = new Label(font)
                .Text(capacity)
                .CanHit(false)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            var capacityHeight = capacityLabel.RichTextLayout.Measure(Width).Y;

            capacityLabel.LayoutData.Position = new Vector2(30 + nameWidth + 12, (HeaderHeight - capacityHeight) / 2);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (args.Type == InputType.MouseLeftClick && Rectangle.Contains(args.MousePosition))
            {
                _toggle();

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }
    }
}
