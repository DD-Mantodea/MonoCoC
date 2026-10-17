using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class VolcanicCragEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:volcanic_crag",

            CommonEncounters.Table,

            IEncounter.Create("mermaidExplore", _ => 1f, _ => MermaidWalkScene.Available(), _ => MermaidWalkScene.Run("volcanic")),

            IEncounter.Create("aprilfools", _ => 1f, _ => DateTime.Now.Month == 4 && DateTime.Now.Day == 1 && CoC.Instance.Flags.Get(Flags.GameFlagIds.DlcAprilFools) == 0, _ => CragAprilFools()),

            IEncounter.Create("drakesheart", _ => 1f, _ => true, _ => LootDrakeHeart()),

            IEncounter.Create("golem", _ => 1f, _ => true, _ => Scene.Go<VolcanicGolemScene>()),

            IEncounter.Create("witch", _ => 1f, _ => true, _ => Scene.Go<CorruptedWitchScene>()),

            IEncounter.Create("circe", _ => 1f,

                _ => SceneInstance<CorruptedCoven>.Get().CirceEnabled() || SceneInstance<CorruptedCoven>.Get().CirceUnlockable(),

                _ => Scene.Go<CorruptedCoven>()),

            IEncounter.Create("obsidianshard", _ => 0.2f, _ => true, _ => LootObsidianShard()),

            IEncounter.Create("walk", _ => 1f, _ => true, _ => Walk()),

            IEncounter.Create("encounterTower", _ => 1f,

                _ => CoC.Instance.Flags.Get(Flags.GameFlagIds.FoundWizardTower) != 1,

                _ => Scene.Go<WizardTowerEntranceScene>()),

            IEncounter.Create("hellmouth", _ => 1f, _ => true, _ => Scene.Go<HellmouthScene>()),

            IEncounter.Create("hellmouthAmbush", _ => 0.3f,

                _ => !SceneInstance<HellmouthScene>.Get().Ambushed

                     && CoC.Instance.Flags.Get(Flags.GameFlagIds.MetHellmouth) > 0,

                _ => SceneInstance<HellmouthScene>.Get().HellmouthAmbush()),

            IEncounter.Create("gilt", _ => 1f, _ => true, _ => Scene.Go<GiltScene>()),

            IEncounter.Create("VolcanicCragExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.VolcanicCrag.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void Walk()
    {
        Scene.ClearOutput();

        Scene.OutputText("Scenes.VolcanicCrag.Walk.UneventfulCragHour".Localized());

        Scene.Go<CampScene>();
    }

    public static void LootDrakeHeart()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileExploringTheCragYouHappen".Localized());

        InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.DrakeHeart), () => Scene.Go<CampScene>());
    }

    public static void LootObsidianShard()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileYoureMindingYourOwnBusiness".Localized());

        if (CoC.Instance.Player.Intelligence <= Random.Shared.Next(80))
        {
            Scene.OutputText("Common.Text.UnfortunatelyYouCutYourFingersOver".Localized());

            CoC.Instance.Player.HealthChange(-Math.Max(5, CoC.Instance.Player.MaxHealth / 50));
        }

        Scene.OutputText("Scenes.VolcanicCrag.LootObsidianShard.SharpShardWeapon".Localized());

        InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.ObsidianShard), () => Scene.Go<CampScene>());
    }

    public static void CragAprilFools()
    {
        CoC.Instance.Flags.Set(Flags.GameFlagIds.DlcAprilFools, 1);

        Scene.ClearOutput();

        Scene.OutputText("Common.Text.AShimmeringImageOfTheExtreme".Localized());

        Scene.Go<CampScene>();
    }
}
}