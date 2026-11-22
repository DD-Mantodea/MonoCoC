using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Components.Views
{
    public class MenuBarView : HjsonView
    {
        private static MenuBarView? _active;

        private static MenuBarView? _instance;

        private static Component? _previousBlock;

        private static Component? _previousFocus;

        public MenuBarView()
        {
            Initialize();
        }

        protected override string ScreenFile => "Views\\MenuBarView.hjson";

        public static void Toggle()
        {
            if (_active is not null)
            {
                Close();

                return;
            }

            var root = UIManager.Instance?.Root;

            if (root is null)
                return;

            _instance ??= new MenuBarView();

            _instance.Of(root);

            _active = _instance;

            _previousBlock = UIManager.Instance!.InputBlock;

            _previousFocus = UIManager.Instance.FocusedComponent;

            UIManager.Instance.InputBlock = _instance;
        }

        public static void Close()
        {
            if (_active is null)
                return;

            var menu = _active;

            _active = null;

            if (UIManager.Instance is not null)
            {
                if (ReferenceEquals(UIManager.Instance.InputBlock, menu))
                    UIManager.Instance.InputBlock = _previousBlock;

                if (ReferenceEquals(UIManager.Instance.FocusedComponent, menu))
                    UIManager.Instance.FocusedComponent = _previousFocus;
            }

            menu.LayoutData.Parent?.RemoveChild(menu);
        }

        public static void OpenView(View view)
        {
            Close();

            ViewNavigator.OpenFromMenu(view);
        }

        [UIEvent("menu.close")]
        private void CloseEvent() => Close();

        [UIEvent("menu.main_menu")]
        private void MainMenu()
        {
            Close();

            UIManager.Instance?.Root?.SwitchView(new MainView());
        }

        [UIEvent("menu.save_load")]
        private void SaveLoad() => OpenView(new SaveLoadView());

        [UIEvent("menu.options")]
        private void Options() => OpenView(new OptionsView());

        [UIEvent("menu.guide")]
        private void Guide() => OpenView(new InstructionsView());

        [UIEvent("menu.credits")]
        private void Credits() => OpenView(new CreditView());

        public override InputResult OnInput(InputEventArgs args)
        {
            if (args.Type == InputType.KeyDown && args.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
            {
                Close();

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }
    }
}
