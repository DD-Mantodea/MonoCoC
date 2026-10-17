using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    internal static class ForestPlaceholder
    {
        public static void Show(string name)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            Scene.DisplayHeader(name);

            Scene.OutputText("Scenes.Forest.Placeholder".Localized(name));

            Scene.AddButton(14, "Common.Back".Localized(), () => Scene.Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }
    }
}