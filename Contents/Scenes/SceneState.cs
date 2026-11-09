using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes
{
    public sealed class SceneState
    {
        public List<SceneAction> Actions { get; } = [];

        public void ClearActions() => Actions.Clear();

        public SceneAction? FindAction(int slot)
        {
            foreach (var action in Actions)
                if (action.Slot == slot)
                    return action;

            return null;
        }

        public SceneAction SetAction(int slot, SceneAction action)
        {
            var existing = FindAction(slot);

            if (existing is null)
            {
                action.Slot = slot;

                Actions.Add(action);

                return action;
            }

            existing.Label = action.Label;
            existing.OnClick = action.OnClick;
            existing.Enabled = action.Enabled;
            existing.Visible = action.Visible;
            existing.Hint = action.Hint;
            existing.HintHeader = action.HintHeader;

            return existing;
        }
    }
}
