using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Framework;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 MainMenu.hx:247 howToPlay()
    [Register<InstructionsScene>("coc:instructions")]
    public class InstructionsScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            DisplayHeader("MainMenu.Instructions.Header".Localized());

            OutputText("MainMenu.Instructions.HowToPlay".Localized());

            OutputText("MainMenu.Instructions.Exploration".Localized());

            OutputText("MainMenu.Instructions.Combat".Localized());

            OutputText("MainMenu.Instructions.Controls".Localized());

            OutputText("MainMenu.Instructions.SaveToFile".Localized());

            ClearButtons();

            AddButton(14, "Common.Back".Localized(), () =>
            {
                UIManager.Instance.Root.SwitchView(new MainView());
            });
        }
    }
}