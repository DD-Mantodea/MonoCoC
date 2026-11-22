using Hjson;
using MonoCoC.Configuration;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Components.Views
{
    internal sealed class OptionsSubPages
    {
        private readonly OptionsView _view;

        public OptionsSubPages(OptionsView view) => _view = view;

        public void Render(JsonObject page)
        {
            _view.Pane!.Header(page.Str("title", "").Localized(), "", "");

            var messageKey = page.Str("message", "");

            if (messageKey.Length > 0)
            {
                var messageOption = page.Str("messageOption", "");

                _view.Pane.Message(messageOption.Length > 0
                    ? messageKey.Localized(OptionStore.Instance.Get(messageOption, 20))
                    : messageKey.Localized());
            }

            switch (page.Str("kind", "message"))
            {
                case "fontSize":
                    RenderSizeRows(page);
                    break;

                case "choice":
                    RenderChoiceRows(page);
                    break;

                case "confirm":
                    RenderConfirmRows(page);
                    break;

                case "hardcore":
                    RenderHardcoreRows(page);
                    break;
            }
        }

        private void RenderSizeRows(JsonObject page)
        {
            var option = page.Str("option", "fontSize");

            var min = page.Int("min", 14);

            var max = page.Int("max", 32);

            var reset = page.Int("default", 20);

            foreach (var value in page.Array("rows") ?? [])
            {
                if (value is not JsonObject row)
                    continue;

                var delta = row.Int("delta", 0);

                _view.Pane!.Toggle(row.Str("label", "").Localized(), "", "", false,
                    ("View.Options.Choose".Localized(), false, () =>
                    {
                        var current = OptionStore.Instance.Get(option, reset);

                        OptionStore.Instance.Set(option, delta == 0 ? reset : Math.Clamp(current + delta, min, max));

                        _view.OpenDetail(() => Render(page));

                        ReloadViews(UIManager.Instance?.Root);
                    }));
            }
        }

        private void RenderChoiceRows(JsonObject page)
        {
            var option = page.Str("option", "");

            foreach (var value in page.Array("rows") ?? [])
            {
                if (value is not JsonObject row)
                    continue;

                var rowValue = row.Int("value", 0);

                _view.Pane!.Toggle(row.Str("label", "").Localized(), "", "", false,
                    ("View.Options.Choose".Localized(), OptionStore.Instance.Get(option) == rowValue, () =>
                    {
                        OptionStore.Instance.Set(option, rowValue);

                        _view.CloseDetail();
                    }));
            }
        }

        private void RenderConfirmRows(JsonObject page)
        {
            var option = page.Str("option", "");

            _view.Pane!.Toggle("View.Options.Enable".Localized(), "", "", false,
                ("View.Options.Enable".Localized(), false, () =>
                {
                    OptionStore.Instance.Set(option, 1);

                    _view.CloseDetail();
                }));

            _view.Pane.Toggle("View.Options.Cancel".Localized(), "", "", false,
                ("View.Options.Cancel".Localized(), false, () => _view.CloseDetail()));
        }

        private void RenderHardcoreRows(JsonObject page)
        {
            var slots = page.Int("slots", 14);

            for (var slot = 1; slot <= slots; slot++)
            {
                var value = slot;

                _view.Pane!.Toggle("View.Options.Slot".Localized(slot), "", "", false,
                    ("View.Options.Enable".Localized(), false, () =>
                    {
                        OptionStore.Instance.Set("hardcore", 1);

                        OptionStore.Instance.Set("hardcoreSlot", value);

                        _view.CloseDetail();
                    }));
            }
        }

        private static void ReloadViews(Component? component)
        {
            if (component is null)
                return;

            if (component is HjsonView view && !ReferenceEquals(view, OptionsView.Current))
                view.Reload();

            foreach (var child in component.LayoutData.Children)
                ReloadViews(child);
        }
    }
}
