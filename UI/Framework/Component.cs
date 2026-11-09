using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Materials;
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

        public string PartName = "";

        public bool IsPressed;
        #endregion

        #region Data
        public LayoutData LayoutData = new();

        public float Alpha => LayoutData.ComputeAlpha();

        public int Width => LayoutData.ComputeWidth();

        public int Height => LayoutData.ComputeHeight();

        public Vector2 Position => LayoutData.ComputePosition();

        public bool CanHit = true;

        // 通用点击回调（原版 OneMonsterView 的 CLICK → selectMonster 对应物）。
        // UIManager 在鼠标左键抬起时，对命中的组件向上找第一个 OnClick 非空的组件并调用。
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

        // 在真正绘制前完成所有延迟布局（例如 TextArea 的文本测量），
        // 避免新场景第一帧以未初始化尺寸绘制。
        public virtual void PrepareDraw()
        {
            foreach (var child in LayoutData.Children)
                child.PrepareDraw();
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Visible)
                return;

            DrawSelf(spriteBatch, gameTime);

            if (Material is not null)
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

            spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);

            try
            {
                DrawChildren(spriteBatch, gameTime);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);
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

            var color = StateStyle.BackgroundFor(Enabled, IsHovering, IsPressed);

            if (color != Color.Transparent)
                spriteBatch.DrawRectangle(Rectangle, color * LayoutData.Alpha);
        }

        private void DrawStateBorder(SpriteBatch spriteBatch)
        {
            if (StateStyle is null)
                return;

            var color = StateStyle.BorderFor(Enabled, IsHovering, IsPressed);

            if (color == Color.Transparent || StateStyle.BorderThickness <= 0)
                return;

            var thickness = StateStyle.BorderThickness;
            var rectangle = Rectangle;

            spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color * LayoutData.Alpha);
            spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color * LayoutData.Alpha);
            spriteBatch.DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color * LayoutData.Alpha);
            spriteBatch.DrawRectangle(new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color * LayoutData.Alpha);
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
                spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);
            }
        }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawStateBorder(spriteBatch);
        }

        public virtual void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var child in LayoutData.Children)
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