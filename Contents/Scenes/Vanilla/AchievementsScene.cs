using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.UI.Framework;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 Achievements.hx:44 achievementsScreen()：条目、顺序、文本全部来自原版表
    [Register<AchievementsScene>("coc:achievements")]
    public class AchievementsScene : Scene
    {
        private static readonly string[] SectionOrder =

        [
            "Storyline", "Zones", "Levels", "Population", "Time", "Dungeons",

            "Fashion", "Wealth", "Combat", "Seasonal Events",

            "Survival/Realistic Mode", "Challenges", "General"
        ];

        public override void Entry()
        {
            ClearOutput();

            DisplayHeader("Achievements");

            OutputText("Note: Some achievements are contradictory and may require multiple playthroughs to obtain every achievement.\n");

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

                OutputText("\n<b><u>" + section + "</u></b>\n");

                foreach (var entry in visible)
                {
                    var unlocked = manager.IsUnlocked(Parse(entry.Id));

                    var title = entry.Title.Replace("{0}", Player.Mf("Master", "Mistress"));

                    var desc = unlocked && entry.DescUnlocked.Length > 0 ? entry.DescUnlocked : entry.DescLocked;

                    var shadowFlair = entry.Shadow

                        ? " - [color:#660066]Shadow Achievement[/color]"

                        : "";

                    if (unlocked)
                        earned++;

                    total++;

                    OutputText("   \u2022  [b:" + title + ":] [color:" + (unlocked ? "#008000" : "#800000")

                        + "]" + (unlocked ? "Unlocked" : "Locked") + "[/color] - " + desc + shadowFlair + "\n");
                }
            }

            ClearButtons();

            AddButtonDisabled(0, $"{earned}/{total} earned", "This is how many achievements you have obtained in total.");

            AddButton(14, "Common.Back".Localized(), () =>
            {
                UIManager.Instance.Root.SwitchView(new UI.Components.Views.MainView());
            });
        }

        private static Identifier Parse(string id) => Identifier.Parse(id);
    }
}
