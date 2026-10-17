// 原版 classes/scenes/places/Swim.hx（extends AbstractLakeContent）
// 从 Boat 地点「Swim」按钮进入：Boat.hx:97-103 → BoatScene.cs AddButton("Swim")
// → SwimExplore()（Swim.hx:67-88）→ explorationEncounter（Swim.hx:30-64）。
using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Lake;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.Swim;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Lake
{
    // 原版 Swim 是地点场景（无自己菜单）：SwimExplore() 依次写入水状态、算污染、抽遭遇。
    [Register("coc:swim")]
    public class SwimScene : Scene
    {
        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static void ReturnToCamp(int hours) => Camp.ReturnToCamp(hours);

        public override void Entry() => SwimExplore();

        // 原版 player.isFullMermaid()（Creature.hx:3465）：下半身 mermaid 且 gills 也是 mermaid。
        private static bool IsFullMermaid =>
            Player.LowerBodyType().GetString("body") == "mermaid"
            && Player.Body.Get<GillsPart>().Id == Identifier.Parse("coc:mermaid");

        // Haxe SwimExplore()   outputText 1
        public void SwimExplore()
        {
            ClearOutput();

            Player.Location = "swim";

            OutputText("Scenes.Swim.SwimExplore.GillsInvigorateDive".Localized());

            // 原版 Swim.hx:72-77 createOrFindStatusEffect(MermaidWatered/Dehydrated).increase()
            // 说明：C# 只有状态 id（VanillaStatusEffects.MermaidWatered/Dehydrated），没有原版 TemporaryBuff
            // 基础设施（MermaidWateredBuff.hx:4-19 的 Spe ± 值与 Player.hx:3784-3785 的 maxSpe 计入、
            // Camp.hx:174-175 的离水移除）。这里只保证状态存在，数值效果未接，已写入任务汇报。
            var status = IsFullMermaid ? VanillaStatusEffects.MermaidWatered : VanillaStatusEffects.MermaidDehydrated;

            if (!Player.HasStatusEffect(status))
                Player.CreateStatusEffect(status);

            Player.HasGottenWashed();

            // 原版 Swim.hx:79-85：按工厂状态给腐蚀
            var shutdown = CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown);

            if (shutdown == 1)
                Player.ApplyStats(new DynStat(VanillaStats.Corruption, 0.1f));
            else if (shutdown == 2)
                Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            else
                Player.ApplyStats(new DynStat(VanillaStats.Corruption, 0.5f));

            ExplorationEncounter.Execute(new EncounterContext
            {
                Player = Player,
                Flags = CoC.Instance.Flags,
                Time = View.Time,
                Random = Random.Shared,
                Write = OutputText,
                ReturnToCamp = () => ReturnToCamp(1),
                ReturnToCampAfter = ReturnToCamp
            });
        }

        // 原版 Swim.hx:30-64：swim 区域的 explorationEncounter 组。
        // 顺序照原版：common → jellyfishgirl → sharkgirl → sharkman → anemone → nothing → lightrailavenger。
        // 懒初始化：条目里要调场景实例方法（NothingSpecial/FindLightRailAvenger），C# 静态字段初始化器不允许。
        private static IEncounter? _explorationEncounter;

        public static IEncounter ExplorationEncounter => _explorationEncounter ??= BuildExplorationEncounter();

        private static IEncounter BuildExplorationEncounter() => IEncounter.Group(
            "coc:swim",
            CommonEncounters.Table,
            // 原版 Swim.hx:31-34 jellyFishGirlScene.jellyFishGirlEncounter
            IEncounter.Create("jellyfishgirl", _ => 1f, _ => true,
                _ => Go(SceneRegistry.CreateOrDefault("coc:jelly_fish_girl_scene"))),
            // 原版 Swim.hx:35-39：chance 0.5，sharkGirlEncounter.bind(1)
            IEncounter.Create("sharkgirl", _ => 0.5f, _ => true,
                _ => Go(SceneRegistry.CreateOrDefault("coc:shark_girl_scene"))),
            // 原版 Swim.hx:40-46：需已遇到 SharkGirl 且 SharkManScene 尚未 encountered；
            // chance = 0.1 + (izmaScene.saveContent.sharkManTalk ? 0.1 : 0)
            IEncounter.Create("sharkman",
                _ => 0.1f + (SaveContent(new Identifier("coc", "IzmaScene")).GetBool("sharkManTalk") ? 0.1f : 0f),
                p => p.HasStatusEffect(VanillaStatusEffects.SharkGirl)
                    && !SceneInstance<SharkManScene>.Get().Encountered,
                _ => Go(SceneRegistry.CreateOrDefault("coc:shark_man_scene"))),
            // 原版 Swim.hx:47-50 game.anemoneScene.mortalAnemoneeeeee：AnemoneScene 未移植
            IEncounter.Disabled("anemone", "待 AnemoneScene 移植（Swim.hx:47-50）"),
            IEncounter.Create("nothing", _ => 1f, _ => true,
                _ => SceneInstance<SwimScene>.Get().NothingSpecial()),
            // 原版 Swim.hx:55-62：silly 且尚未拿到 Light Rail Avenger 时 5%
            IEncounter.Create("lightrailavenger", _ => 0.05f,
                p => CoC.Instance.Flags.Get(VanillaFlags.GrabbedLightRailAvenger) != 1 && silly,
                _ => SceneInstance<SwimScene>.Get().FindLightRailAvenger()));

        // 原版 Swim.hx:90-98
        public void FindLightRailAvenger()
        {
            OutputText("Scenes.Swim.FindLightRailAvenger.LakeCenterExhaustion".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger.GlintAtBottom".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger.InvestigateGlint".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger.DiveDownDecision".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger.DeepUnderwaterObject".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger.RetrieveSword".Localized());
            FindLightRailAvenger2();
        }

        // 原版 Swim.hx:100-108
        public void FindLightRailAvenger2()
        {
            ClearOutput();

            OutputText("Scenes.Swim.FindLightRailAvenger2.SurfaceWithSword".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger2.KatanaDescription".Localized());
            OutputText("Scenes.Swim.FindLightRailAvenger2.DullEdgeless".Localized());
            ClearButtons();
            AddButton(0, "Take Sword", TakeSword);
            AddButton(1, "Leave", () => ReturnToCamp(1));
        }

        // 原版 Swim.hx:110-115
        public void TakeSword()
        {
            ClearOutput();

            OutputText("Scenes.Swim.TakeSword.TakeSwordDecoration".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.GrabbedLightRailAvenger, 1);
            InventoryScene.TakeItem(ItemRegistry.Create(VanillaItems.LightRailAvenger), () => ReturnToCamp(1));
        }

        // 原版 Swim.hx:117-124
        public void NothingSpecial()
        {
            ShowImage("location-swim");
            OutputText("Scenes.Swim.NothingSpecial.YouSwimForNearly".Localized()
                RandomUtils.Choice(
            ["You swim for nearly an hour, until your limbs practically burn with exhaustion.",
            "You give up on finding anything interesting, and decide to go check up on your camp."]
        )
            );
            ReturnToCamp(1);
        }
    }
}
