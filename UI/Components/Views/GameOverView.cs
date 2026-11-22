using MonoCoC.UI.Components;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components.Views
{
    public class GameOverView : HjsonView
    {
        private static GameOverView? _active;

        private GameView? _returnView;

        private Label _label = null!;

        public GameOverView() => Initialize();

        protected override string ScreenFile => "Views\\GameOverView.hjson";

        [UIRequired("gameOver.title")]
        private void BindTitle(Label part) => _label = part;

        public static void Open(string title)
        {
            var view = ViewInstance<GameOverView>.Get();

            if (_active is null)
                view._returnView = GameView.Current;

            _active = view;

            UIManager.Instance.Root.SwitchView(view);

            view._label.Text(title);
        }

        public static void Close()
        {
            if (_active is null)
                return;

            var target = _active._returnView;

            _active = null;

            if (target is not null)
                GameView.SwitchTo(target);
        }
    }
}
