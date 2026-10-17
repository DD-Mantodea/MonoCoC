using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Internals;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Lake
{
    [Register<BoatScene>("coc:boat")]
    public class BoatScene : Scene
    {
        // 原版 Boat.hx:38-78 explorationEncounter：划船时按权重抽取的遭遇组
        public static readonly IEncounter ExplorationEncounter = IEncounter.Group(
            "boat",

            CommonEncounters.Table,

            IEncounter.Create("izmakids", _ => 0.1f,

                _ => CoC.Instance.Flags.Get(GameFlagIds.IzmaKidsInWild) > 0

                    && SceneInstance<Npcs.IzmaScene>.Get().IzmaFollower(),

                _ => SceneInstance<Npcs.IzmaScene>.Get().FindLostIzmaKids()),

            Boat.MaraeScene.EncounterObject,

            IEncounter.Create("nothing", _ => 0.5f, _ => true, _ => SceneInstance<BoatScene>.Get().NothingSpecial()),

            IEncounter.Create("sharkgirl", _ => 1f, _ => true,

                _ => SceneInstance<Boat.SharkGirlScene>.Get().SharkGirlEncounter(1)),

            IEncounter.Create("zealot", _ => 0.8f,

                _ => CoC.Instance.Player.Level >= 2

                    && CoC.Instance.Player.HasStatusEffect(StatusEffectIds.FetishOn),

                _ => SceneInstance<Areas.Lake.FetishZealotScene>.Get().ZealotBoat()),

            IEncounter.Create("anemone", _ => 1f, _ => true,

                _ => SceneInstance<Npcs.AnemoneScene>.Get().MortalAnemoneeeeee()),

            IEncounter.Create("lightrailavenger", _ => 0.05f,

                _ => CoC.Instance.Flags.Get(GameFlagIds.GrabbedLightRailAvenger) != 1 && silly,

                _ => SceneInstance<BoatScene>.Get().FindLightRailAvenger()),

            IEncounter.Create("kaiju", _ => Boat.KaijuScene.EncounterChance(),

                _ => true, _ => Go<Boat.KaijuScene>()));

        public override void Entry()
        {
            IsDiscovered();
        }

        public bool IsDiscovered()
        {

            return Player.HasStatusEffect(StatusEffectIds.BoatDiscovery);
        }

        public void DiscoverBoat()
        {
            ClearOutput();

            Player.CreateStatusEffect(StatusEffectIds.BoatDiscovery, 0, 0, 0, 0);

            OutputText("Scenes.Boat.DiscoverBoat.LakeJourneyFindBoat".Localized());

            OutputText("Scenes.Boat.DiscoverBoat.BoatAddedToPlaces".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BoatExplore()
        {
            ClearOutput();

            Player.Location = "boat";

            Player.ChangeStatusValue(StatusEffectIds.BoatDiscovery, 0, 1);

            OutputText("Scenes.Boat.BoatExplore.ReachDockBoard".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 0)
                OutputText("Scenes.Boat.BoatExplore.ToxicWaterNote".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                {
                    OutputText("Scenes.Boat.BoatExplore.MuddyFaintOdor".Localized());

                    if (Player.Intelligence > 40)
                        OutputText("Scenes.Boat.BoatExplore.SmellsLikeSex".Localized());
                }
                else
                    OutputText("Scenes.Boat.BoatExplore.NeedsPurifying".Localized());

            if (Player.HasGills())
            {
                ClearButtons();

                AddButton(0, "Common.Button.Row".Localized(), GoRowOnTheBoat);

                AddButton(1, "Common.Button.Swim".Localized(), null );
            }
            else
                GoRowOnTheBoat();
        }

        public void GoRowOnTheBoat()
        {

            if (Player.LowerBodyType().GetString("body") == "mermaid")
            {
                OutputText("Scenes.Boat.GoRowOnTheBoat.WarySetOut".Localized());

                Player.CreateStatusEffect(StatusEffectIds.MermaidWatered);
            }
            else
                OutputText("Scenes.Boat.GoRowOnTheBoat.RowOutWondering".Localized());

            var context = new EncounterContext
            {
                Player = Player,

                Flags = CoC.Instance.Flags,

                Time = View.Time,

                Random = Random.Shared,

                Write = OutputText,

                ReturnToCamp = () => ReturnToCamp(1),

                ReturnToCampAfter = ReturnToCamp
            };

            ExplorationEncounter.Execute(context);
        }

        public void FindLightRailAvenger()
        {

            OutputText("Scenes.Boat.FindLightRailAvenger.ArmsBurnRowing".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger.GlintUnderBoat".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger.InvestigateGlint".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger.DiveDownDecision".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger.DeepUnderwaterObject".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger.RetrieveSword".Localized());

            FindLightRailAvenger2();
        }

        public void FindLightRailAvenger2()
        {
            ClearOutput();

            OutputText("Scenes.Boat.FindLightRailAvenger2.SurfaceOnBoat".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger2.KatanaDescription".Localized());

            OutputText("Scenes.Boat.FindLightRailAvenger2.DullEdgeless".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeSword".Localized(), TakeSword);

            AddButton(1, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void TakeSword()
        {
            ClearOutput();

            OutputText("Scenes.Boat.TakeSword.TakeSwordDecoration".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GrabbedLightRailAvenger, 1);

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.LightRailAvenger), () => ReturnToCamp(1));
        }

        public void NothingSpecial()
        {

            ShowImage("location-boat");

            OutputText("Scenes.Boat.NothingSpecial.YouRowForNearly".Localized(
                RandomUtils.Choice(
            ["You row[if (isMer) { as your fin swishes in the waters}] for nearly an hour, until your arms practically burn with exhaustion.",
            "You give up on finding anything interesting, and decide to go check up on your camp."]
        )
            ));

            ReturnToCamp(1);
        }

    }
}