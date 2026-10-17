using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class GlacialRiftEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:glacial_rift",

            CommonEncounters.Table,

            IEncounter.Create("aprilfools", _ => 1f, _ => IsAprilFools() && CoC.Instance.Flags.Get(GameFlagIds.DlcAprilFools) == 0, _ => RiftAprilFools()),

            IEncounter.Create("valkyrie", _ => 1f, _ => true, context =>
            {
                Scene.ClearOutput();

                Scene.ClearButtons();

                context.Write("Areas.GlacialRift.Encounter.Valkyrie".Localized());

                var enemy = EnemyRegistry.Create(EnemyIds.Valkyrie);

                enemy.OnDefeated = hpVictory => SceneInstance<ValkyrieScene>.Get().WinAgainstValkyrie(hpVictory);

                enemy.OnWon = _ => SceneInstance<ValkyrieScene>.Get().LoseToValkyrie();

                CombatScene.Start(enemy, context.ReturnToCamp);
            }),

            IEncounter.Create("yeti", _ => 1f, _ => true, context =>
            {
                Scene.ClearOutput();

                Scene.ClearButtons();

                context.Write("Areas.GlacialRift.Encounter.Yeti.Intro".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MetYetiFirstTime) <= 0)
                {
                    context.Write("Areas.GlacialRift.Encounter.Yeti.Caverns".Localized());

                    if (OptionStore.Instance.Get("silly", 0) > 0)
                        context.Write("Areas.GlacialRift.Encounter.Yeti.CavernsSilly".Localized());

                    context.Write("Areas.GlacialRift.Encounter.Yeti.CavernsEnd".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MetYetiFirstTime, 1);
                }
                else
                    context.Write("Areas.GlacialRift.Encounter.Yeti.Repeat".Localized());

                context.Write("Areas.GlacialRift.Encounter.Yeti.Ready".Localized());

                if (!context.Player.IsUnarmed())
                    context.Write("Areas.GlacialRift.Encounter.Yeti.ReadyArmed".Localized());

                context.Write("Areas.GlacialRift.Encounter.Yeti.Charge".Localized());

                var yeti = EnemyRegistry.Create(EnemyIds.Yeti);

                yeti.OnDefeated = hpVictory => SceneInstance<YetiScene>.Get().WinAgainstYeti();

                yeti.OnWon = _ => SceneInstance<YetiScene>.Get().LoseToYeti();

                CombatScene.Start(yeti, context.ReturnToCamp);
            }),

            IEncounter.Create("amarok", _ => 1f, _ => true, context =>
            {
                Scene.ClearOutput();

                Scene.ClearButtons();

                context.Write("Areas.GlacialRift.Encounter.Amarok".Localized());

                var amarok = EnemyRegistry.Create(EnemyIds.Amarok);

                amarok.OnDefeated = hpVictory => SceneInstance<AmarokScene>.Get().WinAgainstAmarok(hpVictory);

                CombatScene.Start(amarok, context.ReturnToCamp);
            }),

            IEncounter.Create("frostgiant", _ => 1f, _ => true, context =>
            {
                Scene.ClearOutput();

                Scene.ClearButtons();

                context.Write("Areas.GlacialRift.Encounter.FrostGiant".Localized());

                var giant = EnemyRegistry.Create(EnemyIds.FrostGiant);

                giant.OnDefeated = hpVictory => SceneInstance<FrostGiantScene>.Get().WinAgainstGiant();

                giant.OnWon = _ => CombatScene.CleanupAfterCombat();

                CombatScene.Start(giant, context.ReturnToCamp);
            }),

            IEncounter.Create("mora", _ => 1f, _ => true, _ => Scene.Go<MoraScene>()),

            IEncounter.Create("valeria", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.ValeriaFoundInGlacialRift) == 0, _ => SceneInstance<ValeriaScene>.Get().ValeriaAtRift()),

            IEncounter.Create("loot", _ => 0.3f, _ => true, _ => GlacialLoot()),

            IEncounter.Create("ornatechest", _ => 1f, _ => !CoC.Instance.Player.HasKeyItem("Camp - Ornate Chest"), _ => FindOrnateChest()),

            IEncounter.Create("riftCabinScene", _ => 1f, _ => true, _ => Scene.Go<RiftCabinScene>()),

            IEncounter.Create("walk", _ => 1f, _ => true, _ => GlacialWalk()),

            IEncounter.Create("GlacialRiftExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.GlacialRift.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void GlacialWalk()
    {
        Scene.ClearOutput();

        Scene.OutputText("Scenes.GlacialRift.Walk.FruitlessHour".Localized());

        CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, -0.5f), new DynStat(GameStats.Lust, -0.5f));

        Scene.Go<CampScene>();
    }

    public static void FindOrnateChest()
    {
        var gems = 400 + Random.Shared.Next(400);

        Scene.ClearOutput();

        Scene.OutputText("Scenes.GlacialRift.FindChest.ChestSpotted".Localized());

        Scene.OutputText("[pg]As you get closer to the object, you make it out to be an ornately decorated chest halfway buried in the snow and underlying layer. After hacking away at the permanently frozen soil, you free the chest and shatter the ice on the hinges to open it. Inside of it you find several pouches filled with gems, a nice " + gems + "Common.Text.InTotal".Localized());

        CoC.Instance.Player.Gems += gems;

        CoC.Instance.Player.CreateKeyItem("Camp - Ornate Chest");

        Scene.OutputText("Common.Text.PgDecidingThatBesidesTheGems".Localized());

        Scene.Go<CampScene>();
    }

    public static bool IsAprilFools()
    {
        var now = DateTime.Now;

        return now.Month == 4 && now.Day == 1;
    }

    public static void RiftAprilFools()
    {
        CoC.Instance.Flags.Set(GameFlagIds.DlcAprilFools, 1);

        Scene.ClearOutput();

        Scene.OutputText("Common.Text.AStrangeRippleRunsThroughThe".Localized());

        Scene.Go<CampScene>();
    }

    public static void GlacialLoot()
    {
        Scene.ClearOutput();

        if (Random.Shared.Next(2) == 0)
        {
            Scene.OutputText("Common.Text.AsYouCrossOneOfThe".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.Icicle), () => Scene.Go<CampScene>());
        }
        else
        {
            Scene.OutputText("Common.Text.AsYouMakeYourWayAcross2".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.GodMead), () => Scene.Go<CampScene>());
        }
    }
}
}