using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla.Places.Swim;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Lake
{

    [Register<SwimScene>("coc:swim")]
    public class SwimScene : Scene
    {

        public override void Entry() => SwimExplore();

        public static bool IsFullMermaid =>

            Player.LowerBodyType().GetString("body") == "mermaid"

            && Player.Body.Get<GillsPart>().Id == BodyPartTypes.Mermaid;

        public void SwimExplore()
        {
            ClearOutput();

            Player.Location = "swim";

            OutputText("Scenes.Swim.SwimExplore.GillsInvigorateDive".Localized());

            var status = IsFullMermaid ? StatusEffectIds.MermaidWatered : StatusEffectIds.MermaidDehydrated;

            if (!Player.HasStatusEffect(status))
                Player.CreateStatusEffect(status);

            Player.HasGottenWashed();

            var shutdown = CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown);

            if (shutdown == 1)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 0.1f));
            else if (shutdown == 2)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            else
                Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

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

        public static IEncounter? _explorationEncounter;

        public static IEncounter ExplorationEncounter => _explorationEncounter ??= BuildExplorationEncounter();

        public static IEncounter BuildExplorationEncounter() => IEncounter.Group(
            "coc:swim",

            CommonEncounters.Table,

            IEncounter.Create("jellyfishgirl", _ => 1f, _ => true,

                _ => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.JellyFishGirlScene>.Get())),

            IEncounter.Create("sharkgirl", _ => 0.5f, _ => true,

                _ => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Boat.SharkGirlScene>.Get())),

            IEncounter.Create("sharkman",

                _ => 0.1f + (SaveContent(SceneIds.Lake.Swim).GetBool("sharkManTalk") ? 0.1f : 0f),

                p => p.HasStatusEffect(StatusEffectIds.SharkGirl)

                    && !SceneInstance<SharkManScene>.Get().Encountered,

                _ => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.SharkManScene>.Get())),

            // 原版 Swim.hx:47-50 anemone：AnemoneScene.mortalAnemoneeeeee
            IEncounter.Create("anemone", _ => 1f, _ => true,

                _ => SceneInstance<AnemoneScene>.Get().MortalAnemoneeeeee()),

            IEncounter.Create("nothing", _ => 1f, _ => true,

                _ => SceneInstance<SwimScene>.Get().NothingSpecial()),

            IEncounter.Create("lightrailavenger", _ => 0.05f,

                p => CoC.Instance.Flags.Get(GameFlagIds.GrabbedLightRailAvenger) != 1 && silly,

                _ => SceneInstance<SwimScene>.Get().FindLightRailAvenger()));

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

        public void FindLightRailAvenger2()
        {
            ClearOutput();

            OutputText("Scenes.Swim.FindLightRailAvenger2.SurfaceWithSword".Localized());

            OutputText("Scenes.Swim.FindLightRailAvenger2.KatanaDescription".Localized());

            OutputText("Scenes.Swim.FindLightRailAvenger2.DullEdgeless".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeSword".Localized(), TakeSword);

            AddButton(1, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void TakeSword()
        {
            ClearOutput();

            OutputText("Scenes.Swim.TakeSword.TakeSwordDecoration".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GrabbedLightRailAvenger, 1);

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.LightRailAvenger), () => ReturnToCamp(1));
        }

        public void NothingSpecial()
        {
            ShowImage("location-swim");

            OutputText("Scenes.Swim.NothingSpecial.YouSwimForNearly".Localized(
                RandomUtils.Choice(
            ["You swim for nearly an hour, until your limbs practically burn with exhaustion.",
            "You give up on finding anything interesting, and decide to go check up on your camp."]
        )
            ));

            ReturnToCamp(1);
        }
    }
}