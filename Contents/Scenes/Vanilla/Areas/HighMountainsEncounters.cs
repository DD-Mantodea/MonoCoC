using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class HighMountainsEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:high_mountains",

            CommonEncounters.Table,
            // 原版 HighMountains.hx：D3 发现遭遇（lethicesKeep.discoverD3），LethicesKeepScene 已移植
            IEncounter.Create("d3", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) == 0, _ => SceneInstance<LethicesKeepScene>.Get().DiscoverD3()),

            IEncounter.Create("snowangel", _ => 1f, _ => Holidays.IsSaturnalia() && CoC.Instance.Player.Gender != Gender.Genderless && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelDisabled) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelGoodEnded) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GatsAngelQuestBegan) > 1 && !CoC.Instance.Player.HasKeyItem("North Star Key"), _ => Scene.Go<SnowAngelScene>()),

            IEncounter.Create("minerva", _ => 1f, _ => true, _ => Scene.Go<MinervaScene>()),

            IEncounter.Create("minomob", _ => CommonEncounters.FurriteMod(), _ => CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 3 && CoC.Instance.Player.HasVagina(), _ => Scene.Go<MinotaurMobScene>()),

            IEncounter.Create("harpychicken", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.TimesMetChickenHarpy) == 0 || CoC.Instance.Player.HasItem(ConsumableIds.OviElixir), _ => ChickenHarpyEncounter()),

            IEncounter.Create("phoenix", _ => 1f, _ => true, _ => Scene.Go<PhoenixScene>()),

            IEncounter.Create("minotaur", _ => CommonEncounters.FurriteMod(), _ => CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0, _ =>
            {
                var scene = SceneInstance<MinotaurScene>.Get();

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3)
                    scene.MinoAddictionFuck();
                else
                    scene.GetRapedByMinotaur(true);
            }),

            IEncounter.Create("harpy", _ => 1f, _ => true, context =>
            {
                Scene.ClearOutput();

                Scene.ClearButtons();

                context.Write("Areas.HighMountains.Encounter.Harpy".Localized());

                var harpy = EnemyRegistry.Create(EnemyIds.Harpy);

                harpy.OnDefeated = hpVictory => SceneInstance<HarpyScene>.Get().HarpyVictoryuuuuu();

                harpy.OnWon = _ => SceneInstance<HarpyScene>.Get().HarpyLossU();

                CombatScene.Start(harpy, context.ReturnToCamp);
            }),

            IEncounter.Create("basilisk", _ => 1f, _ => true, _ => Scene.Go<BasiliskScene>()),

            IEncounter.Create("cockatrice", _ => 1f, _ => true, _ => Scene.Go<CockatriceScene>()),

            IEncounter.Create("sophie", _ => 1f, _ => true, _ => SceneInstance<SophieScene>.Get().SophieRouter()),

            IEncounter.Create("nephilacoven", _ => 1f, _ => true, _ => SceneInstance<NephilaCovenScene>.Get().EncounterNephilaCoven()),

            IEncounter.Create("izumi", _ => 1f, _ => true, _ => Scene.Go<IzumiScene>()),

            IEncounter.Create("ebonbloom", _ => 0.1f, _ => true, _ => EbonbloomEncounter()),

            IEncounter.Create("coal", _ => 0.1f, _ => true, _ => MountainEncounters.FindCoal()),

            IEncounter.Create("hike", _ => 0.2f, _ => true, _ => HighMountainHike()),

            IEncounter.Create("wingedSpearScene", _ => 1f, _ => true, _ => Scene.Go<WingedSpearScene>()),

            IEncounter.Create("HighMountainsExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.HighMountains.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void HighMountainHike()
    {
        Scene.ClearOutput();

        if (CoC.Instance.Player.Corruption < 90)
        {
            Scene.OutputText("Scenes.HighMountains.Hike.FruitlessHike".Localized());

            CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, 0.25f), new DynStat(GameStats.Speed, 0.5f));
        }
        else
        {
            Scene.OutputText("Common.Text.DuringYourHikeIntoTheHigh".Localized());

            Scene.OutputText("Common.Text.PgItIsAMiracleNo".Localized());

            CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, 0.25f), new DynStat(GameStats.Speed, 0.5f), new DynStat(GameStats.Libido, 0.25f));
        }

        Scene.Go<CampScene>();
    }

    public static void EbonbloomEncounter()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileScalingTheSteepCliffsOf".Localized());

        Scene.OutputText("Common.Text.PgYourHandReachesInAnd".Localized());

        InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.EBNFLWR), () => Scene.Go<CampScene>());
    }

    public static void ChickenHarpyEncounter()
    {
        var first = CoC.Instance.Flags.Get(GameFlagIds.TimesMetChickenHarpy) == 0;

        CoC.Instance.Flags.Set(GameFlagIds.TimesMetChickenHarpy, CoC.Instance.Flags.Get(GameFlagIds.TimesMetChickenHarpy) + 1);

        Scene.ClearOutput();

        if (first)
        {
            Scene.OutputText("Scenes.HighMountains.ChickenHarpy.FirstMeetingCart".Localized());

            Scene.OutputText("Common.Text.PgTheChickenHarpyTellsYou".Localized());
        }
        else
            Scene.OutputText("Common.Text.TakingAStrollAlongTheMountains".Localized());

        Scene.ClearButtons();

        if (CoC.Instance.Player.Inventory.Count(ItemRegistry.Create(ConsumableIds.OviElixir)) >= 2)
            Scene.AddButton(0, "Common.Button.GiveTwo".Localized(), () => ChickenHarpyEggs(2));

        if (CoC.Instance.Player.Inventory.Count(ItemRegistry.Create(ConsumableIds.OviElixir)) >= 3)
            Scene.AddButton(1, "Common.Button.GiveThree".Localized(), () => ChickenHarpyEggs(3));

        Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scene.Go<CampScene>());
    }

    public static void ChickenHarpyEggs(int count)
    {
        CoC.Instance.Player.ConsumeItem(ConsumableIds.OviElixir, count);

        Scene.ClearOutput();

        Scene.OutputText(count == 2

            ? "You hand over two elixirs. The harpy unties a corner of the sheet atop the cart, revealing a collection of eggs."

            : "You hand over three elixirs. The harpy unties a side of the sheet, revealing a larger collection of eggs.");

        Scene.ClearButtons();

        var eggs = count == 2

            ? new[] { ConsumableIds.EggBlack, ConsumableIds.EggBlue, ConsumableIds.EggBrown, ConsumableIds.EggPink, ConsumableIds.EggPurple, ConsumableIds.EggWhite }

            : [ConsumableIds.EggLargeBlack, ConsumableIds.EggLargeBlue, ConsumableIds.EggLargeBrown, ConsumableIds.EggLargePink, ConsumableIds.EggLargePurple, ConsumableIds.EggLargeWhite];

        var names = new[] { "Black", "Blue", "Brown", "Pink", "Purple", "White" };

        for (var i = 0; i < eggs.Length; i++)
        {
            var egg = eggs[i];

            Scene.AddButton(i, names[i], () => ChickenHarpyTakeEgg(egg));
        }
    }

    public static void ChickenHarpyTakeEgg(Identifier egg)
    {
        CoC.Instance.Flags.Set(GameFlagIds.EggsBought, CoC.Instance.Flags.Get(GameFlagIds.EggsBought) + 1);

        InventoryScene.TakeItem(ItemRegistry.Create(egg), () => Scene.Go<CampScene>());
    }
}
}