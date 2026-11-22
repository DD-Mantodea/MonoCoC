using System;

namespace MonoCoC.Contents.Scenes
{
    public enum SceneActionRole
    {
        None,
        Continue,
        Exit
    }

    public class SceneAction
    {
        public int Slot { get; set; } = -1;

        public string Label { get; set; } = "";

        public Action? OnClick { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Visible { get; set; } = true;

        public string Hint { get; set; } = "";

        public string HintHeader { get; set; } = "";

        public SceneActionRole Role { get; set; } = SceneActionRole.None;
    }
}
