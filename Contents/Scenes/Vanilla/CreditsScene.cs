using Hjson;
using MonoCoC.Assets;
using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Framework;
using System.IO;
using System.Linq;
using System.Text;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 MainMenu.hx:263 creditsScreen() / :379 showCredits()
    [Register<CreditsScene>("coc:credits")]
    public class CreditsScene : Scene
    {
        private const string ListMarker = "   \u2022  ";

        private const string SubMarker = "      \u25E6  ";

        private static int _page;

        public override void Entry() => Show(_page);

        private void Show(int page)
        {
            _page = page;

            ClearOutput();

            ClearButtons();

            var sb = new StringBuilder();

            if (page == 2)
            {
                DisplayHeader("MainMenu.Credits.OriginalHeader".Localized());

                sb.Append("MainMenu.Credits.Vanilla".Localized()).Append('\n');
            }
            else
            {
                var root = (JsonObject)HjsonValue.Parse(
                    File.ReadAllText(Path.Combine(Paths.AssetPath("Data/Credits"), "Credits.json")));

                var config = (JsonObject)root[page == 0 ? "hgg" : "revamp"];

                DisplayHeader((string)config["heading"]);

                sb.Append('\n');

                var credits = (JsonArray)config["credits"];

                if (config.TryGetValue("sections", out var sectionsValue) && sectionsValue is JsonArray sections)
                    foreach (var sectionValue in sections)
                    {
                        if (sectionValue is not JsonObject section)
                            continue;

                        var type = (string)section["type"];

                        var matches = credits
                            .OfType<JsonObject>()
                            .Where(c => c.TryGetValue("types", out var t) && t is JsonArray types

                                && types.Any(x => (string)x == type))
                            .ToArray();

                        if (matches.Length == 0)
                            continue;

                        sb.Append("[b:").Append((string)section["heading"]).Append("]:\n");

                        foreach (var credit in matches)
                        {
                            var name = (string)credit["name"];

                            var hash = credit.TryGetValue("hash", out var hashValue) ? (string)hashValue : "";

                            if (type == "Contributions")
                            {
                                sb.Append("[b:").Append(name).Append(']').Append(hash).Append('\n');

                                if (credit.TryGetValue("contributions", out var contributionsValue)
                                    && contributionsValue is JsonArray contributions)

                                    foreach (var contribution in contributions)
                                        if (contribution is JsonArray group)
                                        {
                                            var entries = group.Select(x => (string)x).ToArray();

                                            if (entries.Length == 0)
                                                continue;

                                            sb.Append(ListMarker).Append(entries[0]).Append('\n');

                                            foreach (var entry in entries.Skip(1))
                                                sb.Append(SubMarker).Append(entry).Append('\n');
                                        }
                                        else
                                            sb.Append(ListMarker).Append((string)contribution).Append('\n');

                                sb.Append('\n');
                            }
                            else
                                sb.Append(ListMarker).Append("[b:").Append(name).Append(']')
                                    .Append(hash).Append('\n');
                        }
                    }

                if (config.TryGetValue("displayFooting", out var display) && (bool)display
                    && config.TryGetValue("footing", out var footing))

                    sb.Append((string)footing).Append('\n');
            }

            OutputText(sb.ToString());

            var buttons = new[]
            {
                AddButton(0, "MainMenu.Credits.Hgg".Localized(), () => Show(0), "MainMenu.Credits.HggHint".Localized()),

                AddButton(1, "MainMenu.Credits.Revamp".Localized(), () => Show(1), "MainMenu.Credits.RevampHint".Localized()),

                AddButton(2, "MainMenu.Credits.VanillaTab".Localized(), () => Show(2), "MainMenu.Credits.VanillaHint".Localized()),
            };

            if (page >= 0 && page < buttons.Length && buttons[page] != null)
            {
                buttons[page]!.Enabled = false;

                buttons[page]!.RefreshVisual();
            }

            AddButton(14, "Common.Back".Localized(), () =>
            {
                UIManager.Instance.Root.SwitchView(new MainView());
            });
        }
    }
}
