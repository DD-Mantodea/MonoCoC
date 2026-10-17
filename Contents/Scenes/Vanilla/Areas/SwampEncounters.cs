using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{

    public static class SwampEncounters
    {
        public static readonly IEncounter Table = IEncounter.Group(
            "coc:swamp",

            CommonEncounters.Table,

            IEncounter.Create(
                "bog",

                _ => 1f,

                _ => CoC.Instance.Flags.Get(GameFlagIds.TimesExploredSwamp) >= 25 &&

                     !AreaRegistry.Get(AreaIds.Bog)!.IsDiscovered(CoC.Instance.Player),

                context =>
                {
                    var area = AreaRegistry.Get(AreaIds.Bog)!;

                    AreaRegistry.Discover(area);

                    context.Write(area.DiscoveryKey.Localized());

                    Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
                }),
            // 原版 Swamp.hx:54 kihaxhel：Kiha 与 Hel 的沼泽遭遇（kihaXSalamander）
            IEncounter.Create("kihaxhel", _ => 1f, _ =>
            {
                var flags = CoC.Instance.Flags;

                var player = CoC.Instance.Player;

                return flags.Get(GameFlagIds.KihaKilled) == 0

                    && !SceneInstance<KihaFollowerScene>.Get().FollowerKiha()

                    && player.Corruption < 60

                    && flags.Get(GameFlagIds.KihaAffectionLevel) >= 1

                    && flags.Get(GameFlagIds.HelFuckbuddy) > 0

                    && player.HasCock()

                    && flags.Get(GameFlagIds.KihaAndHelWhoopie) == 0;
            }, _ => SceneInstance<KihaFollowerScene>.Get().KihaXSalamander()),

            IEncounter.Create("ember", _ => 1f, _ => true, _ => Scene.Go<EmberScene>()),

            Forest.GoblinSharpshooterScene.Encounter,

            IEncounter.Create("rogar", _ => 1f, _ => true, _ => Scene.Go<RogarScene>()),

            IEncounter.Create("mspider", _ => 1f, _ => true, _ => Scene.Go<MaleSpiderMorphScene>()),

            IEncounter.Create("fspider", _ => 1f, _ => true, _ => Scene.Go<FemaleSpiderMorphScene>()),

            IEncounter.Create("drider", _ => 1f, _ => true, _ => Scene.Go<CorruptedDriderScene>()),

            IEncounter.Create("kiha", _ => 1f, _ => true, _ => SceneInstance<KihaScene>.Get().EncounterKiha()),

            IEncounter.Create("coc:alraune", _ => 1f, _ => true, _ => Scene.Go<AlrauneScene>()),

            IEncounter.Create("walk", _ => 1f, _ => true, _ => SwampWalk()),

            IEncounter.Create("SwampExplore", _ => 1f, _ => true, context =>
            {
                context.Write("Areas.Swamp.Explore".Localized());

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
            }));

    public static void SwampWalk()
    {
        Scene.ClearOutput();

        Scene.OutputText("Common.Text.YouWanderThroughTheHumidSwamp".Localized());

        Scene.Go<CampScene>();
    }
}
}