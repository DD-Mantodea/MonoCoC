using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using MonoCoC.Contents.World;
using MonoCoC.Configuration;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class MountainEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:mountain",

            CommonEncounters.Table,

            IEncounter.Create("salon", _ => 1f, _ => true, _ => Scene.Go<Salon>()),

            IEncounter.Create(
                "highmountains",

                _ => 1f,

                _ => !AreaRegistry.Get(AreaIds.HighMountains)!.IsDiscovered(CoC.Instance.Player) &&

                     (CoC.Instance.Player.Level >= 5 || CoC.Instance.Flags.Get(GameFlagIds.TimesExploredMountain) >= 40),

                context =>
                {
                    var area = AreaRegistry.Get(AreaIds.HighMountains)!;

                    AreaRegistry.Discover(area);

                    context.Write(area.DiscoveryKey.Localized());

                    Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
                }),

            IEncounter.Create("snowangel", _ => 1f, _ => Holidays.IsSaturnalia() && CoC.Instance.Player.Gender != Gender.Genderless && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelDisabled) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelGoodEnded) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelQuestBegan) > 1 && !CoC.Instance.Player.HasKeyItem("North Star Key"), _ => Scene.Go<SnowAngelScene>()),

            IEncounter.Create("jackfrost", _ => 1f, _ => Holidays.IsSaturnalia() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostYear) < DateTime.Now.Year && OptionStore.Instance.Get("silly", 0) > 0, _ => Scene.Go<JackFrostScene>()),

            IEncounter.Create("hellhound", _ => CommonEncounters.FurriteMod(), _ => true, _ => Scene.Go<HellHoundScene>()),

            IEncounter.Create("infhhound", _ => CommonEncounters.FurriteMod(), _ => true, _ => Scene.Go<InfestedHellhoundScene>()),

            IEncounter.Create("worms1", _ => 1f, _ => true, _ => Scene.Go<WormsScene>()),

            IEncounter.Create("worms2", _ => 1f, _ => true, _ => Scene.Go<WormsScene>()),

            IEncounter.Create("minotaur", _ => MinotaurChance() * CommonEncounters.FurriteMod(), _ => true, _ => Scene.Go<MinotaurScene>()),

            IEncounter.Create("factory", _ => 1f, _ => true, _ => Scene.Go<FactoryScene>()),

            IEncounter.Create("ceraph", _ => 1f, _ => true, _ => CeraphFn()),

            IEncounter.Create("hhound_master", _ => 2f, _ => CoC.Instance.Flags.Get(GameFlagIds.HellhoundMasterProgress) <= 1 && CoC.Instance.Player.Corruption >= 60, _ => SceneInstance<HellHoundScene>.Get().HellHoundMasterEncounter()),

            IEncounter.Create("nephila", _ => 1f, _ => true, _ => Scene.Go<NephilaSlimeScene>()),

            IEncounter.Create("coal", _ => 0.05f, _ => true, _ => FindCoal()),

            IEncounter.Create("hike", _ => 0.2f, _ => true, _ => Hike()),

            IEncounter.Create("MountainExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.Mountain.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void FindCoal()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.TakingAMuchLessScenicRoute".Localized());

        InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.Coal), () => Scene.Go<CampScene>());
    }

    public static void Hike()
    {
        Scene.ClearOutput();

        if (CoC.Instance.Player.Corruption < 90)
        {
            Scene.OutputText("Scenes.Mountain.Hike.FruitlessVistas".Localized());

            CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, 0.25f), new DynStat(GameStats.Speed, 0.5f));
        }
        else
        {
            Scene.OutputText("Scenes.Mountain.Hike.DepravedReplay".Localized());

            Scene.OutputText("Common.Text.PgItIsAMiracleNo".Localized());

            CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, 0.25f), new DynStat(GameStats.Speed, 0.5f), new DynStat(GameStats.Libido, 0.25f));
        }

        Scene.Go<CampScene>();
    }

    // 原版 Mountain.hx minotaurChance()
    public static float MinotaurChance()
    {
        var player = CoC.Instance.Player;

        if (player.HasPerk(PerkIds.MinotaurCumAddict))
            return 3f;

        if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0)
            return 2f;

        return 1f;
    }

    // 原版 Mountain.hx ceraphFn()
    public static void CeraphFn()
    {
        var scene = SceneInstance<CeraphScene>.Get();

        if (scene.HasExhibition() && Random.Shared.Next(2) < 1)
            scene.FriendlyNeighborhoodSpiderManCeraph();
        else
            scene.EncounterCeraph();
    }
}
}