using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Materials;
using MonoCoC.UI.Data;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;

using Hjson;
using MonoCoC.UI.Structures;
using System.Collections.Generic;
using System.Globalization;
using FontStashSharp;
using FontStashSharp.RichText;
using MonoCoC.UI.Components;
namespace MonoCoC.UI.Framework
{
    public class Component
    {
        #region Visual
        public Color BackgroundColor;

        /// <summary>
        /// Optional full-quad material. When set, Component.Draw uses the
        /// material instead of DrawSelf's normal background rendering.
        /// </summary>
        public Material? Material;

        public UIStateStyle? StateStyle;

        public bool Selected;

        public string PartName = "";

        public IUIDataContext? DataContext;

        public virtual void ApplyDataContext(IUIDataContext? context)
        {
            DataContext = context;

            foreach (var child in LayoutData.Children)
                child.ApplyDataContext(context);
        }

        public bool IsPressed;
        #endregion

        #region Data
        public LayoutData LayoutData = new();

        public float Alpha => LayoutData.ComputeAlpha();

        public int Width => LayoutData.ComputeWidth();

        public int Height => LayoutData.ComputeHeight();

        public Vector2 Position => LayoutData.ComputePosition();

        public bool CanHit = true;

        // 閫氱敤鐐瑰嚮鍥炶皟锛堝師鐗?OneMonsterView 鐨?CLICK 鈫?selectMonster 瀵瑰簲鐗╋級銆?
        // UIManager 鍦ㄩ紶鏍囧乏閿姮璧锋椂锛屽鍛戒腑鐨勭粍浠跺悜涓婃壘绗竴涓?OnClick 闈炵┖鐨勭粍浠跺苟璋冪敤銆?
        public Action? OnClick;

        public bool WasHovering = false;

        public bool IsHovering = false;

        public bool Visible = true;

        public bool Enabled = true;

        public bool Scissor = false;

        public Vector2 Size => new(Width, Height);

        public virtual Rectangle Rectangle => Position.BuildRect(Width, Height);

        public virtual Vector2 SlotOf(int index) => Vector2.Zero;

        public virtual Vector2 SlotSize(int index) =>
            Size - LayoutData.Padding.TopLeft() - LayoutData.Padding.BottomRight();
        #endregion

        #region Tree
        public bool AddChild(Component child)
        {
            if (child == null)
                return false;

            // 环检测：把 X 挂到 X 自己的后代下会让 Parent 链成环，
            // 之后每次布局求值（Size/Position）都会沿父链无限递归，最终栈溢出崩溃。
            // 这里拒绝并打日志，避免整棵树被一个错误挂接搞死。
            for (var ancestor = this; ancestor is not null; ancestor = ancestor.LayoutData.Parent)
            {
                if (!ReferenceEquals(ancestor, child))
                    continue;

                MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                    $"[UI] 拒绝挂接：{child.GetType().Name} 是 {GetType().Name} 的祖先，会形成 Parent 环。\n{Environment.StackTrace}");

                return false;
            }

            LayoutData.Children.Add(child);

            child.LayoutData.Parent = this;

            return true;
        }

        public bool RemoveChild(Component child)
        {
            if (child == null || !LayoutData.Children.Remove(child))
                return false;

            child.LayoutData.Parent = null;

            return true;
        }

        public void RemoveAllChild()
        {
            foreach (var child in LayoutData.Children.ToArray())
                RemoveChild(child);
        }

        public bool HasChild(Component child)
        {
            if (child == null)
                return false;

            return LayoutData.Children.Contains(child);
        }
        #endregion

        #region GameLoop
        public virtual void PreDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        // 鍦ㄧ湡姝ｇ粯鍒跺墠瀹屾垚鎵€鏈夊欢杩熷竷灞€锛堜緥濡?TextArea 鐨勬枃鏈祴閲忥級锛?
        // 閬垮厤鏂板満鏅涓€甯т互鏈垵濮嬪寲灏哄缁樺埗銆?
        // 布局阶段：只计算尺寸/位置，不绘制；在 Update 之后、Draw 之前调用。
        public virtual void PerformLayout(GameTime gameTime)
        {
            foreach (var child in LayoutData.Children.ToArray())
                child.PerformLayout(gameTime);
        }

        public Component? Template { get; private set; }

        public bool SuppressDefaultVisual { get; private set; }

        public void ApplyTemplate(Component root)
        {
            Template = root;
            root.LayoutData.LayoutType = LayoutType.Relative;
            root.LayoutData.Position = Vector2.Zero;
            root.Of(this);
            SuppressDefaultVisual = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Visible)
                return;

            if (!SuppressDefaultVisual)
            {
                DrawSelf(spriteBatch, gameTime);

                if (Material is not null && (BackgroundColor == default || BackgroundColor.A == 255))
                    DrawMaterial(spriteBatch);

                DrawStateBackground(spriteBatch);
            }

            if (!Scissor)
            {
                DrawChildren(spriteBatch, gameTime);

                PostDraw(spriteBatch, gameTime);

                return;
            }

            spriteBatch.End();

            UIManager.Instance.ScissorStack.Push(Rectangle);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);

            try
            {
                DrawChildren(spriteBatch, gameTime);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);
            }

            PostDraw(spriteBatch, gameTime);
        }

        public virtual void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (BackgroundColor != default)
                spriteBatch.DrawRectangle(Position.BuildRect(Width, Height), BackgroundColor * LayoutData.Alpha);
        }

        private void DrawStateBackground(SpriteBatch spriteBatch)
        {
            if (StateStyle is null)
                return;

            var color = StateStyle.BackgroundFor(Enabled, IsHovering, IsPressed, Selected);

            if (color != Color.Transparent)
                spriteBatch.DrawRectangle(Rectangle, color * LayoutData.Alpha);
        }

        private void DrawStateBorder(SpriteBatch spriteBatch)
        {
            if (StateStyle is null)
                return;

            var color = StateStyle.BorderFor(Enabled, IsHovering, IsPressed, Selected);

            var sides = StateStyle.SidesFor(Enabled, IsHovering, IsPressed, Selected);

            if (color == Color.Transparent ||
                (sides.Left <= 0 && sides.Top <= 0 && sides.Right <= 0 && sides.Bottom <= 0))
                return;

            var rectangle = Rectangle;

            if (sides.Top > 0)
                spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, sides.Top), color * LayoutData.Alpha);

            if (sides.Bottom > 0)
                spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Bottom - sides.Bottom, rectangle.Width, sides.Bottom), color * LayoutData.Alpha);

            if (sides.Left > 0)
                spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, sides.Left, rectangle.Height), color * LayoutData.Alpha);

            if (sides.Right > 0)
                spriteBatch.DrawRectangle(new Rectangle(rectangle.Right - sides.Right, rectangle.Y, sides.Right, rectangle.Height), color * LayoutData.Alpha);
        }

        private void DrawMaterial(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            try
            {
                Material!.Draw(spriteBatch, Rectangle);
            }
            finally
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);
            }
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawStateBorder(spriteBatch);
        }

        public virtual void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var child in LayoutData.Children.ToArray())
                child.Draw(spriteBatch, gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            WasHovering = IsHovering;

            var blocked = UIManager.Instance?.IsInputBlocked(this) == true;

            IsHovering = !blocked && IsInsideClip(UserInput.GetMousePos()) && UserInput.GetMouseRectangle().Intersects(Rectangle);

            if (IsHovering != WasHovering)
                if (IsHovering)
                    OnMouseEnter();
                else
                    OnMouseLeave();

            UpdateChildren(gameTime);
        }

        public bool IsInsideClip(Vector2 position)
        {
            for (var parent = LayoutData.Parent; parent != null; parent = parent.LayoutData.Parent)
                if (parent.Scissor && !parent.Rectangle.Contains(position))
                    return false;

            return true;
        }

        public virtual void UpdateChildren(GameTime gameTime)
        {
            foreach (var child in LayoutData.Children)
                child.Update(gameTime);
        }

        public virtual void Unload() { }
        #endregion

        #region Interact
        public MouseCursor MouseCursor;

        public virtual Component? HitComponent(Vector2 position)
        {
            if (!Visible)
                return null;

            if (Scissor && !Rectangle.Contains(position))
                return null;

            for (var i = LayoutData.Children.Count - 1; i >= 0; i--)
            {
                var result = LayoutData.Children[i].HitComponent(position);

                if (result != null)
                    return result;
            }

            if (CanHit && Rectangle.Contains(position))
                return this;

            return null;
        }

        public virtual string ToolTipText => "";

        public virtual string ToolTipHeader => "";

        public virtual InputResult OnInput(InputEventArgs args) => InputResult.Continue;

        public virtual void OnMouseEnter()
        {
            OnMouseEnterAction?.Invoke();
        }

        public virtual void OnMouseLeave()
        {
            OnMouseLeaveAction?.Invoke();
        }

        public Action? OnMouseEnterAction;

        public Action? OnMouseLeaveAction;
        #endregion
    
        public void MakeRelative()
        {
            foreach (var child in this.LayoutData.Children)
            {
                child.LayoutData.LayoutType = LayoutType.Relative;

                child.MakeRelative();
            }
        }

        public void ApplyStateStyle(JsonObject node)
        {
            var hasStates = node.TryGetValue("states", out var value) && value is JsonObject;

            var hasThickness = node.TryGetValue("borderThickness", out _);

            var hasBorderColor = node.TryGetValue("borderColor", out _);

            if (!hasStates && !hasThickness && !hasBorderColor)
                return;

            StateStyle = UIStateStyle.Parse(node);
        }

        public static void ApplyState(
            JsonObject states,
            string name,
            out Color background,
            out Color border,
            out Color? text)
        {
            background = Color.Transparent;
            border = Color.Transparent;
            text = null;

            if (!states.TryGetValue(name, out var value) || value is not JsonObject state)
                return;

            if (state.TryGetValue("backgroundColor", out _))
                background = UIParseUtils.ColorOf(state.Str("backgroundColor"));

            if (state.TryGetValue("borderColor", out _))
                border = UIParseUtils.ColorOf(state.Str("borderColor"));

            if (state.TryGetValue("textColor", out _))
                text = UIParseUtils.ColorOf(state.Str("textColor"));
        }

        public static void ApplyStateThickness(JsonObject states, string name, ref int thickness)
        {
            if (!states.TryGetValue(name, out var value) || value is not JsonObject state)
                return;

            if (state.TryGetValue("borderThickness", out var stateThickness) && stateThickness is not JsonArray)
                thickness = UIParseUtils.Int(stateThickness);
        }

        public static BorderSides? ReadSides(JsonValue value)
        {
            if (value is not JsonArray array || array.Count == 0)
                return null;

            var values = new List<int>();

            foreach (var item in array)
                values.Add(UIParseUtils.Int(item));

            return values.Count switch
            {
                1 => BorderSides.Uniform(values[0]),
                2 => new BorderSides(values[0], values[1], values[0], values[1]),
                3 => new BorderSides(values[1], values[0], values[1], values[2]),
                4 => new BorderSides(values[0], values[1], values[2], values[3]),
                _ => null
            };
        }

        public static BorderSides? StateSides(JsonObject states, string name)
        {
            if (!states.TryGetValue(name, out var value) || value is not JsonObject state ||
                !state.TryGetValue("borderThickness", out var thickness))
                return null;

            return ReadSides(thickness);
        }

        public static UIStateStyle? ReadOptionalStateStyle(JsonObject node, string name, UIStateStyle? fallback)
        {
            if (!node.TryGetValue(name, out var value) || value is not JsonObject states)
                return fallback;

            return BuildStateStyle(states, node.Int("borderThickness", 0));
        }

        public static UIStateStyle BuildPartStateStyle(JsonObject states, string part, int thickness)
        {
            var style = new UIStateStyle { BorderThickness = thickness };

            style.NormalBackground = PartColor(states, "normal", part + "Background") ?? Color.Transparent;
            style.NormalText = PartColor(states, "normal", part + "TextColor");
            style.NormalBorder = PartColor(states, "normal", "borderColor") ?? Color.Transparent;

            style.HoverBackground = PartColor(states, "hover", part + "Background") ?? style.NormalBackground;
            style.HoverText = PartColor(states, "hover", part + "TextColor") ?? style.NormalText;
            style.HoverBorder = PartColor(states, "hover", "borderColor") ?? style.NormalBorder;

            style.PressedBackground = PartColor(states, "pressed", part + "Background") ?? style.NormalBackground;
            style.PressedText = PartColor(states, "pressed", part + "TextColor") ?? style.NormalText;
            style.PressedBorder = PartColor(states, "pressed", "borderColor") ?? style.NormalBorder;

            var normalThickness = PartThickness(states, "normal") ?? thickness;
            style.NormalBorderThickness = normalThickness;
            style.HoverBorderThickness = PartThickness(states, "hover") ?? normalThickness;
            style.PressedBorderThickness = PartThickness(states, "pressed") ?? normalThickness;

            return style;
        }

        public static Color? PartColor(JsonObject states, string stateName, string key)
        {
            if (!states.TryGetValue(stateName, out var value) || value is not JsonObject state)
                return null;

            return state.TryGetValue(key, out _) ? UIParseUtils.ColorOf(state.Str(key)) : null;
        }

        public static int? PartThickness(JsonObject states, string stateName)
        {
            if (!states.TryGetValue(stateName, out var value) || value is not JsonObject state)
                return null;

            return state.TryGetValue("borderThickness", out var thickness) ? UIParseUtils.Int(thickness) : null;
        }

        public static bool HasStateField(JsonObject states, string name, string field) =>
            states.TryGetValue(name, out var value) &&
            value is JsonObject state &&
            state.TryGetValue(field, out _);

        public static void InheritStateFallbacks(JsonObject states, UIStateStyle style)
        {
            if (!HasStateField(states, "hover", "backgroundColor"))
                style.HoverBackground = style.NormalBackground;

            if (!HasStateField(states, "hover", "borderColor"))
                style.HoverBorder = style.NormalBorder;

            if (!HasStateField(states, "hover", "textColor"))
                style.HoverText = style.NormalText;

            if (!HasStateField(states, "hover", "borderThickness"))
            {
                style.HoverBorderThickness = style.NormalBorderThickness;
                style.HoverBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "pressed", "backgroundColor"))
                style.PressedBackground = style.NormalBackground;

            if (!HasStateField(states, "pressed", "borderColor"))
                style.PressedBorder = style.NormalBorder;

            if (!HasStateField(states, "pressed", "textColor"))
                style.PressedText = style.NormalText;

            if (!HasStateField(states, "pressed", "borderThickness"))
            {
                style.PressedBorderThickness = style.NormalBorderThickness;
                style.PressedBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "selected", "backgroundColor"))
                style.SelectedBackground = style.NormalBackground;

            if (!HasStateField(states, "selected", "borderColor"))
                style.SelectedBorder = style.NormalBorder;

            if (!HasStateField(states, "selected", "textColor"))
                style.SelectedText = style.NormalText;

            if (!HasStateField(states, "selected", "borderThickness"))
            {
                style.SelectedBorderThickness = style.NormalBorderThickness;
                style.SelectedBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "disabled", "backgroundColor"))
                style.DisabledBackground = style.NormalBackground;

            if (!HasStateField(states, "disabled", "borderColor"))
                style.DisabledBorder = style.NormalBorder;

            if (!HasStateField(states, "disabled", "textColor"))
                style.DisabledText = style.NormalText;

            if (!HasStateField(states, "disabled", "borderThickness"))
            {
                style.DisabledBorderThickness = style.NormalBorderThickness;
                style.DisabledBorderSides = style.NormalBorderSides;
            }
        }

        public void ApplyMaterial(JsonObject node)
        {
            var materialId = node.Str("material");

            if (materialId.Length == 0)
                return;

            var config = node.TryGetValue("materialConfig", out var value) && value is JsonObject json
                ? json
                : null;

            if (MaterialRegistry.Instance.TryCreate(materialId, config, out var material) && material is not null)
                this.Material = material;
        }

        public void ApplyLayout(JsonObject node)
        {
            if (node.TryGetValue("x", out var x))
                this.LayoutData.Position.X = UIParseUtils.Int(x);

            if (node.TryGetValue("y", out var y))
                this.LayoutData.Position.Y = UIParseUtils.Int(y);

            if (node.TryGetValue("width", out var width))
                this.LayoutData.Size.X = UIParseUtils.Int(width);

            if (node.TryGetValue("height", out var height))
                this.LayoutData.Size.Y = UIParseUtils.Int(height);

            if (node.TryGetValue("padding", out var padding))
                this.LayoutData.Padding = UIParseUtils.ParsePadding(padding);

            if (node.TryGetValue("alpha", out var alpha))
                this.LayoutData.Alpha = UIParseUtils.Float(alpha);

            if (node.TryGetValue("alphaType", out _))
                this.LayoutData.AlphaType = UIParseUtils.ParseAlphaType(node.Str("alphaType"));

            if (node.TryGetValue("sizeType", out _))
                this.LayoutData.SizeType = UIParseUtils.ParseSizeType(node.Str("sizeType"));

            if (node.TryGetValue("layout", out _))
                this.LayoutData.LayoutType = UIParseUtils.ParseLayoutType(node.Str("layout"));

            if (node.TryGetValue("gridRow", out var gridRow))
                this.LayoutData.GridRow = UIParseUtils.Int(gridRow);
            else if (node.TryGetValue("row", out var row))
                this.LayoutData.GridRow = UIParseUtils.Int(row);

            if (node.TryGetValue("gridColumn", out var gridColumn))
                this.LayoutData.GridColumn = UIParseUtils.Int(gridColumn);
            else if (node.TryGetValue("column", out var column))
                this.LayoutData.GridColumn = UIParseUtils.Int(column);

            if (node.TryGetValue("gridRowSpan", out var gridRowSpan))
                this.LayoutData.GridRowSpan = Math.Max(1, UIParseUtils.Int(gridRowSpan));
            else if (node.TryGetValue("rowSpan", out var rowSpan))
                this.LayoutData.GridRowSpan = Math.Max(1, UIParseUtils.Int(rowSpan));

            if (node.TryGetValue("gridColumnSpan", out var gridColumnSpan))
                this.LayoutData.GridColumnSpan = Math.Max(1, UIParseUtils.Int(gridColumnSpan));
            else if (node.TryGetValue("columnSpan", out var columnSpan))
                this.LayoutData.GridColumnSpan = Math.Max(1, UIParseUtils.Int(columnSpan));

            ApplyAlignment(node);

            if (node.TryGetValue("visible", out _))
                this.Visible = node.Bool("visible", true);

            if (node.TryGetValue("enabled", out _))
                this.Enabled = node.Bool("enabled", true);

            if (node.TryGetValue("scissor", out _))
                this.Scissor = node.Bool("scissor", false);

            if (node.TryGetValue("canHit", out _))
                this.CanHit = node.Bool("canHit", true);

            if (node.TryGetValue("backgroundColor", out _))
                this.BackgroundColor = UIParseUtils.ColorOf(node.Str("backgroundColor", "#00000000"));

            if (this is TextArea textArea)
            {
                if (node.TryGetValue("padding", out var textPadding))
                    textArea.Padding = UIParseUtils.ParsePadding(textPadding);

                if (node.TryGetValue("autoScroll", out _))
                    textArea.AutoScroll = node.Bool("autoScroll", textArea.AutoScroll);

                if (node.TryGetValue("scrollStep", out var scrollStep))
                    textArea.ScrollStep = UIParseUtils.Int(scrollStep);

                if (node.TryGetValue("textHorizontalAlign", out var textAlignValue))
                {
                    var textAlign = textAlignValue.ToString().Trim('"').Trim().ToLowerInvariant();

                    textArea.Text.HorizontalAlignment = textAlign switch
                    {
                        "center" => TextHorizontalAlignment.Center,
                        "right" => TextHorizontalAlignment.Right,
                        _ => TextHorizontalAlignment.Left
                    };
                }
            }

            var part = node.Str("part");

            if (part.Length > 0)
                this.PartName = part;
        }


        public void ApplyAlignment(JsonObject node)
        {
            var horizontal = node.Str("horizontalAlign", node.Str("hAlign"));

            var vertical = node.Str("verticalAlign", node.Str("vAlign"));

            var both = node.Str("alignment");

            if (both.Length > 0)
            {
                foreach (var part in both.Split('-', StringSplitOptions.RemoveEmptyEntries))
                {
                    var value = part.Trim().ToLowerInvariant();

                    if (value == "center")
                    {
                        horizontal = "center";
                        vertical = "center";
                    }
                    else if (value is "left" or "right" or "stretch")
                        horizontal = value;
                    else if (value is "top" or "bottom")
                        vertical = value;
                    else
                        UIParseUtils.ReportAlignmentError("alignment", value);
                }
            }

            if (horizontal.Length > 0)
                this.LayoutData.HorizontalAlign = UIParseUtils.ParseHorizontalAlign(horizontal);

            if (vertical.Length > 0)
                this.LayoutData.VerticalAlign = UIParseUtils.ParseVerticalAlign(vertical);

        }

        public static UIStateStyle BuildStateStyle(JsonObject states, int thickness) =>
            UIStateStyle.ParseStates(states, thickness);


    

        public void ApplyStyle(JsonObject node)
        {
            ApplyMaterial(node);
            ApplyLayout(node);
            ApplyStateStyle(node);

            if (this is Button button)
                button.RefreshVisual();
        }

        public void ApplyTemplate(JsonObject node, UIBuildContext services)
        {
            var templateId = node.Str("componentTemplate");

            if (templateId.Length == 0 || services.Document is null)
                return;

            if (!services.Document.ComponentTemplates.TryGetValue(templateId, out var template))
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] Unknown component template '{templateId}'");
                return;
            }

            var root = services.BuildTemplate(template);

            if (root is not null)
                ApplyTemplate(root);
        }
        public void BindEvents(JsonObject node, UIEventRegistry events)
        {
            var click = UIParseUtils.EventId(node, "onClick", "command");

            if (click.Length > 0)
            {
                if (this is Button button)
                    button.OnClick = () => events.Raise(click);
                else
                    this.OnClick = () => events.Raise(click);
            }

            if (this is Button mouseButton)
            {
                var mouseDown = UIParseUtils.EventId(node, "onMouseDown");

                if (mouseDown.Length > 0)
                    mouseButton.OnMouseDown = () => events.Raise(mouseDown);

                var mouseUp = UIParseUtils.EventId(node, "onMouseUp");

                if (mouseUp.Length > 0)
                    mouseButton.OnMouseUp = _ => events.Raise(mouseUp);

                var mouseMove = UIParseUtils.EventId(node, "onMouseMove");

                if (mouseMove.Length > 0)
                    mouseButton.OnMouseMove = _ => events.Raise(mouseMove);
            }

            var mouseEnter = UIParseUtils.EventId(node, "onMouseEnter");

            if (mouseEnter.Length > 0)
                this.OnMouseEnterAction = () => events.Raise(mouseEnter);

            var mouseLeave = UIParseUtils.EventId(node, "onMouseLeave");

            if (mouseLeave.Length > 0)
                this.OnMouseLeaveAction = () => events.Raise(mouseLeave);
        }

    
}
}
