using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components.Views
{
    public static class ViewNavigator
    {
        private static readonly Dictionary<View, View> Previous = [];

        private static readonly Dictionary<View, Action> CustomBack = [];

        public static void Open(View view)
        {
            var root = UIManager.Instance?.Root;

            if (root is null)
                return;

            Previous[view] = root.View;

            root.SwitchView(view);
        }

        public static void OpenFromMenu(View view)
        {
            var root = UIManager.Instance?.Root;

            if (root is null)
                return;

            var previous = root.View;

            root.SwitchView(view);

            CustomBack[view] = () =>
            {
                root.SwitchView(previous);

                MenuBarView.Toggle();
            };
        }

        public static bool Back(View view)
        {
            var root = UIManager.Instance?.Root;

            if (root is null)
                return false;

            if (CustomBack.Remove(view, out var custom))
            {
                custom();

                return true;
            }

            if (!Previous.TryGetValue(view, out var previous))
                return false;

            Previous.Remove(view);

            root.SwitchView(previous);

            return true;
        }
    }
}
