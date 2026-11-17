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

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Visible)
                return;

            DrawSelf(spriteBatch, gameTime);

            if (Material is not null && (BackgroundColor == default || BackgroundColor.A == 255))
                DrawMaterial(spriteBatch);

            DrawStateBackground(spriteBatch);

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

            IsHovering = !blocked && UserInput.GetMouseRectangle().Intersects(Rectangle);

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
    }
}
