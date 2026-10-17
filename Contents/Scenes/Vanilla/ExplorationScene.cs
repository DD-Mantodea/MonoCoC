using MonoCoC.Attributes;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Tags;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<ExplorationScene>("coc:exploration")]
    public class ExplorationScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Core.Exploration);

        public Action? currArea;

        public bool doneMermaidWalk = false;

        public string debugName => "Explore";

        public string debugHint => "";

        public override void Entry()
        {
            RunEncounter(null);
        }

        public void DoExplore()
        {
            ClearOutput();

            if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.TimesExplored)))
                CoC.Instance.Flags.Set(GameFlagIds.TimesExplored, 0);

            if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest)))
                CoC.Instance.Flags.Set(GameFlagIds.TimesExploredForest, 0);

            if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.TimesExploredLake)))
                CoC.Instance.Flags.Set(GameFlagIds.TimesExploredLake, 0);

            if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.TimesExploredDesert)))
                CoC.Instance.Flags.Set(GameFlagIds.TimesExploredDesert, 0);

            if (float.IsNaN(CoC.Instance.Flags.Get(GameFlagIds.TimesExploredMountain)))
                CoC.Instance.Flags.Set(GameFlagIds.TimesExploredMountain, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExplored) <= 0)
            {
                ShowImage("event-first-steps");

                OutputText("Scenes.Exploration.DoExplore.StepsAway".Localized());

                OutputText("Scenes.Exploration.DoExplore.CampsiteAhead".Localized());

                OutputText("Scenes.Exploration.DoExplore.PortalReturns".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.TimesExplored, CoC.Instance.Flags.Get(GameFlagIds.TimesExplored) + 1);

                ReturnToCamp(1);

                return;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) <= 0)
                {
                    ShowImage("area-forest");

                    OutputText("Scenes.Exploration.DoExplore.RoamsPinkEarth".Localized());

                    OutputText("Scenes.Exploration.DoExplore.ForestDiscovered".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.TimesExplored, CoC.Instance.Flags.Get(GameFlagIds.TimesExplored) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.TimesExploredForest, CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) + 1);

                    ReturnToCamp(1);

                    return;
                }

            OutputText("Scenes.Exploration.DoExplore.SearchOptions".Localized());

            ClearButtons();

            ClearButtons();

            AddButton(0, "Common.Button.Explore".Localized(), TryDiscover);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) > 0)
                AddButton(1, "Common.Button.Forest".Localized(), () => RunEncounter(AreaRegistry.Get("coc:forest")));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredLake) > 0)
                AddButton(2, "Common.Button.Lake".Localized(), () => RunEncounter(AreaRegistry.Get("coc:lake")));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredDesert) > 0)
                AddButton(3, "Common.Button.Desert".Localized(), () => RunEncounter(AreaRegistry.Get("coc:desert")));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredMountain) > 0)
                AddButton(5, "Common.Button.Mountain".Localized(), () => RunEncounter(AreaRegistry.Get("coc:mountain")));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredSwamp) > 0)
                AddButton(6, "Areas.Swamp.Name".Localized(), () => RunEncounter(AreaRegistry.Get("coc:swamp")));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredPlains) > 0)
                AddButton(7, "Common.Button.Plains".Localized(), () => RunEncounter(AreaRegistry.Get("coc:plains")));

            if (Player.HasStatusEffect(StatusEffectIds.ExploredDeepwoods))
                AddButton(8, "Areas.Deepwoods.Name".Localized(), () => RunEncounter(AreaRegistry.Get("coc:deepwoods")));

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredHighMountain) > 0)
                AddButton(10, "Common.Button.HighMountain".Localized(), () => RunEncounter(AreaRegistry.Get("coc:high_mountains")));

            if (CoC.Instance.Flags.Get(GameFlagIds.BogExplored) > 0)
                AddButton(11, "Areas.Bog.Name".Localized(), () => RunEncounter(AreaRegistry.Get("coc:bog")));

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredGlacialRift) > 0)
                AddButton(12, "Common.Button.GlacialRift".Localized(), () => RunEncounter(AreaRegistry.Get("coc:glacial_rift")));

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredVolcanoCrag) > 0)
                AddButton(13, "Common.Button.VolcanicCrag".Localized(), () => RunEncounter(AreaRegistry.Get("coc:volcanic_crag")));

            if (Debug)
                AddButton(9, "Common.Button.Debug".Localized(), SceneInstance<ExploreDebugScene>.Get().DoExploreDebug);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void RunEncounter(AreaDefinition? area)
        {
            if (area == null)
            {
                currArea = null;

                return;
            }

            currArea = () => Go(SceneRegistry.CreateOrDefault(area.SceneId));

            // 区域带权遭遇掷点在区域场景里完成（如 HighMountainsScene.Explore → HighMountainsEncounters.Table）
            Go(SceneRegistry.CreateOrDefault(area.SceneId));
        }

        public void TryDiscover()
        {
            ClearOutput();

            Player.Location = "wilderness";

            CoC.Instance.Flags.Set(GameFlagIds.TimesExplored, CoC.Instance.Flags.Get(GameFlagIds.TimesExplored) + 1);

            var context = new EncounterContext              {                 Player = Player,                 Flags = Flags,                 Time = View.Time,                 Random = Random.Shared,                 Write = OutputText,                 ReturnToCamp = () => ReturnToCamp(1),                 ReturnToCampAfter = ReturnToCamp             };              CommonEncounters.Table.Execute(context);
        }

        public void GetUrtaHalberd()
        {
            ClearOutput();

            Save.SetBool("foundUrtaHalberd", true);

            OutputText("Scenes.Exploration.GetUrtaHalberd.SpireGlimpse".Localized());

            OutputText("Scenes.Exploration.GetUrtaHalberd.HalberdFound".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.UrtasHalberd), 1);

            ReturnToCamp(1);
        }

        public void WagonOneoff()
        {
            ClearOutput();

            Save.SetBool("foundWagon", true);

            OutputText("Scenes.Exploration.WagonOneoff.OverturnedCart".Localized());

            OutputText("Scenes.Exploration.WagonOneoff.SearchesWagon".Localized());

            OutputText("Scenes.Exploration.WagonOneoff.DiaryFaded".Localized());

            OutputText("Scenes.Exploration.WagonOneoff.ChildDiaryEntry".Localized());

            OutputText("Scenes.Exploration.WagonOneoff.ClosesBook".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.TatteredLeather), 1);

            ReturnToCamp(1);
        }

        public void SkeletonOneoff()
        {
            ClearOutput();

            Save.SetBool("foundSkeleton", true);

            OutputText("Scenes.Exploration.SkeletonOneoff.WhiteShape".Localized());

            OutputText("Scenes.Exploration.SkeletonOneoff.DangerMissed".Localized());

            OutputText("Scenes.Exploration.SkeletonOneoff.CouldDoSomething".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Grave".Localized(), SkeletonGrave);

            AddNextButton("Items.Skull.Short".Localized(), SkeletonSkull);

            AddNextButton("Common.Button.Leave".Localized(), SkeletonLeave);
        }

        public void SkeletonGrave()
        {
            ClearOutput();

            OutputText("Scenes.Exploration.SkeletonGrave.DigsGrave".Localized());

            OutputText("Scenes.Exploration.SkeletonGrave.BuriesBones".Localized());

            ReturnToCamp(1);
        }

        public void SkeletonSkull()
        {
            ClearOutput();

            OutputText("Scenes.Exploration.SkeletonSkull.PriesSkull".Localized());

            OutputText("Scenes.Exploration.SkeletonSkull.StowsSkull".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Skull), 1);

            ReturnToCamp(1);
        }

        public void SkeletonLeave()
        {
            ClearOutput();

            OutputText("Scenes.Exploration.SkeletonLeave.LastLookLeaves".Localized());

            ReturnToCamp(1);
        }

        public Consumable MermaidPickFluid()
        {

            var preferred = new[]
            {
                (Consumable)ItemRegistry.Create(ConsumableIds.SpringWater),

                (Consumable)ItemRegistry.Create(ConsumableIds.IsabellaMilk),

                (Consumable)ItemRegistry.Create(ConsumableIds.MinotaurMilk),
            };

            foreach (var p in preferred)
                if (Player.HasItem(p))
                    return p;

            var consumables = new List<Consumable>();

            foreach (var slot in Player.Inventory.Slots)
                if (!slot.IsEmpty && slot.Item is Consumable consumable)
                    consumables.Add(consumable);

            var fluids = TagSet.WithoutTags(TagSet.WithTags(consumables, "fluid"), "special").ToList();

            if (fluids.Count <= 0)
                return null;

            var nonTransformativeFluids = TagSet.WithoutTags(fluids, "transformative").ToList();

            if (nonTransformativeFluids.Count > 0)
                return nonTransformativeFluids[Random.Shared.Next(nonTransformativeFluids.Count)];
            else
                return fluids[Random.Shared.Next(fluids.Count)];
        }

        public void MermaidWalk()
        {
            ClearOutput();

            var item = MermaidPickFluid();

            bool firstTime = false;

            switch (Player.Location)
            {
                case "desert":
                    {
                        firstTime = !Save.GetBool("desertWaterNeeded");

                        Save.SetBool("desertWaterNeeded", true);
                    }

                    break;

                case "crag":
                    {
                        firstTime = !Save.GetBool("cragWaterNeeded");

                        Save.SetBool("cragWaterNeeded", true);
                    }

                    break;
            }

            if (Player.Location != "desert" && Player.Location != "crag")
                firstTime = true;

            if (firstTime)
            {
                switch (Player.Location)
                {
                    case "desert":
                        {
                            OutputText("Scenes.Exploration.MermaidWalk.SandIrritates".Localized());
                        }

                        break;

                    case "crag":
                        {
                            OutputText("Scenes.Exploration.MermaidWalk.HeatCrest".Localized());
                        }

                        break;
                }

                if (Player.Location != "desert" && Player.Location != "crag")
                    OutputText("Scenes.Exploration.MermaidWalk.InvalidLocation".Localized());
            }

            if (item == null)
            {
                currArea = null;

                if (firstTime)
                {
                    OutputText("Scenes.Exploration.MermaidWalk.NoFluidSource".Localized());

                    Player.ChangeFatigue(30);

                    ReturnToCamp(1);
                }
                else
                {
                    OutputText("Scenes.Exploration.MermaidWalk.NeedsExtraFluid".Localized());

                    DoExplore();
                }

                return;
            }

            if (!firstTime)
            {
                OutputText("Scenes.Exploration.MermaidWalk.WithersTerrain".Localized(
                    ItemRegistry.Create(item.Id).LongName
                ));

                Player.Inventory.Use(ItemRegistry.Create(item.Id), text => OutputText(text), Random.Shared);
            }
            else
                if (item == ConsumableIds.SpringWater)
                    OutputText("Scenes.Exploration.MermaidWalk.MinervaWater".Localized());
                else
                    if (item == ConsumableIds.IsabellaMilk || item == ConsumableIds.MinotaurMilk)
                        OutputText("Scenes.Exploration.MermaidWalk.BovineMilk".Localized());
                    else
                    {
                        OutputText("Scenes.Exploration.MermaidWalk.OwnBottle".Localized(
                            ItemRegistry.Create(item.Id).LongName
                        ));

                        Player.Inventory.Use(ItemRegistry.Create(item.Id), text => OutputText(text), Random.Shared);

                        OutputText("Scenes.Exploration.MermaidWalk.NotIdealSolution".Localized());
                    }

            Player.ConsumeItem(item);

            var debuff = Player.CreateOrFindStatusEffect(StatusEffectIds.MermaidDehydrated);

            // 原版 MermaidWateredDebuff.hx:11：首次附加时 Speed -10，increase() 时收紧到 -20
            if (debuff.Values[0] <= 0)
            {
                debuff.Values[0] = 10;

                Combat.CombatCalculator.AddCombatBuff(Player, GameStats.Speed, -10, "MermaidDehydrated", StatusEffectIds.MermaidDehydrated);
            }

            if (Player.Location == "crag")
            {
                debuff.Values[0] = 20;

                Combat.CombatCalculator.AddCombatBuff(Player, GameStats.Speed, -20, "MermaidDehydrated", StatusEffectIds.MermaidDehydrated);
            }

            doneMermaidWalk = true;

            currArea();
        }

        public void Gargoyle()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GarName) == "")
                SceneInstance<GargoyleScene>.Get().GargoylesTheShowNowOnWBNetwork();
            else
                SceneInstance<GargoyleScene>.Get().ReturnToCathedral();
        }

        public void Reset()
        {

            Save.SetBool("foundWagon", false);

            Save.SetBool("foundSkeleton", false);

            Save.SetBool("foundUrtaHalberd", false);

            Save.SetBool("desertWaterNeeded", false);

            Save.SetBool("cragWaterNeeded", false);
        }

    }
}