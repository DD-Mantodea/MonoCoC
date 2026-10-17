using System;
using System.Linq;
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Registry;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<FollowersScene>("coc:followers")]
    public class FollowersScene : Scene
    {
        public static Identifier pendingRole = FollowerIds.Roles.Follower;

        public readonly PagedMenu<FollowerDefinition> pager = new();

        public Identifier role = FollowerIds.Roles.Follower;

        public static void Open(Identifier role)
        {
            pendingRole = role;

            Go<FollowersScene>();
        }

        public override void Entry()
        {
            role = pendingRole;

            // 每次从营地进入对应列表都从第一页开始
            pager.Reset();

            Render();
        }

        public void Render()
        {
            ClearOutput();

            ClearButtons();

            OutputText(RoleTitle(role) + "\n\n");

            pager.Items = FollowerRegistry.Present(role);

            pager.PageSize = PagedMenu<FollowerDefinition>.DefaultPageSize;

            pager.Render(
                (slot, label, action) =>
                {
                    var current = pager.Items.FirstOrDefault(item => DisplayName(item) == label);

                    if (action == null)
                        AddButtonDisabled(slot, label, current == null ? "" : HintFor(current));
                    else
                        AddButton(slot, label, action, current == null ? null : HintFor(current));
                },

                DisplayName,

                definition => definition.Interact ?? (() => Show(definition)),

                () => Go<CampScene>(),

                Render,

                "Common.Button.Prev".Localized(),

                "Common.Button.Next".Localized(),

                "Common.Back".Localized(),

                0);

            if (pager.Items.Count == 0)
                OutputText("Common.Text.NobodyIsCurrentlyAtCampIn".Localized());
        }

        public void Show(FollowerDefinition definition)
        {
            ClearOutput();

            OutputText(DisplayName(definition) + "\n\n");

            var described = CampFollowerDescriptions.TryDescribe(definition.Id);

            if (!described)
                if (!string.IsNullOrEmpty(definition.Description))
                    OutputText(definition.Description + "\n\n");
                else
                    OutputText("Common.Text.TheyAreCurrentlyRestingAtCamp".Localized());

            if (CampFollowerDescriptions.TryOpen(definition.Id) is { } open)
                AddButton(0, DisplayName(definition), open, HintFor(definition));

            AddButton(14, "Common.Button.Back".Localized(), Render);
        }

        public static string HintFor(FollowerDefinition definition)
        {
            var id = definition.Id.Path;

            if (id.StartsWith("helspawn", StringComparison.Ordinal))
                return "Scenes.Followers.Hint.Helspawn".Localized();

            return id switch
            {
                "anemone" => "Scenes.Followers.Hint.Anemone".Localized(),

                "ember" => "Scenes.Followers.Hint.Ember".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0

                        ? "morph"

                        : CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == 1 ? "boy" : "girl"),

                "holli" => "Scenes.Followers.Hint.Holli".Localized(),

                "shouldra" => "Scenes.Followers.Hint.Shouldra".Localized(),

                "jojo" => "Scenes.Followers.Hint.Jojo".Localized(),

                "rathazul_follower" => "Scenes.Followers.Hint.Rathazul".Localized(),

                "sophie" or "sophie_follower" or "sophie_slave" => "Scenes.Followers.Hint.Sophie".Localized(),

                "amily" or "amily_lover" => "Scenes.Followers.Hint.Amily".Localized(),

                "arian" => "Scenes.Followers.Hint.Arian".Localized(
                    SceneInstance<ArianScene>.Get().ArianMF("his", "hers")),

                "helia" => "Scenes.Followers.Hint.Helia".Localized(),

                "isabella" => "Scenes.Followers.Hint.Isabella".Localized(),

                "izma" => "Scenes.Followers.Hint.Izma".Localized(),

                "kiha" => "Scenes.Followers.Hint.Kiha".Localized(),

                "marble" => "Scenes.Followers.Hint.Marble".Localized(),

                "phylla" => "Scenes.Followers.Hint.Phylla".Localized(),

                "latex_girl" => "Scenes.Followers.Hint.Latex".Localized(
                    SceneInstance<LatexGirlScene>.Get().GooName()),

                "milk_waifu" => "Scenes.Followers.Hint.Milk".Localized(
                    SceneInstance<MilkWaifuScene>.Get().milkName),

                "ceraph" => "Scenes.Followers.Hint.Ceraph".Localized(
                    SceneInstance<CeraphFollowerScene>.Get().CeraphBus()),

                "vapula" => "Scenes.Followers.Hint.Vapula".Localized(),

                _ => definition.Description,
            };
        }

        public static string DisplayName(FollowerDefinition definition)
        {
            var id = definition.Id.Path;

            if (id.StartsWith("helspawn", StringComparison.Ordinal))
                return SceneInstance<HelSpawnScene>.Get().helspawnName;

            if (id == "latex_girl")
                return SceneInstance<LatexGirlScene>.Get().GooName();

            if (id == "milk_waifu")
                return SceneInstance<MilkWaifuScene>.Get().milkName;

            return definition.Name;
        }

        public static string RoleTitle(Identifier role)
        {
            if (role == FollowerIds.Roles.Lover)
                return "Scenes.Followers.Title.Lovers".Localized();

            if (role == FollowerIds.Roles.Slave)
                return "Scenes.Followers.Title.Slaves".Localized();

            return "Scenes.Followers.Title.Followers".Localized();
        }
    }
}