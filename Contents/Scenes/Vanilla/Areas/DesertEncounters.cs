using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Desert;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class DesertEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:desert",

            CommonEncounters.Table,

            IEncounter.Create("mermaidExplore", _ => 1f, _ => MermaidWalkScene.Available(), _ => MermaidWalkScene.Run("desert")),

            IEncounter.Create("coc:naga", _ => 1f, _ => true, _ => Scene.Go<NagaScene>()),

            IEncounter.Create("sandtrap", _ => 1f, _ => true, _ => Scene.Go<SandTrapScene>()),
            // 原版 Desert.hx:171 + ScorpionScene.hx encounterChance()：与马里埃尔聊出感情后才出现
            IEncounter.Create("scorpion",

                _ => (Bog.MarielleScene.Save.GetInt("talks", 0)

                        & Bog.MarielleScene.TALK_FONDNESS) != 0

                    && ScorpionScene.Save.GetInt("fate", 0) == 0

                    && ScorpionScene.Save.GetInt("state", 0) > -1

                        ? 1f

                        : 0f,

                _ => true,

                _ => Scene.Go<ScorpionScene>()),

            IEncounter.Create("sandwitch", _ => 1f, _ => true, _ => Scene.Go<SandWitchScene>()),

            IEncounter.Create("cumwitch", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.CumWitchesFightable) > 0, _ => CombatScene.Start(EnemyRegistry.Create(EnemyIds.CumWitch), () => Scene.Go<CampScene>())),

            IEncounter.Create("wanderer", _ => 1f, _ => true, _ => Scene.Go<Wanderer>()),

            IEncounter.Create("sw_preg", _ => 1f, _ => CoC.Instance.Player.Pregnancy.Event == 2, _ => DesertSandWitchPregnancy()),

            IEncounter.Create("teladre", _ => 1f, _ => true, _ => Scene.Go<TelAdre>()),

            IEncounter.Create("ants", _ => 1f, _ => true, _ => Scene.Go<AntsScene>()),

            IEncounter.Create("dungeon", _ => 1f, _ => true, _ => Contents.Dungeons.DungeonScene.Enter(SceneIds.Dungeon.DesertCave)),

            IEncounter.Create("wstaff", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.FoundWizardStaff) == 0 && CoC.Instance.Player.Intelligence > 50, _ => WizardStaffEncounter()),

            IEncounter.Create("nails", _ => 1f, _ => CoC.Instance.Player.HasKeyItem("Carpenter's Toolbox") && CoC.Instance.Player.KeyItemValue("Carpenter's Toolbox", 1) < CampScene.MaxNailSupply(), _ => NailsEncounter()),

            IEncounter.Create("chest", _ => 0.1f, _ => !CoC.Instance.Player.HasKeyItem("Camp - Chest"), _ => DesertChest()),

            Forest.GoblinSharpshooterScene.Encounter,

            IEncounter.Create("bigjunk", _ => CommonEncounters.BigJunkChance(), _ => CommonEncounters.BigJunkChance() > 0,

                _ => CommonEncounters.BigJunkDesertScene()),

            IEncounter.Create("exgartuan", _ => 1f, _ => true, _ => Scene.Go<ExgartuanScene>()),

            IEncounter.Create("mirage", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.DesertFoundMirage) == 0, _ => DesertMirage()),

            IEncounter.Create("mirage", _ => 1f, _ => CoC.Instance.Flags.Get(GameFlagIds.DesertFoundMirage) == 0, _ => DesertMirage()),

            IEncounter.Create("coc:oasis_shaman", _ => 1f, _ => true, _ => Scene.Go<OasisShamanScene>()),

            IEncounter.Create("DesertExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.Desert.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void DesertWalk()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.YouWalkThroughTheShiftingSands".Localized());

        if (Random.Shared.Next(2) == 0)
            if (Random.Shared.Next(2) == 0 && CoC.Instance.Player.Strength < 50)
            {
                Scene.OutputText("Scenes.Desert.WalkingDesertStatBoost.GainedStrength".Localized());

                CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));
            }
            else if (CoC.Instance.Player.Toughness < 50)
            {
                Scene.OutputText("Common.Text.TheEffortOfStrugglingWithThe".Localized());

                CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Toughness, 0.5f));
            }

        Scene.Go<CampScene>();
    }

    public static void DesertChest()
    {
        Scene.ClearOutput();

        Scene.OutputText("Scenes.Desert.ChestEncounter.ChestFound".Localized());

        CoC.Instance.Player.CreateKeyItem("Camp - Chest");

        Scene.OutputText("Scenes.Desert.ChestEncounter.ChestKeyItem".Localized());

        Scene.Go<CampScene>();
    }

    public static void WizardStaffEncounter()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileExploringTheDesertYouSee".Localized());

        Scene.OutputText("Common.Text.PgTheSourceOfTheInferno".Localized());

        CoC.Instance.Flags.Set(GameFlagIds.FoundWizardStaff, CoC.Instance.Flags.Get(GameFlagIds.FoundWizardStaff) + 1);

        InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.WizardStaff), () => Scene.Go<CampScene>());
    }

    public static void NailsEncounter()
    {
        var player = CoC.Instance.Player;

        var extracted = 5 + Random.Shared.Next((int)player.Intelligence / 5) + Random.Shared.Next((int)player.Strength / 10) + Random.Shared.Next((int)player.Toughness / 10) + Random.Shared.Next((int)player.Speed / 20) + 5;

        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileExploringTheDesertYouFind".Localized());

        Scene.OutputText("[pg]You take your hammer out of your toolbox and spend some time extracting straight nails. After nearly an hour, you've managed to extract " + extracted + "Common.Text.Nails".Localized());

        CoC.Instance.Flags.Set(GameFlagIds.AchievementProgressScavenger, CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressScavenger) + extracted);

        player.AddKeyValue("Carpenter's Toolbox", 1, extracted);

        var max = CampScene.MaxNailSupply();

        if (player.KeyItemValue("Carpenter's Toolbox", 1) > max)
            player.AddKeyValue("Carpenter's Toolbox", 1, -(int)(player.KeyItemValue("Carpenter's Toolbox", 1) - max));

        Scene.OutputText("Common.Text.PgNails".Localized() + player.KeyItemValue("Carpenter's Toolbox", 1) + "/" + max);

        Scene.Go<CampScene>();
    }

    public static void DesertMirage()
    {
        CoC.Instance.Flags.Set(GameFlagIds.DesertFoundMirage, 1);

        Scene.ClearOutput();

        Scene.OutputText("Scenes.Desert.MirageDesert.ShimmeringTower".Localized());

        CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Lust, -15f));

        Scene.Go<CampScene>();
    }

    public static void DesertSandWitchPregnancy()
    {
        var scene = SceneInstance<SandWitchScene>.Get();

        if (CoC.Instance.Flags.Get(GameFlagIds.EggWitchType) == 15)
            scene.SammitchBirthsDriders();
        else
            scene.WitchBirfsSomeBees();
    }
}
}