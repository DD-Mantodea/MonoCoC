using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<ExploreScene>("coc:explore")]
    public class ExploreScene : Scene
    {
        private readonly PagedMenu<MonoCoC.Contents.World.Areas.AreaDefinition> _areas = new();

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            if (Flags.Get(GameFlagIds.TimesExplored) <= 0)
            {
                OutputText("Scenes.Explore.FirstSteps".Localized());

                Flags.Set(GameFlagIds.TimesExplored, Flags.Get(GameFlagIds.TimesExplored) + 1);

                AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(1));

                return;
            }

            if (Flags.Get(GameFlagIds.TimesExploredForest) <= 0)
            {
                OutputText("Areas.Forest.Discovery".Localized());

                Flags.Set(GameFlagIds.TimesExplored, Flags.Get(GameFlagIds.TimesExplored) + 1);

                Flags.Set(GameFlagIds.TimesExploredForest, Flags.Get(GameFlagIds.TimesExploredForest) + 1);

                AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Explore.Intro".Localized());

            AddButton(0, "Scenes.Explore.Explore".Localized(), TryDiscover);

            _areas.Reset();

            RenderAreas();
        }

        private void RenderAreas()
        {
            _areas.Items = MonoCoC.Contents.World.Areas.AreaRegistry.Discovered(Player);

            _areas.PageSize = 11;

            _areas.Render(
                (slot, label, action) =>
                {
                    if (action == null)
                        AddButtonDisabled(slot, label);
                    else
                        AddButton(slot, label, action);
                },

                area => area.NameKey.Localized(),

                area => () => Go(SceneRegistry.CreateOrDefault(area.SceneId)),

                () => Go<CampScene>(),

                RenderAreas,

                "Common.Button.Prev".Localized(),

                "Common.Button.Next".Localized(),

                "Common.Back".Localized(),

                1);
        }

        public static void TryDiscover()
        {
            ClearOutput();

            ClearButtons();

            Flags.Set(GameFlagIds.TimesExplored, Flags.Get(GameFlagIds.TimesExplored) + 1);

            var context = new EncounterContext
            {
                Player = Player,

                Flags = Flags,

                Time = View.Time,

                Random = Random.Shared,

                Write = OutputText,

                ReturnToCamp = () => ReturnToCamp(1)
            };

            DiscoveryTable().Execute(context);
        }

        public static IEncounter DiscoveryTable() => IEncounter.Group(
            "explore_discovery",

            Discovery("lake", AreaIds.Lake, IEncounter.Always),

            Discovery("desert", AreaIds.Desert, 0.33f, AreaIds.Lake),

            Discovery("mountain", AreaIds.Mountain, 0.33f, AreaIds.Desert),

            Discovery("plains", AreaIds.Plains, 0.33f, AreaIds.Mountain),

            Discovery("swamp", AreaIds.Swamp, 0.33f, AreaIds.Plains),

            Discovery("glacial_rift", AreaIds.GlacialRift, 0.25f, AreaIds.Swamp, 10),

            Discovery("volcanic_crag", AreaIds.VolcanicCrag, 0.25f, AreaIds.Swamp, 15),

            IEncounter.Create(
                "nothing",

                _ => 1f,

                _ => true,

                _ =>
                {
                    OutputText("Scenes.Explore.NothingNew".Localized());

                    AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(1));
                }));

        public static IEncounter Discovery(string name, Identifier id, float chance, Identifier? after = null, int minLevel = 0)
        {
            var area = AreaRegistry.Get(id)!;

            return IEncounter.Create(
                name,

                _ => chance,

                _ => !area.IsDiscovered(Player) &&

                     (after == null || AreaRegistry.Get(after)!.IsDiscovered(Player)) &&

                     Player.Level >= minLevel,

                _ =>
                {
                    AreaRegistry.Discover(area);

                    ClearOutput();

                    ClearButtons();

                    OutputText(DiscoveryText(area));

                    AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(id == AreaIds.Swamp ? 2 : 1));
                });
        }

        public static string DiscoveryText(AreaDefinition area)
        {
            if (area.Id != AreaIds.Desert)
                return area.DiscoveryKey.Localized();

            var key = Player.Body.Get<LowerBodyPart>().Id.Path switch
            {
                "hoofed" => "Areas.Desert.SandHoofed",

                "dog" => "Areas.Desert.SandDog",

                "coc:naga" => "Areas.Desert.SandNaga",

                _ => "Areas.Desert.SandHuman"
            };

            return area.DiscoveryKey.Localized(key.Localized());
        }

    }
}