using MonoCoC.Attributes;
using MonoCoC.Contents.Perks;
using MonoCoC.Extensions;
using System.Linq;
using System.Text;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 原版 PlayerInfo.hx:601 displayPerks
    [Register<PerksScene>("coc:perks")]
    public class PerksScene : Scene
    {
        private static readonly PagedMenu<PerkPageActions.Entry> Pager = new();

        public override void Entry() => Render();

        public static void BackToPerks() => Go<PerksScene>();

        private static void Render()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.Perks.Title".Localized());

            var sb = new StringBuilder();

            foreach (var perk in Player.Perks)
            {
                var key = perk.Id.Path.PascalKey();

                var name = ("Perks." + key + ".Name").LocalizedOr(PerkRegistry.GetName(perk.Id));

                var description = ("Perks." + key + ".Desc")
                    .LocalizedOr(PerkRegistry.Get(perk.Id)?.Description ?? "");

                sb.Append("[pg-][b:").Append(name).Append(":] ").Append(description);
            }

            if (Player.Perks.Count == 0)
                sb.Append("Scenes.Perks.None".Localized());

            OutputText(sb.ToString());

            var visible = PerkPageActions.All.Where(entry => entry.Visible()).ToArray();

            foreach (var entry in visible)
                OutputText(entry.Text());

            AddButtonDisabled(0, "Scenes.Perks.Title".Localized());

            AddButton(1, "Scenes.Perks.Database".Localized(), ShowDatabase);

            Pager.Items = visible;

            Pager.PageSize = 10;

            Pager.Render(
                (slot, label, action) =>
                {
                    if (action == null)
                        AddButtonDisabled(slot, label);
                    else
                        AddButton(slot, label, action);
                },

                entry => entry.LabelKey.Localized(),

                entry => entry.Open,

                BackToPerks,

                Render,

                "Common.Button.Prev".Localized(),

                "Common.Button.Next".Localized(),

                "Common.Back".Localized(),

                2);

            // 原版 PlayerInfo.hx:608 displayPerks 的 Back 是退出 Perks 回到游戏，
            // 不是 PagedMenu 的“返回 Perks 首页”。
            AddButton(14, "Common.Back".Localized(), ExitPerks);
        }

        private static void ExitPerks()
        {
            Go<CampScene>();
        }

        private static void ShowDatabase()
        {
            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.Perks.Database".Localized());

            var sb = new StringBuilder();

            foreach (var id in PerkRegistry.Registry.Ids.OrderBy(id => PerkRegistry.GetName(id)))
            {
                var definition = PerkRegistry.Get(id);

                if (definition == null)
                    continue;

                var key = id.Path.PascalKey();

                sb.Append("[pg-][b:").Append(("Perks." + key + ".Name")
                        .LocalizedOr(PerkRegistry.GetName(id))).Append(":] ")
                    .Append(("Perks." + key + ".Desc").LocalizedOr(definition.Description));
            }

            OutputText(sb.ToString());

            AddButton(14, "Common.Back".Localized(), BackToPerks);
        }
    }
}
