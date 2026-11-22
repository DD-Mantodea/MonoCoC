using Hjson;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.IO;
using System.Linq;
using System.Text;

namespace MonoCoC.UI.Components.Views
{
    public class CreditView : HjsonView
    {
        private const string ListMarker = "   \u2022  ";

        private const string SubMarker = "      \u25E6  ";

        private Label _heading = null!;

        private TextArea _text = null!;

        protected override string ScreenFile => "Views\\CreditView.hjson";

        public CreditView()
        {
            Initialize();

            ShowPage(0);
        }

        [UIRequired("credits.heading")]
        private void BindHeading(Label part) => _heading = part;

        [UIRequired("credits.text")]
        private void BindText(TextArea part) => _text = part;

        [UIEvent("credits.tab.hgg")]
        private void ShowHgg() => ShowPage(0);

        [UIEvent("credits.tab.revamp")]
        private void ShowRevamp() => ShowPage(1);

        [UIEvent("credits.tab.vanilla")]
        private void ShowVanilla() => ShowPage(2);

        [UIEvent("credits.back")]
        private void Back() => ViewNavigator.Back(this);

        private void ShowPage(int page)
        {
            if (page == 2)
            {
                _heading.Text("MainMenu.Credits.OriginalHeader".Localized());
                _text.Clear();

                _text.Append("MainMenu.Credits.Vanilla".Localized() + "\n");

                return;
            }

            var root = (JsonObject)MonoCoC.HjsonParser.Parse(
                File.ReadAllText(Path.Combine(Paths.AssetPath("Data/Credits"), "Credits.json")));

            var config = (JsonObject)root[page == 0 ? "hgg" : "revamp"];

            _heading.Text((string)config["heading"]);

            var sb = new StringBuilder();

            var credits = (JsonArray)config["credits"];

            if (config.TryGetValue("sections", out var sectionsValue) && sectionsValue is JsonArray sections)
                foreach (var sectionValue in sections)
                {
                    if (sectionValue is not JsonObject section)
                        continue;

                    var type = (string)section["type"];

                    var matches = credits
                        .OfType<JsonObject>()
                        .Where(c => c.TryGetValue("types", out var t) && t is JsonArray types &&
                                    types.Any(x => (string)x == type))
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

            _text.Clear();
            _text.Append(sb.ToString()
                .Replace("\\n", "\n")
                .Replace("[bu:", "[b:")
                .Replace("[pg]", "\n\n")
                .Replace("[b:", "")
                .Replace("[u:", "")
                .Replace("[i:", "")
                .Replace("]", ""));
        }
    }
}
