using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace MonoCoC.UI.Components.Views
{
    public class SaveLoadView : HjsonView
    {
        private const int SlotCount = 10;

        private Button[] Slots { get; set; } = null!;

        private SegmentedControl Tabs { get; set; } = null!;

        private Button ImportButton { get; set; } = null!;

        private Button DeleteButton { get; set; } = null!;

        private Button PrimaryButton { get; set; } = null!;

        private Button BackButton { get; set; } = null!;

        private Label DetailName { get; set; } = null!;

        private Label DetailMeta { get; set; } = null!;

        private Label DetailNotes { get; set; } = null!;

        private Label DetailSlot { get; set; } = null!;

        private readonly SaveData?[] _slots = new SaveData?[SlotCount];

        private int _selected;

        private int _tab;

        private bool _deletePending;

        public bool FromMainMenu { get; set; }

        public SaveLoadView()
        {
            Initialize();

            Refresh();
        }

        protected override string ScreenFile => "Views\\SaveLoadView.hjson";

        [UIRequired("saveload.rows")]
        private void BindSlots(Grid part)
        {
            Slots = new Button[SlotCount];

            for (var index = 0; index < part.LayoutData.Children.Count && index < SlotCount; index++)
                if (part.LayoutData.Children[index] is Button button)
                    Slots[index] = button;
        }

        [UIRequired("saveload.tabs")]
        private void BindTabs(SegmentedControl part)
        {
            Tabs = part;

            Tabs.SetOptions(
            [
                "Scenes.SaveLoad.Button.Load".Localized(),
                "Scenes.SaveLoad.Button.Save".Localized(),
                "Manage"
            ], 0);

            Tabs.ValueChanged = index =>
            {
                _tab = index;

                _deletePending = false;

                Refresh();
            };
        }

        [UIRequired("saveload.import")]
        private void BindImport(Button part) => ImportButton = part;

        [UIRequired("saveload.delete")]
        private void BindDelete(Button part) => DeleteButton = part;

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

        [UIRequired("saveload.detail.slot")]
        private void BindDetailSlot(Label part) => DetailSlot = part;

        private bool CanSave => MonoCoC.CoC.Instance.Player.Loaded && !FromMainMenu;

        private SaveData? CurrentSave => _slots[Math.Clamp(_selected, 0, SlotCount - 1)];

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
        }

        private void RefreshSlots()
        {
            for (var index = 0; index < Slots.Length; index++)
            {
                var button = Slots[index];

                if (button is null)
                    continue;

                var slotIndex = index;

                var entry = _slots[index];

                button.Visible = true;
                button.Selected = index == _selected;
                button.DisableIf = "";
                button.Enabled = entry is not null || _tab == 1;
                button.DisabledReason = "";

                if (entry is null)
                    button.SetLabel($"Slot {index + 1:00} — Empty Slot\nNo saved game");
                else
                {
                    var selected = index == _selected ? "   ·   Selected" : "";

                    button.SetLabel($"Slot {index + 1:00} — {entry.Name}{selected}\n{GameTime(entry)}   ·   {(string.IsNullOrWhiteSpace(entry.Notes) ? "—" : entry.Notes)}");
                }

                button.OnClick = () =>
                {
                    _selected = slotIndex;

                    _deletePending = false;

                    Refresh();
                };
                button.RefreshVisual();
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
                DetailSlot.Text($"Slot {_selected + 1:00}");

                return;
            }

            DetailName.Text(entry.Name);
            DetailMeta.Text(GameTime(entry));
            DetailNotes.Text(string.IsNullOrWhiteSpace(entry.Notes) ? "—" : entry.Notes);
            DetailSlot.Text($"Slot {_selected + 1:00}   ·   Saved {entry.SaveTimeText}");
        }

        private void RefreshActions()
        {
            ImportButton.SetLabel("Import / Export");
            ImportButton.Enabled = false;
            ImportButton.DisabledReason = "";
            ImportButton.DisableIf = "";
            ImportButton.RefreshVisual();

            DeleteButton.SetLabel(_deletePending ? "Common.Yes".Localized() : "Scenes.SaveLoad.Button.Delete".Localized());
            DeleteButton.Enabled = CurrentSave is not null;
            DeleteButton.DisableIf = "";
            DeleteButton.DisabledReason = "";
            DeleteButton.OnClick = DeleteSelected;
            DeleteButton.RefreshVisual();

            var primary = _tab switch
            {
                0 => "Scenes.SaveLoad.Button.Load".Localized(),
                1 => "Scenes.SaveLoad.Button.Save".Localized(),
                _ => "Manage"
            };

            PrimaryButton.SetLabel(primary);
            PrimaryButton.Enabled = _tab switch
            {
                0 => CurrentSave is not null,
                1 => CanSave,
                _ => false
            };
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
                    SaveSlot(SaveLoad.NextSlot(), "");
                else
                    SaveSlot(entry.Slot, entry.Notes);
            }
        }

        private void DeleteSelected()
        {
            var entry = CurrentSave;

            if (entry is null)
                return;

            if (!_deletePending)
            {
                _deletePending = true;

                RefreshActions();

                return;
            }

            SaveManager.Delete(entry.Slot);

            _deletePending = false;

            Refresh();
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

            if (FromMainMenu)
            {
                FromMainMenu = false;

                UIManager.Instance.Root.SwitchView(new MainView());

                return;
            }

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<CampScene>.Instance);
        }

        private void SaveSlot(string slot, string notes)
        {
            if (!CanSave)
                return;

            var data = SaveData.Capture(MonoCoC.CoC.Instance.Player, slot);

            data.Notes = notes;

            data.Write(slot);

            Refresh();
        }

        private void LoadSlot(string slot)
        {
            if (!SaveData.TryRead(slot, out var data))
                return;

            data.Apply(MonoCoC.CoC.Instance.Player);

            PlayerStats.HiddenByMenu = false;

            GameView.SetPlayerStatsVisible(true);

            UIManager.Instance.Root.SwitchView(new CampView());

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

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<CampScene>.Instance);
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
    }
}
