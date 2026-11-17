using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components.Views
{
    public class InstructionsView : HjsonView
    {
        protected override string ScreenFile => "Views\\InstructionsView.hjson";

        public InstructionsView()
        {
            Initialize();

            Body.Clear();

            Body.Append(string.Join("\n\n",
                "MainMenu.Instructions.HowToPlay".Localized(),
                "MainMenu.Instructions.Exploration".Localized(),
                "MainMenu.Instructions.Combat".Localized(),
                "MainMenu.Instructions.Controls".Localized(),
                "MainMenu.Instructions.SaveToFile".Localized())
                .Replace("\\n", "\n")
                .Replace("[pg]", "\n\n")
                .Replace("[b:", "")
                .Replace("[u:", "")
                .Replace("[i:", "")
                .Replace("]", ""));
        }

        private TextArea Body { get; set; } = null!;

        [UIRequired("instructions.text")]
        private void BindText(TextArea part) => Body = part;

        [UIEvent("instructions.back")]
        private static void Back() => UIManager.Instance.Root.SwitchView(new MainView());
    }
}
