using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Data;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class CharCreationView : HjsonView
    {
        public CharCreationView()
        {
            Initialize();
        }

        public Component Background;

        public TextArea Output;

        public TextBox Input;

        protected override string ScreenFile => "Views\\CharCreationView.hjson";

        [UIRequired("charCreation.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("charCreation.input")]
        private void BindInput(TextBox part) => Input = part;

        [UIRequired("charCreation.output")]
        private void BindOutput(TextArea part) => Output = part;
    }
}
