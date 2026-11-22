using MonoCoC.Contents.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.UI;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Common;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace MonoCoC.UI.Components.Views
{
    public class SaveLoadView : HjsonView
    {
        private const int SlotCount = 10;

        private SlotCard[] Slots { get; set; } = null!;

        private Grid RowsGrid { get; set; } = null!;

        private Container ListPanel { get; set; } = null!;

        private SegmentedComponent Tabs { get; set; } = null!;

        private Button PrimaryButton { get; set; } = null!;

        private Button BackButton { get; set; } = null!;

        private Label DetailName { get; set; } = null!;

        private Label DetailMeta { get; set; } = null!;

        private Label DetailNotes { get; set; } = null!;

        private Label HintLabel { get; set; } = null!;

        private TextBox NotesInput { get; set; } = null!;

        private Label NotesLabel { get; set; } = null!;

        private Button NotesSave { get; set; } = null!;

        private readonly AnimationPlayer _animations = new();

        private readonly SaveData?[] _slots = new SaveData?[SlotCount];

        private int _selected;

        private int _tab;

        private int _scroll;

        private bool _draggingBar;

        private readonly ScrollBarFade _scrollFade = new();

        private const int ListContentHeight = 970;

        public bool FromMainMenu { get; set; }

        public SaveLoadView(bool fromMainMenu = false)
        {
            FromMainMenu = fromMainMenu;

            Initialize();

            Refresh();
        }

        protected override string ScreenFile => "Views\\SaveLoadView.hjson";

        [UIRequired("saveload.rows")]
        private void BindSlots(Grid part)
        {
            RowsGrid = part;

            Slots = new SlotCard[SlotCount];

            for (var index = 0; index < part.LayoutData.Children.Count && index < SlotCount; index++)
            {
                var container = part.LayoutData.Children[index];

                var row = new SlotCard
                {
                    Hit = FindPart(container, "slot.hit") as Button ?? new Button(),
                    Title = FindPart(container, "slot.title") as Label ?? new Label(UIFonts.Get("coc:menu")),
                    Meta = FindPart(container, "slot.meta") as Label ?? new Label(UIFonts.Get("coc:body")),
                    Export = FindPart(container, "slot.icon.save") as Button ?? new Button(),
                    Import = FindPart(container, "slot.icon.load") as Button ?? new Button(),
                    Delete = FindPart(container, "slot.icon.delete") as Button ?? new Button()
                };

                row.ExportImage = SetupIcon(row.Export, "Icons/save_export.svg");

                row.ImportImage = SetupIcon(row.Import, "Icons/save_import.svg");

                row.DeleteImage = SetupIcon(row.Delete, "Icons/save_delete.svg");

                var slotIndex = index;

                row.Hit.OnClick = () => SelectSlot(slotIndex);

                row.Export.OnClick = () => ExecuteSlotAction(slotIndex, 0);

                row.Import.OnClick = () => ExecuteSlotAction(slotIndex, 1);

                row.Delete.OnClick = () => ExecuteSlotAction(slotIndex, 2);

                Slots[index] = row;
            }
        }

        private static UIImage? SetupIcon(Button icon, string svg)
        {
            icon.Visible = false;

            var texture = SvgTextureCache.GetFile(MonoCoC.CoC.Instance.GraphicsDevice, svg, 34, SKColors.White);

            if (texture is null)
                return null;

            return new UIImage(texture)
                .SizeType(SizeType.Fill)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(icon);
        }

        private static Component? FindPart(Component component, string part)
        {
            if (string.Equals(component.PartName, part, StringComparison.OrdinalIgnoreCase))
                return component;

            foreach (var child in component.LayoutData.Children)
            {
                var result = FindPart(child, part);

                if (result is not null)
                    return result;
            }

            return null;
        }

        private void SelectSlot(int index)
        {
            _selected = index;

            ClearHint();

            Refresh();
        }

        private void ShowHint(string text)
        {
            if (HintLabel is null)
                return;

            HintLabel.Text(text);

            HintLabel.TextColor = UIParseUtils.ColorOf("#A23B2AFF");

            HintLabel.LayoutData.Alpha = 0f;

            _animations.StopAll();

            _animations.Play(new FloatAnimation(0f, 1f, value => HintLabel.LayoutData.Alpha = value, 0.22f)
            {
                Easing = Easing.EaseOutCubic
            });
        }

        private void ClearHint()
        {
            if (HintLabel is null)
                return;

            HintLabel.Text("");

            HintLabel.TextColor = UIParseUtils.ColorOf("#A23B2AFF");

            HintLabel.LayoutData.Alpha = 1f;

            _animations.StopAll();
        }

        private void ShowSaved()
        {
            if (HintLabel is null)
                return;

            HintLabel.Text("Saved");

            HintLabel.TextColor = UIParseUtils.ColorOf("#4F7A3FFF");

            HintLabel.LayoutData.Alpha = 1f;

            _animations.StopAll();
        }

        [UIRequired("saveload.list")]
        private void BindList(Container part) => ListPanel = part;

        [UIRequired("saveload.tabs")]
        private void BindTabs(SegmentedComponent part)
        {
            Tabs = part;

            Tabs.SetOptions(
            [
                "Scenes.SaveLoad.Button.Load".Localized(),
                ..(FromMainMenu ? Array.Empty<string>() : new[] { "Scenes.SaveLoad.Button.Save".Localized() })
            ], 0);

            Tabs.ValueChanged = index =>
            {
                _tab = index;

                Refresh();
            };
        }

        [UIRequired("saveload.primary")]
        private void BindPrimary(Button part) => PrimaryButton = part;

        [UIRequired("saveload.back")]
        private void BindBack(Button part) => BackButton = part;

        [UIRequired("saveload.detail.name")]
        private void BindDetailName(Label part) => DetailName = part;

        [UIRequired("saveload.detail.meta")]
        private void BindDetailMeta(Label part) => DetailMeta = part;

        [UIRequired("saveload.detail.notes")]
        private void BindDetailNotes(Label part) => DetailNotes = part;

        [UIRequired("saveload.hint")]
        private void BindError(Label part) => HintLabel = part;

        [UIRequired("saveload.notes.input")]
        private void BindNotesInput(TextBox part) => NotesInput = part;

        [UIRequired("saveload.notes.label")]
        private void BindNotesLabel(Label part) => NotesLabel = part;

        [UIRequired("saveload.notes.save")]
        private void BindNotesSave(Button part)
        {
            NotesSave = part;

            NotesSave.SetLabel("Save");

            NotesSave.OnClick = SaveNotes;
        }

        private void SaveNotes()
        {
            var entry = CurrentSave;

            if (entry is null)
            {
                ShowHint("No saved game");

                return;
            }

            entry.Notes = NotesInput.Text.Trim();

            entry.Write(entry.Slot);

            ClearHint();

            Refresh();
        }

        private bool CanSave => MonoCoC.CoC.Instance.Player.Loaded && !FromMainMenu;

        private SaveData? CurrentSave => _slots[Math.Clamp(_selected, 0, SlotCount - 1)];

        protected override void OnPartsBound()
        {
            base.OnPartsBound();

            if (Slots is null)
                return;

            Refresh();
        }

        private void Refresh()
        {
            var saves = SaveData.List()
                .OrderByDescending(entry => entry.SaveTime)
                .ToArray();

            for (var index = 0; index < SlotCount; index++)
                _slots[index] = index < saves.Length ? saves[index] : null;

            _selected = Math.Clamp(_selected, 0, SlotCount - 1);

            RefreshSlots();

            RefreshDetails();

            RefreshActions();

            ApplyScroll();
        }

        private void RefreshSlots()
        {
            for (var index = 0; index < Slots.Length; index++)
            {
                var row = Slots[index];

                if (row is null)
                    continue;

                var entry = _slots[index];

                row.Hit.Selected = index == _selected;

                var autoSave = entry?.Slot.StartsWith("AutoSave", StringComparison.Ordinal) == true;

                var title = autoSave
                    ? $"AutoSave — {(entry?.Name ?? "Empty Slot")}"
                    : $"Slot {index + 1:00} — {(entry?.Name ?? "Empty Slot")}";

                row.Title.Text(title);

                row.Meta.Text(entry is null
                    ? "No saved game"
                    : $"{GameTime(entry)}   ·   {(string.IsNullOrWhiteSpace(entry.Notes) ? "—" : entry.Notes)}");

                row.Delete.Hint = "Scenes.SaveLoad.Button.Delete".Localized();

                row.Delete.HintHeader = row.Delete.Hint;

                row.HasSave = entry is not null;
            }
        }

        private void RefreshDetails()
        {
            var entry = CurrentSave;

            if (entry is null)
            {
                DetailName.Text("Empty Slot");
                DetailMeta.Text("No saved game");
                DetailNotes.Text("");

                if (NotesInput is not null)
                    NotesInput.Text = "";

                SetNotesVisible(false);

                return;
            }

            SetNotesVisible(true);

            DetailName.Text(entry.Name);

            var level = entry.Player.TryGetValue("level", out var levelValue) ? levelValue.ToString() : "?";

            var location = string.IsNullOrWhiteSpace(entry.Location) ? "Unknown" : entry.Location;

            DetailMeta.Text($"{GameTime(entry)}   ·   Level {level}   ·   {location}   ·   {Playtime(entry)}");

            DetailNotes.Text(string.IsNullOrWhiteSpace(entry.Notes) ? "—" : entry.Notes);

            if (NotesInput is not null)
                NotesInput.Text = entry.Notes;
        }

        private void SetNotesVisible(bool visible)
        {
            if (NotesInput is not null)
                NotesInput.Visible = visible;

            if (NotesLabel is not null)
                NotesLabel.Visible = visible;

            if (NotesSave is not null)
                NotesSave.Visible = visible;
        }

        private static string Playtime(SaveData entry)
        {
            var hours = (int)entry.Playtime;

            var minutes = (int)((entry.Playtime - hours) * 60);

            return $"{hours}h {minutes:00}m";
        }

        private void RefreshActions()
        {
            var primary = _tab == 0
                ? "Scenes.SaveLoad.Button.Load".Localized()
                : "Scenes.SaveLoad.Button.Save".Localized();

            PrimaryButton.SetLabel(primary);
            PrimaryButton.Enabled = _tab == 0 || CanSave;
            PrimaryButton.DisableIf = "";
            PrimaryButton.DisabledReason = "";
            PrimaryButton.OnClick = ExecutePrimary;
            PrimaryButton.RefreshVisual();

            BackButton.SetLabel("Common.Back".Localized());
            BackButton.OnClick = Back;
            BackButton.RefreshVisual();
        }

        private void ExecutePrimary()
        {
            if (_tab == 0)
            {
                if (CurrentSave is not null)
                    LoadSlot(CurrentSave.Slot);

                return;
            }

            if (_tab == 1)
            {
                var entry = CurrentSave;

                if (entry is null)
                    SaveSlot(NextSlot(), "");
                else
                    SaveSlot(entry.Slot, entry.Notes);
            }
        }

        private void DeleteSelected()
        {
            var entry = CurrentSave;

            if (entry is null)
                return;

            ConfirmDialogOverlay.Show(
                "Scenes.SaveLoad.Delete".Localized(),
                "Scenes.SaveLoad.ConfirmDelete".Localized(entry.Name),
                () =>
                {
                    SaveManager.Delete(entry.Slot);

                    Refresh();
                });
        }

        private static string GameTime(SaveData entry)
        {
            var days = entry.Time.TryGetValue("days", out var daysValue) ? (int)daysValue : 1;

            var hours = entry.Time.TryGetValue("hours", out var hoursValue) ? (int)hoursValue : 6;

            var minutes = entry.Time.TryGetValue("minutes", out var minutesValue) ? (int)minutesValue : 0;

            return $"Day {days}, {hours:00}:{minutes:00}";
        }

        private void Back()
        {
            PlayerStats.HiddenByMenu = false;

            if (ViewNavigator.Back(this))
                return;

            if (FromMainMenu)
            {
                FromMainMenu = false;

                UIManager.Instance.Root.SwitchView(new MainView());

                return;
            }

            GameView.SwitchTo<CampView>();

            Scene.Go(SceneInstance<CampScene>.Instance);
        }

        private static string NextSlot()
        {
            for (var index = 1; ; index++)
            {
                var slot = $"Slot{index}";

                if (!SaveManager.Exists(slot))
                    return slot;
            }
        }

        private void SaveSlot(string slot, string notes)
        {
            if (!CanSave)
                return;

            var data = SaveData.Capture(MonoCoC.CoC.Instance.Player, slot);

            data.Notes = notes;

            data.Write(slot);

            Refresh();

            ShowSaved();
        }

        private void LoadSlot(string slot)
        {
            if (!SaveData.TryRead(slot, out var data))
                return;

            data.Apply(MonoCoC.CoC.Instance.Player);

            PlayerStats.HiddenByMenu = false;

            GameView.SetPlayerStatsVisible(true);

            GameView.SwitchTo<CampView>();

            Scene.Go(SceneInstance<CampScene>.Instance);
        }

        private void SaveToFile()
        {
            if (!CanSave)
                return;

            var path = PickSaveFile("CoC save (*.json)|*.json|All files (*.*)|*.*", $"{MonoCoC.CoC.Instance.Player.ShortName}.json");

            if (path is null)
                return;

            var data = SaveData.Capture(MonoCoC.CoC.Instance.Player, "File");

            File.WriteAllText(path, data.ToJson().ToString());
        }

        private void LoadFromFile()
        {
            var path = PickOpenFile("CoC save (*.json)|*.json|All files (*.*)|*.*");

            if (path is null)
                return;

            var json = (Hjson.JsonObject)MonoCoC.HjsonParser.Parse(File.ReadAllText(path));

            var data = SaveData.FromJson(json);

            if (data is null)
                return;

            data.Apply(MonoCoC.CoC.Instance.Player);

            PlayerStats.HiddenByMenu = false;

            GameView.SwitchTo<CampView>();

            Scene.Go(SceneInstance<CampScene>.Instance);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _animations.Update(gameTime);

            foreach (var row in Slots)
            {
                if (row is null)
                    continue;

                var hovered = row.Hit.IsHovering;

                row.Export.Visible = hovered && row.HasSave;

                row.Import.Visible = hovered;

                row.Delete.Visible = hovered && row.HasSave;

                if (row.ExportImage is not null)
                    row.ExportImage.Color = row.Export.IsHovering ? new Color(0x8B, 0x6B, 0x42) : new Color(0x35, 0x29, 0x1F);

                if (row.ImportImage is not null)
                    row.ImportImage.Color = row.Import.IsHovering ? new Color(0x8B, 0x6B, 0x42) : new Color(0x35, 0x29, 0x1F);

                if (row.DeleteImage is not null)
                    row.DeleteImage.Color = row.Delete.IsHovering ? new Color(0xD9, 0x53, 0x4F) : new Color(0xB0, 0x3A, 0x2E);
            }
        }

        private bool ImportLegacy(string path, string targetSlot)
        {
            try
            {
                _ = MonoCoC.Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.ItemManager>.Instance;

                _ = MonoCoC.Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.PerkManager>.Instance;

                var legacy = MonoCoC.Saving.Legacy.SaveLoader.Load(path);

                var root = (Hjson.JsonObject)MonoCoC.HjsonParser.Parse(legacy.ToString());

                var player = new Hjson.JsonObject();

                foreach (var key in new[]
                         {
                             "lust", "fatigue", "level", "gems", "tallness", "femininity",
                             "tone", "thickness", "fertility", "nippleLength", "balls",
                             "ballSize", "cumMultiplier", "hoursSinceCum", "statPoints",
                             "perkPoints", "ascensionPerkPoints", "hunger", "autoSave"
                         })
                    Copy(root, player, key);

                player["shortName"] = root.TryGetValue("short", out var shortName) ? shortName : "You";

                player["health"] = root.TryGetValue("HP", out var hp) ? hp : 50;

                player["age"] = IntValue(root, "age", 0);

                player["startingAge"] = IntValue(root, "startingAge", 0);

                player["experience"] = root.TryGetValue("XP", out var xp) ? xp : 0;

                player["cocks"] = BuildLegacyCocks(root);

                player["breastRows"] = BuildLegacyBreastRows(root);

                player["anal"] = BuildLegacyAnal(root);

                player["stats"] = new Hjson.JsonObject
                {
                    ["coc:strength"] = FloatValue(root, "str", 50),
                    ["coc:toughness"] = FloatValue(root, "tou", 50),
                    ["coc:speed"] = FloatValue(root, "spe", 50),
                    ["coc:intelligence"] = FloatValue(root, "inte", 50),
                    ["coc:libido"] = FloatValue(root, "lib", 50),
                    ["coc:sensitivity"] = FloatValue(root, "sens", 50),
                    ["coc:corruption"] = FloatValue(root, "cor", 0)
                };

                if (root.TryGetValue("keyItems", out var keyItems) && keyItems is Hjson.JsonArray keyItemArray)
                    player["keyItems"] = keyItemArray;

                player["perks"] = BuildLegacyPerks(root);

                player["statusEffects"] = BuildLegacyStatusEffects(root);

                player["masteries"] = BuildLegacyMasteries(root);

                if (root.TryGetValue("gearStorage", out _))
                    player["gearStorage"] = new Hjson.JsonObject
                    {
                        ["slots"] = BuildLegacyGearStorage(root)
                    };

                var legacySlots = BuildLegacyItemSlots(root);

                player["itemSlots"] = legacySlots;

                player["unlockedItemStacks"] = CountLegacyItems(root);

                player["storage"] = BuildLegacyStorage(root);

                player["weapon"] = LegacyEquipmentId(root, "weapon");

                player["shield"] = LegacyEquipmentId(root, "shield");

                player["armor"] = LegacyEquipmentId(root, "armor");

                player["upperGarment"] = LegacyEquipmentId(root, "upperGarment");

                player["lowerGarment"] = LegacyEquipmentId(root, "lowerGarment");

                player["jewelry"] = LegacyEquipmentId(root, "jewelry");

                player["legacyJewelryHp"] = LegacyJewelryHp(root);

                player["extraEquipment"] = new Hjson.JsonObject
                {
                    ["coc:hand_1"] = LegacyEquipmentId(root, "weapon"),
                    ["coc:hand_2"] = LegacyEquipmentId(root, "shield")
                };

                var mappedFlags = new Hjson.JsonObject();

                if (root.TryGetValue("flags", out var legacyFlags) && legacyFlags is Hjson.JsonObject flagsObject)
                    foreach (var pair in flagsObject)
                        if (MonoCoC.Saving.Legacy.LegacyFlagMap.ByName.TryGetValue(pair.Key, out var flagId))
                            mappedFlags[flagId.ToString()] = pair.Value;

                var json = new Hjson.JsonObject
                {
                    ["version"] = root.TryGetValue("version", out var version) ? version : "legacy",
                    ["name"] = player["shortName"],
                    ["saveTime"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ["flags"] = mappedFlags,
                    ["body"] = BuildLegacyBody(root),
                    ["player"] = player,
                    ["time"] = new Hjson.JsonObject
                    {
                        ["days"] = root.TryGetValue("days", out var days) ? days : 1,
                        ["hours"] = root.TryGetValue("hours", out var hours) ? hours : 6,
                        ["minutes"] = root.TryGetValue("minutes", out var minutes) ? minutes : 0,
                    },
                    ["scenes"] = new Hjson.JsonObject(),
                    ["selfSaving"] = root.TryGetValue("selfSaving", out var selfSaving) && selfSaving is Hjson.JsonObject selfSavingObject
                        ? selfSavingObject
                        : new Hjson.JsonObject(),
                    ["achievements"] = new Hjson.JsonArray(),
                };

                var data = SaveData.FromJson(json);

                if (data is null)
                {
                    ShowHint("Import failed");

                    return false;
                }

                data.Slot = targetSlot;

                data.SaveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                data.Write(targetSlot);

                return true;
            }
            catch (Exception exception)
            {
                ShowHint($"Import failed: {exception.Message}");

                return false;
            }
        }

        private static Hjson.JsonArray BuildLegacyPerks(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("perks", out var value) || value is not Hjson.JsonArray perks)
                return result;

            foreach (var entry in perks)
                if (entry is Hjson.JsonObject perk)
                    result.Add(new Hjson.JsonObject
                    {
                        ["id"] = LegacyPerkIdentifier(StringValue(perk, "id", StringValue(perk, "perkName", ""))),
                        ["values"] = LegacyValues(perk)
                    });

            return result;
        }

        private static Hjson.JsonArray BuildLegacyMasteries(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("masteries", out var value) || value is not Hjson.JsonArray masteries)
                return result;

            foreach (var entry in masteries)
                if (entry is Hjson.JsonObject mastery)
                {
                    var id = StringValue(mastery, "id", "");

                    if (id.Length == 0)
                        continue;

                    result.Add(new Hjson.JsonObject
                    {
                        ["id"] = LegacyIdentifier(id),
                        ["level"] = IntValue(mastery, "level", 0),
                        ["xp"] = IntValue(mastery, "xp", 0),
                        ["permed"] = mastery.TryGetValue("isPermed", out var permed) && (bool)permed
                    });
                }

            return result;
        }

        private static Hjson.JsonArray BuildLegacyStatusEffects(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("statusAffects", out var value) || value is not Hjson.JsonArray statuses)
                return result;

            foreach (var entry in statuses)
                if (entry is Hjson.JsonObject status)
                    result.Add(new Hjson.JsonObject
                    {
                        ["id"] = LegacyStatusIdentifier(StringValue(status, "statusAffectName", "")),
                        ["values"] = LegacyValues(status),
                        ["duration"] = FloatValue(status, "duration", 0)
                    });

            return result;
        }

        private static Hjson.JsonArray LegacyValues(Hjson.JsonObject source) =>
            new()
            {
                FloatValue(source, "value1", 0),
                FloatValue(source, "value2", 0),
                FloatValue(source, "value3", 0),
                FloatValue(source, "value4", 0)
            };

        private static string LegacyIdentifier(string value)
        {
            if (value.StartsWith("coc:", StringComparison.OrdinalIgnoreCase))
                return value.ToLowerInvariant();

            var builder = new System.Text.StringBuilder();

            foreach (var character in value.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(character))
                    builder.Append(character);
                else if (builder.Length > 0 && builder[^1] != '_')
                    builder.Append('_');
            }

            return "coc:" + builder.ToString().Trim('_');
        }

        private static string LegacyPerkIdentifier(string name)
        {
            var target = NormalizeKey(name);

            foreach (var perkId in MonoCoC.Contents.Perks.PerkRegistry.Registry.Ids)
                if (NormalizeKey(MonoCoC.Contents.Perks.PerkRegistry.GetName(perkId)) == target)
                    return perkId.ToString();

            return LegacyIdentifier(name);
        }

        private static string LegacyStatusIdentifier(string name)
        {
            var target = NormalizeKey(name);

            foreach (var field in typeof(MonoCoC.Contents.Datas.Vanilla.StatusEffectIds)
                         .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.FieldType != typeof(MonoCoC.Registry.Identifier))
                    continue;

                var id = (MonoCoC.Registry.Identifier)field.GetValue(null)!;

                if (NormalizeKey(id.Path) == target)
                    return id.ToString();
            }

            return LegacyIdentifier(name);
        }

        private static string NormalizeKey(string value) =>
            new(value.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

        private static Hjson.JsonObject BuildLegacyBody(Hjson.JsonObject root)
        {
            var skinTone = StringValue(root, "skinTone", "albino");

            var furColor = StringValue(root, "furColor", "no");

            var skin = new Hjson.JsonObject
            {
                ["id"] = LegacyIdentifier(StringValue(root, "skinType", "plain")),
                ["tone"] = skinTone,
                ["adj"] = StringValue(root, "skinAdj", ""),
                ["furColor"] = furColor,
                ["descKey"] = "Skin.Desc.skin",
            };

            return new Hjson.JsonObject
            {
                ["coc:skin"] = skin,
                ["coc:under_body"] = new Hjson.JsonObject { ["id"] = "coc:none", ["skin"] = skin },
                ["coc:hair"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "hairType", "normal")),
                    ["color"] = StringValue(root, "hairColor", "no"),
                    ["adj"] = StringValue(root, "hairAdj", ""),
                    ["flowerColor"] = StringValue(root, "hairFlowerColor", ""),
                    ["length"] = FloatValue(root, "hairLength", 0),
                },
                ["coc:ears"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "earType", "human")),
                    ["value"] = FloatValue(root, "earValue", 0),
                    ["earsPierced"] = IntValue(root, "earsPierced", 0),
                },
                ["coc:face"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "faceType", "human")),
                    ["lipPierced"] = IntValue(root, "lipPierced", 0),
                    ["eyebrowPierced"] = IntValue(root, "eyebrowPierced", 0),
                    ["nosePierced"] = IntValue(root, "nosePierced", 0),
                },
                ["coc:eyes"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "eyeType", "human")),
                    ["count"] = IntValue(root, "eyeCount", 2)
                },
                ["coc:tongue"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "tongueType", "human")),
                    ["pierced"] = IntValue(root, "tonguePierced", 0),
                },
                ["coc:neck"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["len"] = 2,
                    ["pos"] = false,
                    ["color"] = "no",
                },
                ["coc:lower_body"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "lowerBody", "human")),
                    ["legCount"] = IntValue(root, "legCount", 2),
                    ["incorporeal"] = root.TryGetValue("incorporeal", out var incorporeal) && (bool)incorporeal,
                },
                ["coc:arms"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "armType", "human")),
                    ["claws"] = new Hjson.JsonObject
                    {
                        ["id"] = LegacyIdentifier(StringValue(root, "clawType", "normal")),
                        ["tone"] = StringValue(root, "clawTone", "")
                    },
                },
                ["coc:beard"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["length"] = FloatValue(root, "beardLength", 0),
                },
                ["coc:horns"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "hornType", "none")),
                    ["value"] = FloatValue(root, "horns", 0)
                },
                ["coc:antennae"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "antennae", "none"))
                },
                ["coc:gills"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "gillType", "none"))
                },
                ["coc:rear_body"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "rearBody", "none")),
                    ["color"] = StringValue(root, "furColor", "")
                },
                ["coc:wings"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "wingType", "none")),
                    ["color"] = StringValue(root, "wingColor", ""),
                    ["color2"] = StringValue(root, "wingColor2", "")
                },
                ["coc:tail"] = new Hjson.JsonObject
                {
                    ["id"] = LegacyIdentifier(StringValue(root, "tailType", "none")),
                    ["venom"] = FloatValue(root, "tailVenum", 0),
                    ["recharge"] = FloatValue(root, "tailRecharge", 5)
                },
            };
        }

        private static Hjson.JsonArray BuildLegacyCocks(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("cocks", out var value) || value is not Hjson.JsonArray cocks)
                return result;

            foreach (var item in cocks)
            {
                if (item is not Hjson.JsonObject cock)
                    continue;

                result.Add(new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["length"] = FloatValue(cock, "cockLength", 0),
                    ["thickness"] = FloatValue(cock, "cockThickness", 0),
                    ["knotMultiplier"] = FloatValue(cock, "knotMultiplier", 1),
                    ["pierced"] = IntValue(cock, "pierced", 0),
                    ["piercingShortName"] = StringValue(cock, "pShortDesc", ""),
                    ["piercingLongName"] = StringValue(cock, "pLongDesc", ""),
                    ["sock"] = StringValue(cock, "sock", ""),
                    ["sheathOn"] = false,
                });
            }

            return result;
        }

        private static Hjson.JsonArray BuildLegacyBreastRows(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("breastRows", out var value) || value is not Hjson.JsonArray rows)
                return result;

            foreach (var item in rows)
            {
                if (item is not Hjson.JsonObject row)
                    continue;

                result.Add(new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["breasts"] = IntValue(row, "breasts", 2),
                    ["breastRating"] = FloatValue(row, "breastRating", 0),
                    ["milkFullness"] = FloatValue(row, "milkFullness", 0),
                    ["nipplesPerBreast"] = IntValue(row, "nipplesPerBreast", 1),
                    ["lactation"] = FloatValue(row, "lactationMultiplier", 0),
                    ["loli"] = false,
                    ["fuckable"] = false,
                    ["nippleCocks"] = false,
                    ["nipplesPierced"] = IntValue(root, "nipplesPierced", 0),
                });
            }

            return result;
        }

        private static Hjson.JsonObject BuildLegacyAnal(Hjson.JsonObject root)
        {
            var looseness = 0d;

            var wetness = 0d;

            var fullness = 0d;

            if (root.TryGetValue("ass", out var value) && value is Hjson.JsonObject ass)
            {
                looseness = FloatValue(ass, "analLooseness", 0);

                wetness = FloatValue(ass, "analWetness", 0);

                fullness = FloatValue(ass, "fullness", 0);
            }

            return new Hjson.JsonObject
            {
                ["id"] = "coc:normal",
                ["wetness"] = wetness,
                ["looseness"] = looseness,
                ["fullness"] = fullness,
                ["virgin"] = false,
            };
        }

        private static Hjson.JsonArray BuildLegacyItemSlots(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("items", out var value) || value is not Hjson.JsonArray slots)
                return result;

            foreach (var entry in slots)
            {
                var slot = new Hjson.JsonObject();

                if (entry is Hjson.JsonObject saved)
                {
                    var item = StringValue(saved, "id", "");

                    if (item.Length == 0)
                        item = StringValue(saved, "shortName", "");

                    slot["item"] = item;
                    slot["quantity"] = IntValue(saved, "quantity", 0);
                    slot["damage"] = IntValue(saved, "damage", 0);
                    slot["unlocked"] = true;
                }
                else
                {
                    slot["item"] = "";
                    slot["quantity"] = 0;
                    slot["damage"] = 0;
                    slot["unlocked"] = false;
                }

                result.Add(slot);
            }

            return result;
        }

        private static float LegacyJewelryHp(Hjson.JsonObject root)
        {
            var id = LegacyEquipmentId(root, "jewelry");

            return id switch
            {
                "LifeRng" => 30f,
                "LifeRn2" => 45f,
                "LifeRn3" => 60f,
                _ => 0f
            };
        }

        private static string LegacyEquipmentId(Hjson.JsonObject root, string key)
        {
            if (!root.TryGetValue(key, out var value))
                return "";

            if (value is Hjson.JsonObject item)
                return StringValue(item, "id", "");

            var text = value.ToString();

            return text is "nounder" or "NOTHING!" ? "" : text;
        }

        private static Hjson.JsonArray BuildLegacyGearStorage(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("gearStorage", out var value) || value is not Hjson.JsonArray slots)
                return result;

            foreach (var entry in slots)
            {
                var slot = new Hjson.JsonObject();

                if (entry is Hjson.JsonObject saved)
                {
                    var item = StringValue(saved, "id", "");

                    if (item.Length == 0)
                        item = StringValue(saved, "shortName", "");

                    slot["item"] = item;
                    slot["quantity"] = IntValue(saved, "quantity", 0);
                    slot["damage"] = IntValue(saved, "damage", 0);
                    slot["unlocked"] = true;
                }
                else
                {
                    slot["item"] = "";
                    slot["quantity"] = 0;
                    slot["damage"] = 0;
                    slot["unlocked"] = false;
                }

                result.Add(slot);
            }

            return result;
        }

        private static int CountLegacyItems(Hjson.JsonObject root) =>
            root.TryGetValue("items", out var value) && value is Hjson.JsonArray items ? items.Count : 0;

        private static Hjson.JsonObject BuildLegacyStorage(Hjson.JsonObject root)
        {
            var storage = new Hjson.JsonObject();

            if (root.TryGetValue("itemStorage", out var itemValue) && itemValue is Hjson.JsonArray itemSlots)
                storage["coc:items"] = new Hjson.JsonObject
                {
                    ["slots"] = ConvertLegacySlots(itemSlots)
                };

            if (root.TryGetValue("gearStorage", out var gearValue) && gearValue is Hjson.JsonArray gearSlots)
            {
                var areas = new[]
                {
                    "coc:gear_weapons",
                    "coc:gear_armor",
                    "coc:gear_shields",
                    "coc:gear_undergarments",
                    "coc:gear_jewelry"
                };

                for (var area = 0; area < areas.Length; area++)
                {
                    var slots = new Hjson.JsonArray();

                    for (var index = area * 9; index < Math.Min((area + 1) * 9, gearSlots.Count); index++)
                        slots.Add(ConvertLegacySlot(gearSlots[index]));

                    storage[areas[area]] = new Hjson.JsonObject { ["slots"] = slots };
                }
            }

            return storage;
        }

        private static Hjson.JsonArray ConvertLegacySlots(Hjson.JsonArray source)
        {
            var result = new Hjson.JsonArray();

            foreach (var entry in source)
                result.Add(ConvertLegacySlot(entry));

            return result;
        }

        private static Hjson.JsonObject ConvertLegacySlot(Hjson.JsonValue entry)
        {
            var slot = new Hjson.JsonObject();

            if (entry is Hjson.JsonObject saved)
            {
                var item = StringValue(saved, "id", "");

                if (item.Length == 0)
                    item = StringValue(saved, "shortName", "");

                slot["item"] = item;
                slot["quantity"] = IntValue(saved, "quantity", 0);
                slot["damage"] = IntValue(saved, "damage", 0);
                slot["unlocked"] = true;
            }
            else
            {
                slot["item"] = "";
                slot["quantity"] = 0;
                slot["damage"] = 0;
                slot["unlocked"] = false;
            }

            return slot;
        }

        private static int CountUnlockedLegacySlots(Hjson.JsonObject root)
        {
            if (!root.TryGetValue("itemStorage", out var value) || value is not Hjson.JsonArray slots)
                return 0;

            var count = 0;

            foreach (var entry in slots)
                if (entry is Hjson.JsonObject saved &&
                    saved.TryGetValue("unlocked", out var unlocked) &&
                    unlocked.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    count++;

            return count;
        }

        private static string StringValue(Hjson.JsonObject source, string key, string fallback)
        {
            if (!source.TryGetValue(key, out var value))
                return fallback;

            var text = value.ToString();

            return text.Length >= 2 && text[0] == '"' && text[^1] == '"' ? text[1..^1] : text;
        }

        private static int IntValue(Hjson.JsonObject source, string key, int fallback) =>
            source.TryGetValue(key, out var value) && double.TryParse(value.ToString(), out var number) ? (int)number : fallback;

        private static float FloatValue(Hjson.JsonObject source, string key, float fallback) =>
            source.TryGetValue(key, out var value) && float.TryParse(value.ToString(), out var number) ? number : fallback;

        private static void Copy(Hjson.JsonObject source, Hjson.JsonObject target, string key)
        {
            if (source.TryGetValue(key, out var value))
                target[key] = value;
        }

        private void ExecuteSlotAction(int index, int action)
        {
            var entry = _slots[index];

            switch (action)
            {
                case 0:
                    ExportSlot(index);

                    break;

                case 1:
                    ImportSlot(index);

                    break;

                case 2:
                    if (entry is null)
                        break;

                    ConfirmDialogOverlay.Show(
                        "Scenes.SaveLoad.Delete".Localized(),
                        "Scenes.SaveLoad.ConfirmDelete".Localized(entry.Name),
                        () =>
                        {
                            SaveManager.Delete(entry.Slot);

                            Refresh();
                        });

                    break;
            }
        }

        private void ExportSlot(int index)
        {
            var entry = _slots[index];

            if (entry is null)
            {
                ShowHint("No saved game");

                return;
            }

            var path = PickSaveFile("CoC save (*.hjson;*.json)|*.hjson;*.json|All files (*.*)|*.*", $"{entry.Name}.hjson");

            if (path is null)
                return;

            try
            {
                Hjson.HjsonValue.Save(entry.ToJson(), path);

                ClearHint();
            }
            catch (Exception exception)
            {
                ShowHint($"Export failed: {exception.Message}");
            }
        }

        private void ImportSlot(int index)
        {
            var path = PickOpenFile("CoC save (*.hjson;*.json;*.sol;*.coc)|*.hjson;*.json;*.sol;*.coc|All files (*.*)|*.*");

            if (path is null)
                return;

            var target = _slots[index]?.Slot ?? NextSlot();

            var extension = Path.GetExtension(path).ToLowerInvariant();

            try
            {
                if (extension is ".sol" or ".coc")
                {
                    if (!ImportLegacy(path, target))
                        return;

                    ClearHint();

                    Refresh();

                    return;
                }

                var json = (Hjson.JsonObject)MonoCoC.HjsonParser.Parse(File.ReadAllText(path));

                var data = SaveData.FromJson(json);

                if (data is null)
                {
                    ShowHint("Import failed");

                    return;
                }

                data.Slot = target;

                data.SaveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                data.Write(target);

                ClearHint();

                Refresh();
            }
            catch (Exception exception)
            {
                ShowHint($"Import failed: {exception.Message}");
            }
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (ListPanel is null)
                return base.OnInput(args);

            var view = new Rectangle(
                (int)ListPanel.Position.X + 16,
                (int)ListPanel.Position.Y + 16,
                (int)ListPanel.Width - 32,
                (int)ListPanel.Height - 32);

            if (args.Type == InputType.MouseWheel && ListPanel.Rectangle.Contains(args.MousePosition))
            {
                _scrollFade.Flash();

                _scroll = Math.Clamp(
                    _scroll + (args.WheelDelta > 0 ? -40 : 40),
                    0,
                    Math.Max(0, ListContentHeight - view.Height));

                ApplyScroll();

                return InputResult.Consumed;
            }

            if (args.Type == InputType.MouseLeftDown &&
                ScrollBar.Contains(view, args.MousePosition, ListContentHeight, view.Height))
            {
                _draggingBar = true;

                _scrollFade.Pinned = true;

                _scroll = ScrollBar.ScrollFrom(view, ListContentHeight, view.Height, args.MousePosition.Y);

                ApplyScroll();

                return InputResult.Consumed;
            }

            if (_draggingBar && args.Type == InputType.MouseMove)
            {
                _scroll = ScrollBar.ScrollFrom(view, ListContentHeight, view.Height, args.MousePosition.Y);

                ApplyScroll();

                return InputResult.Consumed;
            }

            if (_draggingBar && args.Type == InputType.MouseLeftUp)
            {
                _draggingBar = false;

                _scrollFade.Pinned = false;

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        public override void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.PostDraw(spriteBatch, gameTime);

            if (ListPanel is null)
                return;

            var view = new Rectangle(
                (int)ListPanel.Position.X + 16,
                (int)ListPanel.Position.Y + 16,
                (int)ListPanel.Width - 32,
                (int)ListPanel.Height - 32);

            _scrollFade.Update(gameTime, view, ScrollBar.Needed(ListContentHeight, view.Height));

            if (!ScrollBar.Needed(ListContentHeight, view.Height) || !_scrollFade.Visible)
                return;

            var thumb = ScrollBar.Thumb(view, ListContentHeight, view.Height, _scroll);

            spriteBatch.DrawRectangle(thumb, _scrollFade.Fade(UIParseUtils.ColorOf("#B79A70FF")));
        }

        private void ApplyScroll()
        {
            if (RowsGrid is null)
                return;

            RowsGrid.LayoutData.Position = new Vector2(20, 20 - _scroll);
        }

        private static string? PickOpenFile(string filter)
        {
            string? selected = null;

            var thread = new Thread(() =>
            {
                using var dialog = new System.Windows.Forms.OpenFileDialog { Filter = filter };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    selected = dialog.FileName;
            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
            thread.Join();

            return selected;
        }

        private static string? PickSaveFile(string filter, string fileName)
        {
            string? selected = null;

            var thread = new Thread(() =>
            {
                using var dialog = new System.Windows.Forms.SaveFileDialog { Filter = filter, FileName = fileName };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    selected = dialog.FileName;
            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
            thread.Join();

            return selected;
        }

        private sealed class SlotCard
        {
            public Button Hit = null!;

            public Label Title = null!;

            public Label Meta = null!;

            public Button Export = null!;

            public Button Import = null!;

            public Button Delete = null!;

            public UIImage? ExportImage;

            public UIImage? ImportImage;

            public UIImage? DeleteImage;

            public bool HasSave;
        }
    }
}
