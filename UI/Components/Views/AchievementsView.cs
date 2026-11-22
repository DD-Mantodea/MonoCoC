using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Achievements;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Linq;
using System.Text;

namespace MonoCoC.UI.Components.Views
{
    public class AchievementsView : HjsonView
    {
        private static readonly string[] SectionOrder =
        [
            "Storyline", "Zones", "Levels", "Population", "Time", "Dungeons",
            "Fashion", "Wealth", "Combat", "Seasonal Events",
            "Survival/Realistic Mode", "Challenges", "General"
        ];

        public AchievementsView()
        {
            Initialize();

            var text = new StringBuilder();

            text.Append("Note: Some achievements are contradictory and may require multiple playthroughs to obtain every achievement.\n");
            text.Append("[pg]");

            var manager = ManagerInstance<AchievementManager>.Instance;
            var earned = 0;
            var total = 0;

            foreach (var section in SectionOrder)
            {
                var visible = AchievementTable.Entries
                    .Where(e => e.Section == section)
                    .Where(e => manager.IsUnlocked(Parse(e.Id)) || !e.Shadow)
                    .ToArray();

                if (visible.Length == 0)
                    continue;

                text.Append("\n<b><u>" + section + "</u></b>\n");

                foreach (var entry in visible)
                {
                    var unlocked = manager.IsUnlocked(Parse(entry.Id));
                    var title = entry.Title.Replace("{0}", MonoCoC.CoC.Instance.Player.Mf("Master", "Mistress"));
                    var desc = unlocked && entry.DescUnlocked.Length > 0 ? entry.DescUnlocked : entry.DescLocked;
                    var shadowFlair = entry.Shadow ? " - [color:#660066]Shadow Achievement[/color]" : "";

                    if (unlocked)
                        earned++;

                    total++;

                    text.Append("   \u2022  [b:" + title + ":] [color:" + (unlocked ? "#008000" : "#800000")
                        + "]" + (unlocked ? "Unlocked" : "Locked") + "[/color] - " + desc + shadowFlair + "\n");
                }
            }

            Body.Clear();

            MonoCoC.Contents.Scenes.Scene.Parser.Reset();

            Body.Append(MonoCoC.Contents.Scenes.Scene.Parser.Parse(text.ToString()));

            Earned.Text($"{earned}/{total} earned");
        }

        private TextArea Body { get; set; } = null!;

        private Label Earned { get; set; } = null!;

        protected override string ScreenFile => "Views\\AchievementsView.hjson";

        [UIRequired("achievements.text")]
        private void BindText(TextArea part) => Body = part;

        [UIRequired("achievements.earned")]
        private void BindEarned(Label part) => Earned = part;

        [UIEvent("achievements.back")]
        private static void Back() => UIManager.Instance.Root.SwitchView(new MainView());

        private static Identifier Parse(string id) => Identifier.Parse(id);
    }
}
