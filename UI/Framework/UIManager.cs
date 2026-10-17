using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.UI.Components;

namespace MonoCoC.UI.Framework
{
    public class UIManager(ViewContainer root)
    {
        public static UIManager? Instance;

        public ScissorStack ScissorStack = new(CoC.Instance.GraphicsDevice);

        public ViewContainer? Root = root;

        public Component? FocusedComponent;

        public Component? InputBlock;

        public bool IsInputBlocked(Component component)
        {
            if (InputBlock == null || InputBlock.LayoutData.Parent == null)
                return false;

            for (var current = component; current != null; current = current.LayoutData.Parent)
                if (ReferenceEquals(current, InputBlock))
                    return false;

            return true;
        }

        public Component? LeftPressed;

        public Component? RightPressed;

        public MouseCursor MouseCursor;

        public ToolTipView ToolTip = new(ManagerInstance<TextureManager>.Instance["tooltip"], UIFonts.Get("tooltipHeader"), UIFonts.Get("tooltipBody"));

        public static readonly RasterizerState ScissorRasterizer = new()
        {
            CullMode = CullMode.None,

            ScissorTestEnable = true
        };

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Root?.PrepareDraw();

            spriteBatch.Begin(rasterizerState: ScissorRasterizer);

            Root?.Draw(spriteBatch, gameTime);

            ToolTip.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            Root?.Update(gameTime);

            UpdateCursor();

            UpdateToolTip();

            DispatchClicks();
        }

        private bool _sceneChanged;

        public void MarkSceneChanged() => _sceneChanged = true;

        // 输入派发期间可能发生 Scene.Go / SwitchView。
        // 在 Draw 前对新建的 UI 树补一次 Update，避免首帧未初始化。
        public void UpdateAfterInput(GameTime gameTime)
        {
            if (!_sceneChanged)
                return;

            _sceneChanged = false;

            Root?.Update(gameTime);
        }

        // 左键抬起时派发点击：命中组件向上找第一个带 OnClick 的组件。
        // Button 自己处理鼠标事件，这里跳过它，避免重复触发。
        private void DispatchClicks()
        {
            var mouse = Mouse.GetState();

            var released = UserInput.PreviousMouseState.LeftButton == ButtonState.Pressed

                && mouse.LeftButton == ButtonState.Released;

            if (!released)
                return;

            for (var component = Root?.HitComponent(UserInput.GetMousePos()); component != null; component = component.LayoutData.Parent)
            {
                if (component is Button)
                    return;

                if (component.OnClick is { } onClick)
                {
                    onClick();

                    return;
                }
            }
        }

        public void UpdateToolTip()
        {
            var hovered = Root?.HitComponent(UserInput.GetMousePos());

            Component? source = null;

            for (var component = hovered; component != null; component = component.LayoutData.Parent)
            {
                if (string.IsNullOrEmpty(component.ToolTipText))
                    continue;

                source = component;

                break;
            }

            if (source != null)
                ToolTip.Show(source);
            else
                ToolTip.Hide();
        }

        public void UpdateCursor()
        {
            var cursor = MouseCursor.Arrow;

            for (var component = Root?.HitComponent(UserInput.GetMousePos()); component != null; component = component.LayoutData.Parent)
            {
                if (component.MouseCursor == null)
                    continue;

                cursor = component.MouseCursor;

                break;
            }

            if (cursor != MouseCursor)
            {
                Mouse.SetCursor(cursor);

                MouseCursor = cursor;
            }
        }

        #region Dispatch
        public InputResult Dispatch(InputEventArgs args)
        {
            if (!UserInput.HasFocus)
                return InputResult.Continue;

            var target = args.Type switch
            {
                InputType.KeyDown or InputType.KeyUp or InputType.KeyKeepPress or InputType.TextInput => FocusedComponent ?? Root,

                InputType.MouseMove or InputType.MouseLeftUp when LeftPressed != null => LeftPressed,

                InputType.MouseRightUp when RightPressed != null => RightPressed,

                _ => Root?.HitComponent(args.MousePosition)
            };

            if (target == null)
                return InputResult.Continue;

            return Bubble(target, args);
        }

        public InputResult Bubble(Component target, InputEventArgs args)
        {
            for (var component = target; component != null; component = component.LayoutData.Parent)
                if (component.OnInput(args) is InputResult.Consumed)
                    return InputResult.Consumed;

            return InputResult.Continue;
        }

        public InputResult Send(Component target, InputType type, Vector2 mousePos) => Bubble(target, new()
        {
            Type = type,

            MousePosition = mousePos,

            MouseButtonState = ButtonState.Released,

            Shift = UserInput.Shift,

            Ctrl = UserInput.Ctrl,

            Alt = UserInput.Alt
        });

        public InputResult DispatchTextInput(char character) => Dispatch(new() { Type = InputType.TextInput, Character = character, Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchMouseMove(Vector2 mousePos, Vector2 mouseDelta) => Dispatch(new() { Type = InputType.MouseMove, MousePosition = mousePos, MouseDelta = mouseDelta, Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchMouseWheel(Vector2 mousePos, int wheelDelta) => Dispatch(new() { Type = InputType.MouseWheel, MousePosition = mousePos, WheelDelta = wheelDelta, Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchMouseLeftDown(Vector2 mousePos)
        {
            LeftPressed = Root?.HitComponent(mousePos);

            return Dispatch(new()
            {
                Type = InputType.MouseLeftDown,

                MousePosition = mousePos,

                MouseButtonState = ButtonState.Pressed,

                Shift = UserInput.Shift,

                Ctrl = UserInput.Ctrl,

                Alt = UserInput.Alt
            });
        }

        public InputResult DispatchMouseLeftUp(Vector2 mousePos)
        {
            var released = LeftPressed;

            var result = Dispatch(new()
            {
                Type = InputType.MouseLeftUp,

                MousePosition = mousePos,

                MouseButtonState = ButtonState.Released,

                Shift = UserInput.Shift,

                Ctrl = UserInput.Ctrl,

                Alt = UserInput.Alt
            });

            LeftPressed = null;

            if (released != null && released.Rectangle.Contains(mousePos))
                Send(released, InputType.MouseLeftClick, mousePos);

            return result;
        }

        public InputResult DispatchMouseLeftClick(Vector2 mousePos) => Dispatch(new() { Type = InputType.MouseLeftClick, MousePosition = mousePos, MouseButtonState = ButtonState.Released, Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchMouseRightDown(Vector2 mousePos)
        {
            RightPressed = Root?.HitComponent(mousePos);

            return Dispatch(new()
            {
                Type = InputType.MouseRightDown,

                MousePosition = mousePos,

                MouseButtonState = ButtonState.Pressed,

                Shift = UserInput.Shift,

                Ctrl = UserInput.Ctrl,

                Alt = UserInput.Alt
            });
        }

        public InputResult DispatchMouseRightUp(Vector2 mousePos)
        {
            var released = RightPressed;

            var result = Dispatch(new()
            {
                Type = InputType.MouseRightUp,

                MousePosition = mousePos,

                MouseButtonState = ButtonState.Released,

                Shift = UserInput.Shift,

                Ctrl = UserInput.Ctrl,

                Alt = UserInput.Alt
            });

            RightPressed = null;

            if (released != null && released.Rectangle.Contains(mousePos))
                Send(released, InputType.MouseRightClick, mousePos);

            return result;
        }

        public InputResult DispatchMouseRightClick(Vector2 mousePos) => Dispatch(new() { Type = InputType.MouseRightClick, MousePosition = mousePos, MouseButtonState = ButtonState.Released, Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchKeyDown(Keys key) => Dispatch(new() { Type = InputType.KeyDown, Key = key, MousePosition = UserInput.GetMousePos(), Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchKeyUp(Keys key) => Dispatch(new() { Type = InputType.KeyUp, Key = key, MousePosition = UserInput.GetMousePos(), Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });

        public InputResult DispatchKeyKeepPress(Keys key) => Dispatch(new() { Type = InputType.KeyKeepPress, Key = key, MousePosition = UserInput.GetMousePos(), Shift = UserInput.Shift, Ctrl = UserInput.Ctrl, Alt = UserInput.Alt });
        #endregion
    }
}