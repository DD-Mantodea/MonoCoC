using MonoCoC.UI.Components;

namespace MonoCoC.Extensions
{
    public static class ButtonExt
    {
        extension(Button button)
        {
            public Button DisableIf(bool condition, string reason = "")
            {
                if (!condition)
                    return button;

                button.Enabled = false;

                button.DisabledReason = reason;

                button.RefreshVisual();

                return button;
            }

            public Button DisableIfNot(bool condition, string reason = "") => button.DisableIf(!condition, reason);

            public Button Hint(string hint, string header = "")
            {
                button.Hint = hint;

                button.HintHeader = header;

                return button;
            }
        }
    }
}
