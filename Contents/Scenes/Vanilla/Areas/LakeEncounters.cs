using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Lake;
using MonoCoC.Contents.Scenes.Vanilla.Places;
using MonoCoC.Contents.Scenes.Vanilla.Places.Lake;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class LakeEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:lake",

            CommonEncounters.Table,

            IEncounter.Create("fetish_cultist", _ => 1f, _ => true, _ => Scene.Go<FetishCultistScene>()),

            IEncounter.Create("goo_girl", _ => 1f, _ => true, _ => Scene.Go<GooGirlScene>()),

            IEncounter.Create("egg", _ => 2.5f, _ => CoC.Instance.Player.Pregnancy.Type == PregnancyIds.OvielixirEggs, _ => LakeEggChooser()),

            IEncounter.Create("callu", _ => 1f, _ => true, _ => Scene.Go<CalluScene>()),

            IEncounter.Create("CoC.Instance.latexGirl.lakeDiscovery", _ => 0.3f, _ => true, _ => Scene.Go<LatexGirlScene>()),

            IEncounter.Create("CoC.Instance.izmaScene", _ => 1f, _ => true, _ => Scene.Go<IzmaScene>()),

            IEncounter.Create("CoC.Instance.rathazul", _ => 1f, _ => true, _ => Scene.Go<RathazulScene>()),

            IEncounter.Create("CoC.Instance.aprilFools.poniesEncounter", _ => 0.25f,

                _ => CoC.Instance.Flags.Get(Flags.GameFlagIds.SillyModePonies) == 0

                    && CoC.Instance.Player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed

                    && CoC.Instance.Player.IsTaur()

                    && AprilFoolsPoniesScene.IsAprilFools(),

                _ => Scene.Go<AprilFoolsPoniesScene>()),

            IEncounter.Create("boat", _ => 1f, _ => !SceneInstance<BoatScene>.Get().IsDiscovered(), _ => SceneInstance<BoatScene>.Get().DiscoverBoat()),

            IEncounter.Create("townruins", _ => 1f, _ => !SceneInstance<TownRuinsScene>.Get().IsDiscovered(), _ => SceneInstance<TownRuinsScene>.Get().DiscoverAmilyVillage()),

            IEncounter.Create("cultistNun", _ => 1f, _ => true, _ => Scene.Go<CultistNunScene>()),

            IEncounter.Create("swordInStone", _ => 1f, _ => true, _ => Scene.Go<SwordInStone>()),

            IEncounter.Create("bigjunk", _ => CommonEncounters.BigJunkChance(), _ => CommonEncounters.BigJunkChance() > 0,

                _ => CommonEncounters.BigJunkForestScene(lake: true)),

            IEncounter.Create("loot", _ => 1f, _ => true, _ => LakeLoot()),
            // 原版 Lake.hx farmEncounterAvailable()：没遇到惠特尼农场、或只遇到过一次时才出现
            IEncounter.Create("farm", _ => 1f,

                _ => !CoC.Instance.Player.HasStatusEffect(StatusEffectIds.MetWhitney)

                    || CoC.Instance.Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) <= 1,

                _ => Scene.Go<FarmScene>()),

            IEncounter.Create("walk", _ => 1f, _ => true, _ => LakeWalk()),

            IEncounter.Create("LakeExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.Lake.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void LakeWalk()
    {
        Scene.ClearOutput();

        if (CoC.Instance.Player.Level < 2 || CoC.Instance.Player.Speed < 50)
        {
            Scene.OutputText("Scenes.Lake.LakeWalk.LakeshoreWalk".Localized());

            if (CoC.Instance.Player.Speed < 50)
            {
                Scene.OutputText("Common.Text.YouBetYouCouldCoverThe".Localized());

                CoC.Instance.Player.ApplyStats(new DynStat(GameStats.Speed, 0.75f));
            }
        }
        else
        {
            Scene.OutputText("Common.Text.YourStrollAroundTheLakeIncreasingly".Localized());

            if (CoC.Instance.Player.Corruption >= 60 || CoC.Instance.Player.LustPercent >= 90 || CoC.Instance.Player.Libido >= 75)
                Scene.OutputText("Scenes.Lake.LakeWalk.RaunchyDaydreams".Localized());
        }

        Scene.Go<CampScene>();
    }

    public static void LakeLoot()
    {
        Scene.ClearOutput();

        if (Random.Shared.Next(2) == 0)
        {
            Scene.OutputText("Common.Text.YouFindALongAndOddly".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.Equinum), () => Scene.Go<CampScene>());
        }
        else
        {
            Scene.OutputText("Common.Text.YouFindAnOddFruitBearing".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.W_FRUIT), () => Scene.Go<CampScene>());
        }
    }

    public static void LakeEggChooser()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.WhileWanderingAlongTheLakeshoreYou".Localized());

        Scene.AddButton(0, "Common.Button.Blue".Localized(), () => LakeEggChoose(2));

        Scene.AddButton(1, "Common.Button.Pink".Localized(), () => LakeEggChoose(3));

        Scene.AddButton(2, "Common.Button.White".Localized(), () => LakeEggChoose(4));

        Scene.AddButton(3, "Common.Button.Black".Localized(), () => LakeEggChoose(5));

        Scene.AddButton(4, "Common.Button.Purple".Localized(), () => LakeEggChoose(1));

        Scene.AddButton(5, "Common.Button.Brown".Localized(), () => LakeEggChoose(0));

        Scene.AddButton(14, "Common.Button.Escape".Localized(), () => LakeEggEscape());
    }

    public static void LakeEggChoose(int eggType)
    {
        var colors = new[] { "brown", "purple", "blue", "pink", "white", "black" };

        Scene.ClearOutput();

        Scene.OutputText("You reach out and touch the " + colors[eggType] + "Scenes.Lake.EggChoose.LightFlowsIn".Localized());

        CoC.Instance.Player.ChangeStatusValue(StatusEffectIds.Eggs, 1, eggType);

        Scene.Go<CampScene>();
    }

    public static void LakeEggEscape()
    {
        Scene.ClearOutput();

        Scene.OutputText("Scenes.Lake.EggChooseEscape.RollAndFlee".Localized());

        Scene.Go<CampScene>();
    }
}
}