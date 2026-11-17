using Hjson;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Contents.Equipment
{
    public sealed class EquipmentPatch
    {
        public string Id = "";
        public string Target = "";
        public List<string> RequiresTags = [];
        public Dictionary<string, int> AddCapacity = [];
        public EquipmentSlotSpec? AddSlot;
        public int AddHands;
        public int Priority;
    }

    public static class EquipmentCatalog
    {
        private static bool _loaded;

        public static readonly List<EquipmentArea> Areas = [];

        public static readonly List<EquipmentSlotSpec> Slots = [];

        public static readonly List<EquipmentPatch> Patches = [];

    // 基础手数（Slots.hjson 的 hands），种族的 addHands 在此基础上叠加。
    public static int BaseHands = 2;

        public static List<EquipmentSlotSpec> Compose(IReadOnlySet<string> tags)        {
            EnsureLoaded();

            var result = new List<EquipmentSlotSpec>();

            foreach (var slot in Slots)
            {
                if (slot.RequiresTags.Count > 0 && !slot.RequiresTags.TrueForAll(tags.Contains))
                    continue;

                result.Add(Clone(slot));
            }

            var orderedPatches = new List<EquipmentPatch>(Patches);

            orderedPatches.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            foreach (var patch in orderedPatches)
            {
                if (patch.RequiresTags.Count > 0 && !patch.RequiresTags.TrueForAll(tags.Contains))
                    continue;

                foreach (var slot in result)
                {
                    if (slot.Id != patch.Target)
                        continue;

                    foreach (var pair in patch.AddCapacity)
                    {
                        slot.Capacity.TryGetValue(pair.Key, out var current);
                        slot.Capacity[pair.Key] = current + pair.Value;
                    }
                }

                if (patch.AddSlot is not null && result.TrueForAll(slot => slot.Id != patch.AddSlot.Id))
                    result.Add(Clone(patch.AddSlot));
            }

            // 手牌槽由手数生成：基础数量 + 匹配种族的 addHands（四只手 = 4 个槽）。
            // 第一个手槽携带 hand 容量，EquipmentRules.Capacity 从这里读，不再硬编码 2。
            var handCount = BaseHands;

            foreach (var patch in orderedPatches)
                if (patch.RequiresTags.Count == 0 || patch.RequiresTags.TrueForAll(tags.Contains))
                    handCount += patch.AddHands;

            for (var hand = 1; hand <= handCount; hand++)
            {
                var first = hand == 1;

                result.Add(new EquipmentSlotSpec
                {
                    Id = $"coc:hand_{hand}",
                    Kind = "hand",
                    LabelKey = "Scenes.Inventory.Slot.Hands",
                    SlotIcon = "Icons/inventory_weapon.svg",
                    Area = "left",
                    Order = hand - 1,
                    IsExtra = true,
                    FallbackAttack = first ? "coc:fists" : "",
                    Accepts = ["coc:weapon", "coc:shield"],
                    Capacity = first ? new Dictionary<string, int> { ["hand"] = handCount } : []
                });
            }

            return result;
        }

        public static EquipmentSlotSpec? FindActive(string id, IReadOnlySet<string> tags)
        {
            foreach (var slot in Compose(tags))
                if (slot.Id == id)
                    return slot;

            return null;
        }

        private static EquipmentSlotSpec Clone(EquipmentSlotSpec slot) => new()
        {
            Id = slot.Id,
            Kind = slot.Kind,
            LabelKey = slot.LabelKey,
            SlotIcon = slot.SlotIcon,
            Area = slot.Area,
            Order = slot.Order,
            IsExtra = slot.IsExtra,
            FallbackAttack = slot.FallbackAttack,
            Accepts = [.. slot.Accepts],
            Capacity = new Dictionary<string, int>(slot.Capacity),
            RequiresTags = [.. slot.RequiresTags]
        };

        public static void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;

            var path = Path.Combine(Paths.Assets, "Data", "Equipment", "Slots.hjson");

            if (!File.Exists(path))
                return;

            if (MonoCoC.HjsonParser.Parse(File.ReadAllText(path)) is not JsonObject root)
                return;

            BaseHands = root.Int("hands", 2);

            if (root.Array("areas") is { } areas)
            {
                foreach (var value in areas)
                {
                    if (value is not JsonObject area)
                        continue;

                    var areaId = area.Str("id");

                    if (FindArea(areaId) is not null)
                    {
                        MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate area '{areaId}'");

                        continue;
                    }

                    Areas.Add(new EquipmentArea
                    {
                        Id = areaId,
                        X = area.Int("x", 0),
                        Y = area.Int("y", 0),
                        Width = area.Int("width", 0),
                        Height = area.Int("height", 0)
                    });
                }
            }

            if (root.Array("slots") is { } slots)
            {
                foreach (var value in slots)
                {
                    if (value is not JsonObject slot)
                        continue;

                    var spec = ReadSlotObject(slot);

                    if (FindSlot(spec.Id) is not null)
                    {
                        MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate slot '{spec.Id}'");

                        continue;
                    }

                    Slots.Add(spec);
                }
            }

            var patchesPath = Path.Combine(Paths.Assets, "Data", "Equipment", "Patches.hjson");

            if (!File.Exists(patchesPath))
                return;

            if (MonoCoC.HjsonParser.Parse(File.ReadAllText(patchesPath)) is not JsonObject patchRoot)
                return;

            if (patchRoot.Array("patches") is not { } patches)
                return;

            foreach (var value in patches)
            {
                if (value is not JsonObject patch)
                    continue;

                var patchId = patch.Str("id");

                if (FindPatch(patchId) is not null)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate patch '{patchId}'");

                    continue;
                }

                Patches.Add(new EquipmentPatch
                {
                    Id = patchId,
                    Target = patch.Str("target"),
                    RequiresTags = ReadStrings(patch, "requiresTags"),
                    AddCapacity = ReadCounts(patch, "addCapacity"),
                    AddHands = patch.Int("addHands", 0),
                    AddSlot = ReadSlot(patch),
                    Priority = patch.Int("priority", 0)
                });
            }

            foreach (var patch in Patches)
            {
                if (patch.Target.Length > 0 && FindSlot(patch.Target) is null)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                        $"[Equipment] patch '{patch.Id}' targets unknown slot '{patch.Target}'");
                }

                if (patch.AddSlot is not null && FindSlot(patch.AddSlot.Id) is not null)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                        $"[Equipment] patch '{patch.Id}' adds duplicate slot '{patch.AddSlot.Id}'");
                }

                if (patch.AddSlot is { Area.Length: > 0 } added && FindArea(added.Area) is null)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                        $"[Equipment] patch '{patch.Id}' adds slot '{added.Id}' to unknown area '{added.Area}'");
                }
            }

            var modsDirectory = Path.Combine(Paths.Assets, "Data", "Equipment", "Mods");

            if (!Directory.Exists(modsDirectory))
                return;

            foreach (var modPath in Directory.GetFiles(modsDirectory, "*.hjson"))
            {
                if (MonoCoC.HjsonParser.Parse(File.ReadAllText(modPath)) is not JsonObject modRoot)
                    continue;

                if (modRoot.Array("areas") is { } modAreas)
                {
                    foreach (var value in modAreas)
                    {
                        if (value is not JsonObject area)
                            continue;

                        var id = area.Str("id");

                        if (FindArea(id) is not null)
                        {
                            MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate area '{id}' in {modPath}");

                            continue;
                        }

                        Areas.Add(new EquipmentArea
                        {
                            Id = id,
                            X = area.Int("x", 0),
                            Y = area.Int("y", 0),
                            Width = area.Int("width", 0),
                            Height = area.Int("height", 0)
                        });
                    }
                }

                if (modRoot.Array("slots") is { } modSlots)
                {
                    foreach (var value in modSlots)
                    {
                        if (value is not JsonObject slot)
                            continue;

                        var spec = ReadSlotObject(slot);

                        if (FindSlot(spec.Id) is not null)
                        {
                            MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate slot '{spec.Id}' in {modPath}");

                            continue;
                        }

                        Slots.Add(spec);
                    }
                }

                if (modRoot.Array("patches") is { } modPatches)
                {
                    foreach (var value in modPatches)
                    {
                        if (value is not JsonObject patch)
                            continue;

                        var patchId = patch.Str("id");

                        if (FindPatch(patchId) is not null)
                        {
                            MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[Equipment] duplicate patch '{patchId}' in {modPath}");

                            continue;
                        }

                        Patches.Add(new EquipmentPatch
                        {
                            Id = patchId,
                            Target = patch.Str("target"),
                            RequiresTags = ReadStrings(patch, "requiresTags"),
                            AddCapacity = ReadCounts(patch, "addCapacity"),
                    AddHands = patch.Int("addHands", 0),
                            AddSlot = ReadSlot(patch),
                            Priority = patch.Int("priority", 0)
                        });
                    }
                }
            }
        }

        private static EquipmentArea? FindArea(string id)
        {
            foreach (var area in Areas)
                if (area.Id == id)
                    return area;

            return null;
        }

        private static EquipmentSlotSpec? FindSlot(string id)
        {
            foreach (var slot in Slots)
                if (slot.Id == id)
                    return slot;

            return null;
        }

        private static EquipmentPatch? FindPatch(string id)
        {
            foreach (var patch in Patches)
                if (patch.Id == id)
                    return patch;

            return null;
        }

        private static EquipmentSlotSpec ReadSlotObject(JsonObject slot) => new()
        {
            Id = slot.Str("id"),
            Kind = slot.Str("kind", "weapon"),
            LabelKey = slot.Str("label"),
            SlotIcon = slot.Str("slotIcon"),
            Area = slot.Str("area"),
            Order = slot.Int("order", 0),
            IsExtra = slot.Bool("extra", false),
            FallbackAttack = slot.Str("fallbackAttack"),
            Accepts = ReadStrings(slot, "accepts"),
            Capacity = ReadCounts(slot, "capacity"),
            RequiresTags = ReadStrings(slot, "requiresTags")
        };

        private static EquipmentSlotSpec? ReadSlot(JsonObject patch)
        {
            if (!patch.TryGetValue("addSlot", out var value) || value is not JsonObject slot)
                return null;

            return new EquipmentSlotSpec
            {
                Id = slot.Str("id"),
                Kind = slot.Str("kind", "weapon"),
                LabelKey = slot.Str("label"),
                SlotIcon = slot.Str("slotIcon"),
                Area = slot.Str("area"),
                Order = slot.Int("order", 0),
                IsExtra = true,
                FallbackAttack = slot.Str("fallbackAttack"),
                Accepts = ReadStrings(slot, "accepts"),
                Capacity = ReadCounts(slot, "capacity"),
                RequiresTags = ReadStrings(slot, "requiresTags")
            };
        }

        private static List<string> ReadStrings(JsonObject node, string key)
        {
            var result = new List<string>();

            if (node.Array(key) is { } array)
                foreach (var value in array)
                    result.Add(value.ToString().Trim('"'));

            return result;
        }

        private static Dictionary<string, int> ReadCounts(JsonObject node, string key)
        {
            var result = new Dictionary<string, int>();

            if (node.TryGetValue(key, out var value) && value is JsonObject obj)
                foreach (var pair in obj)
                    result[pair.Key] = pair.Value is null ? 0 : int.Parse(pair.Value.ToString());

            return result;
        }
    }
}
