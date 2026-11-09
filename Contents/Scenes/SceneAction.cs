using System;

namespace MonoCoC.Contents.Scenes
{
    public sealed class SceneAction
    {
        public int Slot { get; set; } = -1;

        public string Label { get; set; } = "";

        public Action? OnClick { get; set; }

        public bool Enabled { get; set; } = true;

        public bool Visible { get; set; } = true;

        public string Hint { get; set; } = "";

        public string HintHeader { get; set; } = "";
    }
}
