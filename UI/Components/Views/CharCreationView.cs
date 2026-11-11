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
            Initialize(new(), new());
        }

        public Component Background;

        public TextArea Output;

        public TextBox Input;

        protected override string ScreenFile => "Views\\CharCreationView.hjson";

        protected override UIViewContract Contract => CharCreationContract.Value;

        protected override void BindParts()
        {
            Background = RequirePart<Component>("charCreation.background");

            Input = RequirePart<TextBox>("charCreation.input");

            Output = RequirePart<TextArea>("charCreation.output");
        }
    }

    internal static class CharCreationContract
    {
        public static readonly UIViewContract Value = new(
            "CharCreationView",
            [
                new("charCreation.background", typeof(Component)),
                new("charCreation.input", typeof(TextBox)),
                new("charCreation.output", typeof(TextArea)),
                new("charCreation.buttons", typeof(Grid))
            ]);
    }
}
