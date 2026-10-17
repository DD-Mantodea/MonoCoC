using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas
{
    public static class ForestEncounters
    {
        public static readonly Flag<bool> FoundFruit = new("forest_found_fruit");

        public static readonly Flag<bool> FaerieXImp = new("forest_faerie_x_imp");

        public static readonly Flag<bool> FoundWood = new("forest_found_wood");

        public const int MaxWood = 999;

        public static readonly IEncounter Table = IEncounter.Group(
            "forest",

            CommonEncounters.Table,

            IEncounter.Create("coc:tamani", _ => 0.15f, _ => true,

                _ => Scene.Go<TamaniScene>()),

            IEncounter.Create("jojoForest",

                _ => CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) >= 2

                    ? CommonEncounters.FurriteMod()

                    : 1f,

                _ => SceneInstance<JojoScene>.Get().CanEncounterJojo(),

                _ => SceneInstance<JojoScene>.Get().JojoEncounterFn()),

            IEncounter.Create(
                "essrayle",

                _ => 0.10f,

                _ => EssrayleAvailable(),

                _ => Scene.Go<EssrayleScene>()),

            IEncounter.Create("corruptedGlade", _ => CorruptedGladeChance(), _ => true,

                _ => Scene.Go<CorruptedGladeScene>()),

            IEncounter.Create("lumber", _ => 0.50f, _ => CanGatherWood(), GatherWoods),

            IEncounter.Create("deepwoods", _ => IEncounter.Always, _ => CanDiscoverDeepwoods(),

                _ => DeepWoodsScene.Discover()),

            IEncounter.Create("beegirl", _ => 0.50f, _ => true,

                _ => Scene.Go<BeeGirlScene>()),

            IEncounter.Create("tentabeast", _ => 1f, player => player.Level >= 2,

                _ => Scene.Go<TentacleBeastScene>()),

            IEncounter.Create("marble", _ => 0.10f, _ => CanMarbleVsImp(), MarbleVsImp),

            GoblinSharpshooterScene.Encounter,

            IEncounter.Create("trip", _ => 1f, _ => true, Trip),

            ItemEncounters.Find("chitin", "Areas.Forest.FindChitin", () => ItemRegistry.Create(UseableIds.BlackChitin), 0.05f),

            ItemEncounters.Find("healpill", "Areas.Forest.FindHPill", () => ItemRegistry.Create(ConsumableIds.HealingPill), 0.10f),

            ItemEncounters.Find("truffle", "Areas.Forest.FindTruffle", () => ItemRegistry.Create(ConsumableIds.PigtailTruffle), 0.35f),

            IEncounter.Create("bigjunk", _ => CommonEncounters.BigJunkChance(), _ => CommonEncounters.BigJunkChance() > 0,

                _ => CommonEncounters.BigJunkForestScene()),

            IEncounter.Create("walk", _ => 1f, _ => true, Walk),

            IEncounter.Create("fruit", _ => 0.10f, _ => !CoC.Instance.Flags.Get(FoundFruit), Fruit),

            IEncounter.Create("faerieximp", _ => 0.10f, _ => !CoC.Instance.Flags.Get(FaerieXImp), Faerie),

            IEncounter.Create("coc:lumberjack", _ => 1f, _ => LumberjackAvailable(),

                _ => Scene.Go<LumberjackScene>()),

            IEncounter.Create("corrupted_glade_faerie", _ => 0.05f, _ => FaerieGladesEnabled(),

                _ => FaerieScene.GladesEncounter())
        );

        public static bool CanDiscoverDeepwoods()
        {
            var player = CoC.Instance.Player;

            return CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) >= 20

                && !player.HasStatusEffect(StatusEffectIds.ExploredDeepwoods);
        }

        public static bool FaerieGladesEnabled() =>

            CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) < 50 &&

            CoC.Instance.Flags.Get(GameFlagIds.FaerieEncounterDisabled) <= 0;

        public static bool LumberjackAvailable() =>

            !LumberjackScene.Encountered &&

            CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) >= 4;

        internal static float DullahanChance()
        {
            var save = Scene.SaveContent(SceneIds.Forest.Dullahan);

            // 原版 DullahanScene.hx encounterChance()：当天还没遇到就必定触发，否则普通权重 1
            return save.GetInt("encounterDay", 0) < Scene.View.Time.Days

                ? IEncounter.Always

                : 1f;
        }

        internal static bool DullahanAvailable()
        {
            var flags = CoC.Instance.Flags;

            return Scene.View.Time.Hours >= 21 && flags.Get(GameFlagIds.DullahanRude) != 2;
        }

        internal static float CorruptedGladeChance()
        {
            var destroyed = CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed);

            return destroyed >= 100 ? 0 : (100 - destroyed * 0.9f) / 100f;
        }

        public static bool EssrayleAvailable()
        {
            var flags = CoC.Instance.Flags;

            var player = CoC.Instance.Player;

            return player.Gender != Gender.Genderless &&

                   (flags.Get(GameFlagIds.EssyMetInDungeon) == 0 ||

                    flags.Get(GameFlagIds.ToldMotherToReleaseEssy) == 1);
        }

        internal static bool CanGatherWood()
        {
            var player = CoC.Instance.Player;

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= MaxWood)
                return false;

            if (!HasToolbox(player) && !HasLargeAxe(player) && !HasIceAxe(player) && !CanPunch(player))
                return false;

            if (!HasToolbox(player) && CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) < 4)
                return false;

            var fatigue = MinimumChoppingFatigue(player);

            return HasFatigue(player, fatigue * 0.8f) || !CoC.Instance.Flags.Get(FoundWood);
        }

        public static bool HasToolbox(Player player) =>

            player.HasKeyItem(KeyItemIds.CarpentersToolbox);

        public static bool HasLargeAxe(Player player) =>

            HasItem(player, new Identifier("coc", "large_axe")) ||

            HasItem(player, new Identifier("coc", "fel_axe"));

        public static bool HasFelAxe(Player player) =>

            HasItem(player, new Identifier("coc", "fel_axe"));

        public static bool HasIceAxe(Player player) =>

            HasItem(player, WeaponIds.IceAxe);

        public static bool CanPunch(Player player) =>

            IsSilly && player.Strength >= 70;

        public static bool HasItem(Player player, Identifier id) =>

            player.Weapon.Id == id ||

            player.Inventory.Slots.Any(slot => !slot.IsEmpty && slot.Item.Id == id);

        public static bool IsSilly => OptionStore.Instance.Get("silly", 0) > 0;

        public static bool HasFatigue(Player player, float cost) =>

            player.Fatigue + cost < 100;

        public static int MinimumChoppingFatigue(Player player)
        {
            var cost = 50;

            if (HasLargeAxe(player))
                cost = Math.Min(cost, 40);

            if (HasFelAxe(player))
                cost = Math.Min(cost, 25);

            if (HasIceAxe(player))
                cost = Math.Min(cost, 10);

            if (CanPunch(player))
                cost = Math.Min(cost, 50);

            return cost;
        }

        public static int ChoppingFatigue(string tool) => tool switch
        {
            "Toolbox" => 50,

            "Large Axe" => 40,

            "Fel Axe" => 25,

            "Ice Axe" => 10,

            "Punch" => 50,

            _ => 50
        };

        public static int ChoppingTime(string tool) => tool == "Ice Axe" ? 1 : 2;

        public static int ChoppingQuantity(Player player, string tool) => tool switch
        {
            "Toolbox" => 10 + (int)(player.Strength / 10),

            "Large Axe" => 15 + (int)(player.Strength / 8),

            "Fel Axe" => 15 + (int)(player.Strength / 8),

            "Ice Axe" => 0,

            "Punch" => 10 + (int)(player.Strength / 10),

            _ => 0
        };

        internal static void GatherWoods(EncounterContext context)
        {
            var player = context.Player;

            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.GatherWoods".Localized());

            if (HasToolbox(player))
                Scene.AddButton(0, "Areas.Forest.ChopToolbox".Localized(), () => ChopWood(context, "Toolbox"))
                    .DisableIf(!HasFatigue(player, ChoppingFatigue("Toolbox") * 0.8f), "Areas.Forest.TooWornOut".Localized());

            if (HasLargeAxe(player))
                Scene.AddButton(1, "Areas.Forest.ChopAxe".Localized(), () => ChopWood(context, HasFelAxe(player) ? "Fel Axe" : "Large Axe"))
                    .DisableIf(!HasFatigue(player, ChoppingFatigue("Large Axe") * 0.8f), "Areas.Forest.TooWornOut".Localized());

            if (HasIceAxe(player))
                Scene.AddButton(2, "Areas.Forest.ChopIceAxe".Localized(), () => ChopWood(context, "Ice Axe"))
                    .DisableIf(!HasFatigue(player, ChoppingFatigue("Ice Axe")), "Areas.Forest.TooWornOut".Localized());

            if (CanPunch(player))
                Scene.AddButton(3, "Areas.Forest.PunchTree".Localized(), () => ChopWood(context, "Punch"))
                    .DisableIf(!HasFatigue(player, ChoppingFatigue("Punch") * 0.8f), "Areas.Forest.TooWornOut".Localized());

            Scene.AddButton(14, "Common.Back".Localized(), context.ReturnToCamp);
        }

        public static void ChopWood(EncounterContext context, string tool)
        {
            var player = context.Player;

            var amount = ChoppingQuantity(player, tool);

            var flags = CoC.Instance.Flags;

            flags.Set(FoundWood, true);

            flags.Set(
                GameFlagIds.CampCabinWoodResources,

                Math.Min(MaxWood, flags.Get(GameFlagIds.CampCabinWoodResources) + amount));

            player.ChangeFatigue(ChoppingFatigue(tool));

            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.ChoppedWood".Localized(amount));

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), () =>
            {
                if (context.ReturnToCampAfter != null)
                    context.ReturnToCampAfter(ChoppingTime(tool));
                else
                    context.ReturnToCamp();
            });
        }

        public static void Walk(EncounterContext context)
        {
            var player = context.Player;

            if (player.Corruption < 80)
            {
                context.Write("Areas.Forest.Walk".Localized());

                player.Toughness += 0.5f;

                player.Intelligence += 1;

                Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);

                return;
            }

            if (player.IsMale())
                context.Write("Areas.Forest.WalkCorruptMale".Localized());
            else if (player.IsFemale())
                context.Write("Areas.Forest.WalkCorruptFemale".Localized(player.VaginaDescript(0)));
            else if (player.IsHerm())
                context.Write("Areas.Forest.WalkCorruptHerm".Localized(player.VaginaDescript(0)));
            else
                context.Write("Areas.Forest.WalkCorruptNone".Localized());

            player.Toughness += 0.5f;

            player.Libido = Math.Min(100, player.Libido + 0.25f);

            player.Lust = Math.Min(100, player.Lust + player.Libido / 5f);

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
        }

        public static void Trip(EncounterContext context)
        {
            context.Write("Areas.Forest.Trip".Localized());

            context.Player.Health = Math.Max(1, context.Player.Health - 10);

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
        }

        public static bool CanMarbleVsImp()
        {
            var player = CoC.Instance.Player;

            return CoC.Instance.Flags.Get(GameFlagIds.TimesExploredForest) > 0

                && !player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted)

                && !player.HasStatusEffect(StatusEffectIds.NoMoreMarble)

                && player.HasStatusEffect(StatusEffectIds.Marble)

                && CoC.Instance.Flags.Get(GameFlagIds.MarbleWarning) == 0;
        }

        public static void MarbleVsImp(EncounterContext context)
        {
            context.Write("Areas.Forest.MarbleVsImp".Localized());

            Scene.AddButton(0, "Scenes.Explore.Continue".Localized(), context.ReturnToCamp);
        }

        public static void Fruit(EncounterContext context)
        {
            CoC.Instance.Flags.Set(FoundFruit, true);

            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.Fruit".Localized());

            Scene.AddButton(0, "Areas.Forest.FruitEat".Localized(), () => FruitEat(context));

            Scene.AddButton(1, "Areas.Forest.FruitPass".Localized(), () => FruitPass(context));
        }

        public static void FruitEat(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.FruitEatText".Localized());

            context.Player.Fatigue = Math.Max(0, context.Player.Fatigue - 15);

            context.Player.Health = Math.Min(context.Player.MaxHealth, context.Player.Health + context.Player.MaxHealth * 0.25f);

            context.Player.Hunger = Math.Min(100, context.Player.Hunger + 25);

            context.ReturnToCamp();
        }

        public static void FruitPass(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.FruitPassText".Localized());

            context.ReturnToCamp();
        }

        public static void Faerie(EncounterContext context)
        {
            CoC.Instance.Flags.Set(FaerieXImp, true);

            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.Faerie".Localized());

            Scene.AddButton(0, "Areas.Forest.FaerieWatch".Localized(), () => FaerieWatch(context));

            Scene.AddButton(1, "Areas.Forest.FaerieLeave".Localized(), () => FaerieLeave(context));
        }

        public static void FaerieWatch(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.FaerieWatchText".Localized());

            context.ReturnToCamp();
        }

        public static void FaerieLeave(EncounterContext context)
        {
            Scene.ClearOutput();

            Scene.ClearButtons();

            context.Write("Areas.Forest.FaerieLeaveText".Localized());

            context.ReturnToCamp();
        }
    }
}
